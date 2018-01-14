using UnityEngine;

public class Goal : MonoBehaviour
{
    private bool m_GameOver;

    private void Start()
    {
        //------------------------------------------------------
        //Auch Gegner Könige müssen sich registrieren
        //------------------------------------------------------
        FreezeManager.Instance.RegisterRB(GetComponent<Rigidbody>());
    }

    private void Update()
    {
        //------------------------------------------------------
        //Falls der Stein zu weit gekippt wurde
        //------------------------------------------------------
        if (Mathf.Abs(transform.rotation.eulerAngles.x) < 45.0f && !m_GameOver)
        {
            //------------------------------------------------------
            //Dann wird er fallen und das Level ist abgeschlossen
            //------------------------------------------------------
            UIManager.Instance.ShowWin(true);
            //------------------------------------------------------
            //Spiele passenden Soundeffekt
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayWinSound();
            //------------------------------------------------------
            //Das Spiel ist vorbei
            //------------------------------------------------------
            m_GameOver = true;
        }
    }
}