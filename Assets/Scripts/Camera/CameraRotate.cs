using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // Rotate the camera around the mid point of level, if player rotates 2 fingers

    [SerializeField]
    private Vector3 m_RotatePoint = new Vector3(0, 0, 0);

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch l_TouchZero = Input.GetTouch(0);
            Touch l_TouchOne = Input.GetTouch(1);

            Vector2 l_PrevTouchZero = l_TouchZero.position - l_TouchZero.deltaPosition;
            Vector2 l_PrevTouchOne = l_TouchOne.position - l_TouchOne.deltaPosition;
            
            float l_TouchZeroDeltaAngle = Mathf.Atan2((l_TouchZero.position - l_TouchOne.position).y,
                                                      (l_TouchZero.position - l_TouchOne.position).x) * Mathf.Rad2Deg;
            float l_TouchOneDeltaAngle = Mathf.Atan2((l_PrevTouchZero - l_PrevTouchOne).y,
                                                     (l_PrevTouchZero - l_PrevTouchOne).x) * Mathf.Rad2Deg;

            Camera.main.transform.RotateAround(m_RotatePoint, Vector3.up, l_TouchZeroDeltaAngle - l_TouchOneDeltaAngle);
        }
    }
}