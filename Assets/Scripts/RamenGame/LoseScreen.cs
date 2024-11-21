using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class LoseScreen : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _tryAgainButon;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action ExitButtonClicked;
    public event Action TryAgainClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnExitClicked);
        _tryAgainButon.onClick.AddListener(OnTryAgainClicked);
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(OnExitClicked);
        _tryAgainButon.onClick.RemoveListener(OnTryAgainClicked);
    }

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnExitClicked()
    {
        ExitButtonClicked?.Invoke();
    }

    private void OnTryAgainClicked()
    {
        TryAgainClicked?.Invoke();
        Disable();
    }
}
