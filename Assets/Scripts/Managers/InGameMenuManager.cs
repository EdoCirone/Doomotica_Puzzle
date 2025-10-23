using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;


internal class InGameMenuManager
{
    [Header("Panels")]
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _pausePanel;

    [Header("Animation Settings")]
    [SerializeField] private float _appearDuration = 0.5f;
    [SerializeField] private float _disappearDuration = 0.5f;
    [SerializeField] private Ease _appearEase = Ease.OutBack;
    [SerializeField] private Ease _disappearEase = Ease.InBack;

    private void OnEnable()
    {
        if (LVLManager.Instance == null) return;

        LVLManager.Instance.onWinEvent += ShowWinPanel;
        LVLManager.Instance.onLostEvent += ShowLosePanel;
        LVLManager.Instance.onPauseEvent += ShowPausePanel;
        LVLManager.Instance.onResumeEvent += HidePausePanel;
    }

    private void OnDisable()
    {
        if (LVLManager.Instance == null) return;

        LVLManager.Instance.onWinEvent -= ShowWinPanel;
        LVLManager.Instance.onLostEvent -= ShowLosePanel;
        LVLManager.Instance.onPauseEvent -= ShowPausePanel;
        LVLManager.Instance.onResumeEvent -= HidePausePanel;
    }

    private void Start()
    {
        HideAllPanelsImmediate();
    }

    private void HideAllPanelsImmediate()
    {
        _winPanel?.SetActive(false);
        _losePanel?.SetActive(false);
        _pausePanel?.SetActive(false);
    }

    // === Animazioni con DOTween ===

    private void AnimatePanelIn(GameObject panel)
    {
        if (panel == null) return;

        panel.SetActive(true);
        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;

        rt.DOScale(Vector3.one, _appearDuration)
          .SetEase(_appearEase)
          .SetUpdate(true); // SetUpdate(true) fa sì che funzioni anche con Time.timeScale = 0
    }

    private void AnimatePanelOut(GameObject panel)
    {
        if (panel == null) return;

        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.DOScale(Vector3.zero, _disappearDuration)
          .SetEase(_disappearEase)
          .SetUpdate(true)
          .OnComplete(() => panel.SetActive(false));
    }

    // === Metodi collegati agli eventi ===
    private void ShowWinPanel() => AnimatePanelIn(_winPanel);
    private void ShowLosePanel() => AnimatePanelIn(_losePanel);
    private void ShowPausePanel() => AnimatePanelIn(_pausePanel);
    private void HidePausePanel() => AnimatePanelOut(_pausePanel);

    // === Pulsanti UI ===
    public void OnNextLevelButton() => GameManager.Instance.NextLevel();
    public void OnMainMenuButton() => GameManager.Instance.ReturnToMenu();
    public void OnResetLevelButton() => LVLManager.Instance.ResetLevel();
    public void OnResumeButton() => LVLManager.Instance.TogglePause();
}
