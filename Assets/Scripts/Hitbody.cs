using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbody : MonoBehaviour {
	public bool cerca = false;
	// Use this for initialization

	void OnTriggerEnter(Collider other) {
		if (other.tag == "body") {
			cerca = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "body") {
			cerca = false;
		}
	}
}
