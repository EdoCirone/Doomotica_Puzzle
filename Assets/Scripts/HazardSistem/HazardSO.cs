using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hazard", menuName = "ScriptableObjects/HazardSO")]

public class HazardSO : ScriptableObject
{

    [Header("Descrizione")]

    public string hazardName = "Nome Pericolo";
    [TextArea] public string description = "Descrizione del pericolo";

    [Header("Effetti visivi e sonori")]
    public Color hazardColor = Color.red;
    public Color safeColor = Color.white;
    public ParticleSystem hazardVFX;
    public AudioClip hazardSFX;

    [Header("Impatto sul giocatore")]
    public bool killsInstantly = true;
    public bool spreadsOnContact = false;
    public float spreadRadius = 2.0f;
    public float duration = 5.0f; // Durata del tempo in cui il componente è dannoso
    public float timeBeforeDie = 3.0f; // Tempo prima che il giocatore muoia se entra in contatto

    [Header("Cooldown Settings")]
    public bool requiresCooldown = false;
    public float cooldownTime = 10.0f; // Tempo prima che il componente sia dannoso

    public void ApplyVisuals(GameObject target)
    {
        if (target.TryGetComponent(out Renderer r))
            r.material.color = hazardColor;

        if (hazardVFX != null)
        {
            ParticleSystem existingFX = target.GetComponentInChildren<ParticleSystem>();
            if (existingFX == null)
            {
                ParticleSystem fx = Instantiate(hazardVFX, target.transform.position, Quaternion.identity);
                fx.transform.SetParent(target.transform);
                fx.Play();
            }
        }

        if (hazardSFX != null)
            AudioSource.PlayClipAtPoint(hazardSFX, target.transform.position);
    }

    public void RevertVisuals(GameObject target)
    {
        if (target.TryGetComponent(out Renderer r))
            r.material.color = safeColor;

        foreach (ParticleSystem fx in target.GetComponentsInChildren<ParticleSystem>())
        {
            Object.Destroy(fx.gameObject);
        }
    }

}
