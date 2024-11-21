using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class View : MonoBehaviour
{
    [SerializeField] private Button _verifyButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _blackBox;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action VerifyButtonClicked;
    public event Action PauseClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _verifyButton.onClick.AddListener(OnVerifyButtonClicked);
        _pauseButton.onClick.AddListener(OnPauseClicked);
        DisableBlackBox();
    }

    private void OnDisable()
    {
        _verifyButton.onClick.RemoveListener(OnVerifyButtonClicked);
        _pauseButton.onClick.RemoveListener(OnPauseClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void ToggleVerifyButton(bool status)
    {
        _verifyButton.interactable = status;
    }

    public void EnableBlackBox()
    {
        _blackBox.gameObject.SetActive(true);
    }

    public void DisableBlackBox()
    {
        _blackBox.gameObject.SetActive(false);
    }
    
    public void MakeTransparent()
    {
        _screenVisabilityHandler.SetTransperent();
    }

    private void OnVerifyButtonClicked() => VerifyButtonClicked?.Invoke();
    private void OnPauseClicked() => PauseClicked?.Invoke();
}