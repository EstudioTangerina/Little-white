using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackColor(0.8f, 0.2f, 0.2f)]
[TrackBindingType(typeof(GameObject))] // <- Bindaremos um GameObject com os dois componentes
[TrackClipType(typeof(MensageClip))]
public class MensageTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var playable = ScriptPlayable<MensageMixerBehaviour>.Create(graph, inputCount);
        return playable;
    }
}
