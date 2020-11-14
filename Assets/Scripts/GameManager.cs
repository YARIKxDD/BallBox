using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const string SAVEKEY_LEVEL = "Last finished level";

    public Action<GameState> CurrentAppStateChanged;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private AIManager aiManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private LevelManager levelManager;

    private GameState _currentAppState;
    private GameState CurrentAppState
    {
        get
        {
            return _currentAppState;
        }
        set
        {
            _currentAppState = value;

            if (CurrentAppStateChanged != null)
            {
                CurrentAppStateChanged(_currentAppState);
            }
        }
    }

    private int _currentLevelNumber;
    public int CurrentLevelNumber
    {
        get
        {
            return _currentLevelNumber;
        }
        private set
        {
            _currentLevelNumber = value;
        }
    }

    private void OnEnable()
    {
        uiManager.StartClicked += OnStartClicked;
        uiManager.PauseClicked += OnPauseClicked;
        uiManager.ContinueClicked += OnContinueClicked;
        uiManager.NextLevelClicked += OnNextLevelClicked;
        uiManager.ExitClicked += OnExitClicked;

        levelManager.LevelComplited += OnLevelComplited;
    }

    private void OnDisable()
    {
        uiManager.StartClicked -= OnStartClicked;
        uiManager.PauseClicked -= OnPauseClicked;
        uiManager.ContinueClicked -= OnContinueClicked;
        uiManager.NextLevelClicked -= OnNextLevelClicked;
        uiManager.ExitClicked -= OnExitClicked;

        levelManager.LevelComplited -= OnLevelComplited;
    }

    private void Start()
    {
        Load();
        CurrentAppState = GameState.Start;
    }

    private void OnStartClicked()
    {
        levelManager.DestroyLevel();
        levelManager.BuildLevel();
        CurrentAppState = GameState.Game;
    }

    private void OnPauseClicked()
    {
        CurrentAppState = GameState.Pause;
    }

    private void OnContinueClicked()
    {
        CurrentAppState = GameState.Start;
    }

    private void OnNextLevelClicked()
    {
        //
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }

    private void OnLevelComplited()
    {
        PlayerPrefs.SetInt(SAVEKEY_LEVEL, CurrentLevelNumber);
        levelManager.DestroyLevel();
        CurrentLevelNumber++;
        CurrentAppState = GameState.Start;
    }

    private void Load()
    {
        CurrentLevelNumber = PlayerPrefs.GetInt(SAVEKEY_LEVEL, 0) + 1;
    }
}