using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosEasy;
    [SerializeField] private Vector3 _startPosMedium;
    [SerializeField] private float _offsetXEasy, _offsetYEasy;
    [SerializeField] private float _offsetXMedium, _offsetYMedium;
    [SerializeField] private SubGrid _subGridEasyPrefab;
    [SerializeField] private SubGrid _subGridMediumPrefab;
    [SerializeField] private InputPlane _inputPlane;
    [SerializeField] private View _view;
    [SerializeField] private TimerHandler _timer;
    [SerializeField] private LoseScreen _loseScreen;
    [SerializeField] private WinOneLevelScreen _winOneLevelScreen;
    [SerializeField] private WinScreen _winScreen;
    [SerializeField] private PauseScreen _pauseScreen;
    [SerializeField] private StartGameScreen _startGameScreen;
    [SerializeField] private GameObject _emptyPlane;

    private bool hasGameFinished;
    private Cell[,] cells;
    private Cell selectedCell;

    private IEnumerator _touchCoroutine;

    private const int GRID_SIZE = 9;
    private const int SUBGRID_SIZE = 3;
    private const int MAX_LEVEL = 10;

    private void Awake()
    {
        _emptyPlane.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        _inputPlane.SaveClicked += UpdateCellValue;
        _view.VerifyButtonClicked += CheckWin;
        _view.PauseClicked += PauseGame;

        _pauseScreen.ContinueButtonClicked += ContinueGame;
        _pauseScreen.MenuClicked += GoToMainScreen;

        _loseScreen.TryAgainClicked += RestartGame;
        _loseScreen.ExitButtonClicked += GoToMainScreen;

        _winOneLevelScreen.NextLevelClicked += GoToNextLevel;
        _winOneLevelScreen.ExitClicked += GoToMainScreen;

        _winScreen.TryAgainClicked += RestartAllGame;

        _startGameScreen.GameStarted += StartGame;
    }

    private void OnDisable()
    {
        _inputPlane.SaveClicked -= UpdateCellValue;
        _view.VerifyButtonClicked -= CheckWin;
        _view.PauseClicked -= PauseGame;

        _pauseScreen.ContinueButtonClicked -= ContinueGame;
        _pauseScreen.MenuClicked -= GoToMainScreen;

        _loseScreen.TryAgainClicked -= RestartGame;
        _loseScreen.ExitButtonClicked -= GoToMainScreen;

        _winOneLevelScreen.NextLevelClicked -= GoToNextLevel;
        _winOneLevelScreen.ExitClicked -= GoToMainScreen;

        _winScreen.TryAgainClicked -= RestartAllGame;

        _startGameScreen.GameStarted -= StartGame;
    }

    private void Start()
    {
        DisableTouch();
        _winScreen.Disable();
        _winOneLevelScreen.Disable();
        _pauseScreen.Disable();
        _loseScreen.Disable();
        _inputPlane.Disable();
        _startGameScreen.Enable();
        _emptyPlane.gameObject.SetActive(false);
        
        EnableTouch();
    }

    private void EnableTouch()
    {
        if (_touchCoroutine != null)
        {
            StopCoroutine(_touchCoroutine);
        }

        _touchCoroutine = DetectTouch();
        StartCoroutine(_touchCoroutine);
    }

    private void DisableTouch()
    {
        if (_touchCoroutine != null)
        {
            StopCoroutine(_touchCoroutine);
        }

        _touchCoroutine = null;
    }

    private void StartGame()
    {
        hasGameFinished = false;
        cells = new Cell[GRID_SIZE, GRID_SIZE];
        selectedCell = null;
        SpawnCells();
        _inputPlane.Disable();
        ValidateInput();
        _timer.ResetAndStart();
        _winScreen.Disable();
        _winOneLevelScreen.Disable();
        _pauseScreen.Disable();
        _loseScreen.Disable();
    }

    private void PauseGame()
    {
        _timer.Stop();
        DisableTouch();
        _view.MakeTransparent();
        _pauseScreen.Enable(_timer.GetTimerText());
    }

    private void ContinueGame()
    {
        _timer.Activate();
        _view.Enable();
        EnableTouch();
    }

    private void SpawnCells()
    {
        int[,] puzzleGrid = new int[GRID_SIZE, GRID_SIZE];
        int level = PlayerPrefs.GetInt("Level", 0);

        if (level == 0)
        {
            CreateAndStoreLevel(puzzleGrid, 1);
            level = 1;
        }
        else
        {
            GetCurrentLevel(puzzleGrid);
        }


        for (int i = 0; i < GRID_SIZE; i++)
        {
            SubGrid subGrid;

            if (level < 5)
            {
                Vector3 spawnPos = _startPosEasy + i % 3 * _offsetXEasy * Vector3.right +
                                   i / 3 * _offsetYEasy * Vector3.up;
                subGrid = Instantiate(_subGridEasyPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                Vector3 spawnPos = _startPosMedium + i % 3 * _offsetXMedium * Vector3.right +
                                   i / 3 * _offsetYMedium * Vector3.up;
                subGrid = Instantiate(_subGridMediumPrefab, spawnPos, Quaternion.identity);
            }

            List<Cell> subgridCells = subGrid.cells;
            int startRow = (i / 3) * 3;
            int startCol = (i % 3) * 3;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                subgridCells[j].Row = startRow + j / 3;
                subgridCells[j].Col = startCol + j % 3;
                int cellValue = puzzleGrid[subgridCells[j].Row, subgridCells[j].Col];
                subgridCells[j].Init(cellValue);
                cells[subgridCells[j].Row, subgridCells[j].Col] = subgridCells[j];
            }
        }
    }

    private void CreateAndStoreLevel(int[,] grid, int level)
    {
        if (level > MAX_LEVEL) level = MAX_LEVEL;

        int[,] tempGrid = Generator.GeneratePuzzle((Generator.DifficultyLevel)(level / 100));
        string arrayString = "";
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                arrayString += tempGrid[i, j].ToString() + ",";
                grid[i, j] = tempGrid[i, j];
            }
        }

        arrayString = arrayString.TrimEnd(',');
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetString("Grid", arrayString);
    }

    private void GetCurrentLevel(int[,] grid)
    {
        string arrayString = PlayerPrefs.GetString("Grid");
        string[] arrayValue = arrayString.Split(',');
        int index = 0;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                grid[i, j] = int.Parse(arrayValue[index]);
                index++;
            }
        }
    }

    private void GoToNextLevel()
    {
        int level = PlayerPrefs.GetInt("Level", 0);
        if (level < MAX_LEVEL)
        {
            CreateAndStoreLevel(new int[GRID_SIZE, GRID_SIZE], level + 1);
            RestartGame();
        }
        else
        {
            _timer.Stop();
            _view.MakeTransparent();
            _winScreen.Enable();
        }
    }

    private IEnumerator DetectTouch()
    {
        if (hasGameFinished) yield break;

        while (enabled)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);
                    RaycastHit2D hit = Physics2D.Raycast(touchPos2D, Vector2.zero);
                    Cell tempCell = null;

                    if (hit
                        && hit.collider.gameObject.TryGetComponent(out tempCell)
                        && tempCell != selectedCell
                        && !tempCell.IsLocked
                       )
                    {
                        ResetGrid();
                        selectedCell = tempCell;
                        HighLight();
                        _view.MakeTransparent();
                        _inputPlane.Enable();
                        DisableTouch();
                    }
                }
            }
            
            yield return null;
        }

    }

    private void ResetGrid()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].Reset();
            }
        }
    }

    public void UpdateCellValue(int value)
    {
        _view.Enable();
        if (hasGameFinished || selectedCell == null || value == 0) return;
        selectedCell.UpdateValue(value);
        HighLight();
        ValidateInput();

        selectedCell = null;
        EnableTouch();
    }

    private void ValidateInput()
    {
        bool verification = true;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (cells[i, j].Value == 0)
                {
                    verification = false;
                    break;
                }
            }

            if (!verification)
                break;
        }

        _view.ToggleVerifyButton(verification);
    }

    private void CheckWin()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (cells[i, j].IsIncorrect || cells[i, j].Value == 0)
                {
                    _timer.Stop();
                    _loseScreen.Enable();
                    _view.MakeTransparent();
                    return;
                }
            }
        }

        hasGameFinished = true;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].UpdateWin();
            }
        }

        _view.MakeTransparent();
        _winOneLevelScreen.Enable();
        _timer.Stop();
        DisableTouch();
    }

    private void HighLight()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].IsIncorrect = !IsValid(cells[i, j], cells);
            }
        }

        int currentRow = selectedCell.Row;
        int currentCol = selectedCell.Col;

        cells[currentRow, currentCol].Select();
    }

    private bool IsValid(Cell cell, Cell[,] cells)
    {
        int row = cell.Row;
        int col = cell.Col;
        int value = cell.Value;
        cell.Value = 0;

        if (value == 0) return true;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (cells[row, i].Value == value || cells[i, col].Value == value)
            {
                cell.Value = value;
                return false;
            }
        }

        int subGridRow = row - row % SUBGRID_SIZE;
        int subGridCol = col - col % SUBGRID_SIZE;

        for (int r = subGridRow; r < subGridRow + SUBGRID_SIZE; r++)
        {
            for (int c = subGridCol; c < subGridCol + SUBGRID_SIZE; c++)
            {
                if (cells[r, c].Value == value)
                {
                    cell.Value = value;
                    return false;
                }
            }
        }

        cell.Value = value;
        return true;
    }

    public void RestartGame()
    {
        _winScreen.Enable();
        _winOneLevelScreen.Enable();
        _pauseScreen.Enable("");
        _loseScreen.Enable();
        _inputPlane.Enable();
        _emptyPlane.gameObject.SetActive(true);
       SceneManager.LoadScene("Sudoky");
    }

    private void RestartAllGame()
    {
        PlayerPrefs.SetInt("Level", 0);
        _view.EnableBlackBox();
        RestartGame();
    }

    private void GoToMainScreen()
    {
        _view.EnableBlackBox();
        SceneManager.LoadScene("MainScene");
    }
}