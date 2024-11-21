using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class InputPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _input;
    [SerializeField] private Button _1Button;
    [SerializeField] private Button _2Button;
    [SerializeField] private Button _3Button;
    [SerializeField] private Button _4Button;
    [SerializeField] private Button _5Button;
    [SerializeField] private Button _6Button;
    [SerializeField] private Button _7Button;
    [SerializeField] private Button _8Button;
    [SerializeField] private Button _9Button;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _saveButton;

    private string _currentInput = "";
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action<int> SaveClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _1Button.onClick.AddListener(() => OnNumberButtonPressed(1));
        _2Button.onClick.AddListener(() => OnNumberButtonPressed(2));
        _3Button.onClick.AddListener(() => OnNumberButtonPressed(3));
        _4Button.onClick.AddListener(() => OnNumberButtonPressed(4));
        _5Button.onClick.AddListener(() => OnNumberButtonPressed(5));
        _6Button.onClick.AddListener(() => OnNumberButtonPressed(6));
        _7Button.onClick.AddListener(() => OnNumberButtonPressed(7));
        _8Button.onClick.AddListener(() => OnNumberButtonPressed(8));
        _9Button.onClick.AddListener(() => OnNumberButtonPressed(9));

        _deleteButton.onClick.AddListener(OnDeleteButtonPressed);
        _saveButton.onClick.AddListener(OnSaveButtonPressed);
    }

    private void OnDisable()
    {
        _1Button.onClick.RemoveListener(() => OnNumberButtonPressed(1));
        _2Button.onClick.RemoveListener(() => OnNumberButtonPressed(2));
        _3Button.onClick.RemoveListener(() => OnNumberButtonPressed(3));
        _4Button.onClick.RemoveListener(() => OnNumberButtonPressed(4));
        _5Button.onClick.RemoveListener(() => OnNumberButtonPressed(5));
        _6Button.onClick.RemoveListener(() => OnNumberButtonPressed(6));
        _7Button.onClick.RemoveListener(() => OnNumberButtonPressed(7));
        _8Button.onClick.RemoveListener(() => OnNumberButtonPressed(8));
        _9Button.onClick.RemoveListener(() => OnNumberButtonPressed(9));
        
        _deleteButton.onClick.RemoveListener(OnDeleteButtonPressed);
        _saveButton.onClick.RemoveListener(OnSaveButtonPressed);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnNumberButtonPressed(int number)
    {
     
            _currentInput = number.ToString();
        
        
        _input.text = _currentInput;
    }

    private void OnDeleteButtonPressed()
    {
        _currentInput = "";
        _input.text = "";
    }

    private void OnSaveButtonPressed()
    {
        if (!string.IsNullOrEmpty(_currentInput))
        {
            int number = int.Parse(_currentInput);
            SaveClicked?.Invoke(number);
        }

        Disable();
        _currentInput = "";
        _input.text = "";
    }
}