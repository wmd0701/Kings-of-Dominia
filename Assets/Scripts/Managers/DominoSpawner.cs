using UnityEngine;
using System.Collections.Generic;

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
            Replace
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
    //Domino Prefab
    //------------------------------------------------------
    [SerializeField]
    private GameObject m_DominoPrefab;

    //------------------------------------------------------
    //Material & Physical Material von Domino und Domino_Ice
    //------------------------------------------------------
    [SerializeField]
    private Material Domino;

    [SerializeField]
    private Material Domino_Ice;

    [SerializeField]
    private PhysicMaterial Domino_Physic;

    [SerializeField]
    private PhysicMaterial Domino_Ice_Physic;

    //------------------------------------------------------
    //LineRenderer für Spawnlinie
    //------------------------------------------------------    
    private LineRenderer m_Spawner;
    [Header("Settings")]
    //------------------------------------------------------
    //Distanz zwischen Touches und Dominos
    //------------------------------------------------------
    [SerializeField]
    private float m_DominoDistance = 0.01f;
    //------------------------------------------------------
    //Distanz +/- ab Null wo gespawnt werden darf/kann
    //------------------------------------------------------
    [SerializeField]
    private float m_EpsilonSpawnHeight = 0.01f;
    //------------------------------------------------------
    //Um Änderungen rückgängig zu machen
    //------------------------------------------------------
    Stack<DominoChange> m_Changes = new Stack<DominoChange>();

    #endregion

    #region Catch Events

    private void Start()
    {
        //------------------------------------------------------
        //Hole Renderer
        //------------------------------------------------------
        m_Spawner = gameObject.GetComponent<LineRenderer>();
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
                //Erstelle Ray durch die Kamera basierend auf dem Touch
                //------------------------------------------------------
                Ray l_Ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                //------------------------------------------------------
                //Falls nicht im Editmode und Domino getroffen wurde..
                //------------------------------------------------------
                if(!FreezeManager.Instance.Frozen && Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Dominos")))
                {
                    //------------------------------------------------------
                    //..Wende entweder Force auf Rigidbody des Hits an
                    //------------------------------------------------------
                    l_Hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(l_Ray.direction, l_Hit.point, ForceMode.Impulse);
                }
                //------------------------------------------------------
                //Falls im Editmode und Domino getroffen wurde..
                //------------------------------------------------------
                if (FreezeManager.Instance.Frozen && Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Dominos")))
                {
                    //------------------------------------------------------
                    //Ändere nur falls es ein normaler Domino ist
                    //------------------------------------------------------
                    if (l_Hit.transform.GetComponent<Collider>().sharedMaterial.Equals(Domino_Physic) &&
                        l_Hit.transform.GetComponent<MeshRenderer>().sharedMaterial.Equals(Domino))                    
                    {
                        //------------------------------------------------------
                        //Passe Material und Physics-Material des Dominos an
                        //------------------------------------------------------

                        //Es ist so in Unity Dokumentation empfohlen, dass man material statt sharedMaterial zu modifizieren,
                        //weil wenn man shardMaterial verändert, werden vielleicht alle Objekte, die das sharedMaterial nutzen,
                        //verändert. Aber ich weiß nicht warum dieses Phänomen hier bei uns nicht vorkommt.
                        l_Hit.transform.GetComponent<MeshRenderer>().material = Domino_Ice;
                        l_Hit.transform.GetComponent<Collider>().material = Domino_Ice_Physic;
                        
                        //------------------------------------------------------
                        //Speichere den Change
                        //------------------------------------------------------
                        SaveLastChange(new DominoChange(new GameObject[] { l_Hit.transform.gameObject }, DominoChange.enRestoreMode.Replace));
                    }
                }
                //------------------------------------------------------
                //Falls im Editmode und das Level getroffen wurde..
                //------------------------------------------------------
                if (FreezeManager.Instance.Frozen && Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
                {
                    //------------------------------------------------------
                    //..Speichere Hit im Spawnarray falls auf "Nullebene"
                    //------------------------------------------------------
                    if(Mathf.Abs(l_Hit.point.y) < m_EpsilonSpawnHeight)
                    {
                        m_Spawner.positionCount++;
                        m_Spawner.SetPosition(m_Spawner.positionCount-1, l_Hit.point);
                    }
                    //------------------------------------------------------
                    //..Oder spawne Dominos
                    //------------------------------------------------------
                    else
                    {
                        //------------------------------------------------------
                        //Spawne Dominos und setzte Spawner zurück
                        //------------------------------------------------------
                        Vector3[] l_Positions = new Vector3[m_Spawner.positionCount];
                        m_Spawner.GetPositions(l_Positions);
                        SpawnDominos(l_Positions);
                        m_Spawner.positionCount = 0;
                    }
                }
            }
            //------------------------------------------------------
            //Wenn der Touch beendet wurde
            //------------------------------------------------------
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                //------------------------------------------------------
                //Spawne Dominos und setzte Spawner zurück
                //------------------------------------------------------
                Vector3[] l_Positions = new Vector3[m_Spawner.positionCount];
                m_Spawner.GetPositions(l_Positions);
                SpawnDominos(l_Positions);
                m_Spawner.positionCount = 0;
            }
        }
    }

    #endregion

    #region Procedures

    /// <summary>
    /// Spawne Dominos basierend auf Koordinaten
    /// </summary>
    /// <param name="pi_Positions">Koordinatenarray</param>
    private void SpawnDominos(Vector3 [] pi_Positions)
    {
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
        if (pi_Positions.Length > 1)
        {
            //------------------------------------------------------
            //Gehe durch die Positionen
            //------------------------------------------------------
            for (int i = 0; i < pi_Positions.Length - 1; i++)
            {
                //------------------------------------------------------
                //Und durch die Punkte ab dem aktuellen Punkt
                //------------------------------------------------------
                for (int j = i + 1; j < pi_Positions.Length - 1; j++)
                {
                    //------------------------------------------------------
                    //Wenn die Distanz stimmt
                    //------------------------------------------------------
                    if ((pi_Positions[j] - pi_Positions[i]).magnitude > m_DominoDistance)
                    {
                        //----------------------------------------------------------
                        //Füge in die Liste ein und ändere Zähler, verlasse Schleife
                        //----------------------------------------------------------
                        l_WorkingPositions.Add(pi_Positions[i]);
                        i = j;
                        break;
                    }
                }
            }
            //------------------------------------------------------
            //Liste mit interpolierten Zwischenpunkten
            //------------------------------------------------------
            List<Vector3> l_InterpolatedPositions = new List<Vector3>();
            //------------------------------------------------------
            //Gehe durch die Liste der echten Punkte
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
                //Spawne Steine mit entsprechenden Koordinaten und Rotationen
                //-----------------------------------------------------------
                for (int i = 0; i < l_InterpolatedPositions.Count; i++)
                {
                    //-----------------------------------------------------------------
                    //Füge RB zum Manager hinzu und GameObject ins Array
                    //-----------------------------------------------------------------
                    l_Spawned[i] = Instantiate(m_DominoPrefab, l_InterpolatedPositions[i], l_InterpolatedRotations[i]);
                    FreezeManager.Instance.RegisterRB(l_Spawned[i].GetComponent<Rigidbody>());
                }
                //-----------------------------------------------------------
                //Speicher Änderung
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
            foreach (GameObject b_Domino in l_LastChange.m_ChangedDominos)
            {
                //-----------------------------------------------------------
                //Überspringe falls Domino nicht mehr existiert
                //-----------------------------------------------------------
                if (b_Domino == null)
                    continue;
                //-----------------------------------------------------------
                //Je nach Modus
                //-----------------------------------------------------------
                switch (l_LastChange.m_Mode)
                {
                    case DominoChange.enRestoreMode.Destroy:
                        //-----------------------------------------------------------
                        //Entferne altes Objekt
                        //-----------------------------------------------------------
                        FreezeManager.Instance.RemoveRB(b_Domino.GetComponent<Rigidbody>());
                        Destroy(b_Domino);
                        break;
                    case DominoChange.enRestoreMode.Replace:
                        //------------------------------------------------------
                        //Wechsle Material und Physics-Material zurück
                        //------------------------------------------------------
                        b_Domino.GetComponent<MeshRenderer>().sharedMaterial = m_DominoPrefab.GetComponent<MeshRenderer>().sharedMaterial;
                        b_Domino.GetComponent<Collider>().sharedMaterial = m_DominoPrefab.GetComponent<Collider>().sharedMaterial;
                        break;
                }
            }
        }
    }
    #endregion
}