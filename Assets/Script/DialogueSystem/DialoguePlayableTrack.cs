using UnityEngine.Timeline;

[TrackClipType(typeof(DialoguePlayableAsset))]
[TrackBindingType(typeof(DialogueReceiver))]
public class DialoguePlayableTrack : TrackAsset
{
    // Aqui você pode expandir no futuro, por exemplo:
    // - aplicar filtros
    // - definir comportamentos globais
}
