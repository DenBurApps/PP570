using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindTheDifferenceGameController : MonoBehaviour
{
    private const float MaxTimer = 5f * 60;

    [SerializeField] private FindTheDifferenceGameView _view;
    [SerializeField] private CountHolder _countHolder;
    [SerializeField] private DifferencesPicture[] _differencesPictures;
    [SerializeField] private GameObject[] _correctImageHolders;
    [SerializeField] private WinOneLevelScreen _winOneLevelScreen;
    [SerializeField] private WinScreen _winScreen;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private PauseScreen _pauseScreen;
    [SerializeField] private StartGameScreen _startGameScreen;

    private float _currentTime;
    private int _levelsCount;
    private IEnumerator _timerCoroutine;

    private void OnEnable()
    {
        foreach (var picture in _differencesPictures)
        {
            picture.DifferenceIndexFound += ActivateCompleteCount;
            picture.LevelComplete += ProcessGameWon;
        }

        _winOneLevelScreen.NextLevelClicked += StartNextLevel;
        _winOneLevelScreen.ExitClicked += ReturnToMainScene;

        _winScreen.TryAgainClicked += RestartAllGame;

        _view.PauseClicked += PauseGame;

        _pauseScreen.ContinueButtonClicked += ContinueGame;
        _pauseScreen.MenuClicked += ReturnToMainScene;

        _loseScreen.ExitButtonClicked += ReturnToMainScene;
        _loseScreen.TryAgainClicked += RestartLevel;

        _startGameScreen.GameStarted += StartNewGame;
    }

    private void OnDisable()
    {
        foreach (var picture in _differencesPictures)
        {
            picture.DifferenceIndexFound -= ActivateCompleteCount;
            picture.LevelComplete -= ProcessGameWon;
        }

        _winOneLevelScreen.NextLevelClicked -= StartNextLevel;
        _winOneLevelScreen.ExitClicked -= ReturnToMainScene;

        _winScreen.TryAgainClicked -= RestartAllGame;

        _view.PauseClicked -= PauseGame;

        _pauseScreen.ContinueButtonClicked -= ContinueGame;
        _pauseScreen.MenuClicked -= ReturnToMainScene;

        _loseScreen.ExitButtonClicked -= ReturnToMainScene;
        _loseScreen.TryAgainClicked -= RestartLevel;

        _startGameScreen.GameStarted -= StartNewGame;
    }

    private void Start()
    {
        _pauseScreen.Disable();
        _winScreen.Disable();
        _loseScreen.Disable();
        _view.MakeTransparent();
        _levelsCount = _differencesPictures.Length;
        ActivatePicturePlane();
        _startGameScreen.Enable();
    }

    private void StartNewGame()
    {
        ResetValues();

        _view.Enable();

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        _timerCoroutine = StartTimer();
        StartCoroutine(_timerCoroutine);

        ActivatePicturePlane();
    }

    private void RestartLevel()
    {
        foreach (var picture in _differencesPictures)
        {
            if (picture.gameObject.activeSelf)
            {
                picture.Enable();
                picture.ReturnToDefault();
            }
        }

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        _timerCoroutine = StartTimer();
        StartCoroutine(_timerCoroutine);
    }

    private void ResetValues()
    {
        _levelsCount = _differencesPictures.Length;
        _currentTime = 0;
        _view.SetTimeText(0, 0);

        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        _timerCoroutine = null;

        _pauseScreen.Disable();
        _winOneLevelScreen.Disable();
        _winScreen.Disable();
        _loseScreen.Disable();

        DisableAllInputs();
    }

    private void ActivateCompleteCount(int index)
    {
        _countHolder.SetElementCompleted(index);
    }

    private IEnumerator StartTimer()
    {
        while (_currentTime < MaxTimer)
        {
            _currentTime += Time.deltaTime;
            UpdateTimer(_currentTime);
            yield return null;
        }

        ProcessGameLost();
    }

    private void UpdateTimer(float time)
    {
        time += 1;

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        _view.SetTimeText(minutes, seconds);
    }

    private void DisableAllInputs()
    {
        foreach (var picture in _differencesPictures)
        {
            if (picture.gameObject.activeSelf)
            {
                picture.DisableInput();
            }
        }
    }

    private void EnableAllInput()
    {
        foreach (var picture in _differencesPictures)
        {
            if (picture.gameObject.activeSelf)
                picture.EnableInput();
        }
    }

    private void RestartAllGame()
    {
        foreach (var picture in _differencesPictures)
        {
            picture.ReturnToDefault();
        }

        _countHolder.ReturnAllElementsToDefault();
        StartNewGame();
    }

    private void ProcessGameLost()
    {
        _loseScreen.Enable();

        DisableAllInputs();
    }

    private void ProcessGameWon()
    {
        _levelsCount--;

        if (_levelsCount <= 0)
        {
            _view.MakeTransparent();
            _winScreen.Enable();
            DisableAllInputs();
        }
        else
        {
            _view.MakeTransparent();
            _winOneLevelScreen.Enable();
            DisableAllInputs();
            _countHolder.ReturnAllElementsToDefault();
        }
    }

    private void PauseGame()
    {
        DisableAllInputs();

        _view.MakeTransparent();
        _pauseScreen.Enable(_view.GetTimeText());
    }

    private void ContinueGame()
    {
        _view.Enable();

        if (_timerCoroutine == null)
            _timerCoroutine = StartTimer();

        StartCoroutine(_timerCoroutine);
        
        EnableAllInput();
    }

    private void StartNextLevel()
    {
        _view.Enable();
        _currentTime = 0;
        _view.SetTimeText(0, 0);

        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        _timerCoroutine = StartTimer();
        StartCoroutine(_timerCoroutine);

        _pauseScreen.Disable();
        _winOneLevelScreen.Disable();
        _winScreen.Disable();
        _loseScreen.Disable();

        DisableAllInputs();
        ActivatePicturePlane();
    }

    private void ActivatePicturePlane()
    {
        switch (_levelsCount)
        {
            case 5:
                ActivateCurrentPicturePlane(0);
                break;
            case 4:
                ActivateCurrentPicturePlane(1);
                break;
            case 3:
                ActivateCurrentPicturePlane(2);
                break;
            case 2:
                ActivateCurrentPicturePlane(3);
                break;
            case 1:
                ActivateCurrentPicturePlane(4);
                break;
        }
    }

    private void ActivateCurrentPicturePlane(int index)
    {
        for (int i = 0; i < _differencesPictures.Length; i++)
        {
            if (i == index && !_differencesPictures[i].IsComplete)
            {
                _differencesPictures[i].Enable();
                _differencesPictures[i].EnableInput();
                _correctImageHolders[i].gameObject.SetActive(true);
                _countHolder.ActivateElements(_differencesPictures[i].DifferencesCount);
            }
            else
            {
                _differencesPictures[i].Disable();
                _correctImageHolders[i].gameObject.SetActive(false);
            }
        }
    }

    private void ReturnToMainScene()
    {
        _view.EnableBlackScreen();
        SceneManager.LoadScene("MainScene");
    }
}