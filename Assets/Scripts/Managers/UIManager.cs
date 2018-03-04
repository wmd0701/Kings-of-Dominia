using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Declarations

    //------------------------------------------------------
    //Singleton
    //------------------------------------------------------
    public static UIManager Instance = null;
    [Header("UI Elements")]
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
    //Anzahl der Upgrades UI
    //------------------------------------------------------
    [SerializeField]
    private Text m_AvailableUpgradesText; 
    //------------------------------------------------------
    //Spawner
    //------------------------------------------------------
    [Header("Other Settings")]
    [SerializeField]
    private DominoSpawner m_Spawner;
    //------------------------------------------------------
    //Anzahl der Upgrades
    //------------------------------------------------------
    [SerializeField]
    private int m_AvailableUpgrades;
    //------------------------------------------------------
    //Bool ob Overlay aktiv
    //------------------------------------------------------
    private bool m_OverlayEnabled;
    //------------------------------------------------------
    //Bool ob Editmode aktiv
    //------------------------------------------------------
    private bool m_EditEnabled;
    //------------------------------------------------------
    //Bool ob Editieren möglich
    //------------------------------------------------------
    private bool m_EditPossible = true;

    #endregion

    #region Properties

    /// <summary>
    /// Anzahl der verfügbaren Upgrades
    /// </summary>
    public int AvailableUpgrades
    {
        get
        {
            return m_AvailableUpgrades;
        }
        set
        {
            m_AvailableUpgrades = value;
            //------------------------------------------------------
            //Update UI entsprechend
            //------------------------------------------------------
            m_AvailableUpgradesText.text = "Upgrades: " + value;
        }
    }

    /// <summary>
    /// Bool ob aktuell ein Overlay aktiv ist
    /// </summary>
    public bool OverlayEnabled
    {
        get
        {
            return m_OverlayEnabled;
        }
        set
        {
            m_OverlayEnabled = value;
        }
    }

    /// <summary>
    /// Bool ob aktuell der Editmode aktiv ist
    /// </summary>
    public bool EditEnabled
    {
        get
        {
            return m_EditEnabled;
        }
        set
        {
            //------------------------------------------------------
            //Wechsle nur falls kein Overlay aktiv ist
            //------------------------------------------------------
            if (!Instance.OverlayEnabled && EditPossible)
            {
                m_EditEnabled = !m_EditEnabled;
                Time.timeScale = m_EditEnabled ? 0.0f : 1.0f;
            }
        }
    }
    
    /// <summary>
    /// Bool ob Editieren möglich
    /// </summary>
    public bool EditPossible
    {
        get
        {
            return m_EditPossible;
        }
        set
        {
            //------------------------------------------------------
            //Passe zunächst Wert an
            //------------------------------------------------------
            m_EditPossible = value;
            //------------------------------------------------------
            //Falls der Editieren jetzt verboten..
            //------------------------------------------------------
            if (!EditPossible)
            {
                //------------------------------------------------------
                //Passe ensprechend an
                //------------------------------------------------------
                Time.timeScale = 1.0f;
                m_EditEnabled = false;
            }
        }
    }

    #endregion

    #region Catch Events

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

    private void Start()
    {
        //------------------------------------------------------
        //Bei Start schreibe Anzahl der Upgrades
        //------------------------------------------------------
        m_AvailableUpgradesText.text = "Upgrades: " + m_AvailableUpgrades;
    }

    #endregion

    #region Procedures

    /// <summary>
    /// Zeigt an dass das Level abgeschlossen wurde
    /// </summary>
    /// <param name="pi_Show">Bool ob anzeigen/verstecken</param>
    public void ShowWin(bool pi_Show)
    {
        m_OverlayEnabled = pi_Show;
        m_WinUI.SetActive(pi_Show);        
    }

    /// <summary>
    /// Togglet Upgrade-UI
    /// </summary>
    /// <param name="pi_Show">Bool ob anzeigen/verstecken</param>
    public void ShowUpgrade(bool pi_Show)
    {
        m_OverlayEnabled = pi_Show;
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

    /// <summary>
    /// Upgradet Domino zu Pyro
    /// </summary>
    public void UpgradePyro()
    {
        ShowUpgrade(false);
        m_Spawner.UpgradeDomino(Domino.DominoType.Pyro);
    }

    /// <summary>
    /// Läd aktuelles Level neu
    /// </summary>
    public void Reload()
    {
        //------------------------------------------------------
        //Hole aktuelle Scene
        //------------------------------------------------------
        Scene l_Current = SceneManager.GetActiveScene();
        //------------------------------------------------------
        //Lade sie neu
        //------------------------------------------------------
        SceneManager.LoadScene(l_Current.buildIndex);
    }

    /// <summary>
    /// Zurück zum Hauptmenü
    /// </summary>
    public void MainMenu()
    {
        //------------------------------------------------------
        //Lade das Hauptmenü (Szene 0)
        //------------------------------------------------------
        SceneManager.LoadScene(0);
    }

    #endregion
}