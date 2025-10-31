using System.Collections;
using UnityEngine;

/// <summary>
/// FSM della donna nel livello 2.
/// - Aspetta che il robot consegni il cibo → lo prende e mangia (vittoria se avvelenato)
/// - Se il cane abbaia → va a prendere la ciotola e la mette sotto il dispenser
/// </summary>
public class FSMDogWomanLVLTwo : CharacterFSM
{
    [Header("Punti di riferimento")]
    [SerializeField] private Transform _home;              // Divano dove sta inizialmente
    [SerializeField] private Transform _carryPoint;        // Dove tiene gli oggetti in mano
    [SerializeField] private Transform _bowlSpot;          // Dove sta la ciotola inizialmente
    [SerializeField] private Transform _dispenserSpot;     // Dove posare la ciotola (sotto il dispenser)

    [Header("Oggetti")]
    [SerializeField] private CarriableObject _bowl;        // La ciotola del cane

    [Header("Parametri")]
    [SerializeField] private float _eatDuration = 3f;
    [SerializeField] private float _reactionDelay = 0.5f;
    [SerializeField] private float _interactionTime = 1f;  // Tempo per posare/prendere oggetti

    private PoisonableFood _currentFood = null;
    private bool _isInteracting = false;
    private TaskType _currentTask = TaskType.None;  // Cosa sta facendo la donna

    // Enum per distinguere le azioni
    private enum TaskType
    {
        None,
        FetchingFood,    // Sta andando a prendere il cibo del robot
        MovingBowl       // Sta spostando la ciotola del cane
    }

    protected override void Start()
    {
        base.Start();
        _currentState = STATE.IDLE;

        // Si registra all'evento del robot
        Carrier robotCarrier = FindObjectOfType<Carrier>();
        if (robotCarrier != null)
        {
            robotCarrier.onDrop.AddListener(OnFoodDelivered);
        }
    }

    // ========================================
    // EVENTI ESTERNI
    // ========================================

    /// <summary>
    /// Chiamato quando il cane abbaia
    /// </summary>
    public void OnDogBark()
    {
        if (isDeath || _isInteracting) return;
        if (_currentState != STATE.IDLE) return; // Ignora se sta già facendo qualcosa

        _currentTask = TaskType.MovingBowl;
        StartCoroutine(ReactToBark());
    }

    /// <summary>
    /// Chiamato quando il robot consegna il cibo
    /// </summary>
    public void OnFoodDelivered(CarriableObject deliveredObject)
    {
        if (isDeath || _isInteracting) return;
        if (_currentState != STATE.IDLE) return;

        PoisonableFood food = deliveredObject.GetComponent<PoisonableFood>();
        if (food == null) return;

        _currentFood = food;
        _currentTask = TaskType.FetchingFood;
        StartCoroutine(ReactToDelivery());
    }

    private IEnumerator ReactToBark()
    {
        yield return new WaitForSeconds(_reactionDelay);
        SetState(STATE.WALK); // Va verso la ciotola
    }

    private IEnumerator ReactToDelivery()
    {
        yield return new WaitForSeconds(_reactionDelay);
        SetState(STATE.WALK); // Va verso il cibo
    }

    // ========================================
    // STATI
    // ========================================

    protected override void IdleState()
    {
        // Aspetta sul divano
    }

    protected override void WalkState()
    {
        // Destinazione diversa in base al task corrente
        if (_currentTask == TaskType.MovingBowl)
        {
            WalkToBowl();
        }
        else if (_currentTask == TaskType.FetchingFood)
        {
            WalkToFood();
        }
    }

    protected override void InteractState()
    {
        if (_isInteracting) return;
        _isInteracting = true;

        // Interazione diversa in base al task
        if (_currentTask == TaskType.MovingBowl)
        {
            StartCoroutine(MoveBowlSequence());
        }
        else if (_currentTask == TaskType.FetchingFood)
        {
            StartCoroutine(PickupFoodRoutine());
        }
    }

    protected override void ComeBackState()
    {
        if (_home == null)
        {
            SetState(STATE.IDLE);
            return;
        }

        _mover?.MoveTo(_home);

        if (Vector3.Distance(transform.position, _home.position) < 0.5f)
        {
            // Se ha il cibo, lo mangia
            if (_currentTask == TaskType.FetchingFood && !_isInteracting)
            {
                _isInteracting = true;
                StartCoroutine(EatFoodRoutine());
            }
            // Se ha finito con la ciotola, torna in idle
            else if (_currentTask == TaskType.MovingBowl)
            {
                _currentTask = TaskType.None;
                _isInteracting = false;
                SetState(STATE.IDLE);
            }
        }
    }

    protected override void DeathState()
    {
        base.DeathState();
        // Morte per avvelenamento → VITTORIA
    }

    // ========================================
    // LOGICA: Cammina verso la ciotola
    // ========================================
    private void WalkToBowl()
    {
        if (_bowlSpot == null)
        {
            SetState(STATE.IDLE);
            return;
        }

        _mover?.MoveTo(_bowlSpot);

        if (Vector3.Distance(transform.position, _bowlSpot.position) < 0.8f)
        {
            SetState(STATE.INTERACT); // Raccoglie la ciotola
        }
    }

    // ========================================
    // LOGICA: Cammina verso il cibo
    // ========================================
    private void WalkToFood()
    {
        if (_currentFood == null)
        {
            SetState(STATE.IDLE);
            return;
        }

        _mover?.MoveTo(_currentFood.transform);

        if (Vector3.Distance(transform.position, _currentFood.transform.position) < 0.8f)
        {
            SetState(STATE.INTERACT); // Raccoglie il cibo
        }
    }

    // ========================================
    // ROUTINE: Sposta la ciotola (sequenza completa)
    // ========================================
    private IEnumerator MoveBowlSequence()
    {
        // 1. Raccoglie la ciotola
        yield return new WaitForSeconds(_interactionTime);

        if (_bowl != null && _carryPoint != null)
        {
            _bowl.OnPickedUp(_carryPoint);
        }

        yield return new WaitForSeconds(0.3f);

        // 2. Va verso il dispenser
        _isInteracting = false;
        while (Vector3.Distance(transform.position, _dispenserSpot.position) > 0.8f)
        {
            _mover?.MoveTo(_dispenserSpot);
            yield return null;
        }

        // 3. Posa la ciotola sotto il dispenser
        _isInteracting = true;
        yield return new WaitForSeconds(_interactionTime);

        if (_bowl != null)
        {
            _bowl.OnDropped(_dispenserSpot.position);
        }

        yield return new WaitForSeconds(0.3f);

        // 4. Torna al divano
        _isInteracting = false;
        SetState(STATE.COMEBACK);
    }

    // ========================================
    // ROUTINE: Raccoglie il cibo
    // ========================================
    private IEnumerator PickupFoodRoutine()
    {
        yield return new WaitForSeconds(_interactionTime);

        if (_currentFood != null && _carryPoint != null)
        {
            CarriableObject carriable = _currentFood.GetComponent<CarriableObject>();
            if (carriable != null)
            {
                carriable.OnPickedUp(_carryPoint);
            }
        }

        yield return new WaitForSeconds(0.3f);

        _isInteracting = false;
        _currentTask = TaskType.FetchingFood; // Mantieni il compito
        SetState(STATE.COMEBACK);
    }


    // ========================================
    // ROUTINE: Mangia il cibo
    // ========================================
    private IEnumerator EatFoodRoutine()
    {
        yield return new WaitForSeconds(_eatDuration);

        if (_currentFood != null && _currentFood.IsPoisoned)
        {
            // Avvelenato → MUORE → Vittoria
            Debug.Log("[FSMDogWomanLVLTwo] Cibo avvelenato! La donna muore.");
            SetState(STATE.DEATH);
        }
        else
        {
            // Cibo pulito → PERDITA
            if (LVLManager.Instance != null)
            {
                LVLManager.Instance.RegisterLose();
            }
        }

        if (_currentFood != null)
        {
            Destroy(_currentFood.gameObject);
        }

        _currentFood = null;
        _currentTask = TaskType.None;
        _isInteracting = false;
    }

    // ========================================
    // CLEANUP
    // ========================================
    private void OnDestroy()
    {
        Carrier robotCarrier = FindObjectOfType<Carrier>();
        if (robotCarrier != null)
        {
            robotCarrier.onDrop.RemoveListener(OnFoodDelivered);
        }
    }
}