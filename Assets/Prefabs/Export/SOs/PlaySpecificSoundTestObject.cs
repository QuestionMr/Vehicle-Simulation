using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Play Sound", menuName = "LineObjects/Play Sound")]

public class PlaySpecificSoundTestObject : BaseTestObject
{
    public AudioClip m_audioClip;
    public override void Action(TestManager testManager, float value){
        if (testManager.GetCurrentTest() == value) testManager.PlaySound(m_audioClip);
    } 
}
