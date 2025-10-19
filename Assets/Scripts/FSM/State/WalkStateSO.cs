using UnityEngine;

[CreateAssetMenu(fileName = "WalkState", menuName = "ScriptableObjects/FSM/States/WalkState")]
public class WalkStateSO : CharacterStateSO
{

    public Transform destination;

    public float arrivalThreshold = 0.1f;

    public string aniamationName = "isWalking";

    public override void OnEnter(CharacterFSM character)
    {
        //Gestione dell'animazione
        if (character.Animator != null)
        {
            character.Animator.Play(animationName);
        }

        //Gestione del movimento (tramite navMesh) 

        if (character.Mover != null && destination != null)
        {
            character.Mover.MoveTo(destination);
        }
    }

    public override void OnUpdate(CharacterFSM character)
    {

        //Controllo se sono arrivato a destinazione
        if (character.Mover.HasReachedDestination(arrivalThreshold))
        {
            character.SetInterectionComplete(true);
        }

    }

    public override void OnExit(CharacterFSM character)
    {
        Debug.Log($"{character.name} arrivato a destinazione");
    }

}
