using System.Collections;
using UnityEngine;

/// <summary>
/// FSM della donna nel livello 2.
/// Aspetta che il robot consegni il cibo, lo raccoglie, torna al divano e lo mangia.
/// Se il cibo è avvelenato → muore (vittoria), altrimenti → perdita.
/// </summary>
public class FSMDogWomanLVLTwo : CharacterFSM
{
    [Header("Punti di riferimento")]
    [SerializeField] private Transform _home;          // Divano/TV dove sta inizialmente
    [SerializeField] private Transform _carryPoint;    // Dove tiene il cibo in mano (es. "Hand")

    [Header("Parametri")]
    [SerializeField] private float _eatDuration = 3f;      // Tempo per mangiare (animazione)
    [SerializeField] private float _reactionDelay = 0.5f;  // Delay prima di andare a prendere il cibo

    private PoisonableFood _currentFood = null;  // Riferimento al cibo corrente
    private bool _isInteracting = false;         // Flag per evitare sovrapposizioni

    protected override void Start()
    {
        base.Start();
        _currentState = STATE.IDLE;

        // Si registra all'evento del robot quando rilascia il cibo
        Carrier robotCarrier = FindObjectOfType<Carrier>();
        if (robotCarrier != null)
        {
            robotCarrier.onDrop.AddListener(OnFoodDelivered);
        }
    }

    // ========================================
    // CALLBACK: Il robot ha consegnato il cibo
    // ========================================
    public void OnFoodDelivered(CarriableObject deliveredObject)
    {
        if (isDeath || _isInteracting) return;

        // Verifica che sia effettivamente cibo
        PoisonableFood food = deliveredObject.GetComponent<PoisonableFood>();
        if (food == null) return;

        _currentFood = food;
        StartCoroutine(ReactToDelivery());
    }

    private IEnumerator ReactToDelivery()
    {
        yield return new WaitForSeconds(_reactionDelay);
        SetState(STATE.WALK); // Va a prendere il cibo
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
        if (_currentFood == null)
        {
            SetState(STATE.IDLE);
            return;
        }

        // Si muove verso il cibo
        _mover?.MoveTo(_currentFood.transform);

        // Quando arriva vicino, lo raccoglie
        if (Vector3.Distance(transform.position, _currentFood.transform.position) < 0.8f)
        {
            SetState(STATE.INTERACT);
        }
    }

    protected override void InteractState()
    {
        if (_isInteracting) return;
        _isInteracting = true;
        StartCoroutine(PickupFoodRoutine());
    }

    protected override void ComeBackState()
    {
        if (_home == null)
        {
            SetState(STATE.IDLE);
            return;
        }

        // Torna al divano
        _mover?.MoveTo(_home);

        // Quando arriva, mangia
        if (Vector3.Distance(transform.position, _home.position) < 0.5f)
        {
            if (!_isInteracting)
            {
                _isInteracting = true;
                StartCoroutine(EatFoodRoutine());
            }
        }
    }

    protected override void DeathState()
    {
        base.DeathState();
        // Morte per avvelenamento → VITTORIA
    }

    // ========================================
    // ROUTINE: Raccoglie il cibo
    // ========================================
    private IEnumerator PickupFoodRoutine()
    {
        yield return new WaitForSeconds(0.3f);

        // Attacca il cibo al carry point
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
        SetState(STATE.COMEBACK); // Torna al divano
    }

    // ========================================
    // ROUTINE: Mangia e controlla avvelenamento
    // ========================================
    private IEnumerator EatFoodRoutine()
    {
        // Animazione mangiare (per ora solo pausa)
        yield return new WaitForSeconds(_eatDuration);

        // Controlla se il cibo era avvelenato
        if (_currentFood != null && _currentFood.IsPoisoned)
        {
            // Avvelenato → MUORE → Vittoria
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

        // Distrugge il cibo dopo averlo mangiato
        if (_currentFood != null)
        {
            Destroy(_currentFood.gameObject);
        }

        _currentFood = null;
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