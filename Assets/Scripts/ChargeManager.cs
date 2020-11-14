using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeManager : MonoBehaviour
{
    public Action<Vector3> Pulled;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private float minMagnitude;
    [SerializeField] private float maxMagnitude;

    private Transform player;
    private ArrowWidget arrowWidget;

    private Vector3 startPosition;
    private Vector3 currentPosition;
    private Vector3 Offset
    {
        get
        {
            Vector3 offset = startPosition - currentPosition;

            if (offset.magnitude > maxMagnitude) 
                offset = offset.normalized * maxMagnitude;

            return offset;
        }
    }

    private void OnEnable()
    {
        inputManager.MouseStarted += OnMouseStarted;
        inputManager.MouseUpdated += OnMouseUpdated;
        inputManager.MouseStopped += OnMouseStopped;
    }

    private void OnDisable()
    {
        inputManager.MouseStarted -= OnMouseStarted;
        inputManager.MouseUpdated -= OnMouseUpdated;
        inputManager.MouseStopped -= OnMouseStopped;
    }

    private void OnMouseStarted(Vector3 position)
    {
        startPosition = position;
        player.position = arrowWidget.transform.position - Offset;
    }

    private void OnMouseUpdated(Vector3 position)
    {
        currentPosition = position;
        player.position = arrowWidget.transform.position - Offset;

        if (Offset.magnitude < minMagnitude)
        {
            if (arrowWidget.gameObject.activeSelf)
            {
                arrowWidget.gameObject.SetActive(false);
            }
        }
        else
        {
            arrowWidget.Set(Offset);
            if (!arrowWidget.gameObject.activeSelf)
            {
                arrowWidget.gameObject.SetActive(true);
            }
        }
    }

    private void OnMouseStopped()
    {
        arrowWidget.gameObject.SetActive(false);

        Vector3 offset = startPosition - currentPosition;

        if (offset.magnitude < minMagnitude)
        {
            return;
        }

        if (Pulled != null)
        {
            Pulled(offset);
        }
    }

    public void SetRef(Transform player, ArrowWidget arrowWidget)
    {
        this.player = player;
        this.arrowWidget = arrowWidget;
    }
}