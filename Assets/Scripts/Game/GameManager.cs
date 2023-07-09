using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public float horaAtual;
    public float tempoDia = 720;   // 12 horas
    public float tempoNoite = 720; // 12 horas
    public Light2D luz;              // Referência ao componente de luz
    public Color corDia;           // Cor para o dia
    public Color corNoite;         // Cor para a noite
    public Light2D luzLago;

    void Update()
    {
        horaAtual += Time.deltaTime;
        if (horaAtual > (tempoDia + tempoNoite))
        {
            horaAtual = 0;
        }

        // Alterar cor da luz com base na hora atual
        if (horaAtual < tempoDia)
        {
            float t = horaAtual / tempoDia; // Interpolação linear entre 0 e 1
            luz.color = Color.Lerp(corNoite, corDia, t);
            luzLago.intensity = Mathf.Lerp(0.6f, 0f, t);
        }
        else
        {
            float t = (horaAtual - tempoDia) / tempoNoite; // Interpolação linear entre 0 e 1
            luz.color = Color.Lerp(corDia, corNoite, t);
            luzLago.intensity = Mathf.Lerp(0f, 0.6f, t);
        }
    }
}
