using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CarScript : MonoBehaviour
{
    public AnimationCurve m_torqueCurve;
    //[Serializable]
    public List<CarModule> m_carModules;
    public float m_throttle;
    public float m_moveSpeed;
    public float m_steerSpeed;
    public float m_brakePower;
    public float m_wheelDiameter;
    public float m_baseRPMLimit;
    public float m_baseTorque;
        public float m_engineMultiplier;


    public bool m_isAutoReturn;
    public float[] m_gears;
    public List<AxleInfo> m_axleInfo; 
    public UnityEvent<float, float, bool> m_casterEvent = new UnityEvent<float, float, bool>();

    public float m_casterMultiplier;
    private float m_moveDir;
    private float m_steerDir;
    private float m_brakeDir;
    public int m_currentGear;
    private float m_currentTorque;
    private float m_currentEngineRPM;
 
    private Rigidbody rb;
    private float m_minimumWheelDampingRate;
    private float m_defaultKmHSpeed;
    private float m_maxKMH;
    private float m_currentKMHSpeed;
    public AudioSource m_engineSource;
    public float m_minPitch;
    public float m_maxPitch;
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_minimumWheelDampingRate = m_axleInfo[0].m_leftWheel.wheelDampingRate;
        m_defaultKmHSpeed = m_wheelDiameter * 3.14f * 60 / 100000;
        Debug.Log(m_defaultKmHSpeed);
        //m_currentGear = 0;
        m_currentEngineRPM = 0;
        m_currentKMHSpeed = 0;
        m_engineSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float lW = (int)(m_axleInfo[1].m_leftWheel.rpm * 100) / 100;
        float rW = (int)(m_axleInfo[1].m_rightWheel.rpm * 100) / 100;
        int temp = (int)(m_currentEngineRPM * 100);
        float pitch = Mathf.Lerp(m_minPitch, m_maxPitch, m_currentEngineRPM / 1.2f);
        m_engineSource.pitch = pitch;
        float cE = m_currentEngineRPM;
        m_currentKMHSpeed = (int)((lW + rW)* m_defaultKmHSpeed * 0.5f);
        m_maxKMH = Mathf.Max(m_maxKMH, (m_torqueCurve.Evaluate(m_currentEngineRPM) * m_moveSpeed));
        m_moveDir = Input.GetAxis("Vertical");
        //m_moveDir = 1;
        float tempSteerDir = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.E)) {
            m_currentGear++;
            m_currentGear = Mathf.Clamp(m_currentGear, 0, 4);

        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            m_currentGear--;
            m_currentGear = Mathf.Clamp(m_currentGear, 0, 4);

        }
        //if (tempSteerDir != 0) Debug.Log(tempSteerDir);
        m_brakeDir = Input.GetAxisRaw("Jump");
        m_casterEvent.Invoke(Mathf.Abs(m_axleInfo[0].m_leftWheel.rpm) * m_casterMultiplier * Time.deltaTime, tempSteerDir, m_isAutoReturn);
        UpdateWheel();
        foreach (CarModule cm in m_carModules){
            cm.ModuleUpdate();
        }
        //Debug.Log(m_axleInfo[0].m_leftWheel.rpm + " and " + m_axleInfo[0].m_leftWheel.rpm);
        
    }
    public void SetSteerDir(float dir){
        //Debug.Log(dir);
        m_steerDir = dir;
    }

    //Fixed update
    // Calculate caster angle shift
    // Calculate input angle shift
    // If input is active (mouse holding steering wheel OR horizontal input is pressed) -> only calculate input angle
    // In other words, only caster angle shift or input angle shift is updated
    // After angle change update, apply update on both vehicle's front wheels angle AND input element's visual steering wheel angle
    // Note: the input steering wheel's angle change is purely visual, as angles are calculated relative to the image's position, which
    // stays constant
    void FixedUpdate(){

        SolveCurrentTorque();
        //Debug.Log(m_currentEngineRPM + " engine RPM " + m_axleInfo[1].m_leftWheel.rpm + " motor rpm " + m_currentTorque + " current torque curve ");
        m_axleInfo[0].m_leftWheel.steerAngle = m_steerDir * m_steerSpeed;
        m_axleInfo[0].m_rightWheel.steerAngle = m_steerDir * m_steerSpeed;


        // m_axleInfo[0].m_leftWheel.motorTorque = m_moveDir * m_moveSpeed;
        // m_axleInfo[0].m_rightWheel.motorTorque = m_moveDir * m_moveSpeed;
        
        foreach (AxleInfo axelInfo in m_axleInfo){
            axelInfo.m_leftWheel.motorTorque = m_moveDir * m_currentTorque * m_throttle;
            axelInfo.m_rightWheel.motorTorque = m_moveDir * m_currentTorque * m_throttle;

            axelInfo.m_leftWheel.brakeTorque = m_brakeDir * 200f;
            axelInfo.m_rightWheel.brakeTorque = m_brakeDir * 200f;
            // axelInfo.m_leftWheel.wheelDampingRate = Math.Max(m_brakeDir * m_brakePower, m_minimumWheelDampingRate);
            // axelInfo.m_rightWheel.wheelDampingRate = Math.Max(m_brakeDir * m_brakePower, m_minimumWheelDampingRate);
        }
        //Debug.Log(m_axleInfo[1].m_leftWheel.rpm);
        float lW = (int)(m_axleInfo[1].m_leftWheel.rpm * 100) / 100;

    }

    private void UpdateWheel(){
        foreach (AxleInfo axelInfo in m_axleInfo){
            SetWheelPos(axelInfo);
        }
    }

    private void SetWheelPos(AxleInfo axleInfo){
        Quaternion quat;
        Vector3 pos;
        axleInfo.m_leftWheel.GetWorldPose(out pos, out quat);
        axleInfo.m_leftWheelMesh.transform.SetPositionAndRotation(pos, quat);

        axleInfo.m_rightWheel.GetWorldPose(out pos, out quat);
        axleInfo.m_rightWheelMesh.transform.SetPositionAndRotation(pos, quat);
    }
    private void SolveCurrentTorque(){
        m_currentEngineRPM = (Mathf.Abs(m_axleInfo[1].m_leftWheel.rpm + m_axleInfo[1].m_rightWheel.rpm) * 0.5f) * m_gears[m_currentGear] * m_engineMultiplier;
        m_currentTorque = m_torqueCurve.Evaluate(m_currentEngineRPM) * m_moveSpeed * m_gears[m_currentGear];
        //Debug.Log(m_currentTorque);
    }
    public float GetCurrentKMHSpeed(){
        return m_currentKMHSpeed;
    }
    public float GetBrakeDir(){
        return m_brakeDir;
    }
    public void CancelMovement(){
        foreach (AxleInfo axelInfo in m_axleInfo){
            axelInfo.m_leftWheel.enabled = false;
            axelInfo.m_rightWheel.enabled = false;

            // axelInfo.m_leftWheel.enabled = true;
            // axelInfo.m_rightWheel.enabled = true;
        }
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        StartCoroutine(EnableWheels());
    }

    private IEnumerator EnableWheels(){
        yield return new WaitForSeconds(0.5f);
         foreach (AxleInfo axelInfo in m_axleInfo){
            axelInfo.m_leftWheel.enabled = true;
            axelInfo.m_rightWheel.enabled = true;
        }
    }
}
[System.Serializable] 
 public class AxleInfo{
    public WheelCollider m_leftWheel;
    public WheelCollider m_rightWheel;
    public MeshRenderer m_leftWheelMesh;
    public MeshRenderer m_rightWheelMesh;

 }