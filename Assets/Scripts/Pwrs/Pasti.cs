using UnityEngine;

public class Pasti : MonoBehaviour {

    float vida;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Establece la cantidad de vida que va a curar
            vida = Random.Range(0f, 30f) * Random.Range(1, 4);

            // Llama a la funcion curar del jugador
            Run_Control.RC.ManejarVida(true,vida);
            gameObject.SetActive(false);
        }
    }
}
