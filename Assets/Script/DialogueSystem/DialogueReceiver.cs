using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueReceiver : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public KeyCode continueKey = KeyCode.Space;

    public RectTransform dialogueBoxTransform; // NOVO

    public IEnumerator DisplayString(string dialogueLine, float speed)
    {
        dialogueText.text = "";

        foreach (char c in dialogueLine)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(speed);
        }
    }

    public IEnumerator WaitForPlayerInput()
    {
        while (!Input.GetKeyDown(continueKey))
        {
            yield return null;
        }
    }

    public void ClearDialogue()
    {
        dialogueText.text = "";
    }

    public void SetDialogueBoxPosition(Vector2 position) // NOVO
    {
        if (dialogueBoxTransform != null)
        {
            dialogueBoxTransform.anchoredPosition = position;
        }
    }
}
