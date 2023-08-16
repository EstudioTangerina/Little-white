using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;
    public string skipInputButton = "Jump"; // Botão para pular a cutscene (pode ser configurado no Input Manager)
    private bool canSkip = false;

    private void Start()
    {
        cutsceneDirector.stopped += OnCutsceneStopped;
    }

    private void Update()
    {
        if (canSkip && Keyboard.current[skipInputButton].IsPressed())
        {
            SkipCutscene();
        }
    }

    private void SkipCutscene()
    {
        cutsceneDirector.time = cutsceneDirector.duration; // Pula para o final da cutscene
        cutsceneDirector.Play(); // Inicia a reprodução da cutscene (ou próxima etapa)
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        canSkip = false;
    }

    public void EnableSkip()
    {
        canSkip = true;
    }

    public void DisableSkip()
    {
        canSkip = false;
    }
}