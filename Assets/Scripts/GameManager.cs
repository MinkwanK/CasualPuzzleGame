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

    public Text playerHP;
    public Text monsterHP;
    public Text winTxt;
    public Text timerTxt;
    public Text PrologueText_PrologueScene;

    public Image PlayerSmashEffect;
    public Image EnemySmashEffect;

    public Slider playerHP_Slider_InGame;
    public Slider enemyHP_Slider_InGame;

    public Button StartBtn_MainMenu;
    public Button ExitBtn_MainMenu;
    public Button Stage1Btn;
    public Button Stage2Btn;
    public Button PreviousBtn_StageScene;
    public Button ContinueBtn_InGame;
    public Button EndBtn_InGame;
    public Button MenuBtn_InGame;
    public Button WinBtn_InGame;


    public GameObject WinPanel_InGame;
    public GameObject MenuPanel_InGame;

    void Awake()
    {
        //Screen.SetResolution(1080, 1920, true);
    }
    
    void Start()
    {
        if (MenuPanel_InGame != null)
            MenuPanel_InGame.SetActive(false);

        if (WinPanel_InGame != null)
            WinPanel_InGame.SetActive(false);

        if (PlayerSmashEffect != null)
            PlayerSmashEffect.enabled = false;

        if (EnemySmashEffect != null)
            EnemySmashEffect.enabled = false;

        if (GameObject.Find("MusicPlayer") != null)
        musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();

        if (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "StageScene")
        {
            musicPlayer.UpdateMusic(MusicPlayer.EnumMusicList.NiaViliage);
        }
        else if(SceneManager.GetActiveScene().name == "PrologueScene")
        {
            musicPlayer.UpdateMusic(MusicPlayer.EnumMusicList.Mokoko);
        }
        else
            musicPlayer.UpdateMusic(MusicPlayer.EnumMusicList.Liebenheim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //체력바 관리 함수
    
    public void SetPlayerHealthBar(float maxHealth)
    {
        playerHP_Slider_InGame.minValue = 0f;
        playerHP_Slider_InGame.maxValue = maxHealth;
        playerHP_Slider_InGame.value = maxHealth;
    }

    public void SetEnemyHealthBar(float maxHealth)
    {
        enemyHP_Slider_InGame.minValue = 0f;
        enemyHP_Slider_InGame.maxValue = maxHealth;
        enemyHP_Slider_InGame.value = maxHealth;
    }

    public void UpdatePlayerHealthBar(float value)
    {
        playerHP_Slider_InGame.value = value;
    }

    public void UpdateEnemyHealthBar(float value)
    {
        enemyHP_Slider_InGame.value = value;
    }

    
    public void UpdateTimerTxt(string str)
    {
        timerTxt.text = str;
    }

    public void UpdatePrologueText(string str)
    {
        PrologueText_PrologueScene.text = str;
    }

    public void SetPlayerHP(string txt)
    {
        playerHP.text = txt;
    }

    public void SetMonserHP(string txt)
    {
        monsterHP.text = txt;
    }

    public void OnClickExitBtn()
    {
        musicPlayer.PlayBtnSound();
        Application.Quit();
    }

    public void MoveToStage1Scene()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("Stage1");
    }
    public void MoveToStage2Scene()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("Stage2");
    }

    public void MoveToMainScene()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("MainScene");
    }

    public void MoveToPrologueScene()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("PrologueScene");
    }


    public void MoveToStageScene()
    {
        musicPlayer.PlayBtnSound();
        SceneManager.LoadScene("StageScene");
    }
    public void OnContinueBtn()
    {
        musicPlayer.PlayBtnSound();
        HideMenu();
    }

    public void ShowMenu()
    {
        MenuPanel_InGame.SetActive(true);
    }

    public void HideMenu()
    {
        MenuPanel_InGame.SetActive(false);
    }

    public void showWin()
    {
        WinPanel_InGame.SetActive(true);
    }

    public void hideWIn()
    {
        WinPanel_InGame.SetActive(false);
    }

    public void SetLoseText()
    {
        winTxt.text = "Lose";
    }


    public void ShowPlayerSmashEffect()
    {
        PlayerSmashEffect.enabled = true;
        Invoke("HidePlayerSmashEffect", 1.0f);
    }

    public void HidePlayerSmashEffect()
    {
        PlayerSmashEffect.enabled = false;
    }

    public void ShowEnemySmashEffect()
    {
        EnemySmashEffect.enabled = true;
        Invoke("HideEnemySmashEffect", 1.0f);
    }

    public void HideEnemySmashEffect()
    {
        EnemySmashEffect.enabled = false;
    }

}
