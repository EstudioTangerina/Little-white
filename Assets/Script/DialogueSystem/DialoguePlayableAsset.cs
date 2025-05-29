using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePlayableAsset : PlayableAsset
{
    public List<string> dialogueLines = new List<string>();
    public float secondsBetweenCharacters = 0.05f;
    public bool waitForInput = true;

    public Vector2 dialogueBoxPosition = new Vector2(0, 0); // NOVO

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialoguePlayableBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.dialogueLines = dialogueLines;
        behaviour.secondsBetweenCharacters = secondsBetweenCharacters;
        behaviour.waitForInput = waitForInput;
        behaviour.dialogueBoxPosition = dialogueBoxPosition; // NOVO

        return playable;
    }
}
