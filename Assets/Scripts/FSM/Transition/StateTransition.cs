using UnityEngine;

///<summary>
/// Rappresenta una transizione tra due stati in una macchina a stati finiti (FSM) per un personaggio.
/// Contiene lo stato di partenza, lo stato di arrivo e la condizione che deve essere soddisfatta.
///</summary>

[System.Serializable]
public class StateTransition
{

    public CharacterStateSO fromState;
    public CharacterStateSO toState;

    public TransitionConditionSO condition;

    public bool CanTransition(CharacterFSM character)
    {
        bool isInFromState = (fromState == null || fromState == character.CurrentState); // Check se lo stato attuale è lo stato di partenza
    
        bool conditionMet = (condition != null && condition.CanTransition(character)); // Check se la condizione è soddisfatta (o se non c'è condizione)

        return isInFromState && conditionMet; // Permette la transizione solo se entrambe le condizioni sono vere

    }


}
