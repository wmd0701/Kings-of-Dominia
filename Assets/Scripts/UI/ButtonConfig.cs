using UnityEngine;
using UnityEngine.UI;

public class ButtonConfig : MonoBehaviour{
    //------------------------------------------------------
    //Referenz auf das Image
    //------------------------------------------------------
    private Image m_Button;
    //------------------------------------------------------
    //Farben für aktiv/nicht aktiv
    //------------------------------------------------------
    [Header("Settings")]
    [SerializeField]
    private Color m_Active;
    [SerializeField]
    private Color m_NotActive;

    private void Awake()
    {
        //------------------------------------------------------
        //Setzte Text auf nichts, hole Image
        //------------------------------------------------------
        gameObject.GetComponentInChildren<Text>().text = "";
        m_Button = gameObject.GetComponent<Image>();
        m_Button.color = m_NotActive;
    }

    /// <summary>
    /// Wechselt Farbe zw Rot und Weiß
    /// </summary>
    public void ChangeColor()
    {
        if(m_Button.color == m_NotActive)
        {
            m_Button.color = m_Active;
        }
        else
        {
            m_Button.color = m_NotActive;
        }
    }
}