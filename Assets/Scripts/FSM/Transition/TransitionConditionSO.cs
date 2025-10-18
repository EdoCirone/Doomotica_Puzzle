using UnityEngine;

/// <summary>
///  Classe astratta dalla quale deriva la condizione per la quale si cambia di stato   
/// </summary>

public abstract class TransitionConditionSO : ScriptableObject
{
    //semplice metodo che ritorna true o false a seconda se la condizione è soddisfatta o no, lo implementiamo nelle derivate
    public abstract bool CanTransition(CharacterFSM character);

}
