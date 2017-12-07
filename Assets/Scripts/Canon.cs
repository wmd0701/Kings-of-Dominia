using UnityEngine;

class Canon : MonoBehaviour
{
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
    //"Stärke" des Schusses
    //------------------------------------------------------
    public int m_Power = 5;
    //------------------------------------------------------
    //Anzahl Schüsse
    //------------------------------------------------------
    public int m_RemainingShots = 1;

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
            //Reduziere Anzahl der Schüsse
            //------------------------------------------------------
            m_RemainingShots--;
        }
    }    
}