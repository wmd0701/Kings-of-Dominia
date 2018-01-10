using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    //------------------------------------------------------
    //Singleton
    //------------------------------------------------------
    public static UIManager Instance = null;
    //------------------------------------------------------
    //Level abgeschlossen Bild
    //------------------------------------------------------
    [SerializeField]
    private Image m_WinScreen;        

    /// <summary>
    /// Wenn das Script aktiviert wird
    /// </summary>
    private void Awake()
    {
        //------------------------------------------------------
        //Erstelle eine Instanz falls nicht vorhanden
        //------------------------------------------------------
        if (Instance == null)
        {
            Instance = this;
        }
        //------------------------------------------------------
        //..Oder lösche falls diese Instanz nicht die Instanz ist
        //------------------------------------------------------
        else if (Instance != this)
            Destroy(this);
    }

    /// <summary>
    /// Zeigt an dass das Level abgeschlossen wurde
    /// </summary>
    public void ShowWin()
    {
        m_WinScreen.enabled = true;
    }

}