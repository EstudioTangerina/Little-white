using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class MensageMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        GameObject binding = playerData as GameObject;
        if (binding == null) return;

        // ✅ CORRIGIDO: busca nos filhos
        TextMeshProUGUI text = binding.GetComponentInChildren<TextMeshProUGUI>();
        TextCoroutineRunner runner = binding.GetComponentInChildren<TextCoroutineRunner>();

        for (int i = 0; i < playable.GetInputCount(); i++)
        {
            if (playable.GetInputWeight(i) > 0f)
            {
                var inputPlayable = (ScriptPlayable<MensageBehaviour>)playable.GetInput(i);
                var behaviour = inputPlayable.GetBehaviour();

                behaviour.textObject = text;
                behaviour.SetCoroutineRunner(runner);
            }
        }

    }
}

