using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayCollisionScript : TimeTestScript
{
    public override void Action(TestManager testManager, float value){
        if (!testManager.IsTimeOver()) {
            testManager.ChangeScore((int)value, -m_scoreDelta);
        }
    }
}
