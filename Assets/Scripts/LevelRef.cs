using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRef : MonoBehaviour
{
    public Transform Player;
    public ArrowWidget ArrowWidget;
    public OpponentAgent OpponentAgent;
    public Transform BallStartPoint;
    public List<Target> Targets = new List<Target>();
}