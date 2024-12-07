using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Car Speed Score Action")]

public class SpeedTestObject : ScoreTestObject
{
    public float m_minSpeedLimit;
    public float m_maxSpeedLimit;
    public override void Action(TestManager testManager, float value){
        if (testManager.GetComponent<CarScript>().GetCurrentKMHSpeed() < m_minSpeedLimit
        || testManager.GetComponent<CarScript>().GetCurrentKMHSpeed() > m_maxSpeedLimit) 
        testManager.ChangeScore((int)value, m_scoreDelta);
    }
}
