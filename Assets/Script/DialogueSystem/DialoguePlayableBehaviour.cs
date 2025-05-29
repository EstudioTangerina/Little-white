using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayableBehaviour : PlayableBehaviour
{
    public List<string> dialogueLines;
    public float secondsBetweenCharacters;
    public bool waitForInput;

    public Vector2 dialogueBoxPosition; // NOVO

    private DialogueReceiver receiver;
    private PlayableDirector director;
    private bool isPlaying = false;

    public override void OnGraphStart(Playable playable)
    {
        receiver = GameObject.FindObjectOfType<DialogueReceiver>();
        director = GameObject.FindObjectOfType<PlayableDirector>();
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (receiver != null && !isPlaying)
        {
            isPlaying = true;

            if (director != null)
            {
                director.playableGraph.GetRootPlayable(0).SetSpeed(0f);
            }

            receiver.SetDialogueBoxPosition(dialogueBoxPosition); // NOVO
            receiver.StartCoroutine(PlayDialogue(playable));
        }
    }

    private System.Collections.IEnumerator PlayDialogue(Playable playable)
    {
        foreach (string line in dialogueLines)
        {
            yield return receiver.StartCoroutine(receiver.DisplayString(line, secondsBetweenCharacters));

            if (waitForInput)
            {
                yield return receiver.StartCoroutine(receiver.WaitForPlayerInput());
            }
        }

        receiver.ClearDialogue();

        if (director != null)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1f);
        }

        isPlaying = false;
    }
}
