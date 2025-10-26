using UnityEngine;
public enum MenuButtonType
{
    NewGame,
    NextLevel,
    ResetLevel,
    MainMenu,
    Options,
    CloseOptions,
    Resume,
    QuitGame
}

public class MenuButtonBus : MonoBehaviour
{
    public void Execute(MenuButtonType type)
    {
        switch (type)
        {
            case MenuButtonType.NewGame:
                GameManager.Instance.NewGame();
                break;

            case MenuButtonType.NextLevel:
                GameManager.Instance.NextLevel();
                break;

            case MenuButtonType.ResetLevel:
                LVLManager.Instance.ResetLevel();
                break;

            case MenuButtonType.MainMenu:
                GameManager.Instance.ReturnToMenu();
                break;

            case MenuButtonType.Resume:
                LVLManager.Instance.TogglePause();
                break;

            case MenuButtonType.QuitGame:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }

    public void ExecuteNewGame() => Execute(MenuButtonType.NewGame);
    public void ExecuteNextLevel() => Execute(MenuButtonType.NextLevel);
    public void ExecuteResetLevel() => Execute(MenuButtonType.ResetLevel);
    public void ExecuteMainMenu() => Execute(MenuButtonType.MainMenu);
    public void ExecuteOptions() => Execute(MenuButtonType.Options);
    public void ExecuteCloseOptions() => Execute(MenuButtonType.CloseOptions);
    public void ExecuteResume() => Execute(MenuButtonType.Resume);
    public void ExecuteQuitGame() => Execute(MenuButtonType.QuitGame);
}