using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objLanza : MonoBehaviour {
	public GameObject kbom;
	public float tempVida = 3f;
	public bool creado = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "suelo" && !creado) {
			Quaternion qua = other.transform.rotation;
			qua.Set (90, 0, 0, -90);
			GameObject pum = Instantiate (kbom, gameObject.transform.position, qua) as GameObject;
			creado = true;
			//pum.GetComponent<ParticleSystem> ().Play();
			//other.gameObject.SetActive (false);
			Destroy (pum, tempVida);
			Destroy (this.gameObject);
		}
	}
}
