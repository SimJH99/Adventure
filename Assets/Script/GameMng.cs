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

    //다음 스테이지 넘어가기
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

    //스테이지 넘어갈때 스타트지점 설정
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

    //게임 클리어시 버튼 활성화
    private void ViewBtn()
    {
        UIRestartBtn.SetActive(true);
    }

    //게임 재시작
    public void Restart()
    {
        Reset_Timer();
        SceneManager.LoadScene(0);
        totalPoint= 0;
        stageIndex= 0;
        startIndex= 0;
    }

    //게임 시간 출력
    private void Check_Timer()
    {
        timeCurrent += Time.deltaTime;

        Text_Timer.text = string.Format("Time : {0:N2}", timeCurrent);
    }

    //시간 초기화
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
