using UnityEngine;

public class TouchMovement : MonoBehaviour
{
    [Header("Settings")]
    //------------------------------------------------------
    //Geschwindigkeiten für Perspektivisch/Orthografisch
    //------------------------------------------------------
    [SerializeField]
    private float m_ZoomSpeed = 50f;

    [SerializeField]
    private float m_ZoomMin = 0f;

    [SerializeField]
    private float m_ZoomMax = 250f;
    //------------------------------------------------------
    //Kamerabewegungsgeschwindigkeiten
    //------------------------------------------------------    
    [SerializeField]
    private float m_MoveSpeed = 10f;    
    //------------------------------------------------------
    //Kartenmittelpunkt
    //------------------------------------------------------
    [SerializeField]
    private Vector3 m_RotatePoint = new Vector3(0, 0, 0);

    private void Update () {
        //------------------------------------------------------
        //Wenn es genau zwei Touches gab
        //------------------------------------------------------
        if (Input.touchCount == 2)
        {
            //------------------------------------------------------
            //Hole die Touches
            //------------------------------------------------------
            Touch l_TouchZero = Input.GetTouch(0);
            Touch l_TouchOne = Input.GetTouch(1);
            //------------------------------------------------------
            //Berechne vorherige Positionen
            //------------------------------------------------------
            Vector2 l_PrevTouchZero = l_TouchZero.position - l_TouchZero.deltaPosition;
            Vector2 l_PrevTouchOne = l_TouchOne.position - l_TouchOne.deltaPosition;
            //------------------------------------------------------
            //Bestimme Distanz zwischen den Touches
            //------------------------------------------------------
            float l_PrevTouchMag = (l_PrevTouchZero - l_PrevTouchOne).magnitude;
            float l_CurrTouchMag = (l_TouchZero.position - l_TouchOne.position).magnitude;
            //------------------------------------------------------
            //Bestimme Delta zwischen den Touches
            //------------------------------------------------------
            float l_DeltaMagDiff = l_PrevTouchMag - l_CurrTouchMag;
            //------------------------------------------------------
            //Wenn nicht auf einem Punkt gehalten wird
            //------------------------------------------------------
            if (l_TouchZero.phase != TouchPhase.Stationary && l_TouchOne.phase != TouchPhase.Stationary)
            {
                Vector3 l_CurrTouchAvg = (l_TouchOne.position + l_TouchZero.position) / 2;
                Vector3 l_PrevTouchAvg = (l_PrevTouchOne + l_PrevTouchZero) / 2;
                //------------------------------------------------------
                //Bewege Kamera entsprechend
                //------------------------------------------------------
                Camera.main.transform.Translate(new Vector3((l_PrevTouchAvg.x - l_CurrTouchAvg.x) * Time.deltaTime * m_MoveSpeed,
                                                            (l_PrevTouchAvg.y - l_CurrTouchAvg.y) * Time.deltaTime * m_MoveSpeed,
                                                            (l_PrevTouchAvg.y - l_CurrTouchAvg.y) * Time.deltaTime * m_MoveSpeed));
            }
            //------------------------------------------------------
            //Bestimme Delta-Winkel
            //------------------------------------------------------
            float l_TouchZeroDeltaAngle = Mathf.Atan2((l_TouchZero.position - l_TouchOne.position).y,
                                                      (l_TouchZero.position - l_TouchOne.position).x) * Mathf.Rad2Deg;
            float l_TouchOneDeltaAngle = Mathf.Atan2((l_PrevTouchZero - l_PrevTouchOne).y,
                                                     (l_PrevTouchZero - l_PrevTouchOne).x) * Mathf.Rad2Deg;
            //------------------------------------------------------
            //Rotiere Kamera um den Kartenmittelpunkt
            //------------------------------------------------------
            Camera.main.transform.RotateAround(m_RotatePoint, Vector3.up, l_TouchZeroDeltaAngle - l_TouchOneDeltaAngle);
            //------------------------------------------------------
            //Abstand ändern..
            //------------------------------------------------------
            Vector3 l_NewPos = Camera.main.transform.position + 
                Camera.main.transform.forward * -1 * l_DeltaMagDiff * m_ZoomSpeed * Time.deltaTime;
            //------------------------------------------------------
            //..Falls möglich
            //------------------------------------------------------
            if (l_NewPos.y <= m_ZoomMax && l_NewPos.y >= m_ZoomMin)
                Camera.main.transform.position = l_NewPos;
        }
    }
}