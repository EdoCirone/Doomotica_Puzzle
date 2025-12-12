using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    [Header("Animated Panels")]
    [SerializeField] private AnimatedPanel _winPanel;
    [SerializeField] private AnimatedPanel _losePanel;
    [SerializeField] private AnimatedPanel _menuPanel;
    [SerializeField] private AnimatedPanel _optionsPanel;
    [SerializeField] private AnimatedPanel _confirmExitPanel;
    [SerializeField] private AnimatedPanel _confirmRestartPanel;

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
            LVLManager.Instance.onPauseEvent -= ShowMenuPanel;
            LVLManager.Instance.onResumeEvent -= HideAllMenus;
            _isHooked = false;
        }
    }

    private void ConnectToLVLEvents(LVLManager lvl)
    {
        if (_isHooked || lvl == null) return;

        lvl.onWinEvent += ShowWinPanel;
        lvl.onLostEvent += ShowLosePanel;
        lvl.onPauseEvent += ShowMenuPanel;
        lvl.onResumeEvent += HideAllMenus;

        _isHooked = true;
        Debug.Log("[InGameMenuManager] Hooked to LVLManager events.");
    }

    private void ShowWinPanel()
    {
        Debug.Log("[InGameMenuManager] ShowWinPanel");
        _winPanel?.OpenPanel();
    }

    private void ShowLosePanel()
    {
        Debug.Log("[InGameMenuManager] ShowLosePanel");
        _losePanel?.OpenPanel();
    }

    private void ShowMenuPanel()
    {
        Debug.Log("[InGameMenuManager] ShowPausePanel");
        HideAllMenus();
        _menuPanel?.OpenPanel();
    }

    public void ShowOptionsPanel()
    {
        Debug.Log("[InGameMenuManager] ShowOptionsPanel");
        HideAllMenus();
        _optionsPanel?.OpenPanel();
    }


    private void ShowConfirmExitPanel()
    {
        Debug.Log("[InGameMenuManager] ShowConfirmExitPanel");
        HideAllMenus();
        _confirmExitPanel?.OpenPanel();
    }

    private void ShowConfirmRestartPanel()
    {
        Debug.Log("[InGameMenuManager] ShowConfirmRestartPanel");
        HideAllMenus();
        _confirmRestartPanel?.OpenPanel();
    }


    private void HideAllMenus()
    {
        _winPanel?.ClosePanel();
        _losePanel?.ClosePanel();
        _menuPanel?.ClosePanel();
        _optionsPanel?.ClosePanel();
    }

    private void HideAllMenusInstant()
    {
        _winPanel?.gameObject.SetActive(false);
        _losePanel?.gameObject.SetActive(false);
        _menuPanel?.gameObject.SetActive(false);
        _optionsPanel?.gameObject.SetActive(false);
    }
}
