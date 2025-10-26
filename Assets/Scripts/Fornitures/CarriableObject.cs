using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableObject : MonoBehaviour
{

    [SerializeField] private Vector3 _carryOffset = Vector3.zero;
    [SerializeField] private bool _keepWorldRotation = false;

    public bool IsCarried { get; private set; }

    public void OnPickedUp(Transform carryPoint)
    {
        if (carryPoint == null) return;

        IsCarried = true;
        transform.SetParent(carryPoint);

        transform.localPosition = _carryOffset;

        if (!_keepWorldRotation)
        {
            transform.localRotation = Quaternion.identity;
        }

    }

    public void OnDropped(Vector3 dropPosition)
    {
        IsCarried = false;
        transform.SetParent(null);
        transform.position = dropPosition;
    }

}



