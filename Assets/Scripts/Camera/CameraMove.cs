using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Move the camera towards up/bottom/left/right, if player drags in the corresponding direction with 2 fingers

    [SerializeField]
    private float m_MoveSpeed = 10.0f;

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch l_TouchZero = Input.GetTouch(0);
            Touch l_TouchOne = Input.GetTouch(1);

            Vector2 l_PrevTouchZero = l_TouchZero.position - l_TouchZero.deltaPosition;
            Vector2 l_PrevTouchOne = l_TouchOne.position - l_TouchOne.deltaPosition;

            if (l_TouchZero.phase != TouchPhase.Stationary && l_TouchOne.phase != TouchPhase.Stationary)
            {
                
                Vector3 l_CurrTouchAvg = (l_TouchOne.position + l_TouchZero.position) / 2;
                Vector3 l_PrevTouchAvg = (l_PrevTouchOne + l_PrevTouchZero) / 2;

                Camera.main.transform.Translate((l_PrevTouchAvg - l_CurrTouchAvg) * m_MoveSpeed * Time.deltaTime);
            }
        }
    }
}