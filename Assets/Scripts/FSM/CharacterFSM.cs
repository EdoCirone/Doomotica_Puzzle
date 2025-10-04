using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterFSM : MonoBehaviour
{
    [SerializeField] protected STATE _currentState;

    public bool isDeath { get; protected set; } = false;

    protected Animator _animator;
    protected NavMeshMovement _mover;

    protected virtual void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _mover = GetComponent<NavMeshMovement>();
        _currentState = STATE.IDLE;
    }

    protected virtual void Update()
    {
        StateMachine(_currentState);
    }

    protected void StateMachine(STATE newState)
    {
        switch (newState)
        {
            case STATE.IDLE:
                IdleState();
                break;
            case STATE.WALK:
                WalkState();
                break;
            case STATE.INTERACT:
                InteractState();
                break;
            case STATE.COMEBACK:
                ComeBackState();
                break;
            case STATE.DEATH:
                DeathState();
                break;
            default:
                Debug.LogWarning("Stato non gestito: " + newState);
                break;
        }
    }

    protected abstract void IdleState();
    protected abstract void WalkState();
    protected abstract void InteractState();
    protected abstract void ComeBackState();
    protected virtual void DeathState()
    {
        if (isDeath) return;
        isDeath = true;
        if (_animator != null)
        {
            _animator.Play("Death");
        }
        if (_mover != null)
        {
            _mover.SetSpeed(0);
        }

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = Color.red;
        }

        LVLManager.Instance.CheckCharacterAlive();
    }

    protected void SetState(STATE newState)
    {
        if (isDeath && newState != STATE.DEATH) return;
        _currentState = newState;
    }

}


