using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightManagerState{
    OFF,
    HORIZONTAL,
    HORIZONTAL_YELLOW,

    VERTICAL,
    VERTICAL_YELLOW
}
public enum LightState{
    RED,
    GREEN,
    YELLOW
}
public class TrafficLightManager : MonoBehaviour
{
    public List<TrafficLightScript> m_horizontalTrafficLights;
    public List<TrafficLightScript> m_verticalTrafficLights;
    public List<TestLineScript> m_horizontalTestLines;
    public List<TestLineScript> m_verticalTestLines;
    public LightManagerState m_currentLightState;
    public float m_greenLightCooldown;
    public float m_yellowLightCooldown;
    void Start()
    {
        StartCoroutine(SwapTrafficStateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SwapTrafficStateCoroutine(){
        SwapToNextState();
        SetTrafficState();
        float coolDown = m_yellowLightCooldown;
        if (m_currentLightState != LightManagerState.HORIZONTAL_YELLOW &&  m_currentLightState != LightManagerState.VERTICAL_YELLOW ){
            coolDown = m_greenLightCooldown;
        }
        yield return new WaitForSeconds(coolDown);
        StartCoroutine(SwapTrafficStateCoroutine());
    }

    private void SetTrafficState(){
        if (m_currentLightState == LightManagerState.HORIZONTAL){
            //TODO
            foreach (TestLineScript hl in m_horizontalTestLines) hl.UpdateLightState(LightState.GREEN);
            foreach (TestLineScript vl in m_verticalTestLines) vl.UpdateLightState(LightState.RED);

            foreach (TrafficLightScript hl in m_horizontalTrafficLights) hl.UpdateLightState(LightState.GREEN);
            foreach (TrafficLightScript vl in m_verticalTrafficLights) vl.UpdateLightState(LightState.RED);
            //Debug.Log("Horizontal to go");
        }

        else if (m_currentLightState == LightManagerState.VERTICAL){
            //TODO
            foreach (TestLineScript hl in m_horizontalTestLines) hl.UpdateLightState(LightState.RED);
            foreach (TestLineScript vl in m_verticalTestLines) vl.UpdateLightState(LightState.GREEN);
            //Debug.Log("Vertical to go");

            foreach (TrafficLightScript hl in m_horizontalTrafficLights) hl.UpdateLightState(LightState.RED);
            foreach (TrafficLightScript vl in m_verticalTrafficLights) vl.UpdateLightState(LightState.GREEN);
        }
           
        else if (m_currentLightState == LightManagerState.HORIZONTAL_YELLOW){
            //TODO
            foreach (TestLineScript hl in m_horizontalTestLines) hl.UpdateLightState(LightState.YELLOW);
            //Debug.Log("Horizontal to yellow");
            foreach (TrafficLightScript hl in m_horizontalTrafficLights) hl.UpdateLightState(LightState.YELLOW);

        }

        else if (m_currentLightState == LightManagerState.VERTICAL_YELLOW){
            //TODO
            foreach (TestLineScript vl in m_verticalTestLines) vl.UpdateLightState(LightState.YELLOW);
            //Debug.Log("Vertical to yellow");
            foreach (TrafficLightScript vl in m_verticalTrafficLights) vl.UpdateLightState(LightState.YELLOW);
        }

        else{
            //TODO
           // Debug.Log("All lights turned off");
        }
    }

    private void SwapToNextState(){
        if (m_currentLightState < LightManagerState.VERTICAL_YELLOW) m_currentLightState++; 
        else m_currentLightState = LightManagerState.HORIZONTAL;
    }
}
