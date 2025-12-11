using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Simula la caduta del panino con traiettoria parabolica.
/// Nessuna fisica: solo interpolazione visiva.
/// </summary>
[RequireComponent(typeof(CarriableObject))]
public class FallingFood_Parabola : MonoBehaviour
{
    [Header("Riferimento al forno")]
    [SerializeField] private GenericForniture _oven;

    [Header("Caduta")]
    [SerializeField] private Transform _startPoint;   // posizione iniziale (piano forno)
    [SerializeField] private Transform _endPoint;     // posizione finale (pavimento)
    [SerializeField] private float _duration = 1.2f;
    [SerializeField] private float _arcHeight = 0.6f; // altezza del picco della parabola
    //[SerializeField] private Ease _ease = Ease.Linear;

    [Header("Effetti opzionali")]
    [SerializeField] private UnityEngine.Events.UnityEvent _onFallStart;
    [SerializeField] private UnityEngine.Events.UnityEvent _onFallEnd;

    private bool _hasFallen = false;
    private Tween _moveTween;

    private void Awake()
    {
        if (_startPoint != null)
            transform.position = _startPoint.position;
    }

    private void OnEnable()
    {
        if (_oven != null)
            _oven.OnActivate += HandleOvenOpened;
    }

    private void OnDisable()
    {
        if (_oven != null)
            _oven.OnActivate -= HandleOvenOpened;
    }

    private void HandleOvenOpened(GenericForniture furn)
    {
        if (_hasFallen) return;
        _hasFallen = true;
        StartCoroutine(ParabolicFallRoutine());
    }

    private IEnumerator ParabolicFallRoutine()
    {
        _onFallStart?.Invoke();

        Vector3 start = _startPoint != null ? _startPoint.position : transform.position;
        Vector3 end = _endPoint != null ? _endPoint.position : transform.position + Vector3.down * 1f;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _duration;
            float heightOffset = 4 * _arcHeight * (t - t * t); // forma della parabola (0 → arcHeight → 0)
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += heightOffset;
            transform.position = pos;
            yield return null;
        }

        _onFallEnd?.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_startPoint == null || _endPoint == null) return;
        Gizmos.color = Color.yellow;
        Vector3 prev = _startPoint.position;
        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;
            float heightOffset = 4 * _arcHeight * (t - t * t);
            Vector3 next = Vector3.Lerp(_startPoint.position, _endPoint.position, t);
            next.y += heightOffset;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
#endif
}
