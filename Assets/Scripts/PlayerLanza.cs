using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnControls;

public class PlayerLanza : MonoBehaviour {
	public float minForce = 10f;
	public float maxForce = 20f;
	public float tiempoCarga = 0.75f;
	public float fuerza = 0f;
	public float chargeSpeed;
	public PlayerControls pj;
	public Slider sliderCarga;
	public bool atLejos = true;
	public string btnTocado = "";
	//direccion y posicion de objetos
	public Transform Direccion; //direccion para disparar el objeto
	public Transform Posicion;	//posicion donde colocara la barrera
	// Use this for initialization
	void Start () {
		pj = gameObject.GetComponent<PlayerControls> ();
		chargeSpeed = (maxForce - minForce) / tiempoCarga;

	}
	
	// Update is called once per frame
	void Update () {
		if (atLejos) {
			string opcion = "";
			bool disparo = false;
			if (Input.GetButtonDown ("ThrowM") || CnInputManager.GetAxis("ThrowM") == 1) {//conocer el boton tocado
				opcion = "a";
			} else if (Input.GetButtonDown ("ThrowL") || CnInputManager.GetAxis("ThrowL") == 1) {
				opcion = "b";
			} else if (Input.GetButtonDown ("Rock") ||CnInputManager.GetAxis("Rock") == 1) {
				opcion = "c";
			} else if (Input.GetButtonUp ("Fire1") || Input.GetButtonUp ("Fire2") || Input.GetButtonUp ("Fire3")) {
				opcion = "x";
			}

			//CUANDO CARGUE DICHO OBJETO
			if (fuerza >= maxForce && atLejos) {//llegamos a la energia lanzamiento maximo
				fuerza = maxForce;
				disparo = true;
			} else if (btnTocado == "") {//hemos comenzado a cargar
				atLejos = true;
				fuerza = minForce;
				btnTocado = opcion;
			} else if (opcion == btnTocado && atLejos) {//estamos presionando
				fuerza += tiempoCarga * Time.deltaTime;
				sliderCarga.value = fuerza;
			} else if (opcion != btnTocado && atLejos) {//soltamos el boton de carga
				disparo = true;
			}

			//si se acepta el disparo
			if (disparo) {
				switch (btnTocado) {
				case "a":
					lanzar (pj.molotov);
					pj.anim.SetTrigger("Throw");
					break;
				case "b":
					lanzar (pj.lacrimogena);
					pj.anim.SetTrigger("Throw");
					break;
				case "c":
					LanzaPiedra ();
					pj.anim.SetTrigger("Throw");
					break;
				}
				atLejos = false;
				fuerza = minForce;
				StartCoroutine (retraso (3, 2f));
				sliderCarga.value = minForce;
				btnTocado = "";
			}
		}
	}

	public void lanzar(GameObject obj){
		GameObject objeto = Instantiate (obj, Direccion.position, Direccion.rotation) as GameObject;
		Rigidbody rigi = objeto.GetComponent<Rigidbody> ();
		rigi.velocity = Direccion.forward * fuerza;
	}

	public void LanzaPiedra(){
		GameObject objeto = Instantiate (pj.piedra, Direccion.position, Direccion.rotation) as GameObject;
		Rigidbody rigi = objeto.GetComponent<Rigidbody> ();
		rigi.velocity = gameObject.transform.forward * fuerza * 2f;
	}

	//delay entre acciones
	IEnumerator retraso(int o, float t){
		if (o == 1) {	//recibe dano por estar en el fuego
			yield return new WaitForSeconds (t);
		} else if (o == 2) {

			yield return new WaitForSeconds (t);
		} else if (o == 3) { //retraso para lanzamiento de objetos
			print("3 segundos");
			yield return new WaitForSeconds (t);
			atLejos = true;
		}


	}
}
