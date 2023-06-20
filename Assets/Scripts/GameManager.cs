using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//게임의 UI를 관리하는 스크립트입니다. 

public class GameManager : MonoBehaviour
{
    
    //게임의 음악 재생용 뮤직 플레이어
    MusicPlayer musicPlayer;

    public Text playerHP;
    public Text monsterHP;
    public Text ResultText;
    public Text timerTxt;
    public Text StroyText;

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
    public Button PauseBtn_InGame;
    public Button ResultBtn_InGame;


    public GameObject ResultPanel_InGame;
    public GameObject PausePanel_InGame;

    void Awake()
    {
        //Screen.SetResolution(1080, 1920, true);
    }
    
    void Start()
    {
        //메뉴 패널과 결과 화면 패널은 게임 시작 시, 비활성화 시킨다.
        if (PausePanel_InGame != null)
            PausePanel_InGame.SetActive(false);

        if (ResultPanel_InGame != null)
            ResultPanel_InGame.SetActive(false);

        //수정 예정.
        if (PlayerSmashEffect != null)
            PlayerSmashEffect.enabled = false;

        if (EnemySmashEffect != null)
            EnemySmashEffect.enabled = false;

        //각 씬의 이름에 따라 다른 노래를 재생한다.

        musicPlayer = GetComponent<MusicPlayer>();

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


    //체력바 초기화 함수
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

    //체력바 업데이트 함수
    public void UpdatePlayerHealthBar(float value)
    {
        playerHP_Slider_InGame.value = value;
    }

    public void UpdateEnemyHealthBar(float value)
    {
        enemyHP_Slider_InGame.value = value;
    }

    //적의 공격 타이머 업데이트 함수
    public void UpdateTimerTxt(string str)
    {
        timerTxt.text = str;
    }

    //프롤로그 텍스트 업데이트 함수
    public void UpdatePrologueText(string str)
    {
        StroyText.text = str;
    }

    //플레이어, 적 HP Text Update 함수
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

    //Scene 이동 함수
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
        HidePauseMenu();
    }

    public void ShowPauseMenu()
    {
        PausePanel_InGame.SetActive(true);
    }

    public void HidePauseMenu()
    {
        PausePanel_InGame.SetActive(false);
    }

    public void ShowResultPanel()
    {
        ResultPanel_InGame.SetActive(true);
    }

    public void HideResultPanel()
    {
        ResultPanel_InGame.SetActive(false);
    }

    public void SetLoseText()
    {
        ResultText.text = "패배...";
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
