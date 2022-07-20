using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsContainer : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] float smoothTimer;
    [SerializeField] float clampYValue;
    [SerializeField] float xMagnitude;
    [SerializeField] float yMagnitude;
    [SerializeField] float intensity;
    public void SetIntensity(float _value) { intensity = _value; }
    bool isMoving;
    public void SetIsMoving(bool _value){ isMoving = _value; }

    Vector3 supposedPosition;

    float customSinX = -.5f;
    bool approachPeakSinX = true;

    float customSinY = 0f;
    bool approachPeakSinY = true;

    private void Start()
    {
        supposedPosition = transform.localPosition;
        offsetVector = transform.localPosition;
    }

    private void FixedUpdate()
    {
        supposedPosition.x = customSinX * xMagnitude;
        supposedPosition.y = (customSinY + .5f) * yMagnitude;

        MovementOffsetter();
        offsetVector.y = Mathf.Clamp(offsetVector.y, -clampYValue, clampYValue);
        transform.localPosition = supposedPosition + offsetVector;        

        if (isMoving)
        {
            if (customSinX <= -.5f) approachPeakSinX = true; else if (customSinX >= .5f) approachPeakSinX = false;
            if (approachPeakSinX) customSinX += Time.deltaTime * intensity; else customSinX -= Time.deltaTime * intensity;

            if (customSinY <= 0f) approachPeakSinY = true; else if (customSinY >= 1f) approachPeakSinY = false;
            if (approachPeakSinY) customSinY += Time.deltaTime * intensity * 2; else customSinY -= Time.deltaTime * intensity * 2;

            return;
        }

        customSinX = Mathf.Lerp(customSinX, -.5f, 0.005f);
        customSinY = Mathf.Lerp(customSinY, 0, 0.005f);
    }

    Vector3 smoothing_vector;
    Vector3 ref_vector;
    Vector3 offsetVector;
    void MovementOffsetter()
    {
        smoothing_vector = new Vector3(FindRelativeVelocity().x, ObjectsDatabase.singleton.playerAgent.GetRigidbody().velocity.y, FindRelativeVelocity().y) * -offset;
        offsetVector = Vector3.SmoothDamp(offsetVector, smoothing_vector, ref ref_vector, smoothTimer);
    }
    Vector2 FindRelativeVelocity()
    {
        float look_angle = transform.eulerAngles.y;
        float move_angle = Mathf.Atan2(ObjectsDatabase.singleton.playerAgent.GetRigidbody().velocity.x, ObjectsDatabase.singleton.playerAgent.GetRigidbody().velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(look_angle, move_angle);
        float v = 90 - u;

        float magnitude = ObjectsDatabase.singleton.playerAgent.GetRigidbody().velocity.magnitude;
        float y_magnitude = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float x_magnitude = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(x_magnitude, y_magnitude);
    }
}
