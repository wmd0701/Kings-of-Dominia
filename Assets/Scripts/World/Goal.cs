using UnityEngine;

public class Goal : MonoBehaviour {
    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Falls ein Domino den Gegner König berührt
        //------------------------------------------------------
        if (pi_Collision.transform.tag.Contains("Domino"))
        {
            //------------------------------------------------------
            //Dann wird er fallen und das Level ist abgeschlossen
            //------------------------------------------------------
            UIManager.Instance.ShowWin();
            //------------------------------------------------------
            //Spiele passenden Soundeffekt
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayWinSound();
        }
    }
}