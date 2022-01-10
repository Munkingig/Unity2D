using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie3 : MonoBehaviour
{
    public Transform John;//Referencia al PJ,
    public GameObject pj;//Se accede a los objetos del jugador.
    private JohnMovement pjScript;//Para guardar el acceso al Script JohnMovement que forma parte de los scripts del Jugador.
    private Animator Animator; //Contendra las animaciones del Zombie.
    private Rigidbody2D Rigidbody2D;//Contiene las fisicas del Zombie.
    //Necesario para que el Zombie persiga al Jugador y lo ataque, entre y salga del modo patrulla
    private float lastAtack, tiempo, direccion, speed, distance, distanceAtack, distanceRun, distanceMax, tiempoEspera, velocidadPatrulla;
    private bool mover, atacar, colisionJohn, ataqueFin, hit, on;
    private int health;//Vida del Zombie.
    public Collider2D col;//Collider en modo Trigger.
    private Collider2D collision;//Detectar colisiones por parte del Zombie.
    //Necesario para el modo Patrullaje.
    public float minX, maxX;
	private GameObject _LugarObjetivo;
    private IEnumerator corrutina;
    public GameObject sonidoHurt;
    public GameObject sonidoDead;

    void Start()
    {
        health = 3;
        collision = GetComponent<Collider2D>();
        Rigidbody2D = GetComponent<Rigidbody2D>();//Se obtiene el componente referente a las fisicas.
        Animator = GetComponent<Animator>();
        tiempoEspera = 2f;
        speed = 0.0f;
        velocidadPatrulla = 0.15f;
        distanceAtack = 0.159f;
        distanceRun = 0.8f;
        distanceMax = 1.6f;
        tiempo = 0.1f;
        atacar = false;
        ataqueFin = false;
        hit = false;
        colisionJohn = false;
        mover = true;
        pjScript = pj.gameObject.GetComponent<JohnMovement>();
        UpdateObjetivo();
        on = true;
    }

    void Update()
    {
        //El zombie3 entra o sale en modo patrullaje si es capaz de obervar al jugador.
        Vector3 direction = John.position - transform.position;
        distance = Mathf.Abs(John.position.x - transform.position.x);
        if(distance > distanceMax && on){
            mover = false;
            on = false;
            corrutina = Patrullar();
		    StartCoroutine(corrutina);
        }else if(distance <= distanceMax && on == false && false == Animator.GetBool("hurt") && false == Animator.GetBool("dead")){//Cancelar Patrullaje.
            mover = true;
            on = true;
            StopCoroutine(corrutina);
            Animator.SetBool("caminar", false);
        }
        //El zombie ha salido del modo patrullaje y se dirije hacia al jugador a atacarle, este variara su velocidad dependiendo de la distancia hacia el jugador.
        if(mover){
            if (direction.x >= 0.0f){
                transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                direccion = 1.0f;
            }
            else{
                transform.localScale = new Vector3(-0.07f, 0.07f, 0.07f);
                direccion = -1.0f;
            }

            if(distance < distanceRun && distance > distanceAtack && distance <= distanceMax){
                speed = 0.50f;
            } else if(distance <= distanceMax && distance > distanceAtack && distance <= distanceMax){
                speed = 0.15f;
            }else if(distance > distanceMax || distance <= distanceAtack ){
                speed = 0.0f;
            }

            Animator.SetBool("run", distance <= distanceRun && distance > distanceAtack);
            Animator.SetBool("caminar", distanceRun < distance && distanceMax > distance);
            if(permitirMovimiento()){
                Rigidbody2D.velocity = new Vector2(speed*direccion, Rigidbody2D.velocity.y);
            }
            if(distance <= distanceAtack && Time.time > lastAtack + tiempo)//si la distancia es menor a 1 y a pasado el tiempo del anterior disparo. Se dispara.
            {   lastAtack = Time.time;//Se guarda el momento del ataque.
                atacar = true;
                tiempo = 3.25f;
                ataqueFin = false;
            }
            if(atacar){
                Animator.SetBool("atack", true);
                atacar = false;
            }
            if(ataqueFin){
                hit = false;
                Animator.SetBool("atack", false);  
            }
        }
    }

     //Bloque el movimiento de el Zombie3 si se cumple la condicion para mejor control sobre las Animaciones.
    private bool permitirMovimiento(){
        if(Animator.GetBool("atack") || Animator.GetBool("hurt") || Animator.GetBool("dead")){
                return false;
            }
        else{
            return true;
        }
    }

    //Permite que Zombie3 pueda traspasar a los demas enemigos.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Grunt" ){
            Physics2D.IgnoreCollision(collision, col);
        }
        else if(collision.gameObject.tag == "Zombie3" ){
            Physics2D.IgnoreCollision(collision, col);
        }
    }

    //Daño al Zombie3.
    public void Hit()
    {   mover = false;
        health -= 1;
        Animator.SetBool("hurt", true);
        Animator.SetBool("caminar", false);
        Animator.SetBool("run", false);
        Animator.SetBool("atack", false);
        if (health == 0){
            Animator.SetBool("dead", true);
        }
    }
    public void FinHit(){
        Animator.SetBool("hurt", false);
    }
    public void FinDead(){
        Animator.SetBool("dead", false);
        Destroy(gameObject);//Se destruye la instancia del Zombie2.
    }
    //Daño al Jugador.
    public void HitJohn(){
        distance = Mathf.Abs(John.position.x - transform.position.x);
        if(distance <= distanceAtack &&  Mathf.Abs(John.position.y + 0.28f) < Mathf.Abs(transform.position.y + 0.28f)){
            pjScript.Hit();
        }
        ataqueFin = true;
        hit = true;
    }

    //Funciones necesarias para el modo Patrullaje.
    private void UpdateObjetivo()
	{
		// Si es la primera vez iniciar el patrullaje para la izquierda
		if (_LugarObjetivo == null) {
			_LugarObjetivo = new GameObject("Sitio_objetivo");
			_LugarObjetivo.transform.position = new Vector2(minX, transform.position.y);
			transform.localScale = new Vector3(-0.07f, 0.07f, 0.07f);
			return;
		}
		// iniciar el patrullaje para la derecha
		if (_LugarObjetivo.transform.position.x == minX) {
			_LugarObjetivo.transform.position = new Vector2(maxX, transform.position.y);
			transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
		}
		// Cambio de sentido de derecha a izquierda
		else if (_LugarObjetivo.transform.position.x == maxX) {
			_LugarObjetivo.transform.position = new Vector2(minX, transform.position.y);
			transform.localScale = new Vector3(-0.07f, 0.07f, 0.07f);
		}
	}
	private IEnumerator Patrullar()
	{
        Animator.SetBool("atack", false);
        UpdateObjetivo();
		while(Vector2.Distance(transform.position, _LugarObjetivo.transform.position) > 0.05f) {
			// Se desplazará hasta el sitio objetivo
			Vector2 direction = _LugarObjetivo.transform.position - transform.position;
			float xDirection = direction.x;
            Animator.SetBool("caminar", true);
			transform.Translate(direction.normalized * velocidadPatrulla * Time.deltaTime);
			yield return null;
		}

		// En este punto, se alcanzó el objetivo, se establece nuestra posición en la del objetivo.
		transform.position = new Vector2(_LugarObjetivo.transform.position.x, transform.position.y);
		// Esperamos un momento antes de volver a movernos
        Animator.SetBool("caminar", false);
		yield return new WaitForSeconds(tiempoEspera);
        on = true;
	}
}