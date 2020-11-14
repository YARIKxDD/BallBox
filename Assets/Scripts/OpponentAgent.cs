using UnityEngine;

public class OpponentAgent : MonoBehaviour
{
    private Transform leftWall;
    private Transform rightWall;

    private float minX;
    private float maxX;

    private float speed;
    private float neededX;

    private void Start()
    {
        CalculateLimits();
    }

    private void FixedUpdate()
    {
        MoveLogic();
    }

    private void CalculateLimits()
    {
        minX = leftWall.position.x + leftWall.localScale.x / 2 + transform.localScale.x / 2;
        maxX = rightWall.position.x - rightWall.localScale.x / 2 - transform.localScale.x / 2;
    }

    private void MoveLogic()
    {
        float difX = neededX - transform.position.x;
        float maxVX = speed * Time.fixedDeltaTime; 
        float vx = Mathf.Min(Mathf.Abs(difX), maxVX) * Mathf.Sign(difX);
        float newX = Mathf.Clamp(transform.position.x + vx, minX, maxX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void initiate(float speed, Transform leftWall, Transform rightWall)
    {
        this.speed = speed;
        this.leftWall = leftWall;
        this.rightWall = rightWall;
    }

    public void SetNeededX(float value)
    {
        neededX = value;
    }

    public void StopMoving()
    {
        neededX = transform.position.x;
    }
}