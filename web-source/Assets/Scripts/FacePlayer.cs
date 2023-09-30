using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour {

	public GameObject taxi;

	// Use this for initialization
	void Start () {
		taxi = GameObject.Find("Taxi");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(taxi.transform.GetChild(0).gameObject.transform);
	}
}
