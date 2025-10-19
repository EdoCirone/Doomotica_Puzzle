
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "ScriptableObjects/FSM/States/IdleStateSO", order = 1)]
public class IdleStateSO : CharacterStateSO
{
    private void OnValidate()
    {
        // imposta automaticamente il nome di animazione se non è stato modificato
        if (string.IsNullOrEmpty(animationName))
            animationName = "Idle";
    }

    public override void OnEnter(CharacterFSM character)
    {
        if (character.Animator != null)
            character.Animator.Play(animationName);
    }

    public override void OnUpdate(CharacterFSM character)
    {
        // idle non fa nulla
    }

    public override void OnExit(CharacterFSM character)
    {
        // nessuna logica di uscita
    }

}
