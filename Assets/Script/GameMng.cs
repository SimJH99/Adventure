using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMng : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI UIPoint;
    [SerializeField] TextMeshProUGUI UIStage;
    [SerializeField] TextMeshProUGUI Text_Timer;
    [SerializeField] GameObject UIRestartBtn;

    public Player player;
    public GameObject[] StartPoints;
    public GameObject[] Stages;

    public int totalPoint;
    public int stagePoint;
    public int deathPoint;
    public int killPoint;
    public int stageIndex = 0;
    public int startIndex = 0;

    private float timeCurrent;

    private void Start()
    {
        Reset_Timer();
    }

    private void Update()
    {
        UI();
        Check_Timer();
        GameQuit();
    }

    //���� �������� �Ѿ��
    public void NextStage()
    {
        if (stageIndex < Stages.Length + 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }

        if (Stages[stageIndex] == Stages[3])
        {
            player.gameObject.SetActive(false);
            Time.timeScale = 0;
            ViewBtn();
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    //�������� �Ѿ�� ��ŸƮ���� ����
    void PlayerReposition()
    {
        if (startIndex < StartPoints.Length)
        {
            startIndex++;
            player.transform.position = StartPoints[startIndex].transform.position;
        }
        else if (startIndex > 2)
        {
            startIndex = 0;
        }
    }

    void UI()
    {
        //Score
        UIPoint.text = $"Score : {totalPoint + stagePoint + deathPoint + killPoint}";
        //Stage
        UIStage.text = "Stage " + (stageIndex + 1);
        if (stageIndex >= 3)
        {
            UIStage.text = "Stage Clear!";
        }
    }

    //���� Ŭ����� ��ư Ȱ��ȭ
    private void ViewBtn()
    {
        UIRestartBtn.SetActive(true);
    }

    //���� �����
    public void Restart()
    {
        Reset_Timer();
        SceneManager.LoadScene(0);
        totalPoint= 0;
        stageIndex= 0;
        startIndex= 0;
    }

    //���� �ð� ���
    private void Check_Timer()
    {
        timeCurrent += Time.deltaTime;

        Text_Timer.text = string.Format("Time : {0:N2}", timeCurrent);
    }

    //�ð� �ʱ�ȭ
    private void Reset_Timer()
    {
        timeCurrent = 0;
        Time.timeScale = 1;
    }

    void GameQuit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
