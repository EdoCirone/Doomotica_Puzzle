using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLManager : MonoBehaviour
{

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;

    private CharacterFSM[] _characters;
    private float _puzzleTimer;
    public static LVLManager Instance { get; private set; }

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

    public void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
        if (losePanel != null)
            losePanel.SetActive(false);
        _characters = FindObjectsOfType<CharacterFSM>();
        Debug.Log($"Characters found: {_characters.Length}");
        foreach (CharacterFSM character in _characters)
        {
            Debug.Log($"Character: {character.name}");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }


    public void CheckCharacterAlive()
    {
        foreach (CharacterFSM character in _characters)
        {
            if (!character.IsDeath) return;
        }
        OnWin();
    }
    
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            Time.timeScale = pausePanel.activeSelf ? 0f : 1f;
        }
    }

    public void OnWin()
    {

        winPanel?.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("Hai Vinto");
    }

    public void OnLose()
    {

        losePanel?.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("Hai Perso");
    }

    private void OnDestroy()
    {

        if (Instance == this)
        {

            Instance = null;

        }

    }
}
