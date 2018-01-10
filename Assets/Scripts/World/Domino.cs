using UnityEngine;

public class Domino : MonoBehaviour {
    //------------------------------------------------------
    //Bool ob der Stein anstoßbar ist
    //------------------------------------------------------
    private bool m_Flippable = false;

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
        //------------------------------------------------------
        //Falls das Startfeld berührt wird
        //------------------------------------------------------
        if (pi_Collision.transform.tag == "Start")
        {
            //------------------------------------------------------
            //Stein kann angestoßen werden
            //------------------------------------------------------
            m_Flippable = true;
        }
    }

    private void OnCollisionExit(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Wenn das Startfeld nicht länger berührt wird
        //------------------------------------------------------
        if (pi_Collision.transform.tag == "Start")
        {
            //------------------------------------------------------
            //Stein kann nicht mehr angestoßen werden
            //------------------------------------------------------
            m_Flippable = false;
        }
    }

    /// <summary>
    /// Stoße Stein ggf. an
    /// </summary>
    /// <param name="pi_Force">Kraft (Richtung)</param>
    /// <param name="pi_Point">Anwendepunkt</param>
    public void Flip(Vector3 pi_Force, Vector3 pi_Point)
    {
        //------------------------------------------------------
        //Wende Kraft an falls der Stein anstoßbar ist
        //------------------------------------------------------
        if (m_Flippable)
            transform.GetComponent<Rigidbody>().AddForceAtPosition(pi_Force, pi_Point, ForceMode.Impulse);
    }
}