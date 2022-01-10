using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JhonJump : MonoBehaviour
{

    private int JumpAnimatorParameter = Animator.StringToHash("jump");
    private int DownAnimatorParameter = Animator.StringToHash("down");
    private float Vertical;
    private bool Grounded;
    private float JumpForce = 150.0f;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    public GameObject SonidoStep1;
    public GameObject SonidoStep2;
    public GameObject SonidoJump;
    private bool salto, sonido;
    private Collider2D collision;
    


    void Start()
    {
        collision = GetComponent<Collider2D>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Vertical = Input.GetAxisRaw("Vertical");
        salto = false;
    }

    // Update is called once per frame
    void Update()
    {
        LocalInput();

        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if(Physics2D.Raycast(transform.position, Vector3.down, 0.1f)){
            Grounded = true;
        } else{
            Grounded = false;
        }

        if(Input.GetKeyDown(KeyCode.W) && Grounded){
            if(permitirMovimiento()){
                Jump();
            }
        }

        if(Grounded){
            Animator.SetBool(JumpAnimatorParameter, Vertical > 0.0f);
        }
        else if(salto){
            salto = false;
            sonido = true;
        }
        if(sonido){
            Debug.Log("Terminooooo el saltooooooo");
            Instantiate(SonidoStep1);
            Instantiate(SonidoStep2);
            sonido = false;
        }

        if(Vertical < 0 && Input.GetKeyDown(KeyCode.LeftShift)){
            setAnimationDown();
        }
        if(Vertical > 0){
            setAnimationDown();
        }
        
    }


    void LocalInput(){
        Vertical = Input.GetAxisRaw("Vertical");
    }

    void setAnimationDown(){
        Animator.SetBool(DownAnimatorParameter , Vertical < 0.0f);
    }

    public bool permitirMovimiento(){
        Grounded = Physics2D.Raycast(transform.position, Vector3.down, 0.1f);
        if(Grounded){
            if(Animator.GetBool("down") || Animator.GetBool("hurt")){
                return false;
            }
        }
        return true;
    }

    private void Jump(){
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
        salto = true;
        Instantiate(SonidoJump);
        setAnimationJump();
    }

    public void setAnimationJump(){
        Animator.SetBool(JumpAnimatorParameter, Vertical > 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Plattform" ){
            Debug.Log("Sonidooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo");
            Instantiate(SonidoStep1);
        }
    }
}
