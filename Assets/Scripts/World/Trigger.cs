using UnityEngine;

public class Trigger : MonoBehaviour
{
    //------------------------------------------------------
    //Typ dieses Triggers
    //------------------------------------------------------
    [SerializeField]
    private TriggerType m_Type = TriggerType.None;
    //------------------------------------------------------
    //Eckpunkte des Triggers
    //------------------------------------------------------
    [SerializeField]
    private Transform[] m_Corners;

    /// <summary>
    /// Get Ecken des Triggers
    /// </summary>
    public Transform[] Corners
    {
        get
        {
            return m_Corners;
        }
    }

    /// <summary>
    /// Triggertypen
    /// </summary>
    private enum TriggerType
    {
        None,
        Cannon,
        Gate        
    }
    
    private void OnCollisionEnter(Collision pi_Collision)
    {
        //------------------------------------------------------
        //Wenn ein Domino den Trigger berührt hat
        //------------------------------------------------------
        if (pi_Collision.transform.tag.Contains("Domino"))
        {
            //------------------------------------------------------
            //Je nach Triggertyp..
            //------------------------------------------------------
            switch (m_Type)
            {
                case TriggerType.None:
                    //------------------------------------------------------
                    //Warnung das kein Typ festgelegt wurde
                    //------------------------------------------------------                    
                    Debug.LogWarning("No trigger type assigned!");
                    break;
                case TriggerType.Gate:
                    //------------------------------------------------------
                    //Öffne Tor
                    //------------------------------------------------------
                    gameObject.GetComponentInParent<Gate>().OpenGate();
                    break;
                case TriggerType.Cannon:
                    //------------------------------------------------------
                    //Feuere die Kanone ab
                    //------------------------------------------------------
                    gameObject.GetComponentInParent<Cannon>().Shoot();
                    break;
            }
            //------------------------------------------------------
            //Spiele Soundeffekt ab
            //------------------------------------------------------
            SoundEffectManager.Instance.PlayLeverContact();
        }
    }
}