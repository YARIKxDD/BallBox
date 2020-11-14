using UnityEngine;

public class ArrowWidget : MonoBehaviour
{
    [SerializeField] private Transform rotatePart;
    [SerializeField] private Transform scalePart;
    [SerializeField] private AnimationCurve magnitudeToScale;

    public void Set(Vector3 offset)
    {
        rotatePart.localEulerAngles = new Vector3(rotatePart.localEulerAngles.x, GetAngle(offset), rotatePart.localEulerAngles.z);
        scalePart.localScale = new Vector3(magnitudeToScale.Evaluate(offset.magnitude), scalePart.localScale.y, scalePart.localScale.z);
    }

    static float GetAngle(Vector3 vector)
    {
        Vector3 v = vector.normalized;
        if (v.z != 0)
        {
            float angle = Mathf.Atan(v.x / v.z) * Mathf.Rad2Deg;

            if (v.z < 0)
                angle += 180;

            return angle;
        }
        else
        {
            if (v.x == 0 || v.x == 1)
                return 90;

            return 270;
        }
    }
}
