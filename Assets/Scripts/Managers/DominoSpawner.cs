using UnityEngine;
using System.Collections.Generic;

public class DominoSpawner : MonoBehaviour
{
    [Header("Objects")]
    //------------------------------------------------------
    //Domino Prefab
    //------------------------------------------------------
    [SerializeField]
    private GameObject m_DominoPrefab;
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
    //Editormodus
    //------------------------------------------------------
    private bool m_EditMode;

    /// <summary>
    /// Switcht Editormodus
    /// </summary>
    public bool EditMode {
        private get
        {
            return m_EditMode;
        }
        set {
            m_EditMode = !m_EditMode;
            //-----------------------------------------------------------------
            //Friere RBs ein / Taue RBs auf
            //-----------------------------------------------------------------
            FreezeManager.Instance.RBsActive= !m_EditMode;
        }
    }

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
                //Führe Raycast durch
                //------------------------------------------------------
                if(!EditMode && Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Dominos")))
                {
                    //------------------------------------------------------
                    //Wende entweder Force auf Rigidbody des Hits an..
                    //------------------------------------------------------
                    l_Hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(l_Ray.direction, l_Hit.point, ForceMode.Impulse);
                }
                if (EditMode && Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
                {
                    //------------------------------------------------------
                    //..Oder speichere Punkt im Spawnarray
                    //------------------------------------------------------
                    if(Mathf.Abs(l_Hit.point.y) < m_EpsilonSpawnHeight)
                    {
                        m_Spawner.positionCount++;
                        m_Spawner.SetPosition(m_Spawner.positionCount-1, l_Hit.point);
                    }
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
                //Spawne Steine mit entsprechenden Koordinaten und Rotationen
                //-----------------------------------------------------------
                for (int i = 0; i < l_InterpolatedPositions.Count; i++)
                {
                    //-----------------------------------------------------------------
                    //Füge RB zum Manager hinzu
                    //-----------------------------------------------------------------
                    Rigidbody m_NewRB = Instantiate(m_DominoPrefab, l_InterpolatedPositions[i], l_InterpolatedRotations[i]).GetComponent<Rigidbody>();
                    FreezeManager.Instance.RegisterRB(m_NewRB);
                }
            }
        }
    }
}