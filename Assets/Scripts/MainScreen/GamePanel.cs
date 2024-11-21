using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class GamePanel : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _settingsButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action PlayButtonClicked;
    public event Action NextButtonClicked;
    public event Action SettingsClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _nextButton.onClick.RemoveListener(OnNextButtonClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnPlayButtonClicked() => PlayButtonClicked?.Invoke();
    
    private void OnNextButtonClicked()
    {
        NextButtonClicked?.Invoke();
        Disable();
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
        Disable();
    }
}
