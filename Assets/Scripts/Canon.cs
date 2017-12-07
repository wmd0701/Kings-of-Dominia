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
    public int m_Power = 3;

    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Wenn ein Dominostein mit der Kanone kollidiert
        //------------------------------------------------------
        if (pi_Collision.transform.tag == "Dominos")
        {
            //------------------------------------------------------
            //Erstelle ein Projektil
            //------------------------------------------------------
            GameObject l_Ball = Instantiate(m_Canonball, m_CanonSpawn.position, m_CanonSpawn.rotation);
            //------------------------------------------------------
            //Feuere es ab
            //------------------------------------------------------
            l_Ball.transform.GetComponent<Rigidbody>().AddForce(transform.forward * m_Power, ForceMode.Impulse);
        }
    }    
}