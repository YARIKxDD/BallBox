using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Action<Transform> BallCreated;
    public Action BallPulled;
    public Action BallDestroyed;
    public Action LevelComplited;

    [SerializeField] private AIManager aiManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float ballSpeedK;

    private GameObject level;
    private List<Target> targets = new List<Target>();
    private Transform ballStartPoint;

    private Ball ballBuffer;

    private void OnEnable()
    {
        inputManager.PullNow += PullBall;
    }

    private void OnDisable()
    {
        inputManager.PullNow -= PullBall;
    }

    private void Update()
    {
        
    }

    private void CreateBall()
    {
        GameObject newBall = Instantiate(ballPrefab, ballStartPoint);
        ballBuffer = newBall.GetComponent<Ball>();

        ballBuffer.TargetHitted += OnTargetHitted;
        ballBuffer.BallCatched += OnBallCatched;

        if (BallCreated != null)
        {
            BallCreated(ballBuffer.transform);
        }
    }

    private void OnTargetHitted(Target target)
    {
        targets.Remove(target);
        target.OnCatch();

        DestroyBall();
    }

    private void OnBallCatched()
    {
        DestroyBall();
    }

    private void DestroyBall()
    {
        ballBuffer.TargetHitted -= OnTargetHitted;
        ballBuffer.BallCatched -= OnBallCatched;

        if (BallDestroyed != null)
        {
            BallDestroyed();
        }
        Destroy(ballBuffer.gameObject);

        if (targets.Count > 0)
        {
            CreateBall();
        }
        else
        {
            if (LevelComplited != null)
            {
                LevelComplited();
            }
        }
    }

    public void BuildLevel()
    {
        level = Instantiate(levelPrefab, null);

        LevelRef levelRef = level.GetComponent<LevelRef>();
        ballStartPoint = levelRef.BallStartPoint;
        targets.AddRange(levelRef.Targets);
        aiManager.FindNewAgent(levelRef.OpponentAgent);
        inputManager.SetFromRef(levelRef.Player, levelRef.ArrowWidget);

        CreateBall();
    }

    public void DestroyLevel()
    {
        if (level != null)
        {
            Destroy(level);
        }
    }

    public void PullBall(Vector3 startSpeed)
    {
        ballBuffer.StartMove(startSpeed * ballSpeedK);

        if (BallPulled != null)
        {
            BallPulled();
        }
    }
}