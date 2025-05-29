using UnityEngine;
using UnityEngine.Playables;
using TMPro;

[System.Serializable]
public class MensagePlayableAsset : PlayableAsset
{
    public MensageBehaviour template = new MensageBehaviour();

    public ExposedReference<TextMeshProUGUI> textObject;
    public ExposedReference<RectTransform> dialogueBoxRect;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MensageBehaviour>.Create(graph, template);
        MensageBehaviour clone = playable.GetBehaviour();

        clone.textObject = textObject.Resolve(graph.GetResolver());
        clone.dialogueBoxRect = dialogueBoxRect.Resolve(graph.GetResolver());

        return playable;
    }
}
