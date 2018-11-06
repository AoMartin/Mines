using UnityEngine;

public class Lentes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Llama al delegate para que las minas subscritas a el 
            // se vuelvan visibles para el jugador
            Run_Control.RC.MostarMinas();
            gameObject.SetActive(false);
        }
    }
}