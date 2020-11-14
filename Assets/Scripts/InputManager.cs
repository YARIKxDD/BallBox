using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector3> MouseStarted;
    public Action<Vector3> MouseUpdated;
    public Action MouseStopped;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mouseCatchLayerMask;
    [SerializeField] private LayerMask UILayerMask;

    bool canStartTrack;

    bool isTrackingNow;
    Vector3 currentMouseWorldPos;

    private void OnEnable()
    {
        levelManager.BallCreated += OnBallCreated;
        levelManager.BallPulled += OnBallPulled;
    }

    private void OnDisable()
    {
        levelManager.BallCreated -= OnBallCreated;
        levelManager.BallPulled -= OnBallPulled;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canStartTrack)
        {
            if (TryUpdateMouseWorldPosition())
            {
                isTrackingNow = true;
                if (MouseStarted != null)
                {
                    MouseStarted(currentMouseWorldPos);
                }
            }
        } //нажали на экран - если не над UI - начинаем трекать
        else if (Input.GetMouseButton(0) && isTrackingNow)
        {
            if (TryUpdateMouseWorldPosition())
            {
                if (MouseUpdated != null)
                {
                    MouseUpdated(currentMouseWorldPos);
                }
            }
        } //держим и не над UI - апдейтим
        else if (Input.GetMouseButtonUp(0))
        {
            if (isTrackingNow)
            {
                if (MouseStopped != null)
                {
                    MouseStopped();
                }
            }
            isTrackingNow = false;
        }
    }

    private bool TryUpdateMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, 200, mouseCatchLayerMask | UILayerMask);

        if ((UILayerMask & raycastHit.collider.gameObject.layer) == raycastHit.collider.gameObject.layer)
        {

            return false;
        } //курсор над UI

        currentMouseWorldPos = raycastHit.point;
        return true;
    }

    private void OnBallCreated(Transform tr)
    {
        canStartTrack = true;
    }

    private void OnBallPulled()
    {
        canStartTrack = false;
    }
}