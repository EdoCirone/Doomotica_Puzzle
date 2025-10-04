using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericForniture : MonoBehaviour
{
    [SerializeField] private bool _isON;
    [SerializeField] private GameObject _effectsGameObject;


    public bool IsON { get => _isON; private set => _isON = value; }
    public Collider EffectCollider { get; private set ; }

    private void Start()
    {
        if( _effectsGameObject == null)
        {
            Debug.LogWarning("Assegna un effetto a " + gameObject.name);
            return;
        }
        EffectCollider = _effectsGameObject.GetComponentInChildren<Collider>();
    }

    public void SetIsON(bool value)
    {
        _isON = value;
        if (_effectsGameObject != null)
            _effectsGameObject.SetActive(value);
    }
    protected virtual void OnMouseDown()
    {

        if (_isON == true) return;
        SetIsON(true);
    }



}
