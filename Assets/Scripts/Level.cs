using UnityEngine;
using UnityEngine.EventSystems;

public class Level : MonoBehaviour, IPointerDownHandler {
    //------------------------------------------------------
    //Zugriffspunkt aufs Prefab, ändert sich zukünftig
    //------------------------------------------------------
    [SerializeField]
    private GameObject m_DominoPrefab;
    
    /// <summary>
    /// Bei Input
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
            //------------------------------------------------------
            //Führe Raycast durch
            //------------------------------------------------------
            if (Physics.Raycast(l_Ray, out l_Hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
            {
                //------------------------------------------------------
                //Erstelle ggf. Dominostein
                //------------------------------------------------------
                Instantiate(m_DominoPrefab, l_Hit.point,  Quaternion.Euler(-90, Quaternion.LookRotation(l_Ray.direction).eulerAngles.y, 0));
            }            
        }
    }
}