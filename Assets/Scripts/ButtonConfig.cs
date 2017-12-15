using UnityEngine;
using UnityEngine.UI;

public class ButtonConfig : MonoBehaviour{
    //------------------------------------------------------
    //Referenz auf das Image
    //------------------------------------------------------
    private Image m_Text;

    private void Awake()
    {
        //------------------------------------------------------
        //Setzte Text auf nichts, hole Image
        //------------------------------------------------------
        gameObject.GetComponentInChildren<Text>().text = "";
        m_Text = gameObject.GetComponent<Image>();
        m_Text.color = Color.white;
    }

    /// <summary>
    /// Wechselt Farbe zw Rot und Weiß
    /// </summary>
    public void ChangeColor()
    {
        if(m_Text.color == Color.white)
        {
            m_Text.color = Color.red;
        }
        else
        {
            m_Text.color = Color.white;
        }
    }
}