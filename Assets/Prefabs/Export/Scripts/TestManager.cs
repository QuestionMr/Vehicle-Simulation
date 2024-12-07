using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.Events;
enum TestState{
    READY,
    IN_PROGRESS,
    FAIL
}
public enum TimeState{
    NORMAL,
    RUSH,
    WAIT
}
public class TestManager : MonoBehaviour
{
    private int m_currentTest;
    public int m_maxScore;
    public int m_minScore;
    private int m_currentScore;
    private float m_currentTime;

    public bool m_allowStop;
    public int m_startTest;
    public int[] m_testScore;
    private TestState m_testState;
    private TimeState m_timeState;
    private TimeState m_secondaryTimeState;
    private float m_secondaryCurrentTime;
    private float m_secondaryTimeTrack;
    private int m_currentCollideCount;
    private CarScript m_owningCar;

    public TMP_Text m_currentTestNumber;
    public TMP_Text m_currentTestState;
    public TMP_Text m_currentTestScore;
    public TMP_Text m_currentWrongLaneMessage;

    public int m_lineCrossCount;
    public AudioSource m_audioSource;
    public AudioClip[] m_audioClips;
    public AudioClip m_buzzer;
    public AudioClip m_ding;
    private UnityEvent<TimeState> m_timeStateInvoke;
    private int m_currentCheckpoint;
    // Start is called before the first frame update
    void Start()
    {
        m_currentScore = m_maxScore;
        m_currentTest = m_startTest;
        m_currentTime = 0;
        m_owningCar = GetComponent<CarScript>();
        m_timeState = TimeState.NORMAL;
        m_currentCollideCount = 0;
        m_currentTestScore.text = m_currentScore.ToString();
        m_currentTestNumber.text = m_currentTest.ToString();
        m_currentTestState.text = CheckState(m_testState);
        m_lineCrossCount = 0;
        m_allowStop = true;
        m_currentCheckpoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_allowStop && m_owningCar.GetCurrentKMHSpeed() != 0) ChangeScore(m_currentTest, 5); 
        if (m_secondaryTimeState == TimeState.RUSH){
            m_secondaryCurrentTime -= Time.deltaTime;
            //Debug.Log(m_secondaryCurrentTime);

            if (m_secondaryCurrentTime <= 0){
                ChangeScore(m_currentTest, -5);
                StartSecondaryTime(m_secondaryTimeTrack, 0);
            }
        }
        if (m_timeState == TimeState.NORMAL) return;
        m_currentTime -= Time.deltaTime;
        //Debug.Log(m_currentTime);
        if (m_timeState == TimeState.RUSH && m_currentTime <= 0) FailTest();
        else if (m_currentTime <= 0){
            PlaySound(m_ding);
            StartTime(-0.1f, TimeState.NORMAL);            
        }
        Debug.Log(m_currentTestNumber.text);

    }
    private void FailTest(){
        m_testState = TestState.FAIL;
        m_timeState = TimeState.NORMAL;
        m_currentTestState.text = CheckState(m_testState);
        m_lineCrossCount = 0;
    }
    public void StartTest(int test){
        if (m_testState != TestState.READY || test != m_currentTest + 1) return;
                Debug.Log(m_currentTest);
       
        m_audioSource.Play();

        m_testState = TestState.IN_PROGRESS;
        m_currentTest = test;
        m_currentTestNumber.text = m_currentTest.ToString();
        m_currentTestState.text = CheckState(m_testState);
        m_lineCrossCount = 0;
    }

    public void ChangeScore(int test, int scoreDelta){
        if (m_currentTest != test || m_testState == TestState.FAIL) return;
        m_currentScore += scoreDelta;
        Debug.Log("Score reduced by " + scoreDelta);
        if (scoreDelta < 0) PlaySound(m_buzzer);
        m_currentTestScore.text = m_currentScore.ToString();
        if (m_currentScore < m_minScore) FailTest();
    }

    public void EndTest(int test){
        if (m_currentTest != test|| m_testState != TestState.IN_PROGRESS) return;
        m_testState = TestState.READY;
        m_timeState = TimeState.NORMAL;
        m_currentTestNumber.text = m_currentTest.ToString();
        m_currentTestState.text = CheckState(m_testState);
    }

    public void StartTime(float timeLimit, TimeState type){
        //if (m_timeState != TimeState.NORMAL) return;
        m_timeState = type;
        m_currentTime = timeLimit;
        m_timeStateInvoke.Invoke(type);
    }

    public void StartSecondaryTime(float timeLimit, int type){
        //if (m_secondaryTimeState != TimeState.NORMAL) return;
        if (type == 0) m_secondaryTimeState = TimeState.RUSH;
        if (type == 1) m_secondaryTimeState = TimeState.WAIT;
        if (type == 2) m_secondaryTimeState = TimeState.NORMAL;
        m_secondaryTimeTrack = timeLimit;
        m_secondaryCurrentTime = timeLimit;
    }
    public bool IsTimeOver(){
        return ((m_currentTime <= 0 && m_timeState == TimeState.WAIT) || m_timeState != TimeState.WAIT);
    }
    public void ChangeCollideCount(int deltaCount){
        if (m_currentCollideCount == 0) StartSecondaryTime(5, 0);
        m_currentCollideCount += deltaCount;
        Debug.Log(m_currentCollideCount);
        if (m_currentCollideCount == 0) StartSecondaryTime(5, 2);
    }

    private String CheckState(TestState ts){
        if (ts == TestState.READY) return "Ready";
        if (ts == TestState.IN_PROGRESS) return "In Progress";
        return "Failed";
    }

    public void AddLineCross(int delta){
        m_lineCrossCount += delta;
        Debug.Log(m_lineCrossCount);
        if (m_lineCrossCount > 2){
            ChangeScore(m_currentTest, -10);
            m_lineCrossCount = 0;
        }
    }
    public int GetCurrentTest(){
        return m_currentTest;
    }
    public TimeState GetTimeState(){
        return m_timeState;
    }
    public float GetCurrentActiveTime(){
        return m_currentTime;
    }
    public void BindTimeStateInvoke(UnityAction<TimeState> call){
        if (m_timeStateInvoke == null) m_timeStateInvoke = new UnityEvent<TimeState>();
        m_timeStateInvoke.AddListener(call);
    }
    public void SetCurrentCheckpoint(int checkpoint){
        if (m_currentCheckpoint + 1 != checkpoint && m_currentCheckpoint != checkpoint){
            m_currentWrongLaneMessage.gameObject.SetActive(true);
        }
        else {
            m_currentCheckpoint = checkpoint;
            m_currentWrongLaneMessage.gameObject.SetActive(false);
        }
    }
    public void ForceSetCheckpoint(int checkpoint){
        m_currentCheckpoint = checkpoint;
        m_currentWrongLaneMessage.gameObject.SetActive(false);
    }
    public void PlayNextTestSound(){
        if (m_testState == TestState.READY) m_audioSource.PlayOneShot(m_audioClips[m_currentTest], 1);
    }

    public void PlaySound(AudioClip ac){
        m_audioSource.PlayOneShot(ac, 1);
    }
    public void SetStartingTest(int test){
        m_currentTest = test;
        //Debug.Log(m_currentTest);
        m_currentTestNumber.text = m_currentTest.ToString();
        //Debug.Log(m_currentTestNumber.text);

        m_testState = TestState.READY;
        EndTest(test);
        StartTime(0, TimeState.NORMAL);
        StartSecondaryTime(0, 2);
        ChangeScore(test, 100 - m_currentScore);
    }
}
