using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashText : MonoBehaviour {

	public float timePassed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timePassed += Time.deltaTime;
		if (timePassed >= 0.5)
		{
			transform.GetComponent<Text>().enabled = true;
		}
		if (timePassed >= 1)
		{
			transform.GetComponent<Text>().enabled = false;
			timePassed = 0;
		}
	}
}
