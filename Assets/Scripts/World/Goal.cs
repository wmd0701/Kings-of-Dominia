using UnityEngine;

public class Goal : MonoBehaviour
{
    //------------------------------------------------------
    //Bool ob das Spiel vorbei ist (geht nur einmal..)
    //------------------------------------------------------
    private bool m_GameOver;

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
			//Activate the vibration
			//------------------------------------------------------
			Vibration.Vibrate(2000);
			//------------------------------------------------------
            //Das Spiel ist vorbei
            //------------------------------------------------------
            m_GameOver = true;
        }
    }
}