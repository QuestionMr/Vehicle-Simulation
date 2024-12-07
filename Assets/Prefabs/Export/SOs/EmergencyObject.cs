using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Emergency Check", menuName = "LineObjects/Emergency Check")]
public class EmergencyObject : BaseTestObject
{
    public BaseTestObject[] m_parts;
    public override void Action(TestManager testManager, float value){
        if (testManager.GetTimeState() == TimeState.NORMAL && testManager.GetCurrentActiveTime() < 0){
            Debug.Log("a");
            foreach (BaseTestObject bto in m_parts){
                bto.Action(testManager, value);
            }
        }
    }
}
