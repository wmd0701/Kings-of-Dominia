using UnityEngine;
using UnityEngine.EventSystems;

class Canon : MonoBehaviour, IPointerDownHandler
{
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
    //Trailrenderer Prefab
    //------------------------------------------------------
    [SerializeField]
    private GameObject m_TrailRenderer;
    [Header("Settings")]
    //------------------------------------------------------
    //Geschwindigkeit des Schusses
    //------------------------------------------------------
    [SerializeField]
    private int m_ShotVelocity;
    //------------------------------------------------------
    //Anzahl Schüsse
    //------------------------------------------------------
    [SerializeField]
    private int m_RemainingShots = 1;
    //------------------------------------------------------
    //Aktuelle Schussanzeige
    //------------------------------------------------------
    private GameObject m_ShowedShot;

    //------------------------------------------------------
    //um die Umwandlung zwischen canon mode und camera mode kontrollieren
    //------------------------------------------------------
    CameraBehavior c;

    [SerializeField]
    private float CanonMoveSpeed = 5.0f;

    private void Awake()
    {
        c = Camera.main.GetComponent<CameraBehavior>();
    }

    /// <summary>
    /// Feuert Kanone ab
    /// </summary>
    public void Shoot()
    {
        //------------------------------------------------------
        //Falls die Kanone noch Schüsse hat
        //------------------------------------------------------
        if (m_RemainingShots > 0)
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
            //Registriere den RB des Schusses
            //------------------------------------------------------
            FreezeManager.Instance.RegisterRB(l_Ball.GetComponent<Rigidbody>());
            //------------------------------------------------------
            //Reduziere Anzahl der Schüsse
            //------------------------------------------------------
            m_RemainingShots--;
        }
    }

    /// <summary>
    /// Zeige Schuss / Lösche Anzeige
    /// </summary>
    public void OnPointerDown(PointerEventData pi_Ped)
    {
        //------------------------------------------------------
        //Zerstöre / Erstelle je nach dem
        //------------------------------------------------------
        if (m_ShowedShot != null)
            Destroy(m_ShowedShot);
        else
            ShowShot();

        CanonMode();
    }

    private void CanonMode() {
        CameraBehavior c = Camera.main.GetComponent<CameraBehavior>();
        if (!c.enabled)
        {
            Debug.Log("Change from canon mode to camera mode, u can controll camera now");
            c.enabled = true;
        }
        else
        {
            Debug.Log("Change from camera mode to canon mode, u can controll canon now");
            c.enabled = false;
        }
    }

    private void Update()
    {
        //-----------------------------------------------------
        //c.enabled == false ---> canon mode
        //-----------------------------------------------------
        if (c.enabled)
            return;

        if (Input.touchCount == 2)
        {
            Touch m_TouchZero, m_TouchOne;
            Vector2 m_PrevTouchZero, m_PrevTouchOne;

            m_TouchZero = Input.GetTouch(0);
            m_TouchOne = Input.GetTouch(1);

            m_PrevTouchZero = m_TouchZero.position - m_TouchZero.deltaPosition;
            m_PrevTouchOne = m_TouchOne.position - m_TouchOne.deltaPosition;

            float l_TouchZeroDeltaAngle = Mathf.Atan2((m_TouchZero.position - m_TouchOne.position).y,
                                                  (m_TouchZero.position - m_TouchOne.position).x) * Mathf.Rad2Deg;
            float l_TouchOneDeltaAngle = Mathf.Atan2((m_PrevTouchZero - m_PrevTouchOne).y,
                                                      (m_PrevTouchZero - m_PrevTouchOne).x) * Mathf.Rad2Deg;

            gameObject.transform.Rotate(Vector3.up, l_TouchOneDeltaAngle - l_TouchZeroDeltaAngle);
            Destroy(m_ShowedShot);
            ShowShot();
        }
        else if (Input.touchCount == 1)
        {
            RaycastHit l_Hit;
            Ray l_Ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
            gameObject.transform.position = l_Hit.point;
            Destroy(m_ShowedShot);
            ShowShot();
        }
    }

    /// <summary>
    /// Zeigt Schuss an
    /// </summary>
    private void ShowShot()
    {
        //------------------------------------------------------
        //Erstelle neues Anzeigeobjekt
        //------------------------------------------------------
        m_ShowedShot = Instantiate(m_TrailRenderer, m_CanonSpawn.position, m_CanonSpawn.rotation);
        // m_ShowedShot.transform.parent = gameObject.transform;
        //------------------------------------------------------
        //Gebe entsprechende Geschwindigkeit
        //------------------------------------------------------
        m_ShowedShot.GetComponent<Rigidbody>().velocity = m_CanonSpawn.forward.normalized * m_ShotVelocity;        
    }
}