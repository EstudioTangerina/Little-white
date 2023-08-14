using RPGTALK.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkScript : MonoBehaviour
{
    public RPGTalk rpgTalk;

    public UnityEvent EndSpeechs, EndSpeech_Wating, EndSpeech_Normal, EndSpeech_Tag, EndSpeech_Happy;

    public GameObject[] Speechs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Speech_Wating()
    {
        Speechs[0].SetActive(true);
        Invoke("End_Speech_Wating", 1.5f);
    }

    public void End_Speech_Wating()
    {
        Speechs[0].SetActive(false);
        Invoke("Speech_Normal", 0.1f);
    }


    public void Speech_Normal()
    {
        rpgTalk.NewTalk("12", "14", rpgTalk.txtToParse, EndSpeech_Normal);
    }

    public void End_Speech_Normal()
    {
        Speechs[1].SetActive(true);
        Invoke("Speech_Tag", 1.5f);
    }

    public void Speech_Tag()
    {
        Speechs[1].SetActive(false);
        rpgTalk.NewTalk("16", "17", rpgTalk.txtToParse, EndSpeech_Tag);
    }

    public void End_Speech_Tag()
    {
        Speechs[2].SetActive(true);
        Invoke("Speech_Happy", 1.5f);
    }

    public void Speech_Happy()
    {
        Speechs[2].SetActive(false);
        rpgTalk.NewTalk("19", "21", rpgTalk.txtToParse, EndSpeech_Happy);
    }

    public void End_Speech_Happy()
    {
        Speechs[3].SetActive(true);
        Invoke("End_Speechs", 1.5f);
    }

    public void End_Speechs()
    {
        Speechs[3].SetActive(false);
        EndSpeechs.Invoke();
    }
}
