using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class FindTheDifferenceGameView : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _blackScreen;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action PauseClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        EnableBlackScreen();
    }

    private void OnEnable()
    {
        _pauseButton.onClick.AddListener(OnPauseClicked);
        DisableBlackScreen();
    }

    private void OnDisable()
    {
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

    public void EnableBlackScreen()
    {
        _blackScreen.gameObject.SetActive(true);
    }

    public void DisableBlackScreen()
    {
        _blackScreen.gameObject.SetActive(false);
    }

    public string GetTimeText()
    {
        return _timer.text;
    }

    public void SetTimeText(float minutes, float seconds)
    {
        _timer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void MakeTransparent()
    {
        _screenVisabilityHandler.SetTransperent();
    }
    
    private void OnPauseClicked() => PauseClicked?.Invoke();
}
