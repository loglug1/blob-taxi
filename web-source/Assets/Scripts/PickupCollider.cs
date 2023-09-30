using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollider : MonoBehaviour {

	public PlayerController taxi;
	public SpawnController spawnController;
	public AudioClip noise;

	// Use this for initialization
	void Start () {
		taxi = GameObject.Find("Taxi").GetComponent<PlayerController>();
		spawnController = GameObject.Find("SpawnController").GetComponent<SpawnController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//run when taxi drives into collision box
	void OnTriggerStay(Collider other)
	{
		if (taxi.speed == 0 && !taxi.hasPassenger)
		{
			//set boolean for ui and checking future pickups
			taxi.hasPassenger = true;
			taxi.currentSale = 0.01f;

			//replace the pickup
			spawnController.spawnPickup();

			//create dropoff spot
			spawnController.spawnDropoff();

			//play pickup noise
			taxi.musicSource.PlayOneShot(noise);

			//delete object when done
			Destroy(transform.parent.gameObject);
		}
	}
}