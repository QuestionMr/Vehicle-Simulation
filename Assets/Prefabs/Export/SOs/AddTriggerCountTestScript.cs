using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Line Add Trigger",  menuName = "LineObjects/Line Add Trigger")]

public class AddTriggerCountTestScript : BaseTestObject
{
    public int m_addDelta;
    public override void Action(TestManager testManager, float value){
        testManager.ChangeCollideCount(m_addDelta);
    } 
}
