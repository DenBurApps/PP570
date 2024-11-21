using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class PauseScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Button _toMenuButton;
    [SerializeField] private Button _continueButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action ContinueButtonClicked;
    public event Action MenuClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _toMenuButton.onClick.AddListener(OnMenuButtonClicked);
        _continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnDisable()
    {
        _toMenuButton.onClick.RemoveListener(OnMenuButtonClicked);
        _continueButton.onClick.RemoveListener(OnContinueClicked);
    }

    private void Start()
    {
        Disable();
    }

    public void Enable(string text)
    {
        _screenVisabilityHandler.EnableScreen();
        _timeText.text = text;
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnMenuButtonClicked()
    {
        MenuClicked?.Invoke();
        Disable();
    }

    private void OnContinueClicked()
    {
        ContinueButtonClicked?.Invoke();
        Disable();
    }
}
