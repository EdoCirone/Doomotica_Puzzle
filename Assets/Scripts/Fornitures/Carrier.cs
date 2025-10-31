using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Componente generica per oggetti che raccolgono e trasportano CarriableObject.
/// Non conosce nulla del livello: emette solo eventi su Pickup e Drop.
/// </summary>
public class Carrier : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private Transform _carryPoint;     // punto dove tiene l'oggetto
    [SerializeField] private Transform _dropSpot;       // punto dove lo lascia
    [SerializeField] private float _pickupRange = 1.2f; // distanza di raccolta
    [SerializeField] private float _dropRange = 1.0f;   // distanza per rilascio

    [Header("Eventi")]
    public UnityEvent<CarriableObject> onPickup;
    public UnityEvent<CarriableObject> onDrop;

    private CarriableObject _carriedObject;
    private bool _hasDropped = false;

    private void Update()
    {
        if (_carriedObject == null)
        {
            TryPickup();
        }
        else
        {
            // Mantiene l'oggetto in posizione
            _carriedObject.transform.position = _carryPoint.position;

            // Se vicino al punto di rilascio, lascia l’oggetto
            if (_dropSpot != null && !_hasDropped &&
                Vector3.Distance(transform.position, _dropSpot.position) < _dropRange)
            {
                DropObject();
                _hasDropped = true;
            }
        }
    }

    private void TryPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _pickupRange);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out CarriableObject carriable))
            {
                _carriedObject = carriable;
                _carriedObject.OnPickedUp(_carryPoint);
                onPickup?.Invoke(_carriedObject);
                Debug.Log($"[Carrier] Raccolto: {_carriedObject.name}");
                return;
            }
        }
    }

    private void DropObject()
    {
        if (_carriedObject == null) return;

        _carriedObject.OnDropped(_dropSpot.position);
        onDrop?.Invoke(_carriedObject);
        Debug.Log($"[Carrier] Droppato: {_carriedObject.name} a {_dropSpot.name}");

        _carriedObject = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _pickupRange);
        if (_dropSpot != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_dropSpot.position, _dropRange);
        }
    }
}
