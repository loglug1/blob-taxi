using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	public GameObject pickupObject;
	public GameObject dropoffObject;
	public GameObject gasStationObject;
	private Vector3[] blockCoords = new Vector3[] {
		new Vector3(-73.26538f, 1.95f, 49.62268f), new Vector3(73.86562f, 1.95f, 49.62268f), new Vector3(220.99662f, 1.95f, 49.62268f), new Vector3(368.12762f, 1.95f, 49.62268f),
		new Vector3(-73.26538f, 1.95f, 196.74102f), new Vector3(73.86562f, 1.95f, 196.74102f), new Vector3(220.99662f, 1.95f, 196.74102f), new Vector3(368.12762f, 1.95f, 196.74102f),
		new Vector3(-73.26538f, 1.95f, 343.85936f), new Vector3(73.86562f, 1.95f, 343.85936f), new Vector3(220.99662f, 1.95f, 343.85936f), new Vector3(368.12762f, 1.95f, 343.85936f),
		new Vector3(-73.26538f, 1.95f, 490.9777f), new Vector3(73.86562f, 1.95f, 490.9777f), new Vector3(220.99662f, 1.95f, 490.9777f), new Vector3(368.12762f, 1.95f, 490.9777f)
	};
	private Vector3[] sideOffsets = new Vector3[] { new Vector3(0f, 0f, 61.2777f), new Vector3(61.4276f, 0f, 0f), new Vector3(0f, 0f, -61.2777f), new Vector3(-61.4276f, 0f, 0f) };

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 5; i++) {
			spawnPickup();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//generate location for spawn location
	object[] GenerateLocation()
	{
		object[] arr = new object[2];

		//choose random block and side
		int blockNumber = Random.Range(0, 16);
		int side = Random.Range(0, 4);

		//generate coordinates with offsets and assign it to arr[0]
		Vector3 sidewalkOffset;
		if (side % 2 == 0)
		{
			sidewalkOffset = new Vector3(Random.Range(-55, 55), 0f, 0f);
		}
		else
		{
			sidewalkOffset = new Vector3(0f, 0f, Random.Range(-55, 55));
		}
		arr[0] = blockCoords[blockNumber] + sideOffsets[side] + sidewalkOffset;

		//generate the rotation and store it in arr[1]
		int rotation = 90 * side;
		arr[1] = Quaternion.Euler(0, rotation, 0);
	
		
		return arr;
	}

	//spawn pickup at random location
	public void spawnPickup ()
	{
		object[] locationProperties = GenerateLocation();
		Instantiate(pickupObject, (Vector3)locationProperties[0], (Quaternion)locationProperties[1]);
	}

	//spawn dropoff at random location
	public void spawnDropoff ()
	{
		object[] locationProperties = GenerateLocation();
		Instantiate(dropoffObject, (Vector3)locationProperties[0], (Quaternion)locationProperties[1]);
	}

	//spawn gas station at random location
	public void spawnGasStation ()
    {
		object[] locationProperties = GenerateLocation();
		Instantiate(gasStationObject, (Vector3)locationProperties[0], (Quaternion)locationProperties[1]);
	}
}
