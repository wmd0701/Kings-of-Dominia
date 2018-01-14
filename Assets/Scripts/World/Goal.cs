using UnityEngine;

public class Goal : MonoBehaviour
{
    private void Start()
    {
        //------------------------------------------------------
        //Auch Gegner Könige müssen sich registrieren
        //------------------------------------------------------
        FreezeManager.Instance.RegisterRB(GetComponent<Rigidbody>());
    }

    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Falls ein Domino den Gegner König berührt
        //------------------------------------------------------
        if (pi_Collision.transform.tag.Contains("Domino") ||
            pi_Collision.transform.tag == "Canon")
        {
            //------------------------------------------------------
            //Dann wird er fallen und das Level ist abgeschlossen
            //------------------------------------------------------
            UIManager.Instance.ShowWin(true);
            //------------------------------------------------------
            //Spiele passenden Soundeffekt
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayWinSound();
        }
    }
}