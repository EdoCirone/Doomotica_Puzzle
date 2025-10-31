using UnityEngine;
using DG.Tweening;

public class SoapStretch : MonoBehaviour
{
    [Header("Target scale (lunghezza finale del getto)")]
    [SerializeField] private float _targetLength = 1f;
    [SerializeField] private Axis _stretchAxis = Axis.Y;
    [SerializeField] private float _stretchDuration = 1f;
    [SerializeField] private Ease _easeOut = Ease.OutQuad;

    [Header("Trigger Poison")]
    [SerializeField] private SoapPoisonTrigger _poisonTrigger; // <-- AGGIUNTO

    private Tween _scaleTween;
    private bool _hasStretched;
    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;

        switch (_stretchAxis)
        {
            case Axis.Y:
                transform.localScale = new Vector3(_originalScale.x, 0f, _originalScale.z);
                break;
            case Axis.Z:
                transform.localScale = new Vector3(_originalScale.x, _originalScale.y, 0f);
                break;
        }
    }

    public void StartStretch()
    {
        Debug.Log("[SoapStretch] StartStretch() chiamato!"); // <-- AGGIUNGI QUESTO

        if (_hasStretched)
        {
            Debug.Log("[SoapStretch] Già stretched, ignoro");
            return;
        }

        _hasStretched = true;
        _scaleTween?.Kill();

        Vector3 targetScale = _originalScale;
        switch (_stretchAxis)
        {
            case Axis.Y:
                targetScale.y = _targetLength;
                break;
            case Axis.Z:
                targetScale.z = _targetLength;
                break;
        }

        Debug.Log($"[SoapStretch] Inizio tween verso {targetScale}");

        _scaleTween = transform
            .DOScale(targetScale, _stretchDuration)
            .SetEase(_easeOut)
            .OnComplete(() =>
            {
                Debug.Log("[SoapStretch] Tween completato!");
                if (_poisonTrigger != null)
                {
                    _poisonTrigger.ActivateTrigger();
                }
                else
                {
                    Debug.LogError("[SoapStretch] _poisonTrigger è NULL!");
                }
            });
    }

    // Metodo per fermare/ritirare il getto (opzionale)
    public void StopStretch()
    {
        _scaleTween?.Kill();

        Vector3 targetScale = _originalScale;
        switch (_stretchAxis)
        {
            case Axis.Y:
                targetScale.y = 0f;
                break;
            case Axis.Z:
                targetScale.z = 0f;
                break;
        }

        _scaleTween = transform
            .DOScale(targetScale, _stretchDuration * 0.5f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                if (_poisonTrigger != null)
                {
                    _poisonTrigger.DeactivateTrigger();
                }
            });
    }

    private enum Axis { Y, Z }
}