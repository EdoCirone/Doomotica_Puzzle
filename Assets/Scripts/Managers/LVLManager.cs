using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LVLManager : MonoBehaviour
{
    public static LVLManager Instance { get; private set; }
    public static event System.Action<LVLManager> OnInstanceReady; // Evento per notificare quando l'istanza è pronta

    private List<CharacterFSM> _characters = new List<CharacterFSM>();
    private int _deadCount;
    private bool _isLevelEnded;
    private bool _isPaused;

    //EVENTI 
    public event Action onWinEvent;
    public event Action onLostEvent;
    public event Action onResumeEvent;
    public event Action onPauseEvent;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            OnInstanceReady?.Invoke(this); // Notifica che l'istanza è pronta
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _characters.AddRange(FindObjectsOfType<CharacterFSM>());

        // Filtra quelli che non contano per la vittoria
        _characters.RemoveAll(c => !c.CountsForWin);

        foreach (CharacterFSM character in _characters)
        {
            if (character != null)
                character.OnCharacterDeath += OnCharacterDeath;
        }

        Debug.Log($"[LVLManager] Personaggi registrati per la vittoria: {_characters.Count}");
    }

    public void Update()
    {
        if (!_isLevelEnded && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            Debug.Log("Sono In Pausa");
        }
    }


    private void OnCharacterDeath(CharacterFSM character)
    {
        if (_isLevelEnded) return;

        if (character != null) character.OnCharacterDeath -= OnCharacterDeath;

        _deadCount++;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (_deadCount >= _characters.Count)
        {
            OnWin();
        }
    }

    public void RegisterLose()
    {
        if (_isLevelEnded) return;
        OnLose();
    }

    private void OnWin()
    {
        if (_isLevelEnded) return;
        _isLevelEnded = true;

        StartCoroutine(ShowWinDelayed());
    }

    private IEnumerator ShowWinDelayed()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        Debug.Log("Hai vinto!");
        onWinEvent?.Invoke();

    }

    private void OnLose()
    {
        if (_isLevelEnded) return;
        _isLevelEnded = true;
        StartCoroutine(ShowLoseDelay());

    }
    IEnumerator ShowLoseDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        Debug.Log("Hai perso!");
        onLostEvent?.Invoke();
    }

    public void SetPause(bool paused)
    {
        if (_isLevelEnded) return;
        if (_isPaused == paused) return;

        _isPaused = paused;
        Time.timeScale = _isPaused ? 0f : 1f;

        if (_isPaused) onPauseEvent?.Invoke();
        else onResumeEvent?.Invoke();
    }

    public void Resume() => SetPause(false);
    public void Pause() => SetPause(true);
    public void TogglePause() => SetPause(!_isPaused);

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnDestroy()
    {
        foreach (CharacterFSM character in _characters)
        {
            character.OnCharacterDeath -= OnCharacterDeath;
        }
        if (Instance == this)
        {
            Instance = null;
        }
    }

}
