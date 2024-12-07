using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Car Score Action")]

public class ScoreTestObject : BaseTestObject
{
    public int m_scoreDelta;
    public override void Action(TestManager testManager, float value){
        testManager.ChangeScore((int)value, -m_scoreDelta);
    }
 
}
