using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    //public Text coords;
    public Text speedometer;
    public float speed = 0f;
    public float turnSpeed;
    public float accelerationSpeed = 2.5f;
    public float breakSpeed = 10f;
    public float minSpeed;
    public float maxSpeed;
    public float decelerationSpeed;
    public bool inReverse = false;
    private float circlePadX;
    private float speedModifier;
    private bool rButtonDown;
    private bool lButtonDown;
    private bool bButton;
    private bool aButton;
    public GameObject currentLocation;
    public bool hasPassenger = false;
    public SpriteRenderer consoleRenderer;
    public Sprite consoleDrive;
    public Sprite consoleReverse;
    public float currentSale = 0;
    public float totalSale = 0;
    public Text currentSaleMeter;
    public Text totalSaleMeter;
    public float gasTank = 8000;
    public Slider gasMeter;
    public bool gasSpawned;
    public float dropoffCounter = 0;
    public float difficultyModifier = 1;
    public bool startDown;
    public GameObject StartScreen;
    public bool running;
    public GameObject upperTitle;
    public GameObject lowerTitle;
    public GameObject gameOverMessage;
    public bool gameOver = false;
    public int scoreNumber;
    public bool save;
    public List<Scores> scores;
    public AudioClip marioKartIntro;
    public AudioClip marioKart;
    public AudioClip marioPartyIntro;
    public AudioClip marioParty;
    public AudioClip saffronCity;
    public AudioClip funkMusic;
    public AudioClip gameOverNoise;
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //REMEMBER TO CHANGE STORAGE LOCATION WHEN SWITCHING PLATFORMS
        //set the inputs to easy to read variables 3DS BUTTONS
        circlePadX = UnityEngine.N3DS.GamePad.CirclePad.x;
        rButtonDown = UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.R);
        lButtonDown = UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.L);
        aButton = UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.A);
        bButton = UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.B);
        startDown = UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.Start);


        //set the input to easy to read variables PC TESTING
        //circlePadX = Input.GetAxis("Horizontal");
        //rButtonDown = Input.GetKey("up");
        //lButtonDown = Input.GetKey("down");
        //aButton = Input.GetKey("a");
        //bButton = Input.GetKey("b");
        //startDown = Input.GetKey("space");

        //button combo to reset the game
        if (lButtonDown && rButtonDown && startDown)
        {
            reset();
        }

        //save the game if the score beat any scores
        if (save && !gameOver)
        {
            scores.Insert(scoreNumber, new Scores(UnityEngine.N3DS.Keyboard.GetText().ToUpper(), totalSale));
            scores.RemoveAt(5);
            Scores.SaveAll(Scores.rootDir, scores);
            reset();
        }
        else if (save && gameOver)
        {
            reset();
        }

        //controls whether the game is running or if on title screen
        if (!running && !gameOver)
        {
            title();
        }
        else if (running && !gameOver)
        {
            game();
        } else if (gameOver && running)
        {
            StartCoroutine(GameOver());
        }
    }

    //runs the main game
    public void game()
    {

        //if/else decides whether or not the input should use the breaking speed modifier or the acceleration modifier
        if (lButtonDown)
        {
            speedModifier = -breakSpeed;
        }
        else if (rButtonDown)
        {
            speedModifier = accelerationSpeed;
        }
        else
        {
            speedModifier = decelerationSpeed;
        }

        //transmission control -- sets the transmission indicator and also changes the min max speeds
        if (bButton && speed == 0)
        {
            inReverse = true;
            consoleRenderer.sprite = consoleReverse;
            minSpeed = -15;
            maxSpeed = 0;
        }
        if (aButton && speed == 0)
        {
            inReverse = false;
            consoleRenderer.sprite = consoleDrive;
            minSpeed = 0;
            maxSpeed = 35;
        }
        //reverse speed modifier to go backwards if in reverse
        if (inReverse == true)
        {
            speedModifier = -speedModifier;
            circlePadX = -circlePadX;
        }

        //applies the break/acceleration to the speed
        speed += Time.deltaTime * speedModifier;
        //checks to keep the speed within threshold
        if (speed < minSpeed) { speed = minSpeed; }
        if (speed > maxSpeed) { speed = maxSpeed; }

        //play driving noise with the pitch being percent of max speed
        GetComponent<AudioSource>().pitch = speed / 35f;

        //move the vehicle based on direction
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        //turn the vehicle if moving (the vehicle turns wider the faster you go)
        if (speed != 0)
        {
            turnSpeed = 2f * ((30 * Mathf.Pow(0.95f, speed)) + 15f);
            transform.Rotate(Vector3.up, Time.deltaTime * circlePadX * turnSpeed);
        }

        //reset car if falling off map
        if (transform.position.y < -10)
        {
            speed = 0;
            transform.position = new Vector3(6, 2, 0);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        //set the location indicator to the location of the taxi
        currentLocation.transform.position = new Vector3(transform.position.x, 660, transform.position.z);

        //use gas
        gasTank -= Time.deltaTime * Mathf.Abs(speed) * difficultyModifier;
        //display gas
        gasMeter.value = gasTank / 8000;

        //calculate money sales
        if (hasPassenger)
        {
            currentSale += (Time.deltaTime * Mathf.Abs(speed) / 200);
        }
        else
        {
            currentSale = 0;
        }

        //update speedometer
        speedometer.text = ((int)Mathf.Abs(speed)).ToString();
        //coords.text = "X:" + (int)transform.position.x + " Y:" + (int)transform.position.y + " Z:" + (int)transform.position.z;

        //show current sales
        if (currentSale < 3 && currentSale != 0)
        {
            currentSaleMeter.text = "$3.00";
        }
        else if (currentSale == 0)
        {
            currentSaleMeter.text = "$0.00";
        }
        else
        {
            currentSaleMeter.text = "$" + currentSale.ToString("F2");
        }

        //display total sales
        totalSaleMeter.text = "$" + totalSale.ToString("F2");

        //game over
        if (gasTank <= 0 || transform.position.y > 4)
        {
            gameOver = true;
        }
    }

    //runs the titlescreen
    public void title()
    {
        if (startDown)
        {
            Destroy(upperTitle);
            Destroy(lowerTitle);
            //THIS IS FOR THE MARIO MUSIC VVVVV
            //clip 1 is the looped part and clip 0 is the one time intro
            AudioClip[] clip = GetMusic();
            musicSource.clip = clip[1];
            musicSource.PlayOneShot(clip[0]);
            musicSource.PlayScheduled(AudioSettings.dspTime + clip[0].length);

            running = true;
        }
    }

    //used for game over
    IEnumerator GameOver()
    {
        running = false;
        //stop music and play loser sound
        musicSource.Stop();
        musicSource.PlayOneShot(gameOverNoise);
        //show game over message
        Instantiate(gameOverMessage, gameOverMessage.transform.position, gameOverMessage.transform.rotation);
        //make the coroutine wait
        yield return new WaitForSeconds(5);
        //load scores into variable
        scores = Scores.LoadAll(Scores.rootDir);
        //runs through and checks if any scores are beat
        for (int i = 0; i < scores.Count; i++)
        {
            if (totalSale > scores[i].score)
            {
                getName();
                scoreNumber = i;
                i = 7;
                save = true;
                gameOver = false;
            }
        }
        save = true;
    }

    //used for inputing name for scores
    public void getName()
    {
        UnityEngine.N3DS.Keyboard.SetMaxTextLength(3);
        UnityEngine.N3DS.Keyboard.SetFinishableCondition(N3dsKeyboardFinishableCondition.Full);
        UnityEngine.N3DS.Keyboard.SetLineFeedEnabled(false);
        UnityEngine.N3DS.Keyboard.SetPredictionEnabled(false);
        UnityEngine.N3DS.Keyboard.SetFixedWidthMode(true);
        UnityEngine.N3DS.Keyboard.SetNumBottomButtons(1);
        UnityEngine.N3DS.Keyboard.SetSoftwareResetEnabled(false);
        UnityEngine.N3DS.Keyboard.SetType(N3dsKeyboardType.Qwerty);
        UnityEngine.N3DS.Keyboard.Show();
        UnityEngine.N3DS.Keyboard.GetResult();
    }

    //called when you run out of gas or if you run reset combo
    public void reset()
    {
        //stop audio to prevent stuttering
        musicSource.Stop();
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("City");
    }

    public AudioClip[] GetMusic()
    {
        if (lButtonDown)
        {
            return new AudioClip[2] { marioKartIntro, marioKart };
        } else if (rButtonDown)
        {
            return new AudioClip[2] { marioPartyIntro, marioParty };
        } else
        {
            return new AudioClip[2] { funkMusic, funkMusic };
        }
    }
}
