using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{   
    public float m_xSensitivity;
    public float m_ySensitivity;
    public float m_yDis;
    public Transform m_target;
    private Vector3 m_tempPos;

    void Start()
    {
        m_tempPos = m_target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += m_target.position - m_tempPos;
        m_tempPos = m_target.position;
        float xMove = Input.GetAxis("Mouse X") * m_xSensitivity;
        float yMove = Input.GetAxis("Mouse Y") * m_ySensitivity;

        float maxXMove = transform.localEulerAngles.x + yMove;
        if (maxXMove > 180) {
            maxXMove -= 360;
        }
        maxXMove = Mathf.Clamp(maxXMove, -90, 90);
        transform.RotateAround(m_target.transform.position, Vector3.up, xMove);
        transform.RotateAround(m_target.transform.position, transform.right, maxXMove - transform.localEulerAngles.x);

        // transform.localEulerAngles = new Vector3(maxXMove,transform.localEulerAngles.y + xMove, transform.localEulerAngles.z);
        // float offX = Mathf.Cos(transform.
        // transform.position = m_target.position + new Vector3(transform.localEulerAngles)
        // float temp = transform.position.y;
        // transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);
        // transform.position = new Vector3(transform.position.x, temp, transform.position.z);
    } 
}
