using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLVLManager : MonoBehaviour
{
    [SerializeField] private List<Button> _levelButtons;

    // Colori configurabili dall'Inspector
    [SerializeField] private Color _unlockedNormalColor = Color.white;
    [SerializeField] private Color _unlockedHighlightedColor = new Color(0.9f, 0.9f, 1f);
    [SerializeField] private Color _lockedNormalColor = Color.gray;
    [SerializeField] private Color _lockedDisabledColor = new Color(0.6f, 0.6f, 0.6f);

    private void OnEnable()
    {
        int unlocked = GameManager.Instance.UnlockedLevels;

        for (int i = 0; i < _levelButtons.Count; i++)
        {
            int levelIndex = i + 1;
            var button = _levelButtons[i];
            if (button == null)
            {
                Debug.LogWarning($"[SelectLVLManager] Button at index {i} is null. Skipping.");
                continue;
            }

            // Rimuove tutti i listener precedenti per evitare duplicazioni
            button.onClick.RemoveAllListeners();


            bool isUnlocked = levelIndex <= unlocked;
            button.interactable = isUnlocked;

            // Aggiorna il ColorBlock per cambiare l'aspetto del pulsante
            ColorBlock cb = button.colors;
            if (isUnlocked)
            {
                cb.normalColor = _unlockedNormalColor;
                cb.highlightedColor = _unlockedHighlightedColor;
                // mantenere l'opacità per lo stato disabled sugli sbloccati
                cb.disabledColor = new Color(_unlockedNormalColor.r, _unlockedNormalColor.g, _unlockedNormalColor.b, 0.6f);
            }
            else
            {
                cb.normalColor = _lockedNormalColor;
                cb.highlightedColor = _lockedNormalColor;
                cb.disabledColor = _lockedDisabledColor;
            }
            // Forza l'aggiornamento visivo
            button.colors = cb;

            button.onClick.AddListener(() => LoadLevel(levelIndex));
        }

        //// Da Rimuovere: forzava l'aggiornamento dei canvas 
        //Canvas.ForceUpdateCanvases();
    }

    private void LoadLevel(int index)
    {
        // Controllo di sicurezza: evita caricamento non sbloccato
        if (index <= GameManager.Instance.UnlockedLevels)                                                       
        {
            GameManager.Instance.LoadLevel(index);
        }
        else
        {
            Debug.Log($"[SelectLVLManager] Livello {index} non ancora sbloccato!");
        }
    }
}
