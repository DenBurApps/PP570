using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class WinScreen : MonoBehaviour
{
    [SerializeField] private Button _tryAgainButon;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action TryAgainClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _tryAgainButon.onClick.AddListener(OnTryAgainClicked);
    }

    private void OnDisable()
    {
        _tryAgainButon.onClick.RemoveListener(OnTryAgainClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    private void OnTryAgainClicked()
    {
        TryAgainClicked?.Invoke();
        Disable();
    }
}
