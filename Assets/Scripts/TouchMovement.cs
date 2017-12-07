using UnityEngine;

public class TouchMovement : MonoBehaviour {
    //------------------------------------------------------
    //Geschwindigkeiten für Perspektivisch/Orthografisch
    //------------------------------------------------------
    public float m_PerspectiveZoomSpeed = .1f;
    public float m_OrthographicZoomSpeed = .1f;
    //------------------------------------------------------
    //Kamerabewegungsgeschwindigkeiten
    //------------------------------------------------------
    public float m_RotationSpeed = 1f;
    public float m_MoveSpeed = .1f;
    //------------------------------------------------------
    //Kartenmittelpunkt
    //------------------------------------------------------
    public Vector3 m_RotatePoint = new Vector3(0,0,0);    

	void Update () {
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
            //Bestimme Delta-Winkel
            //------------------------------------------------------
            float l_TouchZeroDeltaAngle = Mathf.Atan2((l_TouchZero.position - l_TouchOne.position).y,
                                                      (l_TouchZero.position - l_TouchOne.position).x) * Mathf.Rad2Deg;
            float l_TouchOneDeltaAngle = Mathf.Atan2((l_PrevTouchZero - l_PrevTouchOne).y,
                                                     (l_PrevTouchZero - l_PrevTouchOne).x) * Mathf.Rad2Deg;
            //------------------------------------------------------
            //Rotiere Kamera um den Kartenmittelpunkt
            //------------------------------------------------------
            Camera.main.transform.RotateAround(m_RotatePoint, new Vector3(0, 1, 0), l_TouchZeroDeltaAngle - l_TouchOneDeltaAngle);
            //------------------------------------------------------
            //Je nach Kameraeinstellung..
            //------------------------------------------------------
            if (Camera.main.orthographic)
            {
                //------------------------------------------------------
                //Größe anpassen (darf Limit nicht unterschreiten)
                //------------------------------------------------------
                Camera.main.orthographicSize += l_DeltaMagDiff * m_OrthographicZoomSpeed;
                Camera.main.orthographicSize = Mathf.Max(Mathf.Min(Camera.main.orthographicSize, 1f), .1f);
            }
            else
            {
                //------------------------------------------------------
                //FOV anpassen (darf Limit nicht unterschreiten)
                //------------------------------------------------------
                Camera.main.fieldOfView += l_DeltaMagDiff * m_PerspectiveZoomSpeed;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 5f, 25f);
            }
        }
        else if(Input.touchCount == 1)
        {
            //------------------------------------------------------
            //Hole Touch
            //------------------------------------------------------
            Touch l_TouchZero = Input.GetTouch(0);
            Vector2 l_PrevTouch = l_TouchZero.position - l_TouchZero.deltaPosition;
            //------------------------------------------------------
            //Wenn nicht auf einem Punkt gehalten wird
            //------------------------------------------------------
            if (l_TouchZero.phase != TouchPhase.Stationary)
            {
                //------------------------------------------------------
                //Bewege Kamera entsprechend
                //------------------------------------------------------
                Camera.main.transform.Translate(new Vector3((l_PrevTouch.x - l_TouchZero.position.x) * Time.deltaTime * m_MoveSpeed,
                                                            (l_PrevTouch.y - l_TouchZero.position.y) * Time.deltaTime * m_MoveSpeed,
                                                            (l_PrevTouch.y - l_TouchZero.position.y) * Time.deltaTime * m_MoveSpeed));
            }
        }
    }
}