using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnRun : MonoBehaviour
{
    public float Speed;//Velocidad de movimiento del PJ.
    // Start is called before the first frame update
    private Rigidbody2D Rigidbody2D;//Contiene las fisicas.
    private Animator Animator; //Contendra las animaciones del PJ

    private int _JumpAnimatorParameter = Animator.StringToHash("running");

    private bool Grounded;//True estamos en el suelo, False no estamos en el suelo.
    private float Horizontal;//[a = -1], [ningun boton pulsado = 0], [d = 1]
    private float Vertical;//[s = -1], [ningun boton pulsado = 0], [w = 1]
    private bool animacion = false;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        LocalInput();
        setAnimation();
    }

    void FixedUpdate(){
        HorizontalMove();
        //Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    protected void LocalInput(){
        Horizontal = Input.GetAxisRaw("Horizontal");
        //Vertical = Input.GetAxisRaw("Vertical");
    }

    protected void HorizontalMove(){
        if(permitirMovimiento()){
            animacion = true;
            //setAnimation();
            if(Horizontal < 0.0f){
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            } else if(Horizontal > 0.0f){
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
        }else{
            animacion = false;
        }
    }

    public bool permitirMovimiento(){
        Grounded = Physics2D.Raycast(transform.position, Vector3.down, 0.1f);
        if(Grounded){
            if(Animator.GetBool("hurt")){
                return false;
            }
        }
        return true;
    }

    public void setAnimation(){
        Animator.SetBool(_JumpAnimatorParameter, Horizontal != 0.0f && animacion == true);
    }
}

