using UnityEngine;
using UnityEngine.EventSystems;

class Domino : MonoBehaviour, IPointerDownHandler
{
    //------------------------------------------------------
    //Speicherpunkt für Rigidbody
    //------------------------------------------------------
    private Rigidbody m_Rigidbody;

    public void Start()
    {
        //------------------------------------------------------
        //Hole Rigidbody bei Start
        //------------------------------------------------------
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Bei Input, wird wahrscheinlich ausgelagert
    /// </summary>
    public void OnPointerDown(PointerEventData pi_Ped)
    {
        //------------------------------------------------------
        //Wenn nur ein Touch
        //------------------------------------------------------
        if (Input.touchCount == 1)
        {
            RaycastHit l_Hit;
            //------------------------------------------------------
            //Erstelle Ray durch die Kamera basierend auf dem Touch
            //------------------------------------------------------
            Ray l_Ray = Camera.main.ScreenPointToRay(pi_Ped.position);
            if (Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Dominos")))
            {
                //------------------------------------------------------
                //Wende Force auf Rigidbody des Hits an
                //------------------------------------------------------
                m_Rigidbody.AddForceAtPosition(l_Ray.direction, l_Hit.point, ForceMode.Impulse);
            }
        }
    }
}