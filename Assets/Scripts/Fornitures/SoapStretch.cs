using UnityEngine;
using DG.Tweening;

public class SoapStretch : MonoBehaviour
{
    [Header("Target scale (lunghezza finale del getto)")]
    [SerializeField] private float _targetLength = 1f; // scala finale sull'asse Y o Z
    [SerializeField] private Axis _stretchAxis = Axis.Y; // asse di crescita
    [SerializeField] private float _stretchDuration = 1f;
    [SerializeField] private Ease _easeOut = Ease.OutQuad;

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

    /// <summary>
    /// Avvia l'uscita del sapone (scala una sola volta e poi rimane esteso).
    /// </summary>
    public void StartStretch()
    {
        if (_hasStretched) return;
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

        _scaleTween = transform
            .DOScale(targetScale, _stretchDuration)
            .SetEase(_easeOut);
    }

    // Enum per scegliere su quale asse cresce il getto
    private enum Axis { Y, Z }
}
