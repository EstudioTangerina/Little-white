using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector director;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // tecla ENTER para avançar
        {
            if (director.state == PlayState.Paused)
            {
                director.Play();
                Debug.Log("Timeline retomada!");
            }
        }
    }
}

