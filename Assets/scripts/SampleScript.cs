using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour {

	private Soft Soft;
	
	
	void Start () {
		Debug.Log("START");
		Soft = new Soft();
		Soft.StartClient();
	}
	
	void Update () {
		
	}
}
