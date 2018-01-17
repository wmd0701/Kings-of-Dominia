using UnityEngine;

public class Domino : MonoBehaviour
{
    //------------------------------------------------------
    //Radius der Explosion
    //------------------------------------------------------
    [SerializeField]
    private float m_Radius = 5.0F;
    //------------------------------------------------------
    //Stärke der Explosion
    //------------------------------------------------------
    [SerializeField]
    private float m_Power = 100.0F;

    /// <summary>
    /// Verfügbare Dominotypen
    /// </summary>
    public enum DominoType
    {
        Normal,
        Long,
        Pyro,
        Ice
    }    

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
        //Falls ein Pyro eine Mauer berührt
        //------------------------------------------------------
        else if (pi_Collision.transform.CompareTag("Wall") && transform.CompareTag("DominoPyro"))
        {
            //------------------------------------------------------
            //Entferne die Mauer
            //------------------------------------------------------
            Destroy(pi_Collision.transform.gameObject);
            //------------------------------------------------------
            //Löse Explosion aus
            //------------------------------------------------------
            DoExplode(transform.position);
        }
    }

    /// <summary>
    /// Führe Explosion durch
    /// </summary>
    /// <param name="pi_Position">Ort der Explosion</param>
    private void DoExplode(Vector3 pi_Position)
    {
        //------------------------------------------------------
        //Spiele passenden Soundeffekt
        //------------------------------------------------------
        SoundEffectManager.Instance.PlayCannonShot();
        //------------------------------------------------------
        //Bestimme beeinflusste Dominos
        //------------------------------------------------------          
        Collider[] l_AffectedColliders = Physics.OverlapSphere(pi_Position, m_Radius);
        //------------------------------------------------------
        //Für jeden betroffenen Domino..
        //------------------------------------------------------
        foreach (Collider b_Hit in l_AffectedColliders)
        {
            //------------------------------------------------------
            //Hole den RB des Hits
            //------------------------------------------------------
            Rigidbody l_HitRB = b_Hit.GetComponent<Rigidbody>();
            //------------------------------------------------------
            //Falls es einen RB hat wende Kraft an
            //------------------------------------------------------
            if (l_HitRB != null)
                l_HitRB.AddExplosionForce(m_Power, pi_Position, m_Radius, 3.0F, ForceMode.Impulse);
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
        //Falls der Stein ein Startstein ist
        //------------------------------------------------------
        if (transform.CompareTag("Start"))
        {
            //------------------------------------------------------
            //Wende Kraft auf Stein an
            //------------------------------------------------------
            transform.GetComponent<Rigidbody>().AddForceAtPosition(pi_Force, pi_Point, ForceMode.Impulse);
            //------------------------------------------------------
            //Editmodus ist nun gesperrt
            //------------------------------------------------------
            UIManager.Instance.EditPossible = false;
        }        
    }
}