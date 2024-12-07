using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Play Next Test Sound", menuName = "LineObjects/Play Next Test Sound")]

public class PlayNextTestSoundObject : BaseTestObject
{
    public override void Action(TestManager testManager, float value){
        if (testManager.GetCurrentTest() == value) testManager.PlayNextTestSound();
    } 
}
