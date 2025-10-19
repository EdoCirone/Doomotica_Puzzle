using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GenericForniture : MonoBehaviour
{
    [SerializeField] private bool _isON;
    [SerializeField] private EventChannelForniture _activationChannel;

    [SerializeField] private Transform _interactionPoint;

    [SerializeField] private UnityEvent _onActivate;
    [SerializeField] private UnityEvent _onDeactivate;

    [SerializeField] private HazardSO _currentHazard;

    [SerializeField] private bool _canSetOff;

    public event System.Action<GenericForniture> OnDeactivate;
    public event System.Action<GenericForniture> OnActivate;

    public bool IsON => _isON;
    public Collider EffectCollider { get; private set; }
    public HazardSO CurrentHazard => _currentHazard;


    public void SetIsON(bool value)
    {
        if (_isON == value) return;

        _isON = value;

        if (_isON)
        {
            _onActivate?.Invoke(); //Logica per effetti sonori o visivi
            OnActivate?.Invoke(this); //Logica per gestione FSM
            _activationChannel?.Raise(this);
        }
        else
        {
            _onDeactivate?.Invoke();
            OnDeactivate?.Invoke(this);
            _activationChannel?.Raise(this);
        }

    }
    protected virtual void OnMouseDown()
    {

        if (_isON && _canSetOff)
        {
            SetIsON(false);
        }
        else if (!_isON)
        {
            SetIsON(true);
        }
    }

    public void Contaminate(HazardSO hazard)
    {
        if (hazard == null) return;
        _currentHazard = hazard;
        hazard.ApplyVisuals(gameObject);
        Debug.Log($"{gameObject.name} contaminated with {hazard.hazardName}");

        if (!_isON)
        {
            SetIsON(true);
        }
    }

    public bool IsHazardous()
    {
        return _currentHazard != null;
    }

    public void Decontaminate()
    {
        if (_currentHazard != null)
        {

            _currentHazard.RevertVisuals(gameObject);
            _currentHazard = null;
        }

        return;
    }

}
