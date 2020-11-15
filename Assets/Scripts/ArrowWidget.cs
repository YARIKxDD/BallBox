using UnityEngine;
using UnityEngine.UI;

public class ArrowWidget : MonoBehaviour
{
    [SerializeField] private Transform rotatePart;
    [SerializeField] private Transform scalePart;
    [SerializeField] private Image arrow;
    [SerializeField] private Color colorValidated;
    [SerializeField] private Color colorNotValidated;
    [SerializeField] private AnimationCurve magnitudeToScale;

    public void Set(Vector3 offset, bool validated)
    {
        rotatePart.localEulerAngles = new Vector3(rotatePart.localEulerAngles.x, GetAngle(offset), rotatePart.localEulerAngles.z);
        scalePart.localScale = new Vector3(magnitudeToScale.Evaluate(offset.magnitude), scalePart.localScale.y, scalePart.localScale.z);

        arrow.color = validated ? colorValidated : colorNotValidated;
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
