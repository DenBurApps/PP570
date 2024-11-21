using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class WinOneLevelScreen : MonoBehaviour
{
    [SerializeField] private Button _goNextLevelButon;
    [SerializeField] private Button _exitButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action NextLevelClicked;
    public event Action ExitClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _goNextLevelButon.onClick.AddListener(OnNextLevelClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnDisable()
    {
        _goNextLevelButon.onClick.RemoveListener(OnNextLevelClicked);
        _exitButton.onClick.RemoveListener(OnExitClicked);
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
    
    private void OnNextLevelClicked()
    {
        NextLevelClicked?.Invoke();
        Disable();
    }

    private void OnExitClicked() => ExitClicked?.Invoke();
}
