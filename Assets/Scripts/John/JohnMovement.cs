using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Libreriia necesaria para acceder a componentes.

public class JohnMovement : MonoBehaviour
{
    public GameObject BulletPrefab;//Instancia de la clase bala.
    private Rigidbody2D Rigidbody2D;//Contiene las fisicas.
    private Animator Animator; //Contendra las animaciones del PJ.
    private float LastShoot;//Tiempo en el que se realizo el ultimo disparo.
    private int Health = 1000000;//Cantidad de vida.
    private int AfterHealt = 100;
    private float tiempo;
    private int disparos;
    BarraVida lifeBar; //Barra de vida.
    private int _DownRunParameter = Animator.StringToHash("downRun");
    private int _HurtParameter = Animator.StringToHash("hurt");
    private float hurt = 1.0f;
    public AudioClip Sound;
    public GameObject sonidoHurt;
    public GameObject sonidoHurt1;
    public GameObject sonidoHurt2;
    public GameObject sonidoHurt3;
    public GameObject sonidoDead;
    private GameObject[] coleccion;
    private SpriteRenderer spr;

    private void Start()//Metodo para inicializar las variables.
    {
        tiempo = 0.10f;
        disparos = 0;
        lifeBar = GameObject.Find("Slider").GetComponent<BarraVida>();
        Animator = GetComponent<Animator>();
        coleccion = new GameObject[4];
        coleccion[0] = sonidoHurt; 
        coleccion[1] = sonidoHurt1;
        coleccion[2] = sonidoHurt2;
        coleccion[3] = sonidoHurt3;
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update(){

        //Horizontal = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space)){

            if (Time.time > LastShoot + tiempo)//si la distancia es menor a 1 y a pasado el tiempo del anterior disparo. Se dispara.
            {
            if(disparos == 3){
                disparos = 0;
                tiempo = 3.25f;
            }else{
                tiempo = 0.10f;
            }
            Shoot();//se llama a la funcion disparar.
            LastShoot = Time.time;//Se guarda el momento en el que se disparo.
            }
        }
    }


    private void Shoot()//Metodo para disparar.
    {
        Vector3 direction;//Guarda la direccion donde tiene que ir la bala.
        if (transform.localScale.x == 1.0f) direction = Vector3.right;// si el PJ mira a la derecha "direccion" es igual a derecha.
        else direction = Vector3.left;// si el PJ mira a la izquierda "direccion" es igual a izquierda.

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);//Se crea el objeto bala con una distacia al centro del PJ y con la dirrecion correcta. Sin ninguna rotacion.
        bullet.GetComponent<BulletScript>().SetDirection(direction);//Se llama a la funcion setDireccion del script "BulletScropt".
        disparos++;
    }

    public void Hit()//Metodo para calcular el daño recibido.
    {
        AfterHealt = Health;
        Health -= 25;//Se pierden puntos de vida.
        Instantiate(coleccion[Random.Range(0,3)]);
        spr.color = Color.red;
        //Instantiate(sonidoHurt);
        setAnimation();
        lifeBar.GetComponent<BarraVida>().vida = Health;//Se actualiza la vida de la barra de vida.
        if (Health <= 0){
            Instantiate(sonidoDead);
            Destroy(gameObject);
        }//Si la vida es 0 el PJ desaparece del escenario.
    }

    public void setAnimation(){
        Animator.SetBool(_HurtParameter, true);
    }

    public void setAnimationOff(){
        spr.color = Color.white;
        Animator.SetBool(_HurtParameter, false);
    }

}