using UnityEngine;

[CreateAssetMenu(fileName = "InteractionCompleteCondition", menuName = "ScriptableObjects/FSM/Conditions/InteractionComplete")]
public class InteractionCompleteConditionSO : TransitionConditionSO
{

    
    public override bool CanTransition(CharacterFSM character)
    {
        // Verifica se il personaggio ha completato l'interazione (la flag è gestita nello script CharacterFSM)
        return character.IsInteractionComplete;
    }

}
