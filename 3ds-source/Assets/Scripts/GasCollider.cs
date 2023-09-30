using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCollider : MonoBehaviour {

	public PlayerController taxi;
	public AudioClip noise;

	// Use this for initialization
	void Start()
	{
		taxi = GameObject.Find("Taxi").GetComponent<PlayerController>();
		transform.parent.gameObject.transform.GetChild(2).gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
	}

	// Update is called once per frame
	void Update()
	{

	}

	//run when taxi drives into collision box
	void OnTriggerStay(Collider other)
	{
		if (taxi.speed == 0 && !taxi.hasPassenger)
		{
			//refuel gas tank
			taxi.gasTank = 8000;

			//set bool that gas station was collected
			taxi.gasSpawned = false;

			//play refuel noise
			taxi.musicSource.PlayOneShot(noise);

			//delete object when done
			Destroy(transform.parent.gameObject);
		}
	}
}
