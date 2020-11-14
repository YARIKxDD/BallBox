using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Action<Target> TargetHitted;
    public Action BallCatched;

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private LayerMask oponentLayerMask;
    [SerializeField] private LayerMask targetLayerMask;

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.gameObject.layer;

        if ((oponentLayerMask & (1 << layer)) != 0)
        {
            if (BallCatched != null)
            {
                BallCatched();
            }

            //some bad effect...
        }
        if ((targetLayerMask & (1 << layer)) != 0)
        {
            if (TargetHitted != null)
            {
                TargetHitted(collision.gameObject.GetComponent<Target>());
            }

            //some good effect...
        }
    }

    public void StartMove(Vector3 startSpeed)
    {
        rigidbody.velocity = startSpeed;
    }
}