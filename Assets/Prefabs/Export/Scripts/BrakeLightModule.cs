using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BrakeLightModule : CarModule 
{
    public GameObject m_brakeLights;
    private CarScript m_carScript;
    void Start(){
        m_carScript = GetComponent<CarScript>();
    }
    public override void ModuleUpdate()
    {
        m_brakeLights.SetActive(m_carScript.GetBrakeDir() > 0.1f);
    }
}
