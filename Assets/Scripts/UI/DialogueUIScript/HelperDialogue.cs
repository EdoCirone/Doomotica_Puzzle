using UnityEngine;
using UnityEngine.UI;

public class HelperDialogue : SimpleDialogue
{
    [SerializeField] private Button _HelperButton;

    private int helperCallbackCount = 0;

    private void Update()
    {
        
    }

    private void OnHelperButton()
    {
        helperCallbackCount++;
        switch (helperCallbackCount)
        {
            case 1:
                StartCoroutine(ShowDialogueRoutine());
                currentLine++;
                break;
            case 2:
                StartCoroutine(ShowDialogueRoutine());
                currentLine++;

                Debug.Log("Helper button pressed second time.");
                break;
            default:
                StartCoroutine(ShowDialogueRoutine());
                currentLine++;
                Debug.Log("Helper button pressed multiple times.");
                break;
        }
    }
}
