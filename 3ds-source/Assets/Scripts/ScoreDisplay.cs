using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	public List<Scores> scores;
	public Text scoreObject;
	public GameObject canvas;
	private float timePassed;

	// Use this for initialization
	void Start () {
		scores = Scores.LoadAll(Scores.rootDir);
		for (int i = 0; i < 5; i++)
        {
			Vector3 offset = new Vector3(0, i * 30, 0);
			Text current = Instantiate(scoreObject, scoreObject.transform.position - offset, scoreObject.transform.rotation) as Text;
			current.transform.SetParent(canvas.transform, false);
			current.text = scores[i].name + " - $" + scores[i].score.ToString("F2");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
