
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class AnimatedPanel : MonoBehaviour
{
    private enum MovType
    {
        SCALE,
        SLIDE

    }


    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _easeType = Ease.OutBack;
    [SerializeField] private Ease _closeEaseType = Ease.InBack;
    [SerializeField] private bool _isHideOnStart = true;
    [SerializeField] private MovType _movType;
    [SerializeField] private float _moveOffset;

    [Header("EVENTI")]
    public UnityEngine.Events.UnityEvent OnOpenStart;
    public UnityEngine.Events.UnityEvent OnOpenComplete;
    public UnityEngine.Events.UnityEvent OnClosedStart;
    public UnityEngine.Events.UnityEvent OnClosedComplete;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    private Vector2 _showPosition;
    private bool _isOpen = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _showPosition = _rectTransform.anchoredPosition;

        if (_isHideOnStart)
        {
            switch (_movType)
            {
                case MovType.SCALE:
                    _canvasGroup.alpha = 0f;
                    _rectTransform.localScale = Vector3.zero;
                    break;
                case MovType.SLIDE:
                    _rectTransform.anchoredPosition = new Vector2(_moveOffset, _showPosition.y);
                    break;
            }
        }
    }

    public void Start()
    {
        if (!_isHideOnStart)
        {
            OpenPanel();
        }
    }

    public void OpenPanel()
    {
        if (_isOpen) return;
        _isOpen = true;
    Debug.Log("OpenPanel chiamato su " + gameObject.name);
        //Kill delle animazioni in corso
        _rectTransform.DOKill();
        _canvasGroup.DOKill();

        //Imposta lo stato iniziale
        gameObject.SetActive(true);

        //Invoca l'evento di inizio apertura
        OnOpenStart?.Invoke();

        switch (_movType)
        {
            case MovType.SCALE:
                _rectTransform.localScale = Vector3.zero;
                _canvasGroup.alpha = 0f;
                //Esegui le animazioni di apertura
                _rectTransform.DOScale(Vector3.one, _animationDuration).SetEase(_easeType).SetUpdate(true);
                _canvasGroup.DOFade(1f, _animationDuration).SetEase(_easeType).SetUpdate(true).
                    OnComplete(() => OnOpenComplete?.Invoke()); //Invoca l'evento di completamento apertura alla fine dell'animazione .OnComplete
                break;

            case MovType.SLIDE:
                //Imposta la posizione iniziale fuori schermo a sinistra
                _rectTransform.anchoredPosition = new Vector2(_moveOffset, _showPosition.y);

                //Esegui l'animazione di scorrimento verso la posizione finale (0,0)
                _rectTransform.DOAnchorPos(_showPosition, _animationDuration)
                    .SetEase(_easeType)
                    .SetUpdate(true).OnComplete(() => OnOpenComplete?.Invoke()); ;
                break;
        }
    }

    public void ClosePanel()
    {
        if (!_isOpen) return;
        _isOpen = false;


        _rectTransform.DOKill();
        _canvasGroup.DOKill();

        OnClosedStart?.Invoke();

        switch (_movType)
        {
            case MovType.SCALE:

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
                break;

            case MovType.SLIDE:

                _rectTransform.DOAnchorPos(new Vector2(_moveOffset, _showPosition.y), _animationDuration)
                    .SetEase(_closeEaseType)
                    .SetUpdate(true).OnComplete(() =>
                    {
                        gameObject.SetActive(false); // Disattiva il pannello alla fine dell'animazione
                        OnClosedComplete?.Invoke();
                    });
                ;


                break;

        }


    }
}
