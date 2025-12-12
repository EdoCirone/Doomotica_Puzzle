using UnityEngine;
public enum MenuButtonType
{
    NewGame,
    NextLevel,
    ResetLevel,
    MainMenu,
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
    public void ExecuteQuitGame() => Execute(MenuButtonType.QuitGame);
}