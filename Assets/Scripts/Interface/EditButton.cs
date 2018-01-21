using UnityEngine;
using UnityEngine.UI;

public class EditButton : MonoBehaviour
{
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
        //Hole Hintergrundbild
        //------------------------------------------------------
        m_Button = gameObject.GetComponent<Image>();
        m_Button.color = m_NotActive;
    }

    /// <summary>
    /// Wechselt Farbe zwischen Rot und Weiß
    /// </summary>
    public void ChangeColor()
    {
        //------------------------------------------------------
        //Switche nur falls kein Overlay aktiv ist
        //------------------------------------------------------
        if (!UIManager.Instance.OverlayEnabled)
        {
            m_Button.color = UIManager.Instance.EditEnabled ? m_Active : m_NotActive;
        }
    }
}