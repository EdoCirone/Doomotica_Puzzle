using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SimpleDialogue : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private Image _avatarImage;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private CanvasGroup dialoguePanel;

    [Header("Dialogue Data")]
    [SerializeField] private Sprite[] _avatars;
    [TextArea(2, 4)]
    [SerializeField] private string[] _dialogueLines;
    [SerializeField] private float _textSpeed = 0.05f;

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        Time.timeScale = 0f; // Pause game time
        dialoguePanel.alpha = 0f;
        dialoguePanel.gameObject.SetActive(true);

        StartCoroutine(ShowDialogueRoutine());
        nextButton.onClick.AddListener(OnNextButton);

    }

    private IEnumerator ShowDialogueRoutine()
    {
        yield return null;

        dialoguePanel.DOFade(1f, 0.5f).SetUpdate(true); 
        dialoguePanel.interactable = true;
        dialoguePanel.blocksRaycasts = true;

        yield return new WaitForSecondsRealtime(0.7f);

        currentLine = 0;
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (_avatars != null && _avatars.Length > currentLine)
        {
            _avatarImage.sprite = _avatars[currentLine];
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(_dialogueLines[currentLine]));

    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        _dialogueText.text = "";

        foreach (char c in text)
        {
            _dialogueText.text += c;
            yield return new WaitForSecondsRealtime(_textSpeed);
        }

        isTyping = false;
    }

    private void OnNextButton()
    {
        if (isTyping)
        {
            // If still typing, finish the text immediately
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            _dialogueText.text = _dialogueLines[currentLine];
            isTyping = false;
            return;
        }


        currentLine++;
        if (currentLine >= _dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();

    }

    private void EndDialogue()
    {
        dialoguePanel.DOFade(0f, 0.5f).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1f; // Resume game time

            dialoguePanel.interactable = false;

            dialoguePanel.blocksRaycasts = false;

            dialoguePanel.gameObject.SetActive(false);

        });
        
    }

}
