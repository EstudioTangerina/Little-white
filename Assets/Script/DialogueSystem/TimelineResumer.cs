using UnityEngine;
using UnityEngine.Playables;

public class TimelineResumer : MonoBehaviour
{
    public PlayableDirector director;

    void Update()
    {
        if (director.state == PlayState.Paused && Input.GetKeyDown(KeyCode.Return))
        {
            director.Play();
            Debug.Log("Timeline retomada após input!");
        }
    }
}
