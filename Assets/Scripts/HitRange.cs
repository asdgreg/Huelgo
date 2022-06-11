using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRange : MonoBehaviour {

	public bool lejos = false;
	// Use this for initialization

	void OnTriggerEnter(Collider other) {
		if (other.tag == "body") {
			lejos = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "body") {
			lejos = false;
		}
	}
}
