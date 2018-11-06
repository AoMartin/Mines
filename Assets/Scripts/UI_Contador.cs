using UnityEngine;
using UnityEngine.UI;

public class UI_Contador : MonoBehaviour {

    [SerializeField] Text tiempo;
    [SerializeField] Animator anim;

    bool play = true;
    [SerializeField] float seg = 99f;

    void Update () {
        if (play)
        {
            seg -= Time.deltaTime;

            if (seg < 10f)          //Agrega el 0 delante de los numeros de un digito
                tiempo.text = "0" + seg.ToString("f2");
            else
                tiempo.text = seg.ToString("f2");

            if (seg < 5) anim.SetTrigger("red"); //Cuando queden menos de 5 seg. el reloj se pone rojo

            if (seg <= 0f)          //Cuando llega a cero. Fin.-
            {
                tiempo.text = "00.00";
                TimeCero();
            }
        }
	}

    void TimeCero()
    {
        play = false;
    }
}
