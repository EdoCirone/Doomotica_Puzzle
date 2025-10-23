using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LVLManager : MonoBehaviour
{
    public static LVLManager Instance { get; private set; }

    private List<OldCharacterFSM> _characters = new List<OldCharacterFSM>();
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _characters.AddRange(FindObjectsOfType<OldCharacterFSM>());

        foreach (OldCharacterFSM character in _characters)
        {
            character.OnCharacterDeath += OnCharacterDeath;
        }
    }

    public void Update()
    {
        if (!_isLevelEnded && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }


    private void OnCharacterDeath(OldCharacterFSM character)
    {
        if (_isLevelEnded) return;

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

        Time.timeScale = 0f;
        Debug.Log("Hai vinto!");
        onWinEvent?.Invoke();
    }
    private void OnLose()
    {
        if (_isLevelEnded) return;
        _isLevelEnded = true;

        Time.timeScale = 0f;
        Debug.Log("Hai perso!");
        onLostEvent?.Invoke();
    }

    public void TogglePause()
    {
        if (_isLevelEnded) return;

        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;

        if (_isPaused) onPauseEvent?.Invoke();
        else onResumeEvent?.Invoke();
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnDestroy()
    {
        foreach (OldCharacterFSM character in _characters)
        {
            character.OnCharacterDeath -= OnCharacterDeath;
        }
        if (Instance == this)
        {
            Instance = null;
        }
    }

}
