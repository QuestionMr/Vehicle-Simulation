using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName ="Car Test Action",  menuName = "LineObjects/Car Test Action")]
public class BaseTestObject : ScriptableObject
{
    public virtual void Action(TestManager testManager, float value){
        testManager.StartTest((int)value);
    }
}
