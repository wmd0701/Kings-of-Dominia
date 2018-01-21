using UnityEngine;
using UnityEngine.EventSystems;

class Cannon : MonoBehaviour, IPointerDownHandler
{
    #region Declarations

    [Header("Objects")]
    //------------------------------------------------------
    //Prefab für Projektil
    //------------------------------------------------------
    [SerializeField]
    private GameObject m_Canonball;
    //------------------------------------------------------
    //Spawnkoordinaten des Projektils
    //------------------------------------------------------
    [SerializeField]
    private Transform m_CanonSpawn;
    //------------------------------------------------------
    //LineRenderer Prefab für Schussanzeige
    //------------------------------------------------------    
    private LineRenderer m_ShotRenderer;
    [Header("Settings")]
    //------------------------------------------------------
    //Geschwindigkeit des Schusses
    //------------------------------------------------------
    [SerializeField]
    private int m_ShotVelocity;
    //------------------------------------------------------
    //Distanz zwischen einzelnen Renderpunkten
    //------------------------------------------------------
    [SerializeField]
    private float m_ShotRenderResolution;
    //------------------------------------------------------
    //Anzahl Schüsse
    //------------------------------------------------------
    [SerializeField]
    private int m_ShotCount;
    //------------------------------------------------------
    //Zeitintervall bis der Kanonenmodus beendet wird
    //------------------------------------------------------
    [SerializeField]
    float m_CancelInterval = 0.3f;
    //------------------------------------------------------
    //Referenz auf Kamerascript
    //------------------------------------------------------
    private CameraBehavior m_CameraBehavior;
    //------------------------------------------------------
    //Bool ob Kanone gesteuert wird
    //------------------------------------------------------
    bool m_CanonControllable = false;
    //------------------------------------------------------
    //Aktueller Timer
    //------------------------------------------------------
    float m_CancelCurrent;
    //------------------------------------------------------
    //Gespeicherte Kamera Position
    //------------------------------------------------------
    Vector3 m_CameraPositionSave;
    //------------------------------------------------------
    //Gespeicherte Kamera Rotation
    //------------------------------------------------------
    Quaternion m_CameraRotationSave;

    #endregion

    #region Properties

    /// <summary>
    /// Wechselt zwischen Kamera/Kanonenkontrolle
    /// </summary>
    private bool CannonControl
    {
        get
        {
            return m_CanonControllable;
        }
        set
        {
            //-----------------------------------------------------
            //Falls noch keine Kanone kontrolliert wird
            //-----------------------------------------------------
            if ((m_CameraBehavior.enabled && value) ||
                (!m_CameraBehavior.enabled && !value))
            {
                //------------------------------------------------------
                //Speichere/Lade Kamera Position & Rotation
                //------------------------------------------------------
                if (value)
                {
                    m_CameraPositionSave = Camera.main.transform.position;
                    m_CameraRotationSave = Camera.main.transform.rotation;
                }
                else
                {
                    Camera.main.transform.SetPositionAndRotation(m_CameraPositionSave, m_CameraRotationSave);
                }
                //-----------------------------------------------------
                //Wechsle zw. Kamera/Kanonenkontrolle
                //-----------------------------------------------------
                m_CameraBehavior.enabled = !value;
                m_CanonControllable = value;                
            }
        }
    }

    #endregion

    #region Catch Events

    private void Awake()
    {
        //------------------------------------------------------
        //Hole Referenz auf das Kamera-Script am Anfang
        //------------------------------------------------------
        m_CameraBehavior = Camera.main.GetComponent<CameraBehavior>();
        //------------------------------------------------------
        //Hole Referenz auf LineRenderer
        //------------------------------------------------------
        m_ShotRenderer = gameObject.GetComponent<LineRenderer>();
        //------------------------------------------------------
        //Setzte Interval zurück
        //------------------------------------------------------
        m_CancelCurrent = m_CancelInterval;
    }

    /// <summary>
    /// Wechsle zwischen Kanonen/Kamerakontrolle
    /// </summary>
    public void OnPointerDown(PointerEventData pi_Ped)
    {
        //------------------------------------------------------
        //Wenn im Edit-Mode
        //------------------------------------------------------
        if (UIManager.Instance.EditEnabled)
        {
            //------------------------------------------------------
            //Switche Kanonenkontrolle an/aus
            //------------------------------------------------------
            CannonControl = !CannonControl;
        }
    }    

    private void Update()
    {
        //------------------------------------------------------
        //Schalte Kontrolle ggf. ab (und deaktivere Anzeige)
        //------------------------------------------------------
        if (!UIManager.Instance.EditEnabled)
        {
            if (CannonControl)
                CannonControl = !CannonControl;
            if (m_ShotRenderer.positionCount > 0)
                m_ShotRenderer.positionCount = 0;
        }
        //------------------------------------------------------
        //Rendere ggf. Schussbahn
        //------------------------------------------------------
        else
        {
            if (m_ShotRenderer.positionCount == 0)
                ShowShot();
        }
        //-----------------------------------------------------
        //Return falls nicht kontrolliert wird
        //------------------------------------------------------
        if (!CannonControl)
            return;
        //------------------------------------------------------
        //Kamera schaut nun auf das Ziel der Kanone
        //------------------------------------------------------
        if (m_ShotRenderer.positionCount > 0)
            Camera.main.transform.LookAt(m_ShotRenderer.GetPosition(m_ShotRenderer.positionCount - 1));
        //-----------------------------------------------------
        //Ansonsten falls zwei Finger den Bildschirm berühren
        //-----------------------------------------------------
        if (Input.touchCount == 2)
        {
            Touch m_TouchZero, m_TouchOne;
            Vector2 m_PrevTouchZero, m_PrevTouchOne;
            //-----------------------------------------------------
            //Setzte Timer zurück
            //-----------------------------------------------------
            m_CancelCurrent = m_CancelInterval;
            //-----------------------------------------------------
            //Bestimme Touchpunkte (alte und neue)
            //-----------------------------------------------------
            m_TouchZero = Input.GetTouch(0);
            m_TouchOne = Input.GetTouch(1);
            m_PrevTouchZero = m_TouchZero.position - m_TouchZero.deltaPosition;
            m_PrevTouchOne = m_TouchOne.position - m_TouchOne.deltaPosition;
            //-----------------------------------------------------
            //Bestimme Delta
            //-----------------------------------------------------
            float l_TouchZeroDeltaAngle = Mathf.Atan2((m_TouchZero.position - m_TouchOne.position).y,
                                                      (m_TouchZero.position - m_TouchOne.position).x) * Mathf.Rad2Deg;
            float l_TouchOneDeltaAngle = Mathf.Atan2((m_PrevTouchZero - m_PrevTouchOne).y,
                                                     (m_PrevTouchZero - m_PrevTouchOne).x) * Mathf.Rad2Deg;
            //-----------------------------------------------------
            //Rotiere Kanone
            //-----------------------------------------------------
            gameObject.transform.Rotate(Vector3.up, l_TouchOneDeltaAngle - l_TouchZeroDeltaAngle);
            //-----------------------------------------------------
            //Update Schussanzeige
            //-----------------------------------------------------
            ShowShot();
        }
        else if(Input.touchCount > 0)
        {
            //-----------------------------------------------------
            //Aktualisiere Timer, beende ggf. Kanonenmodus
            //-----------------------------------------------------
            if (m_CancelCurrent <= 0.0f)
            {
                CannonControl = !CannonControl;
                m_CancelCurrent = m_CancelInterval;
            }
            else
            {
                m_CancelCurrent -= Time.unscaledDeltaTime;
            }
        }
    }

    #endregion

    #region Procedures

    /// <summary>
    /// Feuert Kanone ab
    /// </summary>
    public void Shoot()
    {
        //------------------------------------------------------
        //Falls die Kanone noch Schüsse hat
        //------------------------------------------------------
        if (m_ShotCount > 0)
        {
            //------------------------------------------------------
            //Erstelle ein Projektil
            //------------------------------------------------------
            GameObject l_Ball = Instantiate(m_Canonball, m_CanonSpawn.position, m_CanonSpawn.rotation);
            //------------------------------------------------------
            //Gebe dem Projektil eine Geschwindigkeit
            //------------------------------------------------------            
            l_Ball.GetComponent<Rigidbody>().velocity = m_CanonSpawn.forward.normalized * m_ShotVelocity;
            //------------------------------------------------------
            //Spiele passenden Sound
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayCannonShot();
            //------------------------------------------------------
            //Reduziere Anzahl der Schüsse
            //------------------------------------------------------
            m_ShotCount--;
        }
    }

    /// <summary>
    /// Zeigt Schussbahn an
    /// </summary>
    private void ShowShot()
    {
        //-----------------------------------------------------
        //Bestimme Schussgeschwindigkeit
        //-----------------------------------------------------
        Vector3 l_ShotVelocity = m_CanonSpawn.forward.normalized * m_ShotVelocity;
        //-----------------------------------------------------
        //Bestimme Teilgeschwindigkeiten
        //-----------------------------------------------------
        float l_Velocity_XY = Mathf.Sqrt((l_ShotVelocity.x * l_ShotVelocity.x) + (l_ShotVelocity.y * l_ShotVelocity.y));
        float l_Velocity_XZ = Mathf.Sqrt((l_ShotVelocity.x * l_ShotVelocity.x) + (l_ShotVelocity.z * l_ShotVelocity.z));
        //-----------------------------------------------------
        //Bestimme Schusswinkel
        //-----------------------------------------------------
        float l_Angle_XY = Mathf.Atan2(l_ShotVelocity.y, l_ShotVelocity.x);
        float l_Angle_XZ = Mathf.Atan2(l_ShotVelocity.z, l_ShotVelocity.x);
        //-----------------------------------------------------
        //Bestimme Auflösungsskala
        //-----------------------------------------------------
        float l_Resolution = m_ShotRenderResolution / l_ShotVelocity.magnitude;
        //-----------------------------------------------------
        //Erste Position ist der Spawn
        //-----------------------------------------------------
        m_ShotRenderer.positionCount = 1;
        m_ShotRenderer.SetPosition(0, m_CanonSpawn.position);
        //-----------------------------------------------------
        //Maximal 1000 Punkte! (Crash-Safe)
        //-----------------------------------------------------
        for(int i = 1; i< 1000; i++)
        {
            //-----------------------------------------------------
            //Bestimme nächste Koordinaten
            //-----------------------------------------------------
            float l_NextX = l_Velocity_XZ * (i * l_Resolution) * Mathf.Cos(l_Angle_XZ);
            float l_NextY = l_Velocity_XY * (i * l_Resolution) * Mathf.Sin(l_Angle_XY) -
                (Physics.gravity.magnitude * (i * l_Resolution) * (i * l_Resolution) / 2.0f);
            float l_NextZ = l_Velocity_XZ * (i * l_Resolution) * Mathf.Sin(l_Angle_XZ);
            //-----------------------------------------------------
            //Erstelle Punkt
            //-----------------------------------------------------
            Vector3 l_NextPos = new Vector3(m_CanonSpawn.position.x + l_NextX,
                                            m_CanonSpawn.position.y + l_NextY,
                                            m_CanonSpawn.position.z + l_NextZ);
            //------------------------------------------------------------------
            //Falls entweder das Level oder die DeathZone getroffen wurden..
            //------------------------------------------------------------------
            if (Physics.Linecast(m_ShotRenderer.GetPosition(m_ShotRenderer.positionCount - 1), l_NextPos, LayerMask.GetMask("Terrain")) ||
                Physics.Raycast(m_ShotRenderer.GetPosition(m_ShotRenderer.positionCount - 1),
                                m_ShotRenderer.GetPosition(m_ShotRenderer.positionCount - 1) - l_NextPos,
                                Mathf.Infinity, LayerMask.GetMask("DeathZone"), QueryTriggerInteraction.Collide))
            {
                //-----------------------------------------------------
                //..Dann verlasse die Schleife
                //-----------------------------------------------------
                break;
            }
            else
            {
                //-----------------------------------------------------
                //Ansonsten füge neue Position hinzu
                //-----------------------------------------------------
                m_ShotRenderer.positionCount += 1;
                m_ShotRenderer.SetPosition(m_ShotRenderer.positionCount - 1, l_NextPos);
            }
        }
    }

    #endregion
}