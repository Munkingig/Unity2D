using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie2 : MonoBehaviour
{
    public Transform John;//Referencia al PJ,
    public GameObject pj;//Se accede a los objetos del jugador.
    private JohnMovement pjScript;//Para guardar el acceso al Script JohnMovement que forma parte de los scripts del Jugador.
    private Animator Animator; //Contendra las animaciones del Zombie.
    private Rigidbody2D Rigidbody2D;//Contiene las fisicas del Zombie.
    //Necesario para que el Zombie persiga al Jugador y lo ataque, entre y salga del modo patrulla
    private float lastAtack, tiempo, direccion, speed, distance, distanceAtack, distanceRun, distanceMax, tiempoEspera, velocidadPatrulla;
    private bool mover, atacar, colisionJohn, ataqueFin, hit, on, placaje;
    private int health;//Vida del Zombie.
    public Collider2D col;//Collider en modo Trigger.
    private Collider2D collision;//Detectar colisiones por parte del Zombie.
    //Necesario para el modo Patrullaje.
    public float minX, maxX;
	private GameObject _LugarObjetivo;
    private IEnumerator corrutina, corrutina2;
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
        velocidadPatrulla = 0.50f;
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
        //El zombie entra o sale en modo patrullaje si es capaz de obervar al jugador.
        Vector3 direction = John.position - transform.position;//La posicion del pj menos la posicion de grunt es igual al vector poscion al donde tiene que ir el disparo.
        distance = Mathf.Abs(John.position.x - transform.position.x);//La posicion.x del PJ menos la posicion.x de grunt es igual a la distancia.Obtenemos el modulo para evitar negativos.
        if(distance > distanceMax && on && permitirMovimiento()){
            mover = false;
            on = false;
            corrutina = Patrullar();
		    StartCoroutine(corrutina);
        }else if(distance <= distanceMax && on == false && false == Animator.GetBool("hurt") && false == Animator.GetBool("dead")){//Cancelar Patrullaje.
            mover = true;
            on = true;
            StopCoroutine(corrutina);
            Animator.SetBool("walk", false);
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

            if( distance > distanceAtack && distance <= distanceMax ){
                speed = 0.50f;
            }else if( distance <= distanceAtack ){
                speed = 0.0f;
            }

            //Animator.SetBool("run", distance <= distanceRun && distance > distanceAtack);
            Animator.SetBool("walk", distanceAtack < distance && distanceMax > distance);
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

    //Bloque el movimiento de el Zombie1 si se cumple la condicion para mejor control sobre las Animaciones.
    private bool permitirMovimiento(){
        if(Animator.GetBool("atack") || Animator.GetBool("hurt") || Animator.GetBool("dead")){
                return false;
            }
        else{
            return true;
        }
    }

    //Permite que Zombie 1 pueda traspasar a los demas enemigos.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Grunt" ){
            Physics2D.IgnoreCollision(collision, col);
        }
        else if(collision.gameObject.tag == "Zombie2" ){
            Physics2D.IgnoreCollision(collision, col);
        }
        else if(collision.gameObject.tag == "Zombie3" ){
            Physics2D.IgnoreCollision(collision, col);
        }
    }

    //Daño al Zombie1.
    public void Hit()//Funcion de daño.
    {   mover = false;
        health -= 1;//Se le resta 1 de vida a Grunt.
        Animator.SetBool("hurt", true);
        Animator.SetBool("walk", false);
        Animator.SetBool("atack", false);
        Instantiate(sonidoHurt);
        //Animator.SetBool("run", false);
        if (health == 0){//Si la vida es igual a 0
            Instantiate(sonidoDead);
            Animator.SetBool("dead", true);
        }
    }
    public void FinHit(){
        Debug.Log("HIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIITTTTTTTTTTTTTTT");
        Animator.SetBool("hurt", false);
        mover = true;
    }
    public void FinDead(){
        Debug.Log("DEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADDDDDDDDDDDDDDDDDDDDDDDDDDD");
        Animator.SetBool("dead", false);
        Destroy(gameObject);//Se destruye la instancia del objeto Grunt.
    }

    //Daño al Jugador.
    public void HitJohn(){
        distance = Mathf.Abs(John.position.x - transform.position.x);
        if(distance <= distanceAtack &&  Mathf.Abs(John.position.y + 0.28f) < Mathf.Abs(transform.position.y + 0.28f)){
            pjScript.Hit();
            //placaje = false;
            //corrutina2 = Placaje();
		    //StartCoroutine(corrutina2);
        }
        ataqueFin = true;
        hit = true;
    } 
    private IEnumerator Placaje(){
        distance = Mathf.Abs(John.position.x - transform.position.x);
        while(distance < 0.30f){
            if(John.transform.position.x < transform.position.x){
                John.transform.Translate(Vector3.right * -1.50f * Time.deltaTime, Space.World);
            }else{
                John.transform.Translate(Vector3.right * 1.50f  * Time.deltaTime, Space.World);
            }
            yield return null;
        }
        yield return null;
        //placaje = true;
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
            Animator.SetBool("walk", true);
			transform.Translate(direction.normalized * velocidadPatrulla * Time.deltaTime);

			yield return null;
		}

		// En este punto, se alcanzó el objetivo, se establece nuestra posición en la del objetivo.
		transform.position = new Vector2(_LugarObjetivo.transform.position.x, transform.position.y);

		// Esperamos un momento antes de volver a movernos
        Animator.SetBool("walk", false);
		yield return new WaitForSeconds(tiempoEspera);
        on = true;
	}
}
