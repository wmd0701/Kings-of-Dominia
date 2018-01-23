using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //----------------------------------------
    //Referenz auf Levelansicht und Menü
    //----------------------------------------
    [SerializeField]
    private GameObject m_LevelLoader;
    [SerializeField]
    private GameObject m_MainMenu;

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
    /// Switcht zwischen Menü und Levelansicht
    /// </summary>
    public bool LevelLoaderEnabled
    {
        set
        {
            m_MainMenu.SetActive(!value);
            m_LevelLoader.SetActive(value);
        }
    }

    /// <summary>
    /// Läd bestimmtes Level
    /// </summary>
    /// <param name="pi_LevelNum">Level-ID</param>
    public void LoadLevel(int pi_LevelNum)
    {
        //----------------------------------------
        //Falls es die Szene gibt lade sie
        //----------------------------------------
        if (SceneManager.sceneCountInBuildSettings > pi_LevelNum)
        {
            SceneManager.LoadScene(pi_LevelNum);
        }
        else
        {
            Debug.LogError("Scene " + pi_LevelNum + " does not exist");
        }
    }
}