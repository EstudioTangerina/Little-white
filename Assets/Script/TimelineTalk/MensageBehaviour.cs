using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Timeline;

[System.Serializable]
public class MensageBehaviour : PlayableBehaviour
{
    [TextArea]
    public string mensagem;
    public float velocidadeDigitacao = 0.05f;

    public TextMeshProUGUI textObject; // binding direto do TMP

    private MonoBehaviour coroutineRunner;
    private Coroutine typingCoroutine;

    private bool esperandoInput = false;
    private bool textoCompleto = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        // Começa a digitação quando o texto está pronto e ainda não começou
        if (textObject != null && typingCoroutine == null)
        {
            if (coroutineRunner == null)
            {
                coroutineRunner = textObject.GetComponent<TextCoroutineRunner>();
            }

            if (coroutineRunner != null)
            {
                typingCoroutine = coroutineRunner.StartCoroutine(TypeText());
            }
        }

        // Quando o texto termina e ainda não está esperando input
        if (textoCompleto && !esperandoInput && coroutineRunner != null)
        {
            esperandoInput = true;

            var director = playable.GetGraph().GetResolver() as PlayableDirector;
            if (director != null)
            {
                coroutineRunner.StartCoroutine(EsperarInput(director));
            }
        }
    }

    private IEnumerator TypeText()
    {
        textObject.text = "";
        foreach (char c in mensagem)
        {
            textObject.text += c;
            yield return new WaitForSeconds(velocidadeDigitacao);
        }

        textoCompleto = true;
        textObject.text += "\n[Pressione Espaço]";
    }

private IEnumerator EsperarInput(PlayableDirector director)
{
    while (!Input.GetKeyDown(KeyCode.Space))
    {
        yield return null;
    }

    double tempoAtual = director.time;
    double menorProximoInicio = double.MaxValue;

    TimelineAsset timeline = director.playableAsset as TimelineAsset;
    if (timeline != null)
    {
        foreach (var track in timeline.GetOutputTracks())
        {
            foreach (var clip in track.GetClips())
            {
                if (clip.start > tempoAtual && clip.start < menorProximoInicio)
                {
                    menorProximoInicio = clip.start;
                }
            }
        }
    }

    if (menorProximoInicio < double.MaxValue)
    {
        director.time = menorProximoInicio;
    }

    director.playableGraph.GetRootPlayable(0).SetSpeed(1); // Continua a Timeline
}

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        textoCompleto = false; // Reseta o estado do texto completo
        esperandoInput = false; // Reseta o estado de espera por input

        var director = playable.GetGraph().GetResolver() as PlayableDirector;

    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
{
    // Verifica se o clip terminou completamente
    double tempoAtual = playable.GetTime();
    double duracaoTotal = playable.GetDuration();
    bool chegouNoFim = tempoAtual >= duracaoTotal - 0.05f;

    if (!chegouNoFim)
    {
        return; // Não faz nada se ainda não for o fim do clip
    }

    // Para a digitação se estiver ativa
    if (typingCoroutine != null && coroutineRunner != null)
    {
        coroutineRunner.StopCoroutine(typingCoroutine);
        typingCoroutine = null;
    }

    // Pausa a Timeline e inicia a espera pelo espaço
    var director = playable.GetGraph().GetResolver() as PlayableDirector;
    if (director != null)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0); // Pausa a Timeline

        if (coroutineRunner == null && textObject != null)
            coroutineRunner = textObject.GetComponent<MonoBehaviour>();

        if (coroutineRunner != null)
        {
            coroutineRunner.StartCoroutine(EsperarInput(director));
        }
    }
}

    
    public void SetCoroutineRunner(MonoBehaviour runner)
    {
        coroutineRunner = runner;
    }

}
