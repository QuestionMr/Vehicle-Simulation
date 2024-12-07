using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public TMP_Text m_currentTestNumber;
    public TMP_Text m_currentTestState;
    public TMP_Text m_currentTestScore;

    public Transform m_needleImage;
    public CarScript m_carScript;
    private TestManager m_testManager;
    
    public TMP_Text m_RPMText;
    public TMP_Text m_KMHText;
    public TMP_Text m_gearText;
    public TMP_Text m_timeStateIndicator;
    public TMP_Text m_timeStateCounter;

    // Start is called before the first frame update
    void Start()
    {
        m_testManager = m_carScript.GetComponent<TestManager>();
        m_testManager.BindTimeStateInvoke(SetTimeState);
    }

    // Update is called once per frame
    void Update()
    {
        m_KMHText.text = m_carScript.GetCurrentKMHSpeed().ToString();
        m_gearText.text = m_carScript.m_currentGear.ToString();
        m_needleImage.rotation = Quaternion.Euler(0,0,-194 - Mathf.Abs(m_carScript.GetCurrentKMHSpeed()));

       if (m_testManager.GetTimeState() != TimeState.NORMAL)m_timeStateCounter.text = ((int)m_testManager.GetCurrentActiveTime()).ToString();


    }

    void SetTimeState(TimeState ts){
        if (m_testManager.GetTimeState() == TimeState.NORMAL) m_timeStateIndicator.transform.parent.gameObject.SetActive(false);
        else m_timeStateIndicator.transform.parent.gameObject.SetActive(true);

        if (m_testManager.GetTimeState() == TimeState.RUSH) m_timeStateIndicator.text = "Rush";
        if (m_testManager.GetTimeState() == TimeState.WAIT) m_timeStateIndicator.text = "Wait";
    }
}
