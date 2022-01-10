using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnDown : MonoBehaviour
{
    // Start is called before the first frame update
    private float Vertical;
    public Collider2D col;
    private Collider2D coll;
    private Collider2D colision;
    private Rigidbody2D Rigidbody2D;
    private bool Grounded;
    private float timer;
    private bool startTimer;
    private bool permiso;


    void Start()
    {   
        colision = GetComponent<Collider2D>();
        coll = GetComponent<Collider2D>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Vertical = Input.GetAxisRaw("Vertical");
        Grounded = false;
        timer = 0.3f;
        startTimer = false;
        permiso = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vertical = Input.GetAxisRaw("Vertical");
        if(Rigidbody2D.velocity.y <= 0.3 ){
            if(Physics2D.Raycast(transform.position, Vector3.down, 0.1f)){
                Grounded = true;
            } else{
                Grounded = false;
            }
        }

        if(startTimer){
            timer -= Time.deltaTime;
        }
        if(timer < 0){
            Physics2D.IgnoreCollision(colision, col, false);
            timer = 0.3f;
            startTimer = false;
        }

        if(permiso && Vertical < 0){
            Physics2D.IgnoreCollision(colision, col);
            permiso = false;
            startTimer = true;
        }

    }

    void OnTriggerEnter2D(Collider2D coll){
        if(Vertical < 0){
                if(coll.gameObject.tag == "Plattform" && Grounded){
                    Physics2D.IgnoreCollision(coll, col);
                    startTimer = true;
                    colision = coll;
                    permiso = false;
                }
        }else if(coll.gameObject.tag == "Plattform"){
                    colision = coll;
                    permiso = true;
        }else{
            permiso = false;
        }
        
    }

    void OnTriggerStay2D(Collider2D coll){
        if(Vertical < 0){
                if(coll.gameObject.tag == "Plattform" && Grounded){
                    Physics2D.IgnoreCollision(coll, col);
                    startTimer = true;
                    colision = coll;
                    permiso = false;
                }
        }else if(coll.gameObject.tag == "Plattform"){
                    colision = coll;
                    permiso = true;
        }else{
            permiso = false;
        }
    } 

}

