﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Action StartClicked;
    public Action PauseClicked;
    public Action ContinueClicked;
    public Action ExitClicked;

    [SerializeField] private GameManager gameManager;
    [Space]
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject backgroundStartEnd;
    [SerializeField] private GameObject backgroundPause;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject continueButton;

    [SerializeField] private List<Text> levelNumbers = new List<Text>();

    private void OnEnable()
    {
        gameManager.CurrentGameStateChanged += OnCurrentAppStateChanged;
    }

    private void OnDisable()
    {
        gameManager.CurrentGameStateChanged -= OnCurrentAppStateChanged;
    }

    private void OnCurrentAppStateChanged(GameState appState)
    {
        switch (appState)
        {
            case GameState.Start:
                startButton.SetActive(true);
                continueButton.SetActive(false);
                backgroundStartEnd.SetActive(true);
                backgroundPause.SetActive(true);
                foreach(Text text in levelNumbers)
                {
                    text.text = gameManager.CurrentLevelNumber.ToString();
                }
                break;

            case GameState.Pause:
                break;
        }

        menuScreen.SetActive(appState != GameState.Game);
        gameScreen.SetActive(appState == GameState.Game);
    }

    public void ClickStartButton()
    {
        startButton.SetActive(false);
        continueButton.SetActive(true);
        backgroundStartEnd.SetActive(false);
        backgroundPause.SetActive(true);

        if (StartClicked != null)
        {
            StartClicked();
        }
    }

    public void ClickPauseButton()
    {
        if (PauseClicked != null)
        {
            PauseClicked();
        }
    }

    public void ClickContinueButton()
    {
        if (ContinueClicked != null)
        {
            ContinueClicked();
        }
    }

    public void ClickExitButton()
    {
        if (ExitClicked != null)
        {
            ExitClicked();
        }
    }
}