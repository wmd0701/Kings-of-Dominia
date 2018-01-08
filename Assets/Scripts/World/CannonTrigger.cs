using UnityEngine;

public class CannonTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Wenn ein Domino den Trigger berührt hat
        //------------------------------------------------------
        if (pi_Collision.transform.tag.Contains("Domino"))
        {
            //------------------------------------------------------
            //Feuere die Kanone ab
            //------------------------------------------------------
            gameObject.GetComponentInParent<Cannon>().Shoot();
        }
    }
}