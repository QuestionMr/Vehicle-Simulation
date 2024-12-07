using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestLineScript : MonoBehaviour
{
    public int m_testNumber;
    //public UnityEvent m_startTrigger;
    public List<BaseTestObject> m_OnCollisionEnterActions;
    public List<BaseTestObject> m_OnCollisionExitActions;
    public List<BaseTestObject> m_OnCollisionStayActions;
    private LightState m_lightState;
    // Start is called before the first frame update
    void Start()
    {
        m_lightState = LightState.GREEN;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        //Check the type of object that passes this line
        //Debug.Log("Triggered");
        TestManager tm = other.transform.parent.gameObject.GetComponent<TestManager>();
        if (m_lightState == LightState.YELLOW) tm.AddLineCross(1);
        else if (m_lightState == LightState.RED) tm.AddLineCross(5);
        foreach (BaseTestObject baseTestObject in m_OnCollisionEnterActions)
            baseTestObject.Action(tm, m_testNumber);
    }
    void OnTriggerExit(Collider other){
        //Check the type of object that passes this line
        Debug.Log("Exited");
        foreach (BaseTestObject baseTestObject in m_OnCollisionExitActions)
            baseTestObject.Action(other.transform.parent.gameObject.GetComponent<TestManager>(), m_testNumber);
    }

    void OnTriggerStay(Collider other){
        //Check the type of object that passes this line
        foreach (BaseTestObject baseTestObject in m_OnCollisionStayActions)
            baseTestObject.Action(other.transform.parent.gameObject.GetComponent<TestManager>(), m_testNumber);
    }

    public void UpdateLightState(LightState ls){
        m_lightState = ls;
    }
}
