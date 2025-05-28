using TMPro;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class MensageClip : PlayableAsset
{
    [TextArea]
    public string mensagem;
    public float velocidadeDigitacao = 0.05f;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<MensageBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.mensagem = this.mensagem;
        behaviour.velocidadeDigitacao = this.velocidadeDigitacao;

        return playable;
    }
}
