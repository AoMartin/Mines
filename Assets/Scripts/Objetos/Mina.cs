using System.Collections;
using UnityEngine;

public class Mina : MonoBehaviour {

    Animator anim;
    int animHash_boom;
    int animHash_visible;

    SphereCollider col;
    SpriteRenderer sprite;
    [SerializeField] ParticleSystem explot;

    [SerializeField] float rango = 1f;      // El rango de explosion de la mina, tambien es el rango de deteccion del jugador
    [SerializeField] float vel = 1f;        // La velocidad con la que avisa de la explosion, setea la velocidad del Animator
    float daño;                     
    bool  boom = false;                     // Cuando esta en true significa que la mina esta explotando
    float explosionTime = 1.0f;             // El tiempo que dura la explosion, 1 segundo

    private void Awake()
    { 
        anim = GetComponent<Animator>();
        animHash_boom    = Animator.StringToHash("boom");
        animHash_visible = Animator.StringToHash("visible");

        col = GetComponent<SphereCollider>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        sprite.gameObject.transform.localScale = Vector3.one * rango * 0.005f;
        col.radius = rango * 0.025f;

        anim.speed = vel;
    }

    private void Start()
    {
        // Suscribe al delegate del jugador para que cuando este agarre el item "Lentes"
        // las minas se hagan visibles
        Run_Control.RC.MostarMinas += Visible;
    }

    // Si el jugador entra en el rango de la mina, esta se prepara para explotar
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !boom)
        {
            anim.SetTrigger(animHash_boom);
        }
    }

    // Animation Event -
    // Cuando termina la animacion que anticipa la explosion
    // la mina explota y activa las particulas de humo y fuego
    public void Boom()
    {
        boom = true;
        StartCoroutine(Explotando());
        explot.Play();
    }

    // La explosion dura 1 segundo, si el jugador entra en el rango de la mina
    // mientras 'esta explotando' recibira daño. 
    IEnumerator Explotando()
    {
        if (explosionTime > 0.0f)
        {
            explosionTime -= Time.deltaTime;
            yield return null;
        }
        boom = false;
        explosionTime = 1.0f;
    }

    // Si el jugador esta dentro del rango mientras la mina esta explotando
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && boom)
        {
            // Calcula la distancia entre el origen de la mina, y la posicion del jugador
            daño = Vector3.Distance(transform.position,other.transform.position);

            // Usa la distancia para calcular el daño, cuanto mas cerca esta el jugador del centro
            // de la explosion mayor es el daño recibido, ademas le suma un plus de daño aleatorio
            daño = Mathf.InverseLerp( rango, 0f, daño) + Random.Range(0.3f,0.55f);

            // Cuantifica el daño
            daño *= 100.0f;

            // Le avisa al jugador que esta recibiendo daño
            Run_Control.RC.ManejarVida(false,daño);
            boom = false;
        }
    }

    // Este es el evento que se activa con el delegate del jugador cuando agarra los Lentes
    public void Visible()
    {
        anim.SetBool(animHash_visible, true);
    }
    public void Invisible() // <----- NO IMPLEMENTADO !!!
    {
        anim.SetBool(animHash_visible, false);
    }
}
