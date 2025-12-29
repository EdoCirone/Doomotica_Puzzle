#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PlayFromAnywhere
{
    private const string BootScenePath = "Assets/Scenes/BootScene.unity";

    static PlayFromAnywhere()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        SetBootAsStartScene();
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.ExitingEditMode)
            return;

        var activeScene = EditorSceneManager.GetActiveScene();

        if (activeScene.path != BootScenePath)
        {
            // Salva il nome della scena attiva per caricarla dopo il boot
            EditorPrefs.SetString("PLAY_FROM_SCENE_NAME", activeScene.name);
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // L'utente ha annullato il salvataggio, quindi annulla l'entrata in modalità play
            EditorApplication.isPlaying = false;
            return;
        }

        EditorPrefs.SetString("PLAY_FROM_SCENE_NAME", activeScene.name);

        SetBootAsStartScene();
    }

    private static void SetBootAsStartScene()
    {
        // Carica la scena di boot
        var bootAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootScenePath);

        if (bootAsset == null)
        {

            Debug.LogError($"[PlayFronManywhere] Boot scene not found {BootScenePath}");
            return;
        }
        ;

        EditorSceneManager.playModeStartScene = bootAsset;


    }
}
#endif