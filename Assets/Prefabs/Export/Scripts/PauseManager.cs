using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject m_pauseMenu;
    public GameObject m_screenMenu;
    public SpawnScript m_spawnScript;
    public TMP_Dropdown m_dropDown;
    public CarScript m_carScript;
    public Slider m_slider;
    public AudioSource m_audioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_spawnScript.SetStartingTest(0);

    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.B)) {
            SetPause();
        }
    }
    public void SetPause(){
        if (m_screenMenu.activeSelf) {
            Time.timeScale = 0;
            m_carScript.m_engineSource.Pause();
            m_audioSource.Stop();
        }
        else {
            Time.timeScale = 1;
            m_carScript.m_engineSource.Play();
        }
        
        m_pauseMenu.SetActive(m_screenMenu.activeSelf);
        m_screenMenu.SetActive(!m_screenMenu.activeSelf);
    }

    public void SetSpawn(){
        SetPause();
        int temp = m_dropDown.value;
        if (temp >= 7) temp++;
        m_spawnScript.SetStartingTest(temp);
    }

    public void SetThrottle(){
        m_carScript.m_throttle = m_slider.value;
    }
}
