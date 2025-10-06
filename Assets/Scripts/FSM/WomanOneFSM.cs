using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanOneFSM : CharacterFSM
{
    [SerializeField] private Transform _home;
    [SerializeField] private Transform _distraction;
    [SerializeField] private float _interactionTime = 2f;
    [SerializeField] private GenericForniture _kitchen;
    [SerializeField] private GenericForniture _tv;

    private bool _isInteracting = false;

    public bool isDistracted { get; private set; } = false;

    protected override void Start()
    {
        base.Start();
        _currentState = STATE.IDLE;
        isDistracted = false;

        if (_tv == null) { Debug.Log("Assegna la tv"); }
 
        if (_kitchen == null) { Debug.Log("Assegna la tv"); }

    }

    protected override void IdleState()
    {
        if (_animator != null)
        {
            _animator.Play("Idle");
        }
        if (_kitchen.IsON == true && !isDistracted)
        {
            if (_animator != null) _animator.Play("TurnOff");

            _kitchen.SetIsON(false);
        }

        if (_tv?.IsON == true && !isDistracted)
        {
            SetState(STATE.WALK);
        }
    }
    protected override void WalkState()
    {
        if (_mover != null && _distraction != null)
        {
            _mover.MoveTo(_distraction);
            if (Vector3.Distance(transform.position, _distraction.position) < 1.0f)
            {
                isDistracted = true;
                SetState(STATE.INTERACT);
            }
        }
    }

    protected override void InteractState()
    {
        if (_isInteracting == true) return;
        _isInteracting = true;
        StartCoroutine(InteractWithForniture(_interactionTime));
    }

    protected override void ComeBackState()
    {
       
        if (_mover != null && _home != null)
        {
            _mover.MoveTo(_home);
            if (Vector3.Distance(transform.position, _home.position) < 0.5f)
            {
                if (_kitchen.IsON == false)
                {
                    SetState(STATE.IDLE);
                    isDistracted = false;
                }
                if (_kitchen.IsON == true && isDistracted)
                {
                    SetState(STATE.DEATH);
                }
            }
        }
    }
    private IEnumerator InteractWithForniture(float waitTime)
    {
        if (_tv != null)
        {
            _tv.SetIsON(false);
            _animator?.Play("Interaction");

        }

        yield return new WaitForSeconds(waitTime);

        _isInteracting = false;
        SetState(STATE.COMEBACK);
    }

}
