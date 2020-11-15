using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Action<Vector3> PullNow;

    [SerializeField] private BallAndTargetManager ballAndTargetManager;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mouseCatchLayerMask;
    [SerializeField] private float minDebugOffset;
    [SerializeField] private float minMagnitude;
    [SerializeField] private float maxMagnitude;
    [SerializeField] private float minZOffsetToPull;
    [SerializeField] private Vector3 playerDefaultOffset;

    private Transform player;
    private ArrowWidget arrowWidget;

    bool canStartTrack;
    bool isTrackingNow;

    private Vector3 startPosition;
    private Vector3 currentPosition;
    private Vector3 Offset
    {
        get
        {
            Vector3 offset = startPosition - currentPosition;
            offset.y = 0;

            if (offset.magnitude > maxMagnitude)
                offset = offset.normalized * maxMagnitude;

            return offset;
        }
    }

    private void OnEnable()
    {
        ballAndTargetManager.BallCreated += OnBallCreated;
        ballAndTargetManager.BallPulled += OnBallPulled;
    }

    private void OnDisable()
    {
        ballAndTargetManager.BallCreated -= OnBallCreated;
        ballAndTargetManager.BallPulled -= OnBallPulled;
    }

    private void Update()
    {
        if (Time.timeScale == 0) 
            return;

        if (Input.GetMouseButtonDown(0) && canStartTrack)
        {
            UpdateMouseWorldPosition();
            startPosition = currentPosition;
            isTrackingNow = true;
            player.position = arrowWidget.transform.position + playerDefaultOffset;
        }
        else if (Input.GetMouseButton(0) && isTrackingNow)
        {
            UpdateMouseWorldPosition();

            Vector3 offsetForPlayer = Offset;
            if (offsetForPlayer.magnitude < minDebugOffset)
            {
                offsetForPlayer = -playerDefaultOffset;
            }
            else if (offsetForPlayer.magnitude < minMagnitude)
            {
                offsetForPlayer = offsetForPlayer.normalized * minMagnitude;
            }            
            player.position = arrowWidget.transform.position - offsetForPlayer;

            arrowWidget.Set(Offset, IsOffsetValid());
            if (!arrowWidget.gameObject.activeSelf)
            {
                arrowWidget.gameObject.SetActive(true);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isTrackingNow)
            {
                player.position = arrowWidget.transform.position + playerDefaultOffset;
                arrowWidget.gameObject.SetActive(false);
                if (IsOffsetValid() && PullNow != null)
                {
                    PullNow(Offset);
                }
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

    private void UpdateMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, 200, mouseCatchLayerMask);

        currentPosition = raycastHit.point;
    }

    private bool IsOffsetValid()
    {
        if (Offset.magnitude < minMagnitude)
            return false;

        if (Offset.z < minZOffsetToPull)
            return false;

        return true;
    }

    public void SetFromRef(Transform player, ArrowWidget arrowWidget)
    {
        this.player = player;
        this.arrowWidget = arrowWidget;
    }
}