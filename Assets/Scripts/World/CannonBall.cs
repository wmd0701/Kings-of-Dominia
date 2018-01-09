using UnityEngine;

public class CannonBall : MonoBehaviour {
    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Falls ein Schuss einen Domino trifft
        //------------------------------------------------------
        if (pi_Collision.transform.tag.Contains("Domino"))
        {
            //------------------------------------------------------
            //Spiele passenden Sound
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayCanonDominoContact();
        }
    }
}