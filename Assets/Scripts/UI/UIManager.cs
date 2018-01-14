using UnityEngine;

public class UIManager : MonoBehaviour
{
    //------------------------------------------------------
    //Singleton
    //------------------------------------------------------
    public static UIManager Instance = null;
    //------------------------------------------------------
    //Abschluss UI
    //------------------------------------------------------
    [SerializeField]
    private GameObject m_WinUI;
    //------------------------------------------------------
    //Upgrade UI
    //------------------------------------------------------
    [SerializeField]
    private GameObject m_UpgradeUI;
    //------------------------------------------------------
    //Spawner
    //------------------------------------------------------
    [SerializeField]
    private DominoSpawner m_Spawner;
    //-----------------------------------------------------------------
    //Bool ob Overlay aktiv
    //-----------------------------------------------------------------
    private bool m_UIActive;
    
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
    /// Bool ob UI aktuell aktiv
    /// </summary>
    public bool UIActive
    {
        get
        {
            return m_UIActive;
        }
        set
        {
            m_UIActive = value;
        }
    }
    
    /// <summary>
    /// Zeigt an dass das Level abgeschlossen wurde
    /// </summary>
    /// <param name="pi_Show">Bool ob anzeigen/verstecken</param>
    public void ShowWin(bool pi_Show)
    {
        m_UIActive = pi_Show;
        m_WinUI.SetActive(pi_Show);        
    }

    /// <summary>
    /// Togglet Upgrade-UI
    /// </summary>
    /// <param name="pi_Show">Bool ob anzeigen/verstecken</param>
    public void ShowUpgrade(bool pi_Show)
    {
        m_UIActive = pi_Show;
        m_UpgradeUI.SetActive(pi_Show);
        //------------------------------------------------------
        //Spiele beim Öffnen passenden Soundeffekt
        //------------------------------------------------------
        if (pi_Show)
            SoundEffectManager.Instance.PlayDominoSettings();
    }

    /// <summary>
    /// Upgradet Domino zu Eis
    /// </summary>
    public void UpgradeIce()
    {
        ShowUpgrade(false);
        m_Spawner.UpgradeDomino(Domino.DominoType.Ice);
    }

    /// <summary>
    /// Upgradet Domino zu Lang
    /// </summary>
    public void UpgradeLong()
    {
        ShowUpgrade(false);
        m_Spawner.UpgradeDomino(Domino.DominoType.Long);
    }
}