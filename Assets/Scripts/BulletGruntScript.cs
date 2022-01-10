using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGruntScript : MonoBehaviour
{
   public float Speed;//Inidica la velocidad de la bala.
    public AudioClip Sound;//Indica el sonido de la bala.

    private Rigidbody2D Rigidbody2D;//Indica las fisicas de la bala.
    private Vector3 Direction;//Indica la direccion de la bala.

    private void Start()//Se inicializan las variables.
    {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        JohnMovement john = other.GetComponent<JohnMovement>();
        if (john != null)
        {
            john.Hit();
            DestroyBullet();
        }
    }
}
