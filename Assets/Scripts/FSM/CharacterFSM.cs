using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// FSM vera e propria data-driven. Stati e Transizioni sono definiti in asset esterni (ScriptableObject) e configurati nell'Inspector.
/// </summary>

public class CharacterFSM : MonoBehaviour
{
    [Header("FSM Transitions conf")]

    [SerializeField]
    [Tooltip("Initial State of the FSM.")]
    private CharacterStateSO initialState; // Stato iniziale della FSM

    [SerializeField]
    [Tooltip("Possible Transitions")]
    private List<StateTransition> transitions; // Lista di transizioni possibili

    [SerializeField] private CharacterStateSO currentState; // Stato attuale della FSM serializzato per il debug nell'Inspector

    [Header("Event Listener")]
    [SerializeField]
    [Tooltip("Event Listener for interaction with ambient.")]
    List<EventChannelForniture> _channelsToListen;
    private Dictionary<EventChannelForniture, TransitionConditionSO> _eventTransitions;


    //Componenti accessibili dagli stati e dalle condizioni
    public Animator Animator { get; private set; }
    public NavMeshMovement Mover { get; private set; }
    public CharacterStateSO CurrentState => currentState;

    //Flag di Stato

    public bool IsDeath { get; private set; } = false;

    public bool IsDistracted { get; set; } = false;

    private bool isInteractionComplete = false;
    public bool IsInteractionComplete => isInteractionComplete;

    private bool _isTransitioning = false;

    //EVENTI
    public event Action<OldCharacterFSM> OnCharacterDeath;



    // START E UPDATE (EVENTUALI CICLI DI UNITY TIPO AWAKE)

    private void OnEnable()
    {
        _eventTransitions = new Dictionary<EventChannelForniture, TransitionConditionSO>();

        foreach(var channel in _channelsToListen)
        {
            channel.OnEventRaised += OnFornitureEvent;
        }
    }

    private void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        Mover = GetComponent<NavMeshMovement>();

        if (initialState != null)
        {
            ChangeState(initialState);
        }
        else
        {
            Debug.LogError("Initial State is not assigned in CharacterFSM.");
        }
    }

    private void Update()
    {
        currentState?.OnUpdate(this);

        CheckTransitions();
    }

    // GESTIONE DEGLI STATI


    private void CheckTransitions()
    {
        if (_isTransitioning) return;

        foreach (var transition in transitions)
        {
            if (transition.fromState != currentState) continue; // Salta le transizioni che non partono dallo stato attuale
            if (transition.CanTransition(this))
            {
                _isTransitioning = true;
                ChangeState(transition.toState);
                _isTransitioning = false; 
                break;
            }
        }
    }

    private void ChangeState(CharacterStateSO newState)
    {
        if (newState == null) return; // Se lo stato è lo stesso, non fare nulla

        if (IsDeath && !(newState is DeathStateSO))
        {
            Debug.LogWarning("Character is dead and cannot change to a non-death state.");
            return; // Non permettere il cambio di stato se il personaggio è morto e il nuovo stato non è DEATH
        }

        Debug.Log($"{gameObject.name} Transitioning from {currentState?.name ?? "None"} to {newState.name}");

        currentState?.OnExit(this); // Chiamata al metodo OnExit dello stato attuale, se esiste

        currentState = newState; // Aggiorna lo stato attuale

        currentState?.OnEnter(this); // Chiamata al metodo OnEnter del nuovo stato, se esiste
    }

    // informazioni di stato pubbliche

    public void Die()
    {
        if (IsDeath) return;
        IsDeath = true;

        foreach (var transition in transitions)
        {
            if (transition.toState is DeathStateSO)
            {
                ChangeState(transition.toState);
                return; // Esce dopo aver cambiato stato
            }
        }

        Debug.LogWarning($"{gameObject.name}No transition to DeathStateSO found in transitions list.");
    }

    public void OnFornitureEvent(GenericForniture forniture)
    {
        Debug.Log($"{gameObject.name} received Forniture event: {forniture.name}");

        foreach (var transition in transitions)
        {
            if (transition.condition is FurnitureActiveConditionSO fornitureCondition)
            {
                if(fornitureCondition.targetForniture == forniture)
                {
                    Debug.Log($"{gameObject.name} Found matching transition for forniture event: {forniture.name}");
                    if (transition.CanTransition(this))
                    {
                        Debug.Log($"{gameObject.name} Transitioning due to forniture event: {forniture.name}");
                        ChangeState(transition.toState);
                        return; // Esce dopo aver cambiato stato
                    }
                }
            }
        }

    }

    public void SetInterectionComplete(bool value)
    {
        isInteractionComplete = value;
    }

    public void ResetInteractionComplete()
    {
        isInteractionComplete = false;
    }
}

