using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const string SAVEKEY_LEVEL = "Last finished level";

    public Action<GameState> CurrentGameStateChanged;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private AIManager aiManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private BallAndTargetManager ballAndTargetManager;

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

            if (CurrentGameStateChanged != null)
            {
                CurrentGameStateChanged(_currentAppState);
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
        uiManager.ExitClicked += OnExitClicked;

        ballAndTargetManager.LevelComplited += OnLevelComplited;
    }

    private void OnDisable()
    {
        uiManager.StartClicked -= OnStartClicked;
        uiManager.PauseClicked -= OnPauseClicked;
        uiManager.ContinueClicked -= OnContinueClicked;
        uiManager.ExitClicked -= OnExitClicked;

        ballAndTargetManager.LevelComplited -= OnLevelComplited;
    }

    private void Start()
    {
        Load();
        CurrentAppState = GameState.Start;
    }

    private void OnStartClicked()
    {
        ballAndTargetManager.DestroyLevel();
        ballAndTargetManager.BuildLevel();
        CurrentAppState = GameState.Game;
    }

    private void OnPauseClicked()
    {
        Time.timeScale = 0;
        CurrentAppState = GameState.Pause;
    }

    private void OnContinueClicked()
    {
        Time.timeScale = 1;
        CurrentAppState = GameState.Game;
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }

    private void OnLevelComplited()
    {
        PlayerPrefs.SetInt(SAVEKEY_LEVEL, CurrentLevelNumber);
        ballAndTargetManager.DestroyLevel();
        CurrentLevelNumber++;
        CurrentAppState = GameState.Start;
    }

    private void Load()
    {
        CurrentLevelNumber = PlayerPrefs.GetInt(SAVEKEY_LEVEL, 0) + 1;
    }
}