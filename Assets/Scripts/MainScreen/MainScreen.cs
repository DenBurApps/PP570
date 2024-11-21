using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private GamePanel _ramenGamePanel;
    [SerializeField] private GamePanel _findTheDifferenceGamePanel;
    [SerializeField] private GamePanel _sudokyGamePanel;
    [SerializeField] private SettingsScreen _settingsScreen;

    public event Action SettingsClicked;
    
    private void OnEnable()
    {
        _ramenGamePanel.NextButtonClicked += OnRamenNextClicked;
        _ramenGamePanel.PlayButtonClicked += LoadRamenGame;
        _ramenGamePanel.SettingsClicked += OnSettingsClicked;

        _findTheDifferenceGamePanel.NextButtonClicked += OnFindTheDifferenceNextClicked;
        _findTheDifferenceGamePanel.PlayButtonClicked += LoadFindTheDifferenceGame;
        _findTheDifferenceGamePanel.SettingsClicked += OnSettingsClicked;

        _sudokyGamePanel.NextButtonClicked += OnSudokyNextClicked;
        _sudokyGamePanel.PlayButtonClicked += LoadSudokyGame;
        _sudokyGamePanel.SettingsClicked += OnSettingsClicked;

        _settingsScreen.BackButtonClicked += OnSudokyNextClicked;
    }

    private void OnDisable()
    {
        _ramenGamePanel.NextButtonClicked -= OnRamenNextClicked;
        _ramenGamePanel.PlayButtonClicked -= LoadRamenGame;
        _ramenGamePanel.SettingsClicked -= OnSettingsClicked;

        _findTheDifferenceGamePanel.NextButtonClicked -= OnFindTheDifferenceNextClicked;
        _findTheDifferenceGamePanel.PlayButtonClicked -= LoadFindTheDifferenceGame;
        _findTheDifferenceGamePanel.SettingsClicked -= OnSettingsClicked;

        _sudokyGamePanel.NextButtonClicked -= OnSudokyNextClicked;
        _sudokyGamePanel.PlayButtonClicked -= LoadSudokyGame;
        _sudokyGamePanel.SettingsClicked -= OnSettingsClicked;
        
        _settingsScreen.BackButtonClicked -= OnSudokyNextClicked;
    }

    private void Start()
    {
        _ramenGamePanel.Enable();
        _sudokyGamePanel.Disable();
        _findTheDifferenceGamePanel.Disable();
    }

    private void OnRamenNextClicked()
    {
        _ramenGamePanel.Disable();
        _findTheDifferenceGamePanel.Enable();
        _sudokyGamePanel.Disable();
    }

    private void OnFindTheDifferenceNextClicked()
    {
        _ramenGamePanel.Disable();
        _findTheDifferenceGamePanel.Disable();
        _sudokyGamePanel.Enable();
    }

    private void OnSudokyNextClicked()
    {
        _ramenGamePanel.Enable();
        _findTheDifferenceGamePanel.Disable();
        _sudokyGamePanel.Disable();
    }

    private void LoadRamenGame()
    {
        SceneManager.LoadScene("RamenGameScene");
    }

    private void LoadFindTheDifferenceGame()
    {
        SceneManager.LoadScene("FindTheDifferenceScene");
    }

    private void LoadSudokyGame()
    {
        SceneManager.LoadScene("Sudoky");
    }

    private void OnSettingsClicked() => SettingsClicked?.Invoke();
}
