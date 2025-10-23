using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int CurrentLevelIndex { get; private set; } = 1;
    public int UnlockedLevels { get; private set; } = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
    }

    public void NewGame()
    {
        CurrentLevelIndex = 1;
        PlayerPrefs.SetInt("CurrentLevelIndex", 1);
        PlayerPrefs.Save();

        LoadLevel(CurrentLevelIndex);
    }

    //public void ResetGame() //RESETTA TUTTI I PROGRESSI
    //{
    //    PlayerPrefs.SetInt("CurrentLevelIndex", 1);
    //    PlayerPrefs.SetInt("UnlockedLevels", 1);
    //    PlayerPrefs.Save();
    //    LoadLevel(CurrentLevelIndex);
    //}

    public void LoadLevel(int index)
    {
        CurrentLevelIndex = index;
        SceneManager.LoadScene($"Level_{index:D2}");

    }

    public void NextLevel()
    {
        int nextLevelIndex = CurrentLevelIndex + 1;

        if (nextLevelIndex > UnlockedLevels)
        {
            UnlockedLevels = nextLevelIndex;
            PlayerPrefs.SetInt("UnlockedLevels", UnlockedLevels);
            PlayerPrefs.Save();
        }

        if (Application.CanStreamedLevelBeLoaded($"Level_{nextLevelIndex:D2}"))
        {
            LoadLevel(nextLevelIndex);
        }
        else
        {

            SceneManager.LoadScene("ENDMENUSCENE");
        }
    }

    public void ResetLevel()
    {

        SceneManager.LoadScene($"Level_{CurrentLevelIndex:D2}");
    }

    public void ReturnToMenu()
    {

        SceneManager.LoadScene("MainMenu");
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("UnlockedLevels", UnlockedLevels);
        PlayerPrefs.Save();
        Application.Quit();
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

}