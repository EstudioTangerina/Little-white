using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class EndDialogue : MonoBehaviour
{
    public bool fix = false;
    public PlayableDirector director;
    public GameObject player;
    public GameObject TalkArea;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing && !fix && TalkArea.active == false)
        {
            fix = true;
            player.GetComponent<PlayerMovement>().enabled = true;
        }
    }
}
