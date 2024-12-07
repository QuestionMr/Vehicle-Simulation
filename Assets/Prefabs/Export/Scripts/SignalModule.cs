using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum SignalState{
    
}
public class SignalModule : CarModule 
{
    public float m_blinkCooldown;
    public GameObject[] m_turnLights;
    private int m_currentTurnSide;
    private int m_currentSignalPosition;
    private int m_isEmergencyLightOn;
    private Coroutine m_lightUpdateCoroutine;
    public AudioSource m_audioSource;
    public AudioClip[] m_signalAudios;
    void Start(){
        m_currentSignalPosition = 0;
        m_isEmergencyLightOn = 0;
        m_lightUpdateCoroutine = null;
    }
    public override void ModuleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.O)) ShiftSignal(-1);
        if (Input.GetKeyDown(KeyCode.P)) ShiftSignal(1);
        if (Input.GetKeyDown(KeyCode.I)) {
            m_isEmergencyLightOn = 1 - m_isEmergencyLightOn;
            ShiftSignal(0);
        }
    }
    private void ShiftSignal(int delta){
        m_audioSource.PlayOneShot(m_signalAudios[0], 1);
        m_currentSignalPosition += delta;
        m_currentSignalPosition = Mathf.Clamp(m_currentSignalPosition, -1, 1);
        if (m_currentSignalPosition == -1) m_currentTurnSide = 1;
        else if (m_currentSignalPosition == 1) m_currentTurnSide = 2;
        else m_currentTurnSide = 0;
        if (m_isEmergencyLightOn == 1) m_currentTurnSide |= 3;
        if (m_lightUpdateCoroutine == null) m_lightUpdateCoroutine = StartCoroutine(SignalUpdate());
    }
    private IEnumerator SignalUpdate(){
        int currentTurnSide = m_currentTurnSide;
        Debug.Log(m_currentTurnSide);
        for (int i = m_turnLights.Length - 1; i >= 0; i--){
            if (currentTurnSide - (i + 1) >= 0) {
                m_turnLights[i].SetActive(true);
                currentTurnSide -= i + 1;
            }
        }
        m_audioSource.PlayOneShot(m_signalAudios[1], 1);
        yield return new WaitForSeconds(m_blinkCooldown);
        for (int i = m_turnLights.Length - 1; i >= 0; i--){
            m_turnLights[i].SetActive(false);
        }
        m_audioSource.PlayOneShot(m_signalAudios[1], 1);
        yield return new WaitForSeconds(m_blinkCooldown);
        if (m_currentTurnSide > 0) m_lightUpdateCoroutine = StartCoroutine(SignalUpdate());
        else yield return m_lightUpdateCoroutine = null;
    }
    public int GetTurnSide(){
        return m_currentTurnSide;
    }
}
