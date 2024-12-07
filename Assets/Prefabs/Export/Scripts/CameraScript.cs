using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float m_xSensitivity;
    public float m_ySensitivity;
    public float m_downOffset;
    public float m_leftOffset;
    private Vector3 m_ogPos;
    private Vector3 m_newPos;

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxis("Mouse X") * m_xSensitivity;
        float yMove = Input.GetAxis("Mouse Y") * m_ySensitivity;
        
        if (Input.GetKeyDown(KeyCode.G)) transform.localPosition -= new Vector3(m_leftOffset, m_downOffset, 0);
        if (Input.GetKeyUp(KeyCode.G)) transform.localPosition += new Vector3(m_leftOffset, m_downOffset, 0);
       
        float maxXMove = transform.localEulerAngles.x + yMove;
        if (maxXMove > 180) {
            maxXMove -= 360;
        }
        maxXMove = Mathf.Clamp(maxXMove, -90, 90);
        transform.localEulerAngles = new Vector3(maxXMove,transform.localEulerAngles.y + xMove, transform.localEulerAngles.z);
    }


}
