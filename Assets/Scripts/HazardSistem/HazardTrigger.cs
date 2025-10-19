using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardTrigger : MonoBehaviour
{
    [SerializeField] private HazardSO _hazard;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterFSM>(out var character))
        {
            ApplyHazardToCharacter(character);
        }

        if (other.TryGetComponent<GenericForniture>(out var forniture))
        {
            ApplyHazardToForniture(forniture);
        }

        if (other.TryGetComponent<CarriableObject>(out var carriable))
        {
            ApplyHazardToCarriable(carriable);
        }
    }

    private void ApplyHazardToCharacter(CharacterFSM character)
    {
        if (_hazard.killsInstantly)
        {
            character.Die();
        }
        else
        {
            StartCoroutine(DelayedHazardEffect(character));
        }
    }

    private IEnumerator DelayedHazardEffect(CharacterFSM character)
    {
        yield return new WaitForSeconds(_hazard.timeBeforeDie);
        character.Die();
    }

    private void ApplyHazardToForniture(GenericForniture forniture)
    {
        forniture.Contaminate(_hazard);
    }

    private void ApplyHazardToCarriable(CarriableObject carriable)
    {
        carriable.Contaminate(_hazard);
    }

}


