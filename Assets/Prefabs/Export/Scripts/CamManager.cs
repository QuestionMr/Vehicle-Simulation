using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public Camera m_thirdPersonCamera;
    public Camera m_firstPersonCamera;
    public bool m_isThirdPerson;
    // Start is called before the first frame update
    void Start()
    {
         PerspectiveCheck();
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.F)) {
            m_isThirdPerson = !m_isThirdPerson;
            PerspectiveCheck();
        }
    }
    private void PerspectiveCheck(){
        m_thirdPersonCamera.enabled = m_isThirdPerson;
        m_firstPersonCamera.enabled = !m_isThirdPerson;
    }
}
