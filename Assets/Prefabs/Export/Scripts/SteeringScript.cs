using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SteeringScript : MonoBehaviour
{
    public float m_maxRotations;
    public UnityEvent<float> m_steerEvent = new UnityEvent<float>();
    public GameObject m_steeringGameObject;

    public float m_currentTotalAngle;
    private float m_maxAngle;
    private float m_currAngle;
    private float m_angleDiff;
    private bool m_isHoldingWheel; 
    private Vector2 m_pointerPos;
    void Start()
    {
        m_currentTotalAngle = 0;
        m_maxAngle = m_maxRotations * 360;
        GetComponent<UnityEngine.UI.Image>().rectTransform.localEulerAngles = Vector3.zero;

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnMouseDown((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnMouseDragging((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnMouseRelease((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    void Update(){
        GetComponent<UnityEngine.UI.Image>().rectTransform.localEulerAngles = m_currentTotalAngle * Vector3.back;
        m_steeringGameObject.transform.localEulerAngles= new Vector3( m_steeringGameObject.transform.localEulerAngles.x,
        m_steeringGameObject.transform.localEulerAngles.y,
        -m_currentTotalAngle);
    }
    public void SteerAngleUpdate(float shiftAmount, float tempSteerDir, bool isAutoReturn){
        //if (m_isHoldingWheel || m_currentTotalAngle == 0) return;
        if (m_isHoldingWheel) return;
        if (tempSteerDir != 0){
            if (!isAutoReturn)m_currentTotalAngle += 3 * tempSteerDir;
            else m_currentTotalAngle = tempSteerDir * m_maxAngle;
        }
        else{
            if (m_currentTotalAngle == 0) return;
            if (m_currentTotalAngle < 0) shiftAmount *= -1;
            if ((m_currentTotalAngle - shiftAmount) * m_currentTotalAngle < 0) m_currentTotalAngle = 0;
            else m_currentTotalAngle -= shiftAmount;
        }
        m_currentTotalAngle = Mathf.Clamp(m_currentTotalAngle, -m_maxAngle, m_maxAngle);


        m_steerEvent.Invoke(NormalizeSteeringWheelValue()); 
    }

    void OnMouseDown(PointerEventData data){
        m_pointerPos = data.position;
        m_isHoldingWheel = true;
        m_currAngle = Vector2.SignedAngle(Vector2.up, m_pointerPos - (Vector2)GetComponent<UnityEngine.UI.Image>().rectTransform.position);
    }

    void OnMouseDragging(PointerEventData data){
        m_pointerPos = data.position;
        
        float pastAngle = m_currAngle;
        m_currAngle = Vector2.SignedAngle(Vector2.up, m_pointerPos - (Vector2)GetComponent<UnityEngine.UI.Image>().rectTransform.position);
        m_angleDiff = m_currAngle - pastAngle;
        CaluculateInputSteeringValue();
        m_steerEvent.Invoke(NormalizeSteeringWheelValue()); 
    }

    void OnMouseRelease(PointerEventData data){
        m_isHoldingWheel = false;
    }

    private float NormalizeSteeringWheelValue(){
        return m_currentTotalAngle / m_maxAngle;
    }
    private void CaluculateInputSteeringValue(){
        if (Mathf.Abs(m_angleDiff) > 180){
            if (m_angleDiff > 0) m_angleDiff -= 360;
            else m_angleDiff += 360;
        }
        m_currentTotalAngle -= m_angleDiff;
        m_currentTotalAngle = Mathf.Clamp(m_currentTotalAngle, -m_maxAngle, m_maxAngle);
        //m_angleDiff = 0;
    }
    
}
