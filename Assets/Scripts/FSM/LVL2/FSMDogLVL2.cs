using System.Collections;
using UnityEngine;

/// <summary>
/// FSM del cane nel livello 2.
/// - Abbaia quando Alexa si attiva → la donna sposta la ciotola
/// - Se la ciotola passa vicino → la prende, la riporta, mangia
/// - Se il cibo è avvelenato → muore (perdita), altrimenti → torna in idle
/// </summary>
public class FSMDogLVLTwo : CharacterFSM
{
    [Header("Punti di riferimento")]
    [SerializeField] private Transform _home;              // Dove sta il cane inizialmente
    [SerializeField] private Transform _carryPoint;        // Dove tiene la ciotola (bocca)
    [SerializeField] private Transform _bowlOriginalSpot;  // Dove riportare la ciotola

    [Header("Oggetti")]
    [SerializeField] private CarriableObject _bowl;        // La ciotola

    [Header("Parametri")]
    [SerializeField] private float _detectionRadius = 3f;  // Distanza per rilevare la ciotola
    [SerializeField] private float _eatDuration = 2f;      // Tempo per mangiare
    [SerializeField] private float _interactionTime = 1f;  // Tempo per prendere/posare
    [SerializeField] private float _checkInterval = 0.5f;  // Intervallo controllo ciotola vicina

    //[SerializeField] private GenericForniture _alexa;

    private bool _isInteracting = false;
    private bool _hasBowl = false;
    private float _checkTimer = 0f;

    protected override void Start()
    {
        base.Start();
        _currentState = STATE.IDLE;

        //if (_alexa != null)
        //    Debug.LogWarning("Non hai assegnato alexa al cane");
                
    }

   protected override void Update()
    {
        base.Update();
        // Controlla periodicamente se la ciotola è vicina (solo in IDLE)
        if (_currentState == STATE.IDLE && !_isInteracting)
        {
            _checkTimer += Time.deltaTime;
            if (_checkTimer >= _checkInterval)
            {
                _checkTimer = 0f;
                CheckBowlNearby();
            }
        }
    }

    // ========================================
    // EVENTI ESTERNI
    // ========================================

    /// <summary>
    /// Chiamato quando Alexa si attiva
    /// </summary>
    public void OnAlexaActivated()
    {
        if (isDeath) return;
        StartCoroutine(BarkRoutine());
    }

    private IEnumerator BarkRoutine()
    {
        Debug.Log("[Dog] Abbaia!");

        // Qui puoi attivare animazione/audio dell'abbaiare
        yield return new WaitForSeconds(0.5f);

        // Notifica la donna
        FSMDogWomanLVLTwo woman = FindObjectOfType<FSMDogWomanLVLTwo>();
        if (woman != null)
        {
            woman.OnDogBark();
        }
    }

    // ========================================
    // RILEVAMENTO CIOTOLA VICINA
    // ========================================
    private void CheckBowlNearby()
    {
        if (_bowl == null) return;
        if (_bowl.IsCarried) return; // Se già presa da qualcuno, ignora

        float distance = Vector3.Distance(transform.position, _bowl.transform.position);

        if (distance <= _detectionRadius)
        {
            Debug.Log("[Dog] Ciotola rilevata vicino!");
            SetState(STATE.WALK); // Va a prenderla
        }
    }

    // ========================================
    // STATI
    // ========================================

    protected override void IdleState()
    {
        // Aspetta e controlla se la ciotola si avvicina
    }

    protected override void WalkState()
    {
        if (_hasBowl)
        {
            // Sta riportando la ciotola al posto originale
            WalkToOriginalSpot();
        }
        else
        {
            // Sta andando a prendere la ciotola
            WalkToBowl();
        }
    }

    protected override void InteractState()
    {
        if (_isInteracting) return;
        _isInteracting = true;

        if (_hasBowl)
        {
            // Ha la ciotola → la posa e mangia
            StartCoroutine(PlaceBowlAndEatRoutine());
        }
        else
        {
            // Non ha la ciotola → la prende
            StartCoroutine(PickupBowlRoutine());
        }
    }

    protected override void ComeBackState()
    {
        // Non usato per il cane in questo caso
    }

    protected override void DeathState()
    {
        base.DeathState();
        Debug.Log("[Dog] Cane morto per avvelenamento!");

        // Notifica la perdita
        if (LVLManager.Instance != null)
        {
            LVLManager.Instance.RegisterLose();
        }
    }

    // ========================================
    // LOGICA: Vai verso la ciotola
    // ========================================
    private void WalkToBowl()
    {
        if (_bowl == null)
        {
            SetState(STATE.IDLE);
            return;
        }

        _mover?.MoveTo(_bowl.transform);

        if (Vector3.Distance(transform.position, _bowl.transform.position) < 0.8f)
        {
            SetState(STATE.INTERACT); // Prende la ciotola
        }
    }

    // ========================================
    // LOGICA: Vai verso il posto originale della ciotola
    // ========================================
    private void WalkToOriginalSpot()
    {
        if (_bowlOriginalSpot == null)
        {
            SetState(STATE.IDLE);
            return;
        }

        _mover?.MoveTo(_bowlOriginalSpot);

        if (Vector3.Distance(transform.position, _bowlOriginalSpot.position) < 0.8f)
        {
            SetState(STATE.INTERACT); // Posa e mangia
        }
    }

    // ========================================
    // ROUTINE: Prende la ciotola
    // ========================================
    private IEnumerator PickupBowlRoutine()
    {
        Debug.Log("[Dog] Prende la ciotola");
        yield return new WaitForSeconds(_interactionTime);

        if (_bowl != null && _carryPoint != null)
        {
            _bowl.OnPickedUp(_carryPoint);
            _hasBowl = true;
        }

        yield return new WaitForSeconds(0.3f);

        _isInteracting = false;
        SetState(STATE.WALK); // Torna al posto originale
    }

    // ========================================
    // ROUTINE: Posa la ciotola e mangia
    // ========================================
    private IEnumerator PlaceBowlAndEatRoutine()
    {
        Debug.Log("[Dog] Posa la ciotola");
        yield return new WaitForSeconds(_interactionTime);

        // Posa la ciotola
        if (_bowl != null && _bowlOriginalSpot != null)
        {
            _bowl.OnDropped(_bowlOriginalSpot.position);
            _hasBowl = false;
        }

        yield return new WaitForSeconds(0.5f);

        // Mangia
        Debug.Log("[Dog] Mangia dalla ciotola");
        yield return new WaitForSeconds(_eatDuration);

        // Controlla se il cibo nella ciotola era avvelenato
        PoisonableFood poisonableFood = _bowl.GetComponent<PoisonableFood>();
        if (poisonableFood != null && poisonableFood.IsPoisoned)
        {
            Debug.Log("[Dog] Il cibo era avvelenato! Muore...");
            SetState(STATE.DEATH); // PERDITA
        }
        else
        {
            Debug.Log("[Dog] Cibo pulito, torna in idle");
            _isInteracting = false;
            SetState(STATE.IDLE);
        }
    }

    // ========================================
    // DEBUG
    // ========================================
    private void OnDrawGizmosSelected()
    {
        // Disegna il raggio di rilevamento
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        // Disegna linea verso la ciotola se presente
        if (_bowl != null)
        {
            Gizmos.color = _hasBowl ? Color.green : Color.yellow;
            Gizmos.DrawLine(transform.position, _bowl.transform.position);
        }
    }
}