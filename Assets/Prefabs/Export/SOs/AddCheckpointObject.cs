using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Line Add Checkpoint")]

public class AddCheckpointObject : BaseTestObject
{
    public override void Action(TestManager testManager, float value){
        testManager.SetCurrentCheckpoint((int)value);
    } 
}
