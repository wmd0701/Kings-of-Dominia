using UnityEngine;

class Canon : MonoBehaviour
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
    [Header("Settings")]
    //------------------------------------------------------
    //"Stärke" des Schusses
    //------------------------------------------------------
    [SerializeField]
    private int m_Power = 5;
    //------------------------------------------------------
    //Anzahl Schüsse
    //------------------------------------------------------
    [SerializeField]
    private int m_RemainingShots = 1;

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
            //Feuere es ab
            //------------------------------------------------------
            l_Ball.transform.GetComponent<Rigidbody>().AddForce(transform.forward * m_Power, ForceMode.Impulse);
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
}