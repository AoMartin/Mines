using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Control : MonoBehaviour {

    public static Run_Control RC;
    // Las minas se suscriben a este evento y cuando el jugador
    // agarra el item "Lentes" las minas se hacen visibles
    public delegate void ActivarLentes();
    public ActivarLentes MostarMinas;

    Animator anim;
    int animHash_run;
    int animHash_fall;
    int animHash_die;

    Rigidbody rb;
    BarraVida vida;

    bool control;
    float x, z;
    Vector3 movement;
    [SerializeField] float speed = 3.0f;
    //[SerializeField] float rot_speed = 100;

	void Awake () {
        if(RC == null)
            RC = this;

        anim = GetComponent<Animator>();
        animHash_run  = Animator.StringToHash("run");
        animHash_fall = Animator.StringToHash("fall");
        animHash_die  = Animator.StringToHash("die");

        rb = GetComponent<Rigidbody>();
        vida = GetComponent<BarraVida>();
        control = true;
	}

    private void Update()
    {
        if (control)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            // *ANIMAR*
            if (!Mathf.Approximately(z, 0) || !Mathf.Approximately(x, 0))
                anim.SetBool(animHash_run, true);
            else
                anim.SetBool(animHash_run, false);
        }
    }

    private void FixedUpdate()
    {
        if (control)
        {
            // *MOVER*
            movement = new Vector3(x * speed * Time.fixedDeltaTime, 0f, z * speed * Time.fixedDeltaTime); 
            rb.MovePosition(transform.position + movement);

            // *ROTAR*
            if (movement != Vector3.zero) rb.rotation = Quaternion.LookRotation(movement);
        }
    }

    // Es llamado cuando agarra una pastilla curativa o cuando recibe daño de una mina
    // la diferencia se determina por medio del bool -curar-
    public void ManejarVida(bool curar, float cantidad)
    {
        if (curar)
            vida.Curar(cantidad);
        else
            vida.RecibirDaño(cantidad);
    }

    // Cuando esta cerca de una mina que esta explotando
    // el personaje cae al piso y le quita el control al jugador
    // Esta funcion es llamada por la barra de vida si el jugador no muere
    // a causa de la explosion
    public void Caer()
    {
        control = false;
        anim.SetTrigger(animHash_fall);
    }

    // Animation Event -
    // Cuando termina la animacion de 'lenvantarse' devuelve el control al jugador
    public void Levantarse()
    {
        control = true;
    }

    // Esta funcion es llamada por la barra de vida si el calculo del daño 
    // resulta que el jugador se queda sin vida
    public void Morir()
    {
        control = false;
        anim.SetTrigger(animHash_die);
    }
}
