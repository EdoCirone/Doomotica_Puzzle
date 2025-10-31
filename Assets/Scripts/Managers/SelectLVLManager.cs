using UnityEngine;
using UnityEngine.UI;

public class SelectLVLManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _lvl1Button;
    [SerializeField] private Button _lvl2Button;
    [SerializeField] private Button _lvl3Button;
    [SerializeField] private Button _closeButton;

    private void Start()
    {
        // Disabilita il TimeScale se era fermo
        Time.timeScale = 1f;

        int unlocked = GameManager.Instance.UnlockedLevels;

        // Sempre sbloccato
        _lvl1Button.interactable = true;

        // Sblocca progressivamente
        _lvl2Button.interactable = unlocked >= 2;
        _lvl3Button.interactable = unlocked >= 3;

        // Assegna listener ai pulsanti
        _lvl1Button.onClick.AddListener(() => LoadLevel(1));
        _lvl2Button.onClick.AddListener(() => LoadLevel(2));
        _lvl3Button.onClick.AddListener(() => LoadLevel(3));

        _closeButton.onClick.AddListener(CloseMenu);
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

    private void CloseMenu()
    {
        GameManager.Instance.ReturnToMenu();
    }
}
