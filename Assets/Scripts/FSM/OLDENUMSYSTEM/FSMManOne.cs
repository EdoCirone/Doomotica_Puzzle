using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManOne : OldCharacterFSM
{
    [SerializeField] private Transform _home;
    [SerializeField] private Transform _distraction;
    [SerializeField] private float _interactionTime = 2f;
    [SerializeField] private GenericForniture _alexa;
    [SerializeField] private GenericForniture _freezer;
    [SerializeField] private WomanOneFSM _woman;

    private bool _isInteracting = false;
    private bool _isBooster = false;
    private float _resetSpeed;

    public bool isDistracted { get; private set; } = false;

    protected override void Start()
    {
        base.Start();
        isDistracted = false;
        _resetSpeed = _mover.GetSpeed();

    }

    protected override void IdleState()
    {
        _mover?.SetSpeed(_resetSpeed);
        _isBooster = false;

        if (_animator != null)
        {
            _animator.Play("Idle");
        }

        if (_alexa != null && _alexa.IsON && !isDistracted)
        {
            SetState(STATE.WALK);
        }
    }

    protected override void WalkState()
    {
        _mover?.MoveTo(_distraction);

        if (_woman.isDeath == true && !_isBooster)
        {
            _isBooster = true;
            _mover?.SetSpeed(_mover.GetSpeed() * 1.2f);

            
        }

        if (Vector3.Distance(transform.position, _distraction.position) < 1.5f)
        {
            isDistracted = true;
            SetState(STATE.INTERACT);
        }
    }

    protected override void ComeBackState()
    {
        _isBooster = false;
        _mover?.MoveTo(_home);
        if (Vector3.Distance(transform.position, _home.position) < 1.5f)
        {
            if (!_alexa.IsON)
            {
                SetState(STATE.IDLE);
                isDistracted = false;
            }
        }
        _alexa.SetIsON(false);
    }
    protected override void InteractState()
    {

        if (_isInteracting == true) return;

        if (_woman.isDeath)
        {
            LVLManager.Instance.RegisterLose();
        }

        _isInteracting = true;
        StartCoroutine(InteractWithForniture(_interactionTime));

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Hazard"))
        {

            if (_isBooster)
                SetState(STATE.DEATH);

            Debug.Log("Sono entrato nel  Trigger di " + other.name);
        }
    }


    private IEnumerator InteractWithForniture(float waitTime)
    {
 
        _alexa.SetIsON(false);

        yield return new WaitForSeconds(waitTime);
        _isInteracting = false;
        SetState(STATE.COMEBACK);
    }
}
