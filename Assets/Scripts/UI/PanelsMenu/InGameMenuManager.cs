using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    [Header("Animated Panels")]
    [SerializeField] private AnimatedPanel winPanel;
    [SerializeField] private AnimatedPanel losePanel;
    [SerializeField] private AnimatedPanel pausePanel;
    [SerializeField] private AnimatedPanel optionsPanel;

    private bool _isHooked;

    private void Awake()
    {
        HideAllMenusInstant();
    }

    private void OnEnable()
    {
        //  Se l'istanza del LVLManager esiste già (perché il suo Awake è già stato chiamato)
        if (LVLManager.Instance != null)
        {
            ConnectToLVLEvents(LVLManager.Instance);
        }

        //  In ogni caso ascolta la creazione di future istanze (scene diverse ecc.)
        LVLManager.OnInstanceReady += ConnectToLVLEvents;
    }

    private void OnDisable()
    {
        LVLManager.OnInstanceReady -= ConnectToLVLEvents;

        if (_isHooked && LVLManager.Instance != null)
        {
            LVLManager.Instance.onWinEvent -= ShowWinPanel;
            LVLManager.Instance.onLostEvent -= ShowLosePanel;
            LVLManager.Instance.onPauseEvent -= ShowPausePanel;
            LVLManager.Instance.onResumeEvent -= HideAllMenus;
            _isHooked = false;
        }
    }

    private void ConnectToLVLEvents(LVLManager lvl)
    {
        if (_isHooked || lvl == null) return;

        lvl.onWinEvent += ShowWinPanel;
        lvl.onLostEvent += ShowLosePanel;
        lvl.onPauseEvent += ShowPausePanel;
        lvl.onResumeEvent += HideAllMenus;

        _isHooked = true;
        Debug.Log("[InGameMenuManager] Hooked to LVLManager events.");
    }

    private void ShowWinPanel()
    {
        Debug.Log("[InGameMenuManager] ShowWinPanel");
        winPanel?.OpenPanel();
    }

    private void ShowLosePanel()
    {
        Debug.Log("[InGameMenuManager] ShowLosePanel");
        losePanel?.OpenPanel();
    }

    private void ShowPausePanel()
    {
        Debug.Log("[InGameMenuManager] ShowPausePanel");
        HideAllMenus();
        pausePanel?.OpenPanel();
    }

    public void ShowOptionsPanel()
    {
        Debug.Log("[InGameMenuManager] ShowOptionsPanel");
        HideAllMenus();
        optionsPanel?.OpenPanel();
    }

    public void BackToPauseMenu()
    {
        Debug.Log("[InGameMenuManager] BackToPauseMenu");
        HideAllMenus();
        pausePanel?.OpenPanel();
    }

    private void HideAllMenus()
    {
        winPanel?.ClosePanel();
        losePanel?.ClosePanel();
        pausePanel?.ClosePanel();
        optionsPanel?.ClosePanel();
    }

    private void HideAllMenusInstant()
    {
        winPanel?.gameObject.SetActive(false);
        losePanel?.gameObject.SetActive(false);
        pausePanel?.gameObject.SetActive(false);
        optionsPanel?.gameObject.SetActive(false);
    }
}
