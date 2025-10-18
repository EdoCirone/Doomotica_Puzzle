using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableObject : MonoBehaviour
{
    [SerializeField] private HazardSO _currentHazard;
    public HazardSO CurrentHazard => _currentHazard;
    public bool IsCarried { get; private set; }

    public void OnPickedUp(Transform carryPoint)
    {
        IsCarried = true;
        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;

    }

    public void OnDropped( Vector3 dropPosition)
    {
        IsCarried = false;
        transform.SetParent(null);
        transform.position = dropPosition;

    }

    public void Contaminate(HazardSO hazard)
    {
        if (hazard == null) return;
        _currentHazard = hazard;
        hazard.ApplyVisuals(gameObject);
        Debug.Log($"{gameObject.name} contaminated with {hazard.hazardName}");
    }

    public bool IsHazardous()
    {
        return _currentHazard != null;
    }
}



