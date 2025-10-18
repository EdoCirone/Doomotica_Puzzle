using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCleaner : MovingForniture
{
    [Header("pickup settings")]
    [SerializeField] private Transform _carryPoint;

    [Header("PassageEvent")]
    [SerializeField] private EventChannelCarriable _onTriggerChannel;

    private CarriableObject _carriedTray;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (_carriedTray != null)
        {
            Debug.Log($"Dropping tray {_carriedTray.name} ");
            DropTray();
        }

    }

    private void Update()
    {
        if (_carriedTray != null && _carryPoint != null)
        {
            _carriedTray.transform.position = _carryPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {


        if (_carriedTray == null && other.TryGetComponent<CarriableObject>(out CarriableObject tray))
        {
            Debug.Log($"Picking up tray {tray.name}");
            PickUpTray(tray);
        }

        if (other.TryGetComponent(out CharacterFSM character) && _carriedTray != null)
        {
            var data  = new CarriableData (_carriedTray, gameObject);
            _onTriggerChannel?.Raise(data); // notifica che il robot è entrato nel trigger e invia i dati del oggetto trasportato
        }
    }

    private void PickUpTray(CarriableObject tray)
    {
        _carriedTray = tray;
        tray.OnPickedUp(_carryPoint);
        tray.transform.localPosition = Vector3.zero;
        _onTriggerChannel?.Raise(new CarriableData (_carriedTray, gameObject));
    }

    public void DropTray()
    {
        if (_carriedTray != null)
        {
            _carriedTray.OnDropped(transform.position);
            _carriedTray.transform.SetParent(null);
            _carriedTray = null;
        }
        return;
    }

}


