using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorEffect : MonoBehaviour
{
    [SerializeField] private Renderer _render;
    [SerializeField] private Color _allertColor;
    [SerializeField] private float _duration;

    private GenericForniture _forniture;

    private void Start()
    {
        _forniture = GetComponent<GenericForniture>();
        if (_forniture == null)
        {
            Debug.LogError("No GenericForniture component found on " + gameObject.name);
        }
    }

    private void OnEnable()
    {
        if (_forniture != null && _forniture.IsON)
        {
           _render.material.color = _allertColor;
        }
    }


}


