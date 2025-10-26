using System.Collections;
using UnityEngine;

public class FSMDogWomanLVLTwo : CharacterFSM
{
    [Header("References")]
    [SerializeField] private Transform _home;             // posizione TV/sedia
    [SerializeField] private Transform _bowlSpot;         // posizione iniziale della ciotola
    [SerializeField] private Transform _dispenserSpot;    // dove posare la ciotola
    [SerializeField] private Transform _pizzaSpot;        // punto dove arriva la pizza
    [SerializeField] private CarriableObject _bowl;       // la ciotola
    [SerializeField] private Transform _carryPoint;       // punto dove la donna tiene la ciotola
    [SerializeField] private float _interactionTime = 2f; // tempo per interazioni

    private bool _heardBark = false;
    private bool _pizzaArrived = false;
    private bool _isInteracting = false;

    public bool isDistracted { get; private set; } = false;

    protected override void Start()
    {
        base.Start();
        isDistracted = false;
        _currentState = STATE.IDLE;
    }

    // ======================
    // EVENTI ESTERNI
    // ======================
    public void OnDogBark()
    {
        if (isDeath) return;
        _heardBark = true;
    }

    public void OnPizzaArrived()
    {
        if (isDeath) return;
        _pizzaArrived = true;
    }

    // ======================
    // STATI
    // ======================
    protected override void IdleState()
    {
        if (_heardBark && !isDistracted)
        {
            _heardBark = false;
            SetState(STATE.WALK);
        }

        if (_pizzaArrived && !isDistracted)
        {
            _pizzaArrived = false;
            SetState(STATE.INTERACT); // va verso la pizza
        }
    }

    protected override void WalkState()
    {
        if (_bowlSpot == null) return;

        _mover?.MoveTo(_bowlSpot);

        if (Vector3.Distance(transform.position, _bowlSpot.position) < 1f)
        {
            isDistracted = true;
            SetState(STATE.INTERACT); // raccoglie la ciotola
        }
    }

    protected override void InteractState()
    {
        if (_isInteracting) return;

        _isInteracting = true;

        // Decide quale interazione eseguire
        if (isDistracted && _bowl != null)
        {
            // Caso: sposta la ciotola sotto il dispenser
            StartCoroutine(MoveBowlRoutine());
        }
        else if (_pizzaSpot != null)
        {
            // Caso: va a prendere la pizza e muore
            StartCoroutine(FetchPizzaRoutine());
        }
    }

    protected override void ComeBackState()
    {
        if (_home == null) return;

        _mover?.MoveTo(_home);

        if (Vector3.Distance(transform.position, _home.position) < 0.5f)
        {
            SetState(STATE.IDLE);
            isDistracted = false;
        }
    }

    protected override void DeathState()
    {
        base.DeathState();
        Debug.Log("Donna morta (FSMDogWomanLVLTwo)");
    }

    // ======================
    // ROUTINE: sposta ciotola
    // ======================
    private IEnumerator MoveBowlRoutine()
    {
        // Raccoglie la ciotola
        if (_bowl != null && _carryPoint != null)
        {
            _bowl.OnPickedUp(_carryPoint);
            Debug.Log("Donna raccoglie la ciotola");
        }

        yield return new WaitForSeconds(0.5f);

        // Si muove verso il dispenser
        if (_dispenserSpot != null)
        {
            while (Vector3.Distance(transform.position, _dispenserSpot.position) > 0.6f)
            {
                _mover.MoveTo(_dispenserSpot);
                yield return null;
            }

            // Posiziona la ciotola
            _bowl.OnDropped(_dispenserSpot.position);
            Debug.Log("Donna posa la ciotola sotto il dispenser");
        }

        yield return new WaitForSeconds(_interactionTime);
        _isInteracting = false;
        SetState(STATE.COMEBACK);
    }

    // ======================
    // ROUTINE: prende la pizza e muore
    // ======================
    private IEnumerator FetchPizzaRoutine()
    {
        Debug.Log("Donna va a prendere la pizza...");

        // Si muove verso la pizza
        if (_pizzaSpot != null)
        {
            while (Vector3.Distance(transform.position, _pizzaSpot.position) > 0.6f)
            {
                _mover.MoveTo(_pizzaSpot);
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
        SetState(STATE.DEATH); // muore quando arriva
    }
}
