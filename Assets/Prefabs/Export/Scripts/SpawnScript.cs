using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public Transform[] m_spawnPoints;
    public TestManager m_testManager;
    private CarScript m_carScript;
    // Start is called before the first frame update
    void Start()
    {
        m_carScript = m_testManager.GetComponent<CarScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.B)) {
        //     SetStartingTest(4);
        //     Debug.Log("d");
        // }
    }
    public void SetStartingTest(int test){
        if(m_spawnPoints[test] == null) return;
        m_testManager.ForceSetCheckpoint(m_spawnPoints[test].GetComponent<CheckpointData>().m_checkPointData);
        m_testManager.SetStartingTest(test);
        m_carScript.CancelMovement();
        m_testManager.transform.position = new Vector3(m_spawnPoints[test].position.x, m_testManager.transform.position.y, m_spawnPoints[test].position.z);
        m_testManager.transform.eulerAngles = new Vector3
                                            (m_testManager.transform.eulerAngles.x,
                                            m_spawnPoints[test].eulerAngles.y,
                                            m_testManager.transform.eulerAngles.z);
        

    }
}