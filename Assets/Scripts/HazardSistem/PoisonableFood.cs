using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Rende un oggetto "avvelenabile" (come il panino/pizza).
/// Può essere attivato da un getto di sapone o da altri trigger esterni.
/// </summary>
public class PoisonableFood : MonoBehaviour
{
    [Header("Stato attuale")]
    [SerializeField] private bool _isPoisoned = false;

    [Header("Effetti visivi/sonori (opzionali)")]
    [SerializeField] private Color _poisonColor = Color.green;
    [SerializeField] private UnityEvent _onPoisoned;

    private Renderer[] _renderers;
    private Color[] _originalColors;

    public bool IsPoisoned => _isPoisoned;

    private void Awake()
    {
        Debug.Log($"[PoisonableFood] Awake su {gameObject.name}");

        _renderers = GetComponentsInChildren<Renderer>();
        _originalColors = new Color[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _originalColors[i] = _renderers[i].material.color;
        }

        Debug.Log($"[PoisonableFood] Trovati {_renderers.Length} renderer");
    }

    public void Poison()
    {
        Debug.Log($"[PoisonableFood] Poison() chiamato su {gameObject.name}, _isPoisoned={_isPoisoned}");

        if (_isPoisoned)
        {
            Debug.Log("[PoisonableFood] Già avvelenato, ignoro");
            return;
        }

        _isPoisoned = true;

        foreach (var r in _renderers)
            r.material.color = _poisonColor;

        _onPoisoned?.Invoke();
        Debug.Log($"[PoisonableFood] {gameObject.name} è stato avvelenato!");
    }

    /// <summary>
    /// Reset manuale (opzionale, utile per debug o reset livello)
    /// </summary>
    public void ResetPoison()
    {
        _isPoisoned = false;
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material.color = _originalColors[i];
        }
    }
}
