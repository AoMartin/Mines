using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    bool  vivo;
    float vida_actual;
    [SerializeField] float  vida_inicial = 100f;

    [SerializeField] Slider slider;                            // La barra de vida
    [SerializeField] Image  slider_fill;                       // La imagen que grafica la vida y cambiara de color segun el valor 
    [SerializeField] Color  color_vidafull = Color.green;      // Color para representar que la vida esta llena
    [SerializeField] Color  color_vidacero = Color.red;        // Color para representar que la vida esta vacia

    private void Awake()
    {
        vida_actual = vida_inicial;
        slider.maxValue = vida_actual;
        VidaUI();
    }

    public void RecibirDaño(float daño)
    {
        vida_actual -= daño;
        VidaUI();

        if (vida_actual <= 0f)
            Run_Control.RC.Morir();
        else
            Run_Control.RC.Caer();
    }

    public void Curar(float vida)
    {
        vida_actual += vida;
        VidaUI();
    }

    private void VidaUI()
    {
        slider.value = vida_actual;
        slider_fill.color = Color.Lerp(color_vidacero, color_vidafull, vida_actual / vida_inicial);
    }

}