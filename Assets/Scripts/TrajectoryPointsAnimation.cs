using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPointsAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform pointsRT;
    [SerializeField] private float maxOffset;
    [SerializeField] private float speed;

    void Update()
    {
        pointsRT.offsetMin += new Vector2(0, speed * Time.deltaTime);

        if (pointsRT.offsetMin.y < maxOffset)
            pointsRT.offsetMin -= new Vector2(0, maxOffset);
    }
}