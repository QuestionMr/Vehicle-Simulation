using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Car Time Score Action")]

public class TimeTestScript : ScoreTestObject
{
     public override void Action(TestManager testManager, float value){

        if (!testManager.IsTimeOver()) testManager.ChangeScore((int)value, -m_scoreDelta);
    }
}
