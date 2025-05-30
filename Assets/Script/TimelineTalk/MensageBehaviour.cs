using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System.Collections;
using UnityEngine.Timeline;

[System.Serializable]
public class MensageBehaviour : PlayableBehaviour
{
    [TextArea]
    public string mensagem;
    public float velocidadeDigitacao = 0.05f;

    public Vector2 novaPosicao;  // ✅ Vector2 para a posição

    public TextMeshProUGUI textObject;
    public RectTransform dialogueBoxRect;  // ✅ Nova referência

    private MonoBehaviour coroutineRunner;
    private Coroutine typingCoroutine;

    private bool esperandoInput = false;
    private bool textoCompleto = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
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

        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        textoCompleto = false;
        esperandoInput = false;

        // ✅ Mover a caixa de diálogo (não só o texto!)
        if (dialogueBoxRect != null)
        {
            dialogueBoxRect.anchoredPosition = novaPosicao;
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        double tempoAtual = playable.GetTime();
        double duracaoTotal = playable.GetDuration();
        bool chegouNoFim = tempoAtual >= duracaoTotal - 0.05f;

        if (!chegouNoFim)
        {
            return;
        }

        if (typingCoroutine != null && coroutineRunner != null)
        {
            coroutineRunner.StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        var director = playable.GetGraph().GetResolver() as PlayableDirector;
        if (director != null)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0);

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
