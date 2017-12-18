using System.Collections.Generic;
using UnityEngine;

public class FreezeManager : MonoBehaviour {
    #region Declarations

    //-----------------------------------------------------------------
    //Singleton    
    //-----------------------------------------------------------------
    public static FreezeManager Instance = null;
    //-----------------------------------------------------------------
    //RigidBodys
    //-----------------------------------------------------------------
    private List<Rigidbody> m_RBs = new List<Rigidbody>();
    //-----------------------------------------------------------------
    //Dictionary mit Geschwindigkeiten
    //-----------------------------------------------------------------
    private Dictionary<Rigidbody, Vector3> m_Velocities = new Dictionary<Rigidbody, Vector3>();
    //-----------------------------------------------------------------
    //RBs aktiv oder nicht
    //-----------------------------------------------------------------
    private bool m_Forzen;

    #endregion

    #region Properties

    /// <summary>
    /// RBs aktuell eingeforen oder nicht
    /// </summary>
    public bool Frozen
    {
        get
        {
            return m_Forzen;
        }
        set
        {
            m_Forzen = !m_Forzen;
            SetRBStatus();
        }
    }

    #endregion

    #region Catch Events

    /// <summary>
    /// Wenn das Script aktiviert wird
    /// </summary>
    private void Awake()
    {
        //-----------------------------------------------------------------
        //Erstelle eine Instanz falls nicht vorhanden
        //-----------------------------------------------------------------
        if (Instance == null)
        {
            Instance = this;
        }
        //-----------------------------------------------------------------
        //..Oder lösche falls diese Instanz nicht die Instanz ist
        //-----------------------------------------------------------------
        else if (Instance != this)
            Destroy(this);
    }

    #endregion

    #region Procedures

    /// <summary>
    /// Füge neuen RB hinzu
    /// </summary>
    /// <param name="pi_RB">Neuer RB</param>
    public void RegisterRB(Rigidbody pi_RB)
    {
        //-----------------------------------------------------------------
        //Speichere ggf. Geschwindigkeit
        //-----------------------------------------------------------------
        if (pi_RB.velocity.magnitude > 0)
            m_Velocities.Add(pi_RB, pi_RB.velocity);
        //-----------------------------------------------------------------
        //Friere ggf. direkt ein
        //-----------------------------------------------------------------
        if (Frozen)        
            pi_RB.constraints = RigidbodyConstraints.FreezeAll;
        //-----------------------------------------------------------------
        //Füge hinzu
        //-----------------------------------------------------------------
        m_RBs.Add(pi_RB);
    }

    /// <summary>
    /// Entferne RB
    /// </summary>
    /// <param name="pi_RB">Zu entfernender RB</param>
    public void RemoveRB(Rigidbody pi_RB)
    {
        m_RBs.Remove(pi_RB);
    }

    /// <summary>
    /// Passe RB aktiv an
    /// </summary>
    private void SetRBStatus()
    {
        List<Rigidbody> l_Remove = new List<Rigidbody>();
        //-----------------------------------------------------------------
        //Gehe durch die RBs
        //-----------------------------------------------------------------
        foreach (Rigidbody b_RB in m_RBs)
        {
            //-----------------------------------------------------------------
            //Falls nicht vorhanden muss er entfernt werden
            //-----------------------------------------------------------------
            if (b_RB == null)
            {
                l_Remove.Add(b_RB);
            }
            //-----------------------------------------------------------------
            //Ansonsten..
            //-----------------------------------------------------------------
            else
            {
                //-----------------------------------------------------------------
                //..Falls RBs aktiv..
                //-----------------------------------------------------------------
                if (!Frozen)
                {
                    //-----------------------------------------------------------------
                    //Entferne Constraints..
                    //-----------------------------------------------------------------
                    b_RB.constraints = RigidbodyConstraints.None;
                    //-----------------------------------------------------------------
                    //Stelle Geschwindigkeit wieder her
                    //-----------------------------------------------------------------
                    if (m_Velocities.ContainsKey(b_RB))
                    {
                        b_RB.velocity = m_Velocities[b_RB];
                        m_Velocities.Remove(b_RB);
                    }
                }
                //-----------------------------------------------------------------
                //..Ansonsten..
                //-----------------------------------------------------------------
                else
                {
                    //-----------------------------------------------------------------
                    //Speichere ggf.  Geschwindigkeit
                    //-----------------------------------------------------------------
                    if (b_RB.velocity.magnitude > 0)                    
                        m_Velocities.Add(b_RB, b_RB.velocity);                    
                    //-----------------------------------------------------------------
                    //Friere ein
                    //-----------------------------------------------------------------
                    b_RB.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
        //-----------------------------------------------------------------
        //Entferne gelöschte RBs
        //-----------------------------------------------------------------
        foreach (Rigidbody b_RB in l_Remove)
        {
            RemoveRB(b_RB);
        }
    }

    #endregion
}