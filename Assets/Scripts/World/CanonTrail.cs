using UnityEngine;

public class CanonTrail : MonoBehaviour {    
    
    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Friere bei Kollision ein
        //------------------------------------------------------
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}