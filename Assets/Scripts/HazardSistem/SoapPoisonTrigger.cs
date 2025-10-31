using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SoapPoisonTrigger : MonoBehaviour
{
    [Header("Riferimento al getto visivo (SoapStretch)")]
    [SerializeField] private SoapStretch _soapStretch;

    [Header("Impostazioni trigger")]
    [SerializeField] private float _activationDelay = 0.1f;

    private Collider _triggerCollider;
    private bool _isActive = false;

    private void Awake()
    {
        Debug.Log($"[SoapPoisonTrigger] Awake su {gameObject.name}");

        _triggerCollider = GetComponent<Collider>();
        _triggerCollider.isTrigger = true;
        _triggerCollider.enabled = false;

        // AGGIUNGI UN RIGIDBODY SE NON C'È
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            Debug.Log("[SoapPoisonTrigger] Rigidbody kinematic aggiunto automaticamente");
        }

        if (_soapStretch == null)
        {
            Debug.LogWarning($"[SoapPoisonTrigger] {name} non ha un riferimento a SoapStretch!");
        }
        else
        {
            Debug.Log($"[SoapPoisonTrigger] SoapStretch assegnato: {_soapStretch.name}");
        }
    }

    public void ActivateTrigger()
    {
        Debug.Log($"[SoapPoisonTrigger] ActivateTrigger chiamato! _isActive={_isActive}");

        if (_isActive) return;
        _isActive = true;

        CancelInvoke(nameof(EnableCollider));
        Invoke(nameof(EnableCollider), _activationDelay);

        Debug.Log($"[SoapPoisonTrigger] Invoke schedulato con delay {_activationDelay}s");
    }

    // METODO AGGIUNTO
    public void DeactivateTrigger()
    {
        Debug.Log($"[SoapPoisonTrigger] DeactivateTrigger chiamato! _isActive={_isActive}");

        if (!_isActive) return;

        _isActive = false;
        CancelInvoke(nameof(EnableCollider));
        _triggerCollider.enabled = false;

        Debug.Log("[SoapPoisonTrigger] Getto disattivato");
    }

    private void EnableCollider()
    {
        _triggerCollider.enabled = true;
        Debug.Log($"[SoapPoisonTrigger] Collider abilitato! Bounds: {_triggerCollider.bounds}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SoapPoisonTrigger] OnTriggerEnter con {other.name}, _isActive={_isActive}");

        if (!_isActive) return;

        if (other.TryGetComponent(out PoisonableFood poisonable))
        {
            poisonable.Poison();
            Debug.Log($"[SoapPoisonTrigger] {poisonable.name} è stato avvelenato.");
        }
        else
        {
            Debug.Log($"[SoapPoisonTrigger] {other.name} NON ha PoisonableFood component");
        }
    }

    // OPZIONALE: visualizza il collider nell'editor
    private void OnDrawGizmos()
    {
        if (_triggerCollider == null)
        {
            _triggerCollider = GetComponent<Collider>();
            if (_triggerCollider == null) return;
        }

        Gizmos.color = _isActive ? Color.green : Color.red;

        if (_triggerCollider is BoxCollider box)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(box.center, box.size);
        }
        else if (_triggerCollider is SphereCollider sphere)
        {
            Gizmos.DrawWireSphere(transform.position + sphere.center, sphere.radius);
        }
        else if (_triggerCollider is CapsuleCollider capsule)
        {
            Gizmos.DrawWireSphere(transform.position + capsule.center, capsule.radius);
        }
    }
}