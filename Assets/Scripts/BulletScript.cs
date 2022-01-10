using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed;//Inidica la velocidad de la bala.
    public AudioClip Sound;//Indica el sonido de la bala.

    private Rigidbody2D Rigidbody2D;//Indica las fisicas de la bala.
    private Vector3 Direction;//Indica la direccion de la bala.
    private bool parar;

    private void Start()//Se inicializan las variables.
    {
        parar = false;
        Rigidbody2D = GetComponent<Rigidbody2D>();//Se accede al componente RigidBody.
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    private void FixedUpdate()//Metodo para actualizar las fisicas.
    {
        Rigidbody2D.velocity = Direction * Speed;//Se modifica la velocidad de las fisicas.
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;//Se obtiene la direccion de la bala.
    }

    public void DestroyBullet()//Funcion para destruir la bala, esta funcion es llamada por el ultimo sprite de la animacion de la bala.
    {
        Destroy(gameObject);//Se destruye el objeto, en este caso la bala.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GruntScript grunt = collision.GetComponent<GruntScript>();
        Zombie1 zombie1 = collision.GetComponent<Zombie1>();
        Zombie2 zombie2 = collision.GetComponent<Zombie2>();
        Zombie3 zombie3 = collision.GetComponent<Zombie3>();
        if(collision.gameObject.tag == "Grunt" &&  grunt != null && parar == false){
            parar = true;
            grunt.Hit();
            DestroyBullet();
        }
        if(collision.gameObject.tag == "Zombie1" && zombie1 != null && parar == false){
            parar = true;
            zombie1.Hit();
            DestroyBullet();
        }
        if(collision.gameObject.tag == "Zombie2" && zombie2 != null && parar == false){
            parar = true;
            zombie2.Hit();
            DestroyBullet();
        }
        if(collision.gameObject.tag == "Zombie3" && zombie3 != null && parar == false){
            parar = true;
            zombie3.Hit();
            DestroyBullet();
        }
               
    }
}
