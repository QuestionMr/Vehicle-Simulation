using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineCounter : MonoBehaviour
{
    private int m_count = 0;
    // Start is called before the first frame update
    void Start()
    {
        int current = 0;
        for (int i = 0; i < transform.childCount; i++){
            if (transform.GetChild(i).GetComponent<TestLineScript>().m_testNumber != 0) current = transform.GetChild(i).GetComponent<TestLineScript>().m_testNumber;
            else transform.GetChild(i).GetComponent<TestLineScript>().m_testNumber = current;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
