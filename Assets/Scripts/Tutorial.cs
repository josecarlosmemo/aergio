using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    
    public List<TutorialItem> items;

    private int currentZone;

    public float timeToAppear = 3f;
    float timeWhenDisappear;

    bool isShowingZone = false;

    
    void Start()
    {
        PlayerController.onZoneEntered += findZone;
        
    }

    void Update()
    {
        

        if (items[currentZone].ui.activeSelf && (Time.time >= timeWhenDisappear) )
        {
            disableCurrentZone();
            
        }
    }

   

    void findZone(string zoneToFind){
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].zone.name == zoneToFind)
            {
                disableCurrentZone();
                currentZone = i;
                showCurrentZone();
                
            }
            
            
        }
        
    }

  
    void disableCurrentZone(){
        items[currentZone].ui.SetActive(false);
        timeWhenDisappear = Time.time;

    }

    void showCurrentZone(){
        timeWhenDisappear = Time.time + timeToAppear;
        items[currentZone].ui.SetActive(true);
        
    }

    private void OnDestroy() {
        PlayerController.onZoneEntered -= findZone;

        
    }




}



[System.Serializable]
public class TutorialItem{
  public GameObject zone, ui;

}
