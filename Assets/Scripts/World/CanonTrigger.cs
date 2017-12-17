using UnityEngine;

public class CanonTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Wenn ein Domino den Trigger berührt hat
        //------------------------------------------------------
        if (pi_Collision.transform.tag == "Dominos")
        {
            //------------------------------------------------------
            //Feuere die Kanone ab
            //------------------------------------------------------
            gameObject.GetComponentInParent<Canon>().Shoot();
        }
    }
}