using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightScript : MonoBehaviour
{
    public GameObject m_redLight;
    public GameObject m_greenLight;
    public GameObject m_yellowLight;
    private LightState m_lightState;
    public void UpdateLightState(LightState ls){
        m_lightState = ls;
        m_redLight.SetActive(m_lightState == LightState.RED);
        m_greenLight.SetActive(m_lightState == LightState.GREEN);
        m_yellowLight.SetActive(m_lightState == LightState.YELLOW);
    }
}
