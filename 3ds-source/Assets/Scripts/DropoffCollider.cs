using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropoffCollider : MonoBehaviour {

	public PlayerController taxi;
	public SpawnController spawnController;
	public AudioClip noise;

	// Use this for initialization
	void Start () {
		taxi = GameObject.Find("Taxi").GetComponent<PlayerController>();
		spawnController = GameObject.Find("SpawnController").GetComponent<SpawnController>();
		transform.parent.gameObject.transform.GetChild(2).gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//run when taxi drives into collision box
	void OnTriggerStay(Collider other)
	{
		if (taxi.speed == 0 && taxi.hasPassenger)
		{

			float currentSale = taxi.currentSale;
			//add earnings to total (current is cleared in player controller)
			if (currentSale < 3)
            {
				taxi.totalSale += 3;
			}
			else
            {
				taxi.totalSale += currentSale;
            }

			//increment counter
			taxi.dropoffCounter++;

			//every fourth dropoff, increase difficulty and check to spawn gas station
			if (taxi.dropoffCounter >= 4)
            {
				//increase difficulty
				taxi.difficultyModifier *= 1.1f;
				
				//maybe spawn gas station
				if (!taxi.gasSpawned)
				{
					taxi.gasSpawned = true;
					spawnController.spawnGasStation();
				}
				
				//reset the counter
				taxi.dropoffCounter = 0;
            }

			//play dropoff noise
			taxi.musicSource.PlayOneShot(noise);

			//set boolean for ui and checking future pickups
			taxi.hasPassenger = false;

			//delete object when done
			Destroy(transform.parent.gameObject);
		}
	}
}
