using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public float typingSpeed = 0.05f; // Velocidade de digitação

    private bool dialogueActive;
    private bool isTyping;
    private string currentText;
    private Coroutine typingCoroutine;
    private int currentLine;

    private ContentSizeFitter contentSizeFitter;
    private LayoutGroup layoutGroup;

    void Start()
    {
        dialogueBox.SetActive(false);
        dialogueActive = false;

        contentSizeFitter = dialogueText.GetComponentInParent<ContentSizeFitter>();
        layoutGroup = dialogueText.GetComponentInParent<LayoutGroup>();
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Se o texto ainda estiver sendo digitado, pule para o fim
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentText;
                isTyping = false;
            }
            else
            {
                // Caso contrário, avance para a próxima linha
                DisplayNextLine();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        dialogueBox.SetActive(true);
        dialogueActive = true;
        currentLine = 0;
        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            currentText = dialogueLines[currentLine];
            dialogueText.text = "";
            typingCoroutine = StartCoroutine(TypeText(currentText));
            currentLine++;
        }
        else
        {
            // Fim do diálogo
            dialogueBox.SetActive(false);
            dialogueActive = false;
            currentLine = 0;
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

        // Atualizar tamanho da caixa de diálogo após o texto ser totalmente exibido
        contentSizeFitter.SetLayoutVertical();
        layoutGroup.SetLayoutVertical();
    }
}
