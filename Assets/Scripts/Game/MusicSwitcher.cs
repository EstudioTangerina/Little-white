using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    public Animator MusicVillager;
    public Animator MusicForest;
    public AudioSource audioMSCForest;
    public AudioSource audioMSCVillager;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se o objeto que colidiu é o jogador
        {
            audioMSCVillager.Play();
            MusicVillager.SetBool("MusicOn", true);
            MusicForest.SetBool("MusicOff", true);

            MusicForest.SetBool("MusicOn", false);
            MusicVillager.SetBool("MusicOff", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioMSCForest.Play();
            MusicForest.SetBool("MusicOn", true);
            MusicVillager.SetBool("MusicOff", true);

            MusicVillager.SetBool("MusicOn", false);
            MusicForest.SetBool("MusicOff", false);
        }
       
    }
}


