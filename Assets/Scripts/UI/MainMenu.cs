using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Startet Spiel beim ersten Level
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Beendet Spiel
    /// </summary>
    public void CloseGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }

    /// <summary>
    /// Läd bestimmtes Level
    /// </summary>
    /// <param name="pi_LevelNum">Level-ID</param>
    public void LoadLevel(int pi_LevelNum)
    {
        Debug.Log("ToDo: Implement level loader");
    }
}