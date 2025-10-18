using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InteractStateSO : CharacterStateSO
{
    [Header("Interaction Settings")]
    public GenericForniture distractionForniture;
    public float interactionDuration = 1f;

    [Header("Animation")]
    public string aniamationName = "isInteracting";

    //flag per sapere se il personaggio sta interagendo o meno
    public bool isInteracting = false;
    public override void OnEnter(CharacterFSM character)
    {
        Debug.Log($"{character.name}Entering Interact State");

        if (!isInteracting)
        {
            isInteracting = true;
            //Gestione dell'animazione
            if (character.Animator != null)
            {
                character.Animator.Play(aniamationName);
            }

            //Avvio della coroutine per la durata dell'interazione
            character.StartCoroutine(InteractCoroutine(character));
        }
    }

    public override void OnUpdate(CharacterFSM character)
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit(CharacterFSM character)
    {
        Debug.Log($"{character.name} Exiting Interact State");
        isInteracting = false;
    }

    // Coroutine per gestire la durata dell'interazione, gli SO non possono avere Coroutine direttamente quindi la gestiamo da character.StartCoroutine()
    private IEnumerator InteractCoroutine(CharacterFSM character)
    {

        if (distractionForniture != null)
        {
            distractionForniture.SetIsON(false);
        }

        if (character.Animator != null)
        {
            character.Animator.Play(aniamationName);
        }

        yield return new WaitForSeconds(interactionDuration); //Attende la durata dell'interazione
        if (character.CurrentState == this)
            {

                isInteracting = false;
                character.SetInterectionComplete(true); //Notifica al CharacterFSM che l'interazione è completa
            }
    }

}
