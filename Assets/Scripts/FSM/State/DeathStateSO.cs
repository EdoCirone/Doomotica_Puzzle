using UnityEngine;

[CreateAssetMenu(fileName = "DeathState", menuName = "FSM/States/Death")]
public class DeathStateSO : CharacterStateSO
{
    public override void OnEnter(CharacterFSM character)
    {
        Debug.Log($"{character.name} entered DEATH state.");
        // Eventuale logica morte
    }

    public override void OnUpdate(CharacterFSM character) { }

    public override void OnExit(CharacterFSM character) { }
}
