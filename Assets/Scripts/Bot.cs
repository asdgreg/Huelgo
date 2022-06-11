using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Bot : MonoBehaviour {

	public Transform player; //Transform del Player principal
	public int Vida = 100; //Vida actual del Enemigo
	public Slider slideVida;
	public float cuerpoMuerto = 2.5f;
	public int puntajeRecibir = 10; 
	NavMeshAgent nav;	//Navegacion
	Animator anim;		//animator del personaje
	//colliders	
	Vector3 distancia;
	public Transform Direccion;	//direccion en que se lanzan los objetos a distancia
	//armas
	public GameObject Molotov;	//arma molotov
	public GameObject Gas;		//arma Gas de humo
	public float potencia = 10f;	//potencia que se lanza el objeto
	public bool atCerca = true;		//permiso de atacar de cerca TRUE puede accionar
	public bool atLejos = true;		//permiso de atacar de lejos TRUE puede accionar
	public float delay = 1f;		//tiempo de espera entre ataques

	//efectos sobre personaje
	//public status Stats;
	//bool muerto = false;
	public bool normal = true;		//efectos normales estando FUERA DE GAS
	private bool optFuego = false;	//efectos estando SOBRE FUEGO

	// Use this for initialization
	void Start(){
		anim = GetComponent<Animator>();		//obtiene la animacion propia
		NotificationCenter.DefaultCenter ().AddObserver (this,"pj_hit");	//activa un LISTENER cuando el Enemigo recibe danio
		player = GameObject.FindGameObjectWithTag("Player").transform; //busca la posicion del jugador y lo almacena
		nav = GetComponent<NavMeshAgent> ();		//obtiene la navegacion propia del enemigo
	}

	//listener
	void pj_hit(Notification noti){	//funcion ejecutada al recibir algo el LISTENER
		if (distancia.magnitude <=5) {
			TakeDamage((int)noti.data);
		}
	}
	//EVENTOS IMPORTANTES
	void TakeDamage(int damage){
		Vida -= damage;
		if (Vida <= 0) {
			print ("MUERE BOT");
			anim.SetTrigger ("Die");
			slideVida.value = 0f;
		}

	}

	// Update is called once per frame
	void Update () {

		distancia = player.position - transform.transform.position;
		if (Vida >= 1) {
			slideVida.value = Vida;
			anim.SetFloat ("Speed", 1, 0.2f, 2 * Time.deltaTime);
			nav.SetDestination (player.position);
			if (distancia.magnitude <= 25 && distancia.magnitude >= 15 && atLejos) {
				lanzar (Molotov);
				atLejos = false;
				//StartCoroutine (espera (1, delay));
			}
			if (distancia.magnitude < 5f && atCerca) {
				atCerca = false;
				anim.SetTrigger ("Attack");
				NotificationCenter.DefaultCenter ().PostNotification (this, "bot_dano", 5);
				StartCoroutine (espera (2, 1f));
			}
		} else {
			nav.enabled = false;
		}
	}

	//script para lanzar objetos
	public void lanzar(GameObject obj){
		GameObject objeto = Instantiate (obj, Direccion.position, Direccion.rotation) as GameObject;
		Rigidbody rigi = objeto.GetComponent<Rigidbody> ();
		rigi.velocity = Direccion.forward * potencia;

	}
	//funcion que crea un Delay en eventos
	IEnumerator espera(int a, float t){
		yield return new WaitForSeconds(t);
		if (a == 1) {
			atLejos = true;
		} else {
			atCerca = true;
		}
	}

	//cuando entre en un collider
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Fuego" && !optFuego) {
			optFuego = true;
			StartCoroutine(ciclo (1f));
			print ("Entra fuego BOT");
		}

		if (other.tag == "Gas") {
			normal = false;
			nav.acceleration = 5f;
			atCerca = false;
			print ("Entra gas");
		}
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Fuego" && !optFuego) {
			optFuego = true;
			StartCoroutine(ciclo (1.5f));
		}

		if (other.tag == "Gas") {
			normal = false;
			//moveSpeed = 5f;
			atCerca = false;
			print ("Entra gas");
		}
	}

	//cuando salga de un collider
	void OnTriggerExit(Collider other) {
		if (other.tag == "Fuego") {
			optFuego = false;
			print ("Sale Fuego");
		}

		if (other.tag == "Gas") {
			normal = true;
			nav.acceleration = 15f;
			atCerca = true;
			print ("Sale gas");
		}
	}


	//accion redundante
	IEnumerator ciclo(float espera){
		TakeDamage (10);
		yield return new WaitForSeconds (espera);
		optFuego = false;
	}

	//delay estando sobre algun tipo de efecto
	IEnumerator danos(int o, float t){
		if (o == 2) {
			yield return new WaitForSeconds (t);
		} else if (o == 3) {

		}
	}



}
