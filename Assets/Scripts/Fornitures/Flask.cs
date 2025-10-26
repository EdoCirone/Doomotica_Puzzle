using UnityEngine;
using DG.Tweening;

public class Flask : MonoBehaviour
{
    [Header("Riferimento alla lavatrice (GenericForniture)")]
    [SerializeField] private GenericForniture _washingMachine;

    [Header("Rotazione di caduta")]
    [SerializeField] private Vector3 _fallEuler = new Vector3(0f, 0f, 90f); // direzione e angolo di caduta
    [SerializeField] private float _fallDuration = 1f; // durata rotazione
    [SerializeField] private Ease _fallEase = Ease.OutBack; // tipo di interpolazione

    [Header("Effetto Sapone (opzionale)")]
    [SerializeField] private SoapStretch _soap;

    private bool _hasFallen; // true dopo la prima caduta
    private Tween _rotationTween;

    private void OnEnable()
    {
        if (_washingMachine == null)
            _washingMachine = GetComponentInParent<GenericForniture>();

        // Iscrizione all’evento di attivazione
        if (_washingMachine != null)
            _washingMachine.OnActivate += HandleActivate;
    }

    private void OnDisable()
    {
        // Disiscrizione per sicurezza
        if (_washingMachine != null)
            _washingMachine.OnActivate -= HandleActivate;
    }

    private void HandleActivate(GenericForniture furn)
    {
        if (_hasFallen) return; // evita doppia animazione
        _hasFallen = true;

        _rotationTween?.Kill();

        // Rotazione verso l'angolo specificato
        _rotationTween = transform
            .DOLocalRotate(_fallEuler, _fallDuration)
            .SetEase(_fallEase);

        // Attiva l’uscita del sapone
        _soap?.StartStretch();
    }
}
