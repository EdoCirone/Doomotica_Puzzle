using UnityEngine;
using DG.Tweening;

/// <summary>
/// Componente generica per vibrazione continua tramite DOTween.
/// Attiva/disattiva la vibrazione chiamando i metodi pubblici StartVibrating e StopVibrating.
/// </summary>
public class VibrationTween : MonoBehaviour
{
    [Header("Vibration Settings")]
    [SerializeField] private float _cycleDuration = 0.25f; // durata di un ciclo di shake
    [SerializeField] private float _strength = 0.05f;      // ampiezza vibrazione
    [SerializeField] private int _vibrato = 30;            // quante oscillazioni per ciclo
    [SerializeField] private bool _fadeOut = true;         // attenua fine di ogni ciclo
    [SerializeField] private bool _ignoreTimeScale = false;
    [SerializeField] private bool StartOnAwake = false;

    private Vector3 _originalLocalPosition;
    private Tween _vibrationTween;

    private void Awake()
    {
        _originalLocalPosition = transform.localPosition;
    }

    private void Start()
    {
        if(StartOnAwake)
        {
            StartVibrating();
        }
        
    }

    /// <summary>
    /// Avvia vibrazione continua (loop infinito) fino a StopVibrating(). Non ho trovato un metodo migliore per aggirare la duration di DoShake
    /// </summary>
    [ContextMenu("Start Vibrating")]
    public void StartVibrating()
    {
        StopVibrating();

        _vibrationTween = transform
            .DOShakePosition(
                duration: _cycleDuration,
                strength: _strength,
                vibrato: _vibrato,
                randomness: 90,
                snapping: false,
                fadeOut: _fadeOut
            )
            .SetLoops(-1, LoopType.Restart)
            .SetUpdate(_ignoreTimeScale)
            .OnKill(() => transform.localPosition = _originalLocalPosition);
    }

    [ContextMenu("Stop Vibrating")] 
    public void StopVibrating()
    {
        if (_vibrationTween != null && _vibrationTween.IsActive())
        {
            _vibrationTween.Kill();
        }

        transform.localPosition = _originalLocalPosition;
        _vibrationTween = null;
    }
}
