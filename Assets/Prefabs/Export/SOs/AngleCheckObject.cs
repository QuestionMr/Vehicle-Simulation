using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Car Angle Check", menuName = "LineObjects/Car Angle Check")]
public class AngelCheckObject : BaseTestObject
{
    public float m_angleLimit;
    public int m_scoreDelta;
     public override void Action(TestManager testManager, float value){
        float angle = Mathf.Abs(testManager.transform.rotation.eulerAngles.y);
        if (angle > 180) angle = 360 - angle;
        Debug.Log(testManager.transform.position.x + " " + angle);
        if (angle > m_angleLimit) testManager.ChangeScore((int)value, m_scoreDelta);
        else {
            testManager.EndTest((int)value);
            testManager.StartTime(0, TimeState.NORMAL);
        }
    }
}
