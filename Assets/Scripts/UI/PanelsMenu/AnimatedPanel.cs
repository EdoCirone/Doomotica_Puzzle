
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class AnimatedPanel : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _easeType = Ease.OutBack;
    [SerializeField] private Ease _closeEaseType = Ease.InBack;
    [SerializeField] private bool _isHideOnStart = true;

    [Header("EVENTI")]
    public UnityEngine.Events.UnityEvent OnOpenStart;
    public UnityEngine.Events.UnityEvent OnOpenComplete;
    public UnityEngine.Events.UnityEvent OnClosedStart;
    public UnityEngine.Events.UnityEvent OnClosedComplete;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private bool _isOpen = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();


        if (_isHideOnStart)
        {
            _rectTransform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0f;
            //gameObject.SetActive(false);
        }
    }


    public void OpenPanel()
    {
        if (_isOpen) return;
        _isOpen = true;

        //Kill delle animazioni in corso
        _rectTransform.DOKill();
        _canvasGroup.DOKill();

        //Imposta lo stato iniziale
        gameObject.SetActive(true);
        _rectTransform.localScale = Vector3.zero;
        _canvasGroup.alpha = 0f;

        //Invoca l'evento di inizio apertura
        OnOpenStart?.Invoke();

        //Esegui le animazioni di apertura
        _rectTransform.DOScale(Vector3.one, _animationDuration).SetEase(_easeType).SetUpdate(true);
        _canvasGroup.DOFade(1f, _animationDuration).SetEase(_easeType).SetUpdate(true).
            OnComplete(() => OnOpenComplete?.Invoke()); //Invoca l'evento di completamento apertura alla fine dell'animazione .OnComplete
    }

    public void ClosePanel()
    {
        if (!_isOpen) return;
        _isOpen = false;

       
        _rectTransform.DOKill();
        _canvasGroup.DOKill();

        OnClosedStart?.Invoke();

        _rectTransform.DOScale(Vector3.zero, _animationDuration)
            .SetEase(_closeEaseType)
            .SetUpdate(true);
        
        _canvasGroup.DOFade(0f, _animationDuration)
            .SetEase(_closeEaseType)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                gameObject.SetActive(false); // Disattiva il pannello alla fine dell'animazione
                OnClosedComplete?.Invoke();
            });
    
    
    }
}
