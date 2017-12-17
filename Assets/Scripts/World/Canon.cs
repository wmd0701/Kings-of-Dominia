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
        //------------------------------------------------------
        //Gebe entsprechende Geschwindigkeit
        //------------------------------------------------------
        m_ShowedShot.GetComponent<Rigidbody>().velocity = m_CanonSpawn.forward.normalized * m_ShotVelocity;        
    }
}