using System.Collections.Generic;
using UnityEngine;

public class DominoSpawner : MonoBehaviour, UndoChange
{
    #region Classes
    
    /// <summary>
    /// Speicher letzten Dominospawn
    /// </summary>
    private class DominoChange : Change
    {
        /// <summary>
        /// Enum für Undo-Modi
        /// </summary>
        public enum enRestoreMode
        {
            Destroy,
            UpgradeIce,
            UpgradeLong
        }

        /// <summary>
        /// Get/Set Änderungen
        /// </summary>
        public GameObject[] m_ChangedDominos { get; private set; }

        /// <summary>
        /// Wiederherstellmodus
        /// </summary>
        public enRestoreMode m_Mode { get; private set; }

        /// <summary>
        /// Initialisiert neue Änderung
        /// </summary>
        /// <param name="pi_LastSpawned">Array mit den Steinen</param>
        public DominoChange(GameObject[] pi_ChangedDominos, enRestoreMode pi_Mode) {
            m_ChangedDominos = pi_ChangedDominos;
            m_Mode = pi_Mode;
        }
    }

    #endregion

    #region Declarations

    [Header("Objects")]
    //------------------------------------------------------
    //Domino Prefabs (Suche mit Tag)
    //------------------------------------------------------
    [SerializeField]
    private GameObject[] m_DominoPrefabs;
    //------------------------------------------------------
    //LineRenderer für Spawnlinie
    //------------------------------------------------------    
    private LineRenderer m_Spawner;
    [Header("Settings")]
    //------------------------------------------------------
    //Distanz zwischen Touches und Dominos
    //------------------------------------------------------
    [SerializeField]
    private float m_DominoDistance = 1.0f;
    //------------------------------------------------------
    //Distanz +/- ab Null wo gespawnt werden darf/kann
    //------------------------------------------------------
    [SerializeField]
    private float m_EpsilonHeightDiff = 0.01f;
    //------------------------------------------------------
    //Dauer des Timers bis Upgrade UI aktiviert wird
    //------------------------------------------------------
    [SerializeField]
    private float m_HoldUntilUpgrade = 0.5f;
    //------------------------------------------------------
    //Um Änderungen rückgängig zu machen
    //------------------------------------------------------
    Stack<DominoChange> m_Changes = new Stack<DominoChange>();
    //------------------------------------------------------
    //Abbruchkriterium für aktuellen Touch
    //------------------------------------------------------
    private bool m_CancelTouch;
    //------------------------------------------------------
    //Bool ob zeichnen aktuell erlaubt ist
    //------------------------------------------------------
    private bool m_DrawEnabled;
    //------------------------------------------------------
    //Letztes ausgewählter Domino
    //------------------------------------------------------
    private GameObject m_LastSelected;
    //------------------------------------------------------
    //Aktueller Timer
    //------------------------------------------------------
    private float m_UpgradeTimer;

    #endregion

    #region Catch Events

    private void Start()
    {
        //------------------------------------------------------
        //Hole Renderer
        //------------------------------------------------------
        m_Spawner = gameObject.GetComponent<LineRenderer>();
        //------------------------------------------------------
        //Initialisiere Timer
        //------------------------------------------------------
        m_UpgradeTimer = m_HoldUntilUpgrade;
    }

    private void Update()
    {
        //------------------------------------------------------
        //Falls es nur einen Touch gab
        //------------------------------------------------------
        if (Input.touchCount == 1)
        {
            //------------------------------------------------------
            //Und dieser nicht gecancelt wurde
            //------------------------------------------------------
            if (Input.GetTouch(0).phase != TouchPhase.Canceled)
            {
                RaycastHit l_Hit;
                //------------------------------------------------------
                //Bei Beginn setzten Abbruchkriterium zurück
                //------------------------------------------------------
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    m_CancelTouch = false;
                //------------------------------------------------------
                //Erstelle Ray durch die Kamera basierend auf dem Touch
                //------------------------------------------------------
                Ray l_Ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                //------------------------------------------------------
                //Falls nicht im Editmode und Domino getroffen wurde..
                //------------------------------------------------------
                if(!FreezeManager.Instance.Frozen && !UIManager.Instance.UIActive && !m_CancelTouch &&
                    Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Dominos")))
                {
                    //------------------------------------------------------
                    //..Versuche Stein anzustoßen
                    //------------------------------------------------------
                    l_Hit.transform.GetComponent<Domino>().TryFlip(l_Ray.direction, l_Hit.point);
                }
                //------------------------------------------------------
                //Falls ein Domino getroffen wurde (stationär)
                //------------------------------------------------------
                if (FreezeManager.Instance.Frozen && !UIManager.Instance.UIActive && !m_CancelTouch &&
                    Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Dominos")) && 
                    Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    //------------------------------------------------------
                    //Falls es ein normaler Domino ist
                    //------------------------------------------------------
                    if (l_Hit.transform.CompareTag("DominoStandart"))
                    {
                        //------------------------------------------------------
                        //Falls der Timer noch läuft
                        //------------------------------------------------------
                        if (m_UpgradeTimer > 0)
                        {
                            //------------------------------------------------------
                            //Ziehe vergangene Zeit ab
                            //------------------------------------------------------
                            m_UpgradeTimer -= Time.deltaTime;
                        }
                        else
                        {
                            //------------------------------------------------------
                            //Setzte Timer zurück
                            //------------------------------------------------------
                            m_UpgradeTimer = m_HoldUntilUpgrade;
                            //------------------------------------------------------
                            //Aktiviere Upgrade Menü
                            //------------------------------------------------------
                            UIManager.Instance.ShowUpgrade(true);
                            //------------------------------------------------------
                            //Speichere GameObject
                            //------------------------------------------------------
                            m_LastSelected = l_Hit.transform.gameObject;
                            //------------------------------------------------------
                            //Breche Touch ab
                            //------------------------------------------------------
                            m_CancelTouch = true;
                        }                    
                    }
                    else
                    {
                        //------------------------------------------------------
                        //Ansonsten setzte Timer zurück
                        //------------------------------------------------------
                        m_UpgradeTimer = m_HoldUntilUpgrade;
                    }
                }
                //--------------------------------------------------------------
                //Falls ein Domino getroffen wurde (und gezeichnet wird)
                //--------------------------------------------------------------
                if (FreezeManager.Instance.Frozen && !UIManager.Instance.UIActive && !m_CancelTouch &&
                    Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Dominos")))
                {
                    //------------------------------------------------------
                    //Ändere nur falls es ein normaler Domino ist
                    //------------------------------------------------------
                    if (l_Hit.transform.CompareTag("DominoStandart") || l_Hit.transform.CompareTag("Start"))
                    {
                        //------------------------------------------------------
                        //Füge Position in die Liste ein
                        //------------------------------------------------------
                        m_Spawner.positionCount++;
                        m_Spawner.SetPosition(m_Spawner.positionCount - 1, l_Hit.transform.position);
                        //------------------------------------------------------
                        //Erlaube zeichnen
                        //------------------------------------------------------
                        m_DrawEnabled = true;
                    }
                    //--------------------------------------------------------------
                    //Ansonsten falls der Endstein getroffen wurde spawne
                    //--------------------------------------------------------------
                    else if (l_Hit.transform.CompareTag("Goal"))
                    {
                        SpawnDominos();
                        //------------------------------------------------------
                        //Breche Touch ab
                        //------------------------------------------------------
                        m_CancelTouch = true;
                    }
                }
                //------------------------------------------------------
                //Falls im Editmode und ein Trigger getroffen..
                //------------------------------------------------------
                if (FreezeManager.Instance.Frozen && !UIManager.Instance.UIActive && !m_CancelTouch &&
                    Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Trigger")) && m_DrawEnabled)
                {
                    //------------------------------------------------------
                    //Hole Ecken des Triggers (angeordnet in CCW)
                    //------------------------------------------------------
                    Transform[] l_TriggerCorners = l_Hit.transform.GetComponentInChildren<Trigger>().Corners;
                    //------------------------------------------------------
                    //Gehe durch die Ecken
                    //------------------------------------------------------
                    for (int i = 0; i < l_TriggerCorners.Length; i++)
                    {
                        //-------------------------------------------------------------
                        //Bestimme Linie 1 (Touch außerhalb und innerhalb des Triggers)
                        //-------------------------------------------------------------
                        Vector3 l_Line1_2 = l_Hit.point;
                        Vector3 l_Line1_1 = m_Spawner.GetPosition(m_Spawner.positionCount - 1);
                        //------------------------------------------------------
                        //Bestimme Linie 2 (aktuelle Kante)
                        //------------------------------------------------------
                        Vector3 l_Line2_1 = l_TriggerCorners[i].position;
                        Vector3 l_Line2_2 = l_TriggerCorners[i < l_TriggerCorners.Length - 1 ? i + 1 : 0].position;
                        //------------------------------------------------------
                        //Überprüfe ob diese Kante geschnitten wurde
                        //------------------------------------------------------
                        if (Intersect(l_Line1_1, l_Line1_2, l_Line2_1, l_Line2_2))
                        {
                            //------------------------------------------------------
                            //Falls dem so ist bestimme Mittelpunkt
                            //------------------------------------------------------
                            Vector3 l_TriggerStone = (l_Line2_1 + l_Line2_2) / 2.0f;
                            //------------------------------------------------------
                            //Füge als Spawnpunkt hinzu
                            //------------------------------------------------------
                            m_Spawner.positionCount++;
                            m_Spawner.SetPosition(m_Spawner.positionCount - 1, l_TriggerStone);
                            //------------------------------------------------------
                            //Spawne Dominos
                            //------------------------------------------------------
                            SpawnDominos();
                            //------------------------------------------------------
                            //Breche Touch ab
                            //------------------------------------------------------
                            m_CancelTouch = true;
                            //------------------------------------------------------
                            //Verlasse Schleife
                            //------------------------------------------------------
                            break;
                        }
                    }
                }
                //------------------------------------------------------
                //Falls im Editmode und das Level getroffen wurde..
                //------------------------------------------------------
                if (FreezeManager.Instance.Frozen && !UIManager.Instance.UIActive && !m_CancelTouch &&
                    Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Terrain")) && m_DrawEnabled)
                {
                    //--------------------------------------------------------------
                    //..Speichere Hit im Spawnarray falls ungefähr auf gleicher Höhe
                    //--------------------------------------------------------------
                    if (Mathf.Abs(l_Hit.point.y - m_Spawner.GetPosition(m_Spawner.positionCount - 1).y) < m_EpsilonHeightDiff)
                    {
                        m_Spawner.positionCount++;
                        m_Spawner.SetPosition(m_Spawner.positionCount - 1, l_Hit.point);
                    }
                    //------------------------------------------------------
                    //..Oder spawne Dominos
                    //------------------------------------------------------
                    else
                    {
                        SpawnDominos();
                        //------------------------------------------------------
                        //Breche Touch ab
                        //------------------------------------------------------
                        m_CancelTouch = true;
                    }
                }
            }
            //------------------------------------------------------
            //Wenn der Touch beendet wurde
            //------------------------------------------------------
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                //------------------------------------------------------
                //Spawne Dominos
                //------------------------------------------------------
                SpawnDominos();
            }
        }
        //------------------------------------------------------
        //Bei mehr Touches..
        //------------------------------------------------------
        else
        {
            //------------------------------------------------------
            //..Breche Touch ab und lösche ggf. Eingabe
            //------------------------------------------------------
            m_Spawner.positionCount = 0;
            m_CancelTouch = true;
        }
    }

    #endregion

    #region Procedures

    /// <summary>
    /// Upgradet zuletzt ausgewählten Stein
    /// </summary>
    /// <param name="pi_Type">Upgradetyp</param>
    public void UpgradeDomino(Domino.DominoType pi_Type)
    {
        //------------------------------------------------------
        //Falls ein Upgrade möglich ist
        //------------------------------------------------------
        if (m_LastSelected != null)
        {
            GameObject l_Replace;
            //------------------------------------------------------
            //Je nach Upgrade-Typ
            //------------------------------------------------------
            switch (pi_Type)
            {
                case Domino.DominoType.Ice:
                    //------------------------------------------------------
                    //Finde Eis Domino und instanziere ihn
                    //------------------------------------------------------
                    l_Replace = System.Array.Find(m_DominoPrefabs, b_Search => b_Search.CompareTag("DominoIce"));
                    l_Replace = Instantiate(l_Replace, m_LastSelected.transform.position, m_LastSelected.transform.rotation);
                    //------------------------------------------------------
                    //Speichere RB im Manager
                    //------------------------------------------------------
                    FreezeManager.Instance.RegisterRB(l_Replace.GetComponent<Rigidbody>());
                    //------------------------------------------------------
                    //Speichere den Change
                    //------------------------------------------------------
                    SaveLastChange(new DominoChange(new GameObject[] { l_Replace, m_LastSelected }, DominoChange.enRestoreMode.UpgradeIce));
                    //------------------------------------------------------
                    //Deaktivere normalen Domino
                    //------------------------------------------------------
                    m_LastSelected.SetActive(false);
                    break;
                case Domino.DominoType.Long:
                    //------------------------------------------------------
                    //Finde langen Domino und instanziere ihn
                    //------------------------------------------------------
                    l_Replace = System.Array.Find(m_DominoPrefabs, b_Search => b_Search.CompareTag("DominoLong"));
                    l_Replace = Instantiate(l_Replace, m_LastSelected.transform.position, m_LastSelected.transform.rotation);
                    //------------------------------------------------------
                    //Speichere RB im Manager
                    //------------------------------------------------------
                    FreezeManager.Instance.RegisterRB(l_Replace.GetComponent<Rigidbody>());
                    //------------------------------------------------------
                    //Speichere den Change
                    //------------------------------------------------------
                    SaveLastChange(new DominoChange(new GameObject[] { l_Replace, m_LastSelected }, DominoChange.enRestoreMode.UpgradeLong));
                    //------------------------------------------------------
                    //Deaktivere normalen Domino
                    //------------------------------------------------------
                    m_LastSelected.SetActive(false);
                    break;
                //------------------------------------------------------
                //Fehler!
                //------------------------------------------------------
                default:
                    Debug.LogError("Case not implemented (upgrade)");
                    break;
            }
            //------------------------------------------------------
            //Setzte Auswahl zurück
            //------------------------------------------------------
            m_LastSelected = null;
        }
        else
        {
            Debug.LogWarning("Trying to upgrade null");
        }
    }

    /// <summary>
    /// Spawne Dominos basierend auf Koordinaten
    /// </summary>
    private void SpawnDominos()
    {
        //------------------------------------------------------
        //Deaktiviere zeichnen
        //------------------------------------------------------
        m_DrawEnabled = false;
        //------------------------------------------------------
        //Erstelle Array für Positionen
        //------------------------------------------------------
        Vector3[] l_Positions = new Vector3[m_Spawner.positionCount];
        //------------------------------------------------------
        //Hole Positionen
        //------------------------------------------------------
        m_Spawner.GetPositions(l_Positions);
        //------------------------------------------------------
        //Setzte LineRenderer zurück
        //------------------------------------------------------
        m_Spawner.positionCount = 0;
        //------------------------------------------------------
        //Erstelle Listen für plausible Positionen..
        //------------------------------------------------------
        List<Vector3> l_WorkingPositions = new List<Vector3>();
        //------------------------------------------------------
        //..Und deren Rotationen
        //------------------------------------------------------
        List<Quaternion> l_InterpolatedRotations = new List<Quaternion>();
        //------------------------------------------------------
        //Falls es mindestens zwei Positionen gibt..
        //------------------------------------------------------
        if (l_Positions.Length > 1)
        {
            //------------------------------------------------------
            //Gehe durch die Positionen
            //------------------------------------------------------
            for (int i = 0; i < l_Positions.Length - 1; i++)
            {
                //------------------------------------------------------
                //Und durch die Punkte ab dem aktuellen Punkt
                //------------------------------------------------------
                for (int j = i + 1; j < l_Positions.Length - 1; j++)
                {
                    //------------------------------------------------------
                    //Wenn die Distanz groß genug ist
                    //------------------------------------------------------
                    if ((l_Positions[j] - l_Positions[i]).magnitude > m_DominoDistance)
                    {
                        //----------------------------------------------------------
                        //Füge den entfernten Punkt in die Liste ein,
                        //passe Zähler an dann verlasse die innere Schleife
                        //----------------------------------------------------------
                        l_WorkingPositions.Add(l_Positions[i]);
                        i = j;
                        break;
                    }
                }
            }
            //------------------------------------------------------
            //Füge letzten aufgezeichneten Punkt zwangsläufig ein
            //------------------------------------------------------
            l_WorkingPositions.Add(l_Positions[l_Positions.Length - 1]);
            //------------------------------------------------------
            //Liste mit interpolierten Zwischenpunkten
            //------------------------------------------------------
            List<Vector3> l_InterpolatedPositions = new List<Vector3>();
            //------------------------------------------------------
            //Gehe durch die Liste der Punkte die erlaubt sind
            //------------------------------------------------------
            for (int i = 0; i < l_WorkingPositions.Count - 1; i++)
            {
                //------------------------------------------------------
                //Bestimme Distanz
                //------------------------------------------------------
                Vector3 l_Diff = l_WorkingPositions[i + 1] - l_WorkingPositions[i];
                //---------------------------------------------------------------------------------------
                //Erstelle Zwischenpunkte basierend auf der gewünschten Spawndistanz zwischen den Steinen
                //---------------------------------------------------------------------------------------
                for (int j = 0; j < (l_Diff.magnitude / m_DominoDistance) - 1; j++)
                {
                    l_InterpolatedPositions.Add(l_WorkingPositions[i] + ((l_Diff / (l_Diff.magnitude / m_DominoDistance)) * j));
                }
            }            
            //---------------------------------------------------------
            //Wenn es mindestens eine Position gibt bestimme Rotationen
            //---------------------------------------------------------
            if (l_InterpolatedPositions.Count > 1)
            {
                //----------------------------------------------------------
                //Bestimme Rotation des ersten Steins (abhängig vom Zweiten)
                //----------------------------------------------------------
                l_InterpolatedRotations.Add(Quaternion.Euler(-90.0f, (Mathf.Atan2((l_InterpolatedPositions[0] - l_InterpolatedPositions[1]).x,
                                                                                  (l_InterpolatedPositions[0] - l_InterpolatedPositions[1]).z) * 180 / Mathf.PI), 0.0f));
                //------------------------------------------------------
                //Falls es mindestens drei Steine gibt
                //------------------------------------------------------
                if (l_InterpolatedPositions.Count > 2)
                {
                    //--------------------------------------------------------
                    //Bestimme Rotation basierend auf Vorgänger und Nachfolger
                    //--------------------------------------------------------
                    for (int i = 1; i < l_InterpolatedPositions.Count - 1; i++)
                    {
                        //------------------------------------------------------
                        //Und füge entsprechend ein
                        //------------------------------------------------------
                        l_InterpolatedRotations.Add(Quaternion.Euler(-90.0f, (Mathf.Atan2((l_InterpolatedPositions[i - 1] - l_InterpolatedPositions[i + 1]).x,
                                                                                          (l_InterpolatedPositions[i - 1] - l_InterpolatedPositions[i + 1]).z) * 180 / Mathf.PI), 0.0f));
                    }
                }
                //----------------------------------------------------------------
                //Bestimme Rotation des letzten Steins basierend auf dem Vorgänger
                //----------------------------------------------------------------
                l_InterpolatedRotations.Add(Quaternion.Euler(-90.0f, (Mathf.Atan2((l_InterpolatedPositions[l_InterpolatedPositions.Count - 1] - l_InterpolatedPositions[l_InterpolatedPositions.Count - 2]).x,
                                                                                  (l_InterpolatedPositions[l_InterpolatedPositions.Count - 1] - l_InterpolatedPositions[l_InterpolatedPositions.Count - 2]).z) * 180 / Mathf.PI), 0.0f));
                //-----------------------------------------------------------
                //Erstelle Array für zu spawnende Steine
                //-----------------------------------------------------------
                GameObject[] l_Spawned = new GameObject[l_InterpolatedPositions.Count];
                //-----------------------------------------------------------
                //Finde Standart Domino
                //-----------------------------------------------------------
                GameObject l_Standart = System.Array.Find(m_DominoPrefabs, b_Search => b_Search.gameObject.CompareTag("DominoStandart"));
                //----------------------------------------------------------------------
                //Spawne Steine mit entsprechenden Koordinaten und Rotationen
                //Der erste Stein wird übersprungen, da dort bereits einer stehen sollte
                //----------------------------------------------------------------------
                for (int i = 1; i < l_InterpolatedPositions.Count; i++)
                {
                    //-----------------------------------------------------------------
                    //Füge RB zum Manager hinzu und GameObject ins Array
                    //-----------------------------------------------------------------
                    l_Spawned[i] = Instantiate(l_Standart, l_InterpolatedPositions[i], l_InterpolatedRotations[i]);
                    FreezeManager.Instance.RegisterRB(l_Spawned[i].GetComponent<Rigidbody>());                    
                }
                //-----------------------------------------------------------
                //Speichere Änderung
                //-----------------------------------------------------------
                SaveLastChange(new DominoChange(l_Spawned, DominoChange.enRestoreMode.Destroy));
            }
        }
    }

    /// <summary>
    /// Speichert letzte Änderung
    /// </summary>
    /// <param name="pi_Change">Änderung</param>
    public void SaveLastChange(Change pi_Change)
    {
        //-----------------------------------------------------------
        //Speichere nur falls noch nicht gespeichert
        //-----------------------------------------------------------
        if (!m_Changes.Contains((DominoChange)pi_Change))
        {
            m_Changes.Push((DominoChange) pi_Change);
        }
        else
        {
            Debug.LogWarning("Trying to save saved state");
        }
    }

    /// <summary>
    /// Lädt Stand vor der letzten Änderung
    /// </summary>
    public void RestoreLastSave()
    {
        //-----------------------------------------------------------
        //Geht nur falls es eine Änderung gab bzw. im Edit-Mode
        //-----------------------------------------------------------
        if (m_Changes.Count == 0 || !FreezeManager.Instance.Frozen)
            return;
        else
        {
            //-----------------------------------------------------------
            //Bestimme letzten Change
            //-----------------------------------------------------------
            DominoChange l_LastChange = m_Changes.Pop();
            //-----------------------------------------------------------
            //Gehe durch die geänderten Dominos
            //-----------------------------------------------------------
            for(int i = 0; i < l_LastChange.m_ChangedDominos.Length; i++)
            {
                //-----------------------------------------------------------
                //Überspringe falls Domino nicht mehr existiert
                //-----------------------------------------------------------
                if (l_LastChange.m_ChangedDominos[i] == null)
                    continue;
                //-----------------------------------------------------------
                //Je nach Modus
                //-----------------------------------------------------------
                switch (l_LastChange.m_Mode)
                {
                    //------------------------------------------------------
                    //Entferne Reihe
                    //------------------------------------------------------
                    case DominoChange.enRestoreMode.Destroy:
                        //-----------------------------------------------------------
                        //Entferne altes Objekt
                        //-----------------------------------------------------------
                        FreezeManager.Instance.RemoveRB(l_LastChange.m_ChangedDominos[i].GetComponent<Rigidbody>());
                        Destroy(l_LastChange.m_ChangedDominos[i]);
                        break;
                    //------------------------------------------------------
                    //Entferne Upgrade
                    //------------------------------------------------------
                    case DominoChange.enRestoreMode.UpgradeIce:
                    case DominoChange.enRestoreMode.UpgradeLong:
                        //------------------------------------------------------
                        //An Position Null ist der Upgradestein gespeichert..
                        //------------------------------------------------------
                        if (i == 0)
                        {
                            //------------------------------------------------------
                            //Entferne vom FreezeManager
                            //------------------------------------------------------
                            FreezeManager.Instance.RemoveRB(l_LastChange.m_ChangedDominos[i].GetComponent<Rigidbody>());
                            //------------------------------------------------------
                            //Und zerstöre den Stein
                            //------------------------------------------------------
                            Destroy(l_LastChange.m_ChangedDominos[i]);
                        }
                        else
                        {
                            //------------------------------------------------------
                            //An Position Eins ist der alte Stein -> Reaktiviere
                            //------------------------------------------------------
                            l_LastChange.m_ChangedDominos[i].SetActive(true);
                        }
                        //------------------------------------------------------
                        //Fertig
                        //------------------------------------------------------
                        break;
                    //------------------------------------------------------
                    //Fehler!
                    //------------------------------------------------------
                    default:
                        Debug.LogError("Case not implemented (restore)");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Bestimmt ob sich zwei Linien schneiden
    /// </summary>
    /// <param name="pi_Line1_1">Linie 1 Punkt 1</param>
    /// <param name="pi_Line1_2">Linie 1 Punkt 2</param>
    /// <param name="pi_Line2_1">Linie 2 Punkt 1</param>
    /// <param name="pi_Line2_2">Linie 2 Punkt 2</param>
    /// <returns>Bool ob Schnitt</returns>
    private bool Intersect(Vector3 pi_Line1_1, Vector3 pi_Line1_2,
                           Vector3 pi_Line2_1, Vector3 pi_Line2_2)
    {
        //-----------------------------------------------------------
        //Falls die Kombinationen CCW abgelaufen werden können
        //dann schneiden sich die Strecken, ansonsten eben nicht
        //-----------------------------------------------------------
        return ((CounterClockWise(pi_Line1_1, pi_Line1_2, pi_Line2_1) * CounterClockWise(pi_Line1_1, pi_Line1_2, pi_Line2_2)) <= 0 &&
                (CounterClockWise(pi_Line2_1, pi_Line2_2, pi_Line1_1) * CounterClockWise(pi_Line2_1, pi_Line2_2, pi_Line1_2)) <= 0);
    }

    /// <summary>
    /// Bestimmt in welche Richtung (CW, CCW) das Ablaufen der drei Punkte geschieht
    /// </summary>
    /// <param name="pi_Point0">Punkt 1</param>
    /// <param name="pi_Point1">Punkt 2</param>
    /// <param name="pi_Point2">Punkt 3</param>
    /// <returns>1 falls CCW, 0 falls unentschieden, -1 falls CW</returns>
    private int CounterClockWise(Vector3 pi_Point0, Vector3 pi_Point1, Vector3 pi_Point2)
    {
        //-----------------------------------------------------------
        //Bestimme Unterschied zw. Punkt 0/1 und 0/2
        //-----------------------------------------------------------
        float l_MagX1 = pi_Point1.x - pi_Point0.x;
        float l_MagZ1 = pi_Point1.z - pi_Point0.z;
        float l_MagX2 = pi_Point2.x - pi_Point0.x;
        float l_MagZ2 = pi_Point2.z - pi_Point0.z;
        //-----------------------------------------------------------
        //Steigung nach Point 1 größer als nach Point 2 -> CCW
        //-----------------------------------------------------------
        if (l_MagX1 * l_MagZ2 > l_MagZ1 * l_MagX2)
            return 1;
        //-----------------------------------------------------------
        //Steigung nach Point 1 kleiner als nach Point 2 -> CW
        //-----------------------------------------------------------
        else if (l_MagX1 * l_MagZ2 < l_MagZ1 * l_MagX2)
            return -1;
        //-----------------------------------------------------------
        //Noch nicht eindeutig (Strecken aufeinander)..
        //-----------------------------------------------------------
        else if (l_MagX1 * l_MagZ2 == l_MagZ1 * l_MagX2)
        {
            //-----------------------------------------------------------
            //Point 0 liegt zwischen Point 1 und Point 2 -> CW
            //-----------------------------------------------------------
            if (l_MagX1 * l_MagX2 < 0 || l_MagZ1 * l_MagZ2 < 0)
                return -1;
            //-----------------------------------------------------------
            //Point 2 liegt zwischen Point 1 und Point 2 -> Spezialfall
            //-----------------------------------------------------------
            else if ((l_MagX1 * l_MagX1 + l_MagZ1 * l_MagZ1) >= (l_MagX2 * l_MagX2 + l_MagZ2 * l_MagZ2))
                return 0;
            //-----------------------------------------------------------
            //Ansonsten Point 1 zwischen Point 0 und Point 2 -> CCW
            //-----------------------------------------------------------
            else
                return 1;
        }
        //-----------------------------------------------------------
        //Ansonsten CCW
        //-----------------------------------------------------------
        else
            return 1;
    }

    #endregion
}