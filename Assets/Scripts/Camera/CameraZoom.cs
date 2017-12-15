using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // Move the camera towards/backwards the looking direction, if player moves 2 fingers closer/further
    // which equals to zoom the camera in/out

    [SerializeField]
    private float m_ZoomSpeed = 100.0f;
    
    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch l_TouchZero = Input.GetTouch(0);
            Touch l_TouchOne = Input.GetTouch(1);

            Vector2 l_PrevTouchZero = l_TouchZero.position - l_TouchZero.deltaPosition;
            Vector2 l_PrevTouchOne = l_TouchOne.position - l_TouchOne.deltaPosition;

            float l_PrevTouchMag = (l_PrevTouchZero - l_PrevTouchOne).magnitude;
            float l_CurrTouchMag = (l_TouchZero.position - l_TouchOne.position).magnitude;

            float l_DeltaMagDiff = l_PrevTouchMag - l_CurrTouchMag;

            Camera.main.transform.Translate(Camera.main.transform.forward * -1 * l_DeltaMagDiff * m_ZoomSpeed * Time.deltaTime, Space.World);

            // Camera.main.fieldOfView += l_DeltaMagDiff * m_ZoomSpeed;
            // Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, m_MinFOV, m_MaxFOV);            
        }
    }
}