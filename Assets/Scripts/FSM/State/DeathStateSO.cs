using UnityEngine;

[CreateAssetMenu(fileName = "DeathState", menuName = "ScriptableObjects/FSM/States/Death")]
public class DeathStateSO : CharacterStateSO
{
    [SerializeField]private EventChannelCharacter _deathEventChannel;
    public override void OnEnter(CharacterFSM character)
    {
        if (_deathEventChannel == null)
        {
            Debug.LogWarning($"{name} is missing a reference to _deathEventChannel!");
            return;
        }

        _deathEventChannel.Raise(character);
        Debug.Log($"{character.name} entered DEATH state.");
    }

    public override void OnUpdate(CharacterFSM character) { }

    public override void OnExit(CharacterFSM character) { }
}
