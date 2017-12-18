using UnityEngine;

class DeathZone: MonoBehaviour
{
    private void OnTriggerExit(Collider pi_Collision)    
    {
        //------------------------------------------------------
        //Zerstört alles was die Todeszone verlässt
        //------------------------------------------------------
        if(pi_Collision.tag != "Canon")
            Destroy(pi_Collision.gameObject);        
    }
}