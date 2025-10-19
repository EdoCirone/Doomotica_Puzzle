using UnityEngine;

[CreateAssetMenu(fileName = "FornitureAndCondition", menuName = "ScriptableObjects/FSM/Conditions/FurnitureAnd")]
public class AndConditionSO : TransitionConditionSO
{
    public TransitionConditionSO[] conditions; // le condizioni che devono essere tutte vere in contemporanea

    public override bool CanTransition(CharacterFSM character)
    {
        if (conditions == null || conditions.Length == 0)
        {
            Debug.LogWarning("No conditions assigned in AndConditionSO");
            return false;
        }

        foreach (var condition in conditions)
        {
            if (condition == null || !condition.CanTransition(character))
            {
                return false;
            }
        }

        return true;
    }

}
