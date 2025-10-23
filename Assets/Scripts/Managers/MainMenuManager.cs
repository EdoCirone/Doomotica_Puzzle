using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MainMenuManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private RectTransform _mainMenuPanel;
    [SerializeField] private GameObject _optionsPanel;

    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private Ease _ease = Ease.OutBack;

    private void Start()
    {
        Time.timeScale = 1f;
        if (_mainMenuPanel != null)
        {
            _mainMenuPanel.localScale = Vector3.zero;
            _mainMenuPanel.DOScale(Vector3.one, _fadeDuration).SetEase(_ease);
        }

        _optionsPanel.SetActive(false);
    }

    // ===Navigation Methods===

    public void OnNewGameButton() => GameManager.Instance.NewGame();
    public void OnSelectLevelButton() => GameManager.Instance.LoadLevelSelection();
    public void OnCreditsButton() => GameManager.Instance.LoadCreditsScene();
    public void OnQuitButton() => Application.Quit();

    //===Options Pannel ===
    public void OnOptionsButton()
    {
        _optionsPanel.SetActive(true);
        _optionsPanel.transform.localScale = Vector3.zero;
        _optionsPanel.transform.DOScale(Vector3.one, _fadeDuration).SetEase(_ease);
    }

    public void OnCloseOptions()
    {
        _optionsPanel.transform.DOScale(Vector3.zero, _fadeDuration)
            .SetEase(_ease).
            OnComplete(() => _optionsPanel.SetActive(false));
    }


    // === Socials === 

    public void OnInstagramButton() => Application.OpenURL("https://www.instagram.com/yourgameprofile/");
    public void OnTwitterButton() => Application.OpenURL("https://www.x.com/yourgameprofile/");
    public void OnDiscordButton() => Application.OpenURL("https://discord.gg/yourgamediscord");
}
