using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RamenGameController : MonoBehaviour
{
    private const float MaxTimerCount = 1f * 60;

    [SerializeField] private InteractableObjectSpawner _spawner;
    [SerializeField] private RamenPlayer _player;
    [SerializeField] private RamenGameControllerView _view;
    [SerializeField] private StartGameScreen _startGameScreen;
    [SerializeField] private PauseScreen _pauseScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinScreen _winScreen;

    private float _currentTime;
    private int _count;

    private IEnumerator _timerCoroutine;

    private void OnEnable()
    {
        _view.PauseClicked += ProcessGamePaused;

        _pauseScreen.MenuClicked += GoToMainScene;
        _pauseScreen.ContinueButtonClicked += ContinueGame;

        _player.CorrectObjectCatched += UpdateCount;
        _player.IncorrectObjectCatched += ProcessGameLost;

        _loseScreen.ExitButtonClicked += GoToMainScene;
        _loseScreen.TryAgainClicked += ProcessNewGameStart;

        _winScreen.TryAgainClicked += ProcessNewGameStart;
    }

    private void OnDisable()
    {
        _view.PauseClicked -= ProcessGamePaused;

        _pauseScreen.MenuClicked -= GoToMainScene;
        _pauseScreen.ContinueButtonClicked -= ContinueGame;

        _player.CorrectObjectCatched -= UpdateCount;
        _player.IncorrectObjectCatched -= ProcessGameLost;

        _loseScreen.ExitButtonClicked -= GoToMainScene;
        _loseScreen.TryAgainClicked -= ProcessNewGameStart;

        _winScreen.TryAgainClicked -= ProcessNewGameStart;
        
        _startGameScreen.GameStarted -= ProcessNewGameStart;
    }

    private void Start()
    {
        _view.MakeTransparent();
        _pauseScreen.Disable();
        _winScreen.Disable();
        _loseScreen.Disable();
        _startGameScreen.Enable();
        _startGameScreen.GameStarted += ProcessNewGameStart;
    }

    private void ProcessNewGameStart()
    {
        ResetValues();

        _view.Enable();
        if (_timerCoroutine == null)
            _timerCoroutine = StartTimer();

        StartCoroutine(_timerCoroutine);

        _spawner.EnableSpawn();
        _player.EnableMovement();
    }

    private void ResetValues()
    {
        _currentTime = 0;
        _count = 0;

        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        _spawner.StopSpawning();
        _player.DisableMovement();
        _timerCoroutine = null;

        _pauseScreen.Disable();
        _winScreen.Disable();
        _loseScreen.Disable();

        _view.SetCount(_count);
        _view.SetTimeText(0, 0);
    }

    private IEnumerator StartTimer()
    {
        while (_currentTime < MaxTimerCount)
        {
            _currentTime += Time.deltaTime;
            UpdateTimer(_currentTime);
            yield return null;
        }

        ProcessGameWon();
    }

    private void UpdateTimer(float time)
    {
        time += 1;

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        _view.SetTimeText(minutes, seconds);
    }

    private void ProcessGameWon()
    {
        _winScreen.Enable();
        
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        _spawner.StopSpawning();
        _player.DisableMovement();
    }

    private void ProcessGameLost()
    {
        _view.MakeTransparent();
        _loseScreen.Enable();
        
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        _spawner.StopSpawning();
        _player.DisableMovement();
    }

    private void UpdateCount(InteractableObject interactableObject)
    {
        _spawner.ReturnToPool(interactableObject);

        _count++;
        _view.SetCount(_count);
    }

    private void ProcessGamePaused()
    {
        _view.MakeTransparent();
        
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        _spawner.StopSpawning();
        _player.DisableMovement();
        _pauseScreen.Enable(_view.GetTimerText());
    }

    private void ContinueGame()
    {
        _view.Enable();
        
        if (_timerCoroutine == null)
        {
            _timerCoroutine = StartTimer();
            StartCoroutine(_timerCoroutine);
        }

        _spawner.EnableSpawn();
        _player.EnableMovement();
    }

    private void GoToMainScene()
    {
        _view.EnableBlackScreen();
        SceneManager.LoadScene("MainScene");
    }
}