using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericForniture : MonoBehaviour
{
    [SerializeField] private bool _isON;
    [SerializeField] private EventChannelForniture _activationChannel;

    [SerializeField] private UnityEvent _onActivate;
    [SerializeField] private UnityEvent _onDeactivate;

    public event System.Action<GenericForniture> OnDeactivate;
    public event System.Action<GenericForniture> OnActivate;

    public bool IsON => _isON;
    public Collider EffectCollider { get; private set; }

    public void SetIsON(bool value)
    {
        //if (_isON == value) return;

        _isON = value;
        if (value)
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

        if (_isON == true) return;
        SetIsON(true);
    }



}
