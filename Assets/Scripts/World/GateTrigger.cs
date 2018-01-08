using UnityEngine;

public class GateTrigger : MonoBehaviour {
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
            gameObject.GetComponentInParent<Gate>().OpenGate();
        }
    }
}