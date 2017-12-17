using UnityEngine;

class DeathZone: MonoBehaviour
{
    private void OnTriggerExit(Collider pi_Collision)    
    {
        //------------------------------------------------------
        //Zerstört alles was die Todeszone verlässt
        //------------------------------------------------------
        Destroy(pi_Collision.gameObject);        
    }
}