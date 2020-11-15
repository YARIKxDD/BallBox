using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector3> PullNow;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mouseCatchLayerMask;
    [SerializeField] private LayerMask UILayerMask;
    [SerializeField] private float minMagnitude;
    [SerializeField] private float maxMagnitude;
    [SerializeField] private float minZOffsetToPull;
    [SerializeField] private Vector3 playerDefaultOffset;

    private Transform player;
    private ArrowWidget arrowWidget;

    bool canStartTrack;
    bool isTrackingNow;
    Vector3 currentMouseWorldPos;

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
                MouseStarted(currentMouseWorldPos);
            }
        } //нажали на экран - если не над UI - начинаем трекать
        else if (Input.GetMouseButton(0) && isTrackingNow)
        {
            if (TryUpdateMouseWorldPosition())
            {
                MouseUpdated(currentMouseWorldPos);
            }
        } //держим и не над UI - апдейтим
        else if (Input.GetMouseButtonUp(0))
        {
            if (isTrackingNow)
            {
                MouseStopped();
            }
            isTrackingNow = false;
        }
    }

    private void OnBallCreated(Transform tr)
    {
        canStartTrack = true;
    }

    private void OnBallPulled()
    {
        canStartTrack = false;
    }

    private void MouseStarted(Vector3 position)
    {
        startPosition = position;
        player.position = arrowWidget.transform.position;
    }

    private void MouseUpdated(Vector3 position)
    {
        currentPosition = position;
        player.position = arrowWidget.transform.position - Offset;

        arrowWidget.Set(Offset, IsOffsetValid());
        if (!arrowWidget.gameObject.activeSelf)
        {
            arrowWidget.gameObject.SetActive(true);
        }
    }

    private void MouseStopped()
    {
        player.position = arrowWidget.transform.position + playerDefaultOffset;
        arrowWidget.gameObject.SetActive(false);

        if (!IsOffsetValid())
        {
            return;
        }

        if (PullNow != null)
        {
            PullNow(Offset);
        }
    }

    private bool IsOffsetValid()
    {
        if (Offset.magnitude < minMagnitude)
            return false;

        if (Offset.z < minZOffsetToPull)
            return false;

        return true;
    }

    private bool TryUpdateMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, 200, mouseCatchLayerMask | UILayerMask);

        if ((UILayerMask & (1 << raycastHit.transform.gameObject.layer)) != 0)
        {
            return false;
        }//курсор над UI

        currentMouseWorldPos = raycastHit.point;
        return true;
    }

    public void SetFromRef(Transform player, ArrowWidget arrowWidget)
    {
        this.player = player;
        this.arrowWidget = arrowWidget;
    }
}