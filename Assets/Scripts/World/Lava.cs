using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Falls ein Domino Lava berührt
        //------------------------------------------------------
        if (pi_Collision.transform.tag.Contains("Domino"))
        {
            //------------------------------------------------------
            //Zerstöre den Domino
            //------------------------------------------------------
            Destroy(pi_Collision.transform.gameObject);
            //------------------------------------------------------
            //Spiele passenden Sound
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayLavaDestroy();
        }
    }
}