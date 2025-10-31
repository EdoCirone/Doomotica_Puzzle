using UnityEngine;
using DG.Tweening;

/// <summary>
/// Ruota un oggetto su un asse specifico (es. porta del forno) usando DOTween.
/// Ideale da collegare a GenericForniture.OnActivate.
/// </summary>
public class RotationTween : MonoBehaviour
{
    [Header("Rotazione")]
    [SerializeField] private Axis _rotationAxis = Axis.X;
    [SerializeField] private float _openAngle = 90f;       // gradi da ruotare quando si apre
    [SerializeField] private float _duration = 0.8f;
    [SerializeField] private Ease _ease = Ease.OutBack;
    [SerializeField] private bool _startClosed = true;     // se true, parte da rotazione 0
    [SerializeField] private bool _canToggle = true;       // se true, clic successivo la richiude

    private bool _isOpen = false;
    private Tween _rotationTween;
    private Quaternion _initialRotation;

    private void Awake()
    {
        _initialRotation = transform.localRotation;
        if (!_startClosed)
        {
            // Avvia già in posizione aperta
            Vector3 axis = GetAxisVector(_rotationAxis);
            transform.localRotation = _initialRotation * Quaternion.AngleAxis(_openAngle, axis);
            _isOpen = true;
        }
    }

    [ContextMenu("Start Rotation (Apri/Chiudi)")]
    public void StartRotation()
    {
        // Se un tween è ancora attivo, lo interrompo
        _rotationTween?.Kill();

        Vector3 axis = GetAxisVector(_rotationAxis);
        float targetAngle = _isOpen ? 0f : _openAngle;

        Quaternion targetRot = _initialRotation * Quaternion.AngleAxis(targetAngle, axis);

        _rotationTween = transform.DOLocalRotateQuaternion(targetRot, _duration)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                if (_canToggle)
                    _isOpen = !_isOpen;
                else
                    _isOpen = true;
            });
    }

    /// <summary>Forza l'apertura (senza toggle)</summary>
    [ContextMenu("Open Door")]
    public void OpenDoor()
    {
        if (_isOpen) return;
        _isOpen = true;
        RotateToAngle(_openAngle);
    }

    /// <summary>Forza la chiusura (senza toggle)</summary>
    [ContextMenu("Close Door")]
    public void CloseDoor()
    {
        if (!_isOpen) return;
        _isOpen = false;
        RotateToAngle(0f);
    }

    private void RotateToAngle(float angle)
    {
        _rotationTween?.Kill();
        Vector3 axis = GetAxisVector(_rotationAxis);
        Quaternion targetRot = _initialRotation * Quaternion.AngleAxis(angle, axis);
        _rotationTween = transform.DOLocalRotateQuaternion(targetRot, _duration).SetEase(_ease);
    }

    private Vector3 GetAxisVector(Axis axis)
    {
        return axis switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.right
        };
    }

    private enum Axis { X, Y, Z }

    private void OnDisable()
    {
        _rotationTween?.Kill();
    }
}
