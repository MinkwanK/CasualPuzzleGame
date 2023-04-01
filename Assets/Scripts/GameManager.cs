using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//게임의 UI를 관리하는 스크립트입니다. 버튼 효과음을 출력합니다.

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    MusicPlayer musicPlayer;

    public Text moveTxt;
    public Text goalTxt;
    public Text winTxt;
    
    public Button StartBtn;
    public Button ExitBtn;
    public Button Stage1Btn;
    public Button Stage2Btn;
    public Button Stage_PreviousBtn;
    public Button RestartBtn;
    public Button EndBtn;
    public Button MenuBtn;
    public Button WinBtn;
    public GameObject winPanel;
    public GameObject menuPanel;

    void Awake()
    {
        //Screen.SetResolution(1080, 1920, true);
    }
    
    void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        

      

        if (GameObject.Find("MusicPlayer") != null)
        musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoveTxt(string move)
    {
        moveTxt.text = move;
    }
    public void SetGoalTxt(string goal)
    {
        goalTxt.text = goal;
    }

    public void OnClickStartBtn()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("StageScene");
    }
    
    public void OnClickExitBtn()
    {
        musicPlayer.PlayBtnSound();
        Application.Quit();
    }

    public void OnClickStage1()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("Stage1");
    }
    public void OnClickStage2()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("Stage2");
    }

    public void OnClickPreviousBtnInStage()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("MainScene");
    }

    public void OnContinueBtn()
    {
        musicPlayer.PlayBtnSound();
        HideMenu();
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
    }

    public void showWin()
    {
        winPanel.SetActive(true);
    }

    public void hideWIn()
    {
        winPanel.SetActive(false);
    }

    public void LoseTxt()
    {
        winTxt.text = "You Lose";
    }


}
