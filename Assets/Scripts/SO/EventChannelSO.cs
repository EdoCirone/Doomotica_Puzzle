using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     La classe generica EventChannelSO<T> permette di creare ScriptableObject che fungono da canali di eventi.
/// </summary>
public abstract class EventChannelSO<T> : ScriptableObject
{

    public event System.Action<T> OnEventRaised;

    public virtual void Raise(T value)
    {
        OnEventRaised?.Invoke(value);
    }

    public virtual void OnDisable() 
    {
        OnEventRaised = null;
    }

}


/// <summary>
///     I miei SO basati sulla classe generica EventChannelSO<T>
/// </summary>


[CreateAssetMenu(fileName = "New Int Channel", menuName = "ScriptableObjects/EventChannels/Int")]
public class EventChannelInt : EventChannelSO<int> { }


[CreateAssetMenu(fileName = "New Forniture Channel", menuName = "ScriptableObjects/EventChannels/Forniture")]
public class EventChannelForniture : EventChannelSO<GenericForniture> { }


[CreateAssetMenu(fileName = "New Carriable Channel", menuName = "ScriptableObjects/EventChannels/Carriable")]
public class EventChannelCarriable : EventChannelSO<CarriableData> { }



//Mi faccio anche un Void Channel per eventi che non hanno bisogno di passare parametri
[System.Serializable] public struct Void { }

[CreateAssetMenu(fileName = "New Void Channel", menuName = "ScriptableObjects/EventChannels/Void")]
public class EventChannelVoid : EventChannelSO<Void> { }