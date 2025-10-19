using UnityEngine;

[CreateAssetMenu(fileName = "ReachedDestinationCondition", menuName = "ScriptableObjects/FSM/Conditions/ReachedDestination")]
public class ReachedDestinationConditionSO : TransitionConditionSO
{
    public Transform destination;

    public float thresholdDistance = 0.1f;

    public override bool CanTransition(CharacterFSM character)
    {
        if (destination == null)
        {
            Debug.LogWarning("Destination is not assigned in ReachedDestinationConditionSO");
            return false;
        }

        float distance = Vector3.Distance(character.transform.position, destination.position);

        return distance <= thresholdDistance;
    }

}
