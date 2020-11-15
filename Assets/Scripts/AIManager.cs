using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BallAndTargetManager ballAndTargetManager;
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;
    [SerializeField] private float speedAtLevel1;
    [SerializeField] private float addSpeedPerLevel;

    private OpponentAgent agentBuffer;
    private Transform ballBuffer;

    private void OnEnable()
    {
        ballAndTargetManager.BallCreated += OnBallCreated;
        ballAndTargetManager.BallDestroyed += OnBallDestroyed;
    }

    private void OnDisable()
    {
        ballAndTargetManager.BallCreated -= OnBallCreated;
        ballAndTargetManager.BallDestroyed -= OnBallDestroyed;
    }

    private void FixedUpdate()
    {
        ControllAgent();
    }

    private void OnBallCreated(Transform ball)
    {
        ballBuffer = ball;
    }

    private void OnBallDestroyed()
    {
        ballBuffer = null;
        agentBuffer.StopMoving();
    }

    private void ControllAgent()
    {
        if (ballBuffer == null || agentBuffer == null)
        {
            return;
        }

        agentBuffer.SetNeededX(ballBuffer.position.x);

    }

    public void FindNewAgent(OpponentAgent opponentAgent)
    {
        agentBuffer = opponentAgent;
        opponentAgent.initiate(speedAtLevel1 + (gameManager.CurrentLevelNumber - 1) * addSpeedPerLevel, leftWall, rightWall);
    }
}