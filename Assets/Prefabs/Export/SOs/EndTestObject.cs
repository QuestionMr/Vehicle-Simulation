using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Car End Test Action", menuName = "LineObjects/Car End Test Action")]
public class EndTestObject : BaseTestObject
{
     public override void Action(TestManager testManager, float value){
        testManager.EndTest((int)value);
    }
}
