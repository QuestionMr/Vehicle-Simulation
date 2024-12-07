using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Car Signal Check", menuName = "LineObjects/Car Signal Check")]
public class SignalCheckObject : BaseTestObject
{
    public int m_leftCheck;
    public int m_rightCheck;

    public int m_scoreDelta;
    public override void Action(TestManager testManager, float value){
        SignalModule signalModule = testManager.GetComponent<SignalModule>();
        Debug.Log(m_leftCheck + m_rightCheck * 2 + " " + signalModule.GetTurnSide());
        if (m_leftCheck + m_rightCheck * 2 != signalModule.GetTurnSide()) {
            testManager.ChangeScore((int)value, m_scoreDelta);
            Debug.Log("Y");
        }
    }
}
