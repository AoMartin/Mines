using UnityEngine;

public class Radar : MonoBehaviour {

    Transform player;                               // Referencia al jugador
    [SerializeField] Transform guia;                // Se usa de referencia para mediar la distancia entre el radar y el jugador
                                                    // para medir el rango actual del radar al encontrar un mina, se usa esta
                                                    // referencia para calcular facil y rapido el rango del radar sin depender de la escala
    [SerializeField] MeshRenderer exclama;          // El signo de exclamacion aparece sobre la cabeza del personaje cunado hay una mina en rango
    SpriteRenderer sprite;                          // El sprite que grafica el radar
    
    float dist_full;                                // Se usa para calcular la distancia total eficaz entre el jugador y la mina
    float distancia;                                // Distancia relativa al rango actual del radar con respecto a la mina y su zona de influencia
    [SerializeField] float calibracion = 0.5f;      // Se usa para acentuar/atenuar el efecto del Lerp, el cambio de color del radar
    [SerializeField] float rango = 5f;              // Que tan grande es el rango en el que barre el radar
    [SerializeField] float vel = 1f;                // Que tan rapida es la velocidad de barrido 

    Color rojo   = new Color(1f,0f,0f,1f);          // Rojo, cuando haya una mina cercana
    Color blanco = new Color(1f,1f,1f,0.6f);        // Blanco, cuando el camino dentro del rango sea seguro

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;       
    }

    private void Update()
    {
        //Setea la posicion del radar en el jugador con un ligero offset en Y para estar sobre el suelo
        transform.position = new Vector3(player.position.x, player.position.y+0.01f, player.position.z);

        Barrer();
    }

    // Modifica agrandando la escala del radar en funcion del tiempo
    void Barrer()
    {
        transform.localScale += Vector3.one * Time.deltaTime * vel;
        if (transform.localScale.x >= rango * 0.2f)
        {
            transform.localScale = Vector3.zero;
        }
    }

    // Cuando el radar encuentra una mina, osea entra dentro de su rango o zona de influencia
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mina"))
        {
            // Calcula la distancia entre la guia y el jugador, osea el rango actual al momento de encontrar una mina
            distancia = Vector3.Distance(guia.transform.position, player.position);

            // Calcula la distancia enre el origen de la mina y el jugador
            dist_full = Vector3.Distance(other.transform.position,player.position);

            // Para calcular la distancia total eficaz, le resta el rango actual al momento de hacer contacto 
            // con el rango de la mina, calculado primeramente
            dist_full -= distancia - calibracion;

            // Activa la señal de precaucion, el signo de exclamacion sobre el personaje
            exclama.gameObject.SetActive(true);
        }
    }

    // Cuando el radar se acerca mas a la posicion origen de la mina, se vuelve mas rojo
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mina")){

            // Calcula la distancia entre la guia y el jugador, osea el rango actual al momento de encontrar una mina
            distancia = Vector3.Distance(guia.transform.position, player.position);

            // Calcula la distancia actual eficaz entre el rango del radar y la mina
            distancia = Vector3.Distance(other.transform.position,player.position) - distancia - calibracion;

            // Calcula el coeficiente en funcion de la distancia para luego cambiar el color del radar
            distancia = Mathf.InverseLerp( 0f, dist_full, distancia);

            // Cambia el color del radar dependiendo del coefiente de distancia, cuanto mas cerca a una mina, mas rojo
            sprite.color = Color.Lerp(rojo, blanco, distancia);
        }
    }

    // Cuando la mina sale del rango, vuelve a la señal de seguridad
    // Volviendose blanco y desactivando el signo de exclamacion 
    private void OnTriggerExit(Collider other)
    {
        sprite.color = blanco;
        exclama.gameObject.SetActive(false);
    }
}
