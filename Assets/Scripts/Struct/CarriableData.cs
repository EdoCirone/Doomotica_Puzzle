using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<summary>
// Creo una struct per gestire i dati degli oggetti trasportabili e condividerli via eventChannels
//</summary>

[System.Serializable]
public struct CarriableData
{

    public CarriableObject carriable; // Riferimento all'oggetto trasportabile
    public HazardSO hazard; // Riferimento al pericolo associato
    public GameObject lastCarrier; // Riferimento all'ultimo giocatore che ha trasportato l'oggetto
    public bool isContaminated; // Indica se l'oggetto è contaminato
    public float timeOfInteraction; // Tempo dell'ultima interazione con l'oggetto, serve nel caso di cooldown


    // Costruttore per inizializzare la struct
    public CarriableData(CarriableObject obj, GameObject carrirer)
    {
        carriable = obj;
        hazard = obj.CurrentHazard;
        lastCarrier = carrirer;
        isContaminated = obj.IsHazardous();
        timeOfInteraction = Time.time;
    }

}
