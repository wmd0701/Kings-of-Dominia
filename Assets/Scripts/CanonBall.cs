
using UnityEngine;

class CanonBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Wenn ein Projektil mit einem Domino kollidiert
        //------------------------------------------------------
        if (pi_Collision.transform.tag == "Dominos")
        {
            //------------------------------------------------------
            //Lösche es
            //------------------------------------------------------
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        //------------------------------------------------------
        //Aufräumen
        //------------------------------------------------------
        Destroy(gameObject);
    }
}