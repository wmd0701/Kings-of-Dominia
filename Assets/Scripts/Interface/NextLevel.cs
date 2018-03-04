using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData pi_PED)
    {
        //------------------------------------------------------
        //Hole aktuelle Szene
        //------------------------------------------------------
        Scene l_Current = SceneManager.GetActiveScene();
        //------------------------------------------------------
        //Lade die nächste falls es eine solche gibt
        //------------------------------------------------------        
        if (SceneManager.sceneCountInBuildSettings > l_Current.buildIndex + 1)
        {
            SceneManager.LoadScene(l_Current.buildIndex + 1);
        }
    }    
}