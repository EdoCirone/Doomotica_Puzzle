using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BootManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private CanvasGroup _fadeCanvas;      // bianco / nero
    [SerializeField] private CanvasGroup _loadingCanvas;   // canvas “Loading...”
    [SerializeField] private TMP_Text _loadingText;

    [Header("Settings")]
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _stopTimeSeconds = 6.5f; // ⏱️ tempo in secondi prima del glitch
    [SerializeField] private string _sceneToLoad = "MainMenu";

    private void Awake()
    {
        if (_videoPlayer == null)
        {
            Debug.LogError("BootManager: manca il VideoPlayer!");
            return;
        }

        _fadeCanvas.alpha = 0f;
        _loadingCanvas.alpha = 0f;
        _loadingCanvas.gameObject.SetActive(false);

        StartCoroutine(BootSequence());
    }

    private IEnumerator BootSequence()
    {
        // Avvio video e caricamento scena
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(_sceneToLoad);
        loadOp.allowSceneActivation = false;

        _videoPlayer.Play();
        Debug.Log("[BOOT] Video avviato");

        // Attendi fino al punto di stop desiderato
        while (_videoPlayer.time < _stopTimeSeconds)
        {
            yield return null;
        }

        // Fermiamo il video, c'è un glitch e sono troppo pigro per tagliarlo 
        _videoPlayer.Pause();
        Debug.Log($"[BOOT] Video fermato a {_videoPlayer.time:F2}s → inizio fade bianco");

        // Fade bianco
        yield return StartCoroutine(FadeCanvas(_fadeCanvas, 0f, 1f, _fadeDuration, Color.white));

        // Mostra il “Loading...” se la scena non è ancora pronta
        _loadingCanvas.gameObject.SetActive(true);
        yield return StartCoroutine(FadeCanvas(_loadingCanvas, 0f, 1f, 0.6f, Color.clear));
        StartCoroutine(AnimateLoadingText());

        while (loadOp.progress < 0.9f)
        {
            yield return null;
        }

        // Fade nero e cambio scena
        _fadeCanvas.GetComponent<Image>().color = Color.black;
        yield return StartCoroutine(FadeCanvas(_fadeCanvas, 0f, 1f, _fadeDuration, Color.black));
        loadOp.allowSceneActivation = true;
    }

    // Fade generico CanvasGroup
    private IEnumerator FadeCanvas(CanvasGroup canvas, float from, float to, float duration, Color? fadeColor = null)
    {
        float t = 0f;
        Image img = canvas.GetComponent<Image>();
        if (img != null && fadeColor.HasValue)
            img.color = fadeColor.Value;

        canvas.alpha = from;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvas.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        canvas.alpha = to;
    }

    // Animazione “Loading...”
    private float _dotTimer = 0f;
    private int _dotCount = 0;

    private IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            _dotTimer += Time.unscaledDeltaTime;
            if (_dotTimer > 0.5f)
            {
                _dotTimer = 0f;
                _dotCount = (_dotCount + 1) % 4;
                _loadingText.text = "Loading" + new string('.', _dotCount);
            }
            yield return null;
        }
    }
}
