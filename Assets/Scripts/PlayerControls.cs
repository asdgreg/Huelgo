using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using CnControls;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour {
	//Stats
	public float Vida = 100f;
	public Slider slideVida;
	public Image EfectGolpe;
	public float flashSpeed = 10f;                               
	public Color flashColourLess = new Color(1f, 0f, 0f, 0.1f); 
	public Color flashColourAdd = new Color(0f, 0f, 1f, 0.1f);
	//armas
	public int damage = 20;                  // The damage inflicted by each bullet.
	public GameObject lacrimogena;
	public GameObject molotov;
	public GameObject bate;
	public GameObject tolete;
	public GameObject piedra;
	public GameObject Escudo;
	public GameObject llantas;
	//cantidades
	public int[] consumibles = new int[5]; //0-Piedras, 1-llantas, 2-Molotov, 3-gas, 4-barrera, 5-Gasolina

	//direccion y posicion de objetos
	public Transform Direccion; //direccion para disparar el objeto
	public Transform Posicion;	//posicion donde colocara la barrera
	//tiempos de vida
	public float potencia = 15f;
	public float tempBarrera = 5f;
	//delay
	public float delay = 2f; //son dos segundos
	public bool barrDis = true;	//barrera disponible (true = puede poner)
    public Animator anim;
	//acciones

	public bool atCerca = true;
	public bool atLejos = true;
    //Move Speed SET IN EDITOR
    public float moveSpeed;
	//efectos sobre personaje
	//public status Stats;
	public bool normal = true;
	public bool optFuego = false;

	//****************************************************//
	//Agregado por Douglas para movimiento con el Joystick
	private Vector3 movement;
	public float velocity;
	//***************************************************//


	// Use this for initialization
	void Start () {
        //Get Animator Component
        anim = GetComponent<Animator>();
		//cuerpo = gameObject.GetComponentInChildren<Hitbody>();
		NotificationCenter.DefaultCenter ().AddObserver (this,"bot_dano");
	}
	//notifications
	void bot_dano(Notification noti){
		TakeDamage((int)noti.data);
	}
	//Eventos importantes
	void TakeDamage(int damage){
		Vida -= damage;
		EfectGolpe.color = flashColourLess;
		if (Vida <= 0) {
			print ("MUERE");
			anim.SetTrigger("Die");
			slideVida.value = 0f;
		}

	}
	// Update is called once per frame
	void Update () {
		EfectGolpe.color = Color.Lerp (EfectGolpe.color, Color.clear, flashSpeed * Time.deltaTime);
		slideVida.value = Vida;
        //Sets Block to true if you and pressing RMB, and false if not
		//Bloque para colocar Barrera.
		if (((CnInputManager.GetAxis("Barrier") == 1) && barrDis)){
			barrDis = false;
			StartCoroutine(poner(llantas));
		}


		//Bloque para Defender.
		if (CnInputManager.GetAxis("Defend") == 1)
		{
			anim.SetBool("Block", true);
		}
		else
		{
			anim.SetBool("Block", false);
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Locomotion"))
		{
			//*********************************************************

			movement = new Vector3(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"), 0f);

			//float angulo_giro = Mathf.Rad2Deg * Mathf.Atan2(movement.y, movement.x);
			//Debug.Log(Mathf.FloorToInt(angulo_giro));
			if (Mathf.Abs(movement.x) >= 0.1f || Mathf.Abs(movement.y) >= 0.1f)
			{
				if (anim.GetFloat("Speed") < 1)
					anim.SetFloat("Speed", 1, 0.2f, 2 * Time.deltaTime);

				//Store WASD input in variables for use belowa
				float h = movement.x;
				float v = movement.y;

				//Sets rotation according to WASD input
				transform.forward = new Vector3(h, 0, v);

				//Move player only forward as he is now rotated the right direction
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
			}
			else
			{
				//Smooth Animator float "Speed" from run (1) to idle (0)
				if (anim.GetFloat("Speed") > 0)
					anim.SetFloat("Speed", 0, 0.2f, 2 * Time.deltaTime);
			}

			if (CnInputManager.GetAxis("Punch")== 1)
			{
				anim.SetTrigger("Attack");
				NotificationCenter.DefaultCenter ().PostNotification (this, "pj_hit",damage);
			}
		}
	}

	IEnumerator poner(GameObject obj){
		GameObject objeto = Instantiate (obj,Posicion.position, Posicion.rotation) as GameObject;
		Destroy (objeto, tempBarrera);
		yield return new WaitForSeconds(delay);
		barrDis = true;
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Fuego" && !optFuego) {
			optFuego = true;
			StartCoroutine(ciclo (1.5f));
		}

		if (other.tag == "Gas") {
			normal = false;
			moveSpeed = 5f;
			print ("Entra gas");
		}
	}



	//cuando entre en un collider
	void OnTriggerEnter(Collider other) {


		if (other.tag == "BOT") {
			print (other.gameObject.name);
		}

		switch(other.tag){
		case "oVida":
			print ("Aumento de Vida");
			Vida += 40;
			EfectGolpe.color = flashColourAdd;
			Destroy (other.gameObject);
					break;
		case "oLlanta":
			print ("LLanta recogida");
			consumibles [1]++;
			Destroy (other.gameObject);
					break;
			case "oBomba":
				print ("Bomba Recogida");
			consumibles [2]++;
			Destroy (other.gameObject);		
			break;
		case "oGas":
			print ("Gas Recogido");
			consumibles [3]++;
			Destroy (other.gameObject);		
			break;
		case "oPiedra":
				print ("Piedra Recogida");
			consumibles [0]++;
			Destroy (other.gameObject);	
			break;
		case "oTanque":
			consumibles [5]++;
				print ("Tanque Recogido");
			Destroy (other.gameObject);		
			break;
		}
			
	}

	//cuando salga de un collider
	void OnTriggerExit(Collider other) {
		if (other.tag == "Fuego") {
			optFuego = false;
			print ("Sale Fuego PJ");
		}

		if (other.tag == "Gas") {
			normal = true;
			moveSpeed = 15;
			print ("Sale gas");
		}
	}

	//accion redundante
	IEnumerator ciclo(float espera){
		TakeDamage (10);
		yield return new WaitForSeconds (espera);
		optFuego = false;
	}


	//delay entre acciones
	IEnumerator retraso(int o, float t){
		 if (o == 2) {
			yield return new WaitForSeconds (t);
		} else if (o == 3) { //retraso para lanzamiento de objetos
			
			yield return new WaitForSeconds (t);
			atLejos = true;
		}


	}
		
	//btones
	public void Reset(){
		SceneManager.LoadScene(0);
	}

	public void dos(){
		SceneManager.LoadScene(1);
	}
	public void tres(){
		SceneManager.LoadScene(2);
	}

}