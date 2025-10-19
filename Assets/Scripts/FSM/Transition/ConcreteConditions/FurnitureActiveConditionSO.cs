using UnityEngine;

// non capisco se devo serializzare il sistema per vedere le proprietà nell'inspector
[CreateAssetMenu(fileName = "FornitureActiveCondition", menuName = "ScriptableObjects/FSM/Conditions/Furniture Active")]
public class FurnitureActiveConditionSO : TransitionConditionSO
{
    public GenericForniture targetForniture;

    public bool shouldBeActive = true;

    public override bool CanTransition(CharacterFSM character)
    {
        if (targetForniture == null)
        {
            Debug.LogWarning("Target forniture is not assigned in FurnitureActiveConditionSO");
            return false;
        }

        return targetForniture.IsON == shouldBeActive;
    }
}
