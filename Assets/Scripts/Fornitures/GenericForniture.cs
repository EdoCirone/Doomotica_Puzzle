using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericForniture : MonoBehaviour
{
    // ==================== SETTINGS ====================
    [Header("Activation")]
    [SerializeField] private bool _isON;
    [SerializeField] private bool _canSetOff;
    [SerializeField] private UnityEvent _onActivate;
    [SerializeField] private UnityEvent _onDeactivate;

    [Header("Highlight")]
    [SerializeField] private Color _highlightColor = Color.yellow;
    [SerializeField] private float _emissionIntensity = 2f;

    // ==================== EVENTS ====================
    public event System.Action<GenericForniture> OnActivate;
    public event System.Action<GenericForniture> OnDeactivate;

    // ==================== PROPERTIES ====================
    public bool IsON => _isON;

    // ==================== PRIVATE ====================
    private Renderer[] _renderers;
    private Dictionary<Renderer, Color> _originalEmissionColors;

    // ==================== UNITY ====================
    private void Awake()
    {
        InitializeRenderers();
    }

    private void OnMouseDown() => HandleClick();
    private void OnMouseEnter() => SetEmission(true);
    private void OnMouseExit() => SetEmission(false);

    // ==================== METHODS ====================
    public void SetIsON(bool value)
    {
        if (_isON == value) return;
        _isON = value;

        if (_isON)
        {
            _onActivate?.Invoke();
            OnActivate?.Invoke(this);
        }
        else
        {
            _onDeactivate?.Invoke();
            OnDeactivate?.Invoke(this);
        }
    }

    private void InitializeRenderers()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _originalEmissionColors = new Dictionary<Renderer, Color>();

        foreach (var renderer in _renderers)
        {
            foreach (var mat in renderer.materials)
            {
               
                if (mat.HasProperty(ShaderPropertyIDs.Emission.Color))
                    _originalEmissionColors[renderer] = mat.GetColor(ShaderPropertyIDs.Emission.Color);
            }
        }
    }

    private void SetEmission(bool enabled)
    {
        foreach (var renderer in _renderers)
        {
            foreach (var mat in renderer.materials)
            {
  
                if (!mat.HasProperty(ShaderPropertyIDs.Emission.Color)) continue;

                if (enabled)
                {
                    mat.EnableKeyword(ShaderPropertyIDs.Emission.Keyword);
                    mat.SetColor(ShaderPropertyIDs.Emission.Color, _highlightColor * _emissionIntensity);
                }
                else
                {
                    if (_originalEmissionColors.TryGetValue(renderer, out Color original))
                        mat.SetColor(ShaderPropertyIDs.Emission.Color, original);
                    else
                    {
                        mat.DisableKeyword(ShaderPropertyIDs.Emission.Keyword);
                        mat.SetColor(ShaderPropertyIDs.Emission.Color, Color.black);
                    }
                }
            }
        }
    }

    private void HandleClick()
    {
        if (_isON && _canSetOff)
            SetIsON(false);
        else if (!_isON)
            SetIsON(true);
    }
}