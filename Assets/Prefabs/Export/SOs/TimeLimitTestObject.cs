using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Car Time Limit Action")]

public class TimeLimitTestObject : BaseTestObject
{
    public float m_timeLimit;
    public TimeState m_timeType;
    public override void Action(TestManager testManager, float value){
        if (value == testManager.GetCurrentTest()) testManager.StartTime(m_timeLimit, m_timeType);
    }
}
