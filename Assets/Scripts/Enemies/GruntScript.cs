using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntScript : MonoBehaviour
{
    public Transform John;
    
    public GameObject BulletPrefab;
    private Animator Animator; 
    private Rigidbody2D Rigidbody2D;
    private int disparos;
    private float tiempo;
    public Transform PitCheck, WallCheck, GroundCheck, JumpCheck;
    public bool walldetected, pitdetected, isGround, Grounded, jumpdetected;
    public float detectionRadius;
    public LayerMask whatisGround;
    private float distance;
    private bool caminar, saltar, avanzar;

    private int JumpGruntAnimatorParameter = Animator.StringToHash("jumpGrunt");
    private float Speed;
    private float Direccion;

    private int Health = 3;
    private float LastShoot;
    public Collider2D col;
    private Collider2D collision;

    private void Start(){
        Speed = 0.25f;
        tiempo = 0.25f;
        disparos = 0;
        detectionRadius = 0.1f;
        Rigidbody2D = GetComponent<Rigidbody2D>();//Se obtiene el componente referente a las fisicas.
        Animator = GetComponent<Animator>();
        collision = GetComponent<Collider2D>();
        caminar = true;
        isGround = false;
        avanzar = false;
    }

    void Update()
    {
        pitdetected = !Physics2D.OverlapCircle(PitCheck.position, detectionRadius, whatisGround);
        jumpdetected = Physics2D.OverlapCircle(JumpCheck.position, detectionRadius, whatisGround);
        isGround = Physics2D.OverlapCircle(JumpCheck.position, detectionRadius, whatisGround);

        if (John == null) return;//Si el PJ no existe no hacemos nada.

        if(pitdetected){
            caminar = false;
            }
        else{
            caminar = true;
        }
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if(Physics2D.Raycast(transform.position, Vector3.down, 0.1f)){
            Grounded = true;
        }else{
            Grounded = false;
        }

        if(caminar == false && isGround && jumpdetected && Grounded){
            saltar = true;
            avanzar = true;
        }else{
            saltar = false;
        }

        if(avanzar){
            if(Direccion < 0){
                Direccion = Mathf.Abs(Direccion) + 0.02f;
                Direccion = (Direccion * (-1.0f));
            }
            else{
                Direccion = Direccion + 0.02f;
            }
           
            if(isGround && caminar && Grounded){
                avanzar = false;
            }
            Rigidbody2D.velocity = new Vector2(Direccion * Speed, Rigidbody2D.velocity.y);
            return;
        }

        if(caminar){
            Vector3 direction = John.position - transform.position;//La posicion del pj menos la posicion de grunt es igual al vector poscion al donde tiene que ir el disparo.
            if (direction.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);//Si la direccion es 1 se apunta a la derecha.
            else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);//Si la direccion es -1 se apunta a la izquierda.
            Direccion = direction.x;
            distance = Mathf.Abs(John.position.x - transform.position.x);//La posicion.x del PJ menos la posicion.x de grunt es igual a la distancia.Obtenemos el modulo para evitar negativos.
            Animator.SetBool("runing", distance > 1.0f && distance < 1.5f);//Si el PJ se encuentra en movimiento se activa la animacion de correr.
            if(distance > 1.0f && distance < 1.5f){
                Rigidbody2D.velocity = new Vector2(Direccion * Speed, Rigidbody2D.velocity.y);
            }
        }
        
        if (distance <= 1.0f && Time.time > LastShoot + tiempo)//si la distancia es menor a 1 y a pasado el tiempo del anterior disparo. Se dispara.
            {
            if(disparos == 3){
                disparos = 0;
                tiempo = 3.25f;
            }else{
                tiempo = 0.25f;
            }
            Shoot();//se llama a la funcion disparar.
            LastShoot = Time.time;//Se guarda el momento en el que se disparo.
            }
       
    }
    void FixedUpdate(){
        if(saltar){
            Jump();
        }
    }

    private void Jump(){
        Rigidbody2D.AddForce(Vector2.up * 2200.0f);
    }


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

    private void Shoot()//Funcionb de disparo.
    {

        Vector3 direction = new Vector3(transform.localScale.x, 0.0f, 0.0f);//Se guarda la direccion a la que tiene que ir la bala.
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);//se crea una instancia de bala, con al direccion guardada anteriormente y sin rotacion.
        bullet.GetComponent<BulletGruntScript>().SetDirection(direction);//se ejecuta el script que maneja la bala.
        disparos++;
    }

    public void Hit()//Funcion de daño.
    {
        Health -= 1;//Se le resta 1 de vida a Grunt.
        if (Health == 0) Destroy(gameObject);//Si la vida es igual a 0 entonces se destruye la instancia del objeto Grunt.
    }
}
