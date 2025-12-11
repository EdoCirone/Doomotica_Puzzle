using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuManager : MonoBehaviour
{

    [Header("Panels")]
    //[SerializeField] private AnimatedPanel _mainMenuPanel;
    [SerializeField] private AnimatedPanel _creditsPanel;
    [SerializeField] private AnimatedPanel _levelSelectionPanel;
    [SerializeField] private AnimatedPanel _optionsPanel;

    private static MainMenuManager _instance;
    private List<AnimatedPanel> _panels = new List<AnimatedPanel>();

    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ListPanels();
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        //if (_mainMenuPanel == null) return;
        //_mainMenuPanel.OpenPanel();

    }

    //Faccio una lista di tutti i pannelli così da poterli chiudere tutti tranne quello selezionato

    private void ListPanels()
    {
        
        foreach (var panel in new AnimatedPanel[] { _creditsPanel,  _levelSelectionPanel, _optionsPanel })
        {
            if (panel == null)
            {
            
                Debug.LogWarning($"[MainMenuManager] Panel reference is missing in the inspector.");
            }

            _panels.Add(panel);
        }

    }

    private void CloseAllPanelsExceptThis(AnimatedPanel exception)
    {
        if (exception == null) return;

        foreach (var panel in _panels)
        {
            if (panel != exception )
            panel.ClosePanel();
        }
    }

    public void OnNewGameButton() => GameManager.Instance.NewGame();
    public void OnSelectLevelButton()
    {
        if (_levelSelectionPanel == null) return;
        CloseAllPanelsExceptThis(_levelSelectionPanel);
        _levelSelectionPanel.OpenPanel();


    }

    public void OnOptionButton()
    {
        if (_optionsPanel == null) return;
        CloseAllPanelsExceptThis(_optionsPanel);
        _optionsPanel.OpenPanel();

    }
    public void OnCreditsButton()
    {
        if (_creditsPanel == null) return;
        CloseAllPanelsExceptThis(_creditsPanel);
        _creditsPanel.OpenPanel();
    }
    public void OnQuitButton() => GameManager.Instance.OnApplicationQuit();

    // === Socials === 

    public void OnInstagramButton() => Application.OpenURL("https://www.instagram.com/yourgameprofile/");
    public void OnTwitterButton() => Application.OpenURL("https://www.x.com/yourgameprofile/");
    public void OnDiscordButton() => Application.OpenURL("https://discord.gg/yourgamediscord");



}