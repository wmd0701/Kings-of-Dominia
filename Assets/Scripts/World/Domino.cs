using UnityEngine;

public class Domino : MonoBehaviour {
    //------------------------------------------------------
    //Bool ob der Stein König und damit anstoßbar ist
    //------------------------------------------------------
    [SerializeField]
    private bool m_isKing;

    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Falls ein Domino einen Domino berührt
        //------------------------------------------------------
        if (pi_Collision.transform.tag.Contains("Domino"))
        {
            //------------------------------------------------------
            //Spiele passenden Soundeffekt
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayDominoFall();
        }
    }

    /// <summary>
    /// Stoße Stein ggf. an
    /// </summary>
    /// <param name="pi_Force">Kraft (Richtung)</param>
    /// <param name="pi_Point">Anwendepunkt</param>
    public void TryFlip(Vector3 pi_Force, Vector3 pi_Point)
    {
        //------------------------------------------------------
        //Wende Kraft an falls der Stein anstoßbar ist
        //------------------------------------------------------
        if (m_isKing)        
            transform.GetComponent<Rigidbody>().AddForceAtPosition(pi_Force, pi_Point, ForceMode.Impulse);                
    }
}