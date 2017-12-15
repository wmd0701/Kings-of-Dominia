using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField]
    private float m_ZoomSpeed = 50f;

    [SerializeField]
    private float m_ZoomMin = 0f;

    [SerializeField]
    private float m_ZoomMax = 250f;

    [SerializeField]
    private float m_MoveSpeed = 10f;

    [SerializeField]
    private Vector3 m_RotatePoint = new Vector3(0, 0, 0);


    private Touch l_TouchZero, l_TouchOne;
    private Vector2 l_PrevTouchZero, l_PrevTouchOne;

    private void Update () {
        if (Input.touchCount == 2)
        {
            l_TouchZero = Input.GetTouch(0);
            l_TouchOne = Input.GetTouch(1);

            l_PrevTouchZero = l_TouchZero.position - l_TouchZero.deltaPosition;
            l_PrevTouchOne = l_TouchOne.position - l_TouchOne.deltaPosition;

            CameraRotate();
            CameraMove();
            CameraZoom();            
        }
    }

    private void CameraRotate() {
        float l_TouchZeroDeltaAngle = Mathf.Atan2((l_TouchZero.position - l_TouchOne.position).y,
                                                  (l_TouchZero.position - l_TouchOne.position).x) * Mathf.Rad2Deg;
        float l_TouchOneDeltaAngle =  Mathf.Atan2((l_PrevTouchZero - l_PrevTouchOne).y,
                                                  (l_PrevTouchZero - l_PrevTouchOne).x) * Mathf.Rad2Deg;
        
        Camera.main.transform.RotateAround(m_RotatePoint, Vector3.up, l_TouchZeroDeltaAngle - l_TouchOneDeltaAngle);
    }

    private void CameraMove() {
        if (l_TouchZero.phase != TouchPhase.Stationary && l_TouchOne.phase != TouchPhase.Stationary)
        {
            Vector3 l_CurrTouchAvg = (l_TouchOne.position + l_TouchZero.position) / 2;
            Vector3 l_PrevTouchAvg = (l_PrevTouchOne + l_PrevTouchZero) / 2;

            Camera.main.transform.Translate((l_PrevTouchAvg - l_CurrTouchAvg) * Time.deltaTime * m_MoveSpeed);
        }
    }

    private void CameraZoom() {
        float l_PrevTouchMag = (l_PrevTouchZero - l_PrevTouchOne).magnitude;
        float l_CurrTouchMag = (l_TouchZero.position - l_TouchOne.position).magnitude;

        float l_DeltaMagDiff = l_PrevTouchMag - l_CurrTouchMag;

        Vector3 l_NewPos = Camera.main.transform.position +
                           Camera.main.transform.forward * -1 * l_DeltaMagDiff * m_ZoomSpeed * Time.deltaTime;
        
        if (l_NewPos.y <= m_ZoomMax && l_NewPos.y >= m_ZoomMin)
            Camera.main.transform.position = l_NewPos;
    }
}