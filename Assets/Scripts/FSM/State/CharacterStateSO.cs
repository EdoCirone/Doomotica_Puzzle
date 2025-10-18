
using UnityEngine;

public abstract class CharacterStateSO : ScriptableObject
{

    public string animationName;

    public abstract void OnEnter(CharacterFSM character);
    public abstract void OnExit(CharacterFSM character);
    public abstract void OnUpdate(CharacterFSM character);

}
