using UnityEngine;

class DeathZone: MonoBehaviour
{

    private void OnTriggerExit(Collider pi_Collision)    
    {     
        //------------------------------------------------------
        //Zerstört alles was mit der Todeszone kollidiert
        //------------------------------------------------------
        Destroy(pi_Collision.gameObject);
    }
}