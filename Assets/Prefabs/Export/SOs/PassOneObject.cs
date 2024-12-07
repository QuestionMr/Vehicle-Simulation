using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Pass Once Action")]
public class PassOneObject : ScoreTestObject
{
     public override void Action(TestManager testManager, float value){
        if (testManager.GetCurrentTest() == value) testManager.ChangeScore((int) value, m_scoreDelta);
    }
}
