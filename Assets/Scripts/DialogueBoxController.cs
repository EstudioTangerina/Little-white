using UnityEngine;
using TMPro;

public class DialogueBoxController : MonoBehaviour
{
    public TMP_Text dialogueTextComponent;
    public RectTransform dialogueBoxRectTransform;
    public float padding = 20f; // Espaçamento adicional para ajuste do tamanho
    public float typingSpeed = 0.05f; // Velocidade de digitação do texto

    private string dialogueText;
    private bool isAnimating = false;

    public void SetDialogueText(string text)
    {
        dialogueText = text;
        dialogueTextComponent.text = "";
        dialogueBoxRectTransform.sizeDelta = new Vector2(dialogueBoxRectTransform.sizeDelta.x, padding * 2f);
        isAnimating = true;
        StartCoroutine(TypeText());
    }

    private System.Collections.IEnumerator TypeText()
    {
        int currentCharIndex = 0;

        while (currentCharIndex < dialogueText.Length)
        {
            dialogueTextComponent.text += dialogueText[currentCharIndex];
            currentCharIndex++;
            dialogueBoxRectTransform.sizeDelta = new Vector2(dialogueBoxRectTransform.sizeDelta.x, dialogueTextComponent.preferredHeight + padding * 2f);

            yield return new WaitForSeconds(typingSpeed);
        }

        isAnimating = false;
    }
}
