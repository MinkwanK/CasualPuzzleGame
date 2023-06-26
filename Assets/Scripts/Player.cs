using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //적과 교전하는 플레이어 스크립트

    GameManager gamemanager;
    Timer timer;

    public Sprite IdleSprite;
    public Sprite hitSprite;
    public Sprite dieSprite;

    enum enumPlayerType
    {
        Peasant = 1,
        Looter,
    }

    
    struct Character
    {
        public int hp;
        public int damage;
        public int defence;

        public Character(int hp, int damage, int defence)
        {
            this.hp = hp;
            this.damage = damage;
            this.defence = defence;
        }
    }

    Character player;

    private enumPlayerType playerType;

    // Start is called before the first frame update
    void Start()
    {

        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        string sceneName = SceneManager.GetActiveScene().name;


        if (sceneName == "Stage1")
        {
            playerType = enumPlayerType.Peasant;
            player = new Character(30, 1, 0);
        }

        else if(sceneName == "Stage2")
        {
            player = new Character(30, 1, 0);
        }

        gamemanager.SetPlayerHealthBar(player.hp);
        gamemanager.SetPlayerHP(player.hp.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //플레이어 체력 차감
    public void DecreasePlayerHP(int attackDamage)
    {
  
        player.hp -= attackDamage;

        if (player.hp <= 0)
        {
            player.hp = 0;

            gamemanager.SetPlayerHP(player.hp.ToString());
            gamemanager.UpdatePlayerHealthBar(player.hp);
            gamemanager.SetLoseText();
            gamemanager.ShowResultPanel();

            //플레이어의 체력이 0이 되면 게임 종료
            timer.SetGameEnd();

            this.GetComponent<SpriteRenderer>().sprite = dieSprite;
        }
        else
        {

            gamemanager.SetPlayerHP(player.hp.ToString());
            gamemanager.UpdatePlayerHealthBar(player.hp);

            this.GetComponent<SpriteRenderer>().sprite = hitSprite;
            Invoke("UpdateSprite", 1.0f);
        }

        
    }

    public void UpdateSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = IdleSprite;
    }

  

}
