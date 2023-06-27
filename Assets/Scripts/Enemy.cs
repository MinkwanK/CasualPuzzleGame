using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
    플레이어와 교전하는 적 스크립트
 */
public class Enemy : MonoBehaviour
{
    
    enum enumEnemyType
    {
        peasant = 1,
        manAtArms,
    }

    public struct Character
    {
        public int hp;
        public int damage;
        public int defence;

        public Character(int hp,int damage, int defence)
        {
            this.hp = hp;
            this.damage = damage;
            this.defence = defence;
        }
    }


    GameManager gamemanager;
    public Character enemyStatus;
    bool bGameEnd = false;

    public Sprite IdleSprite;
    public Sprite hitSprite;
    public Sprite dieSprite;

    private enumEnemyType enemyType;


    
    void Start()
    {

        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        string sceneName = SceneManager.GetActiveScene().name;
        
        if(sceneName == "Stage1")
        {
            enemyType = enumEnemyType.peasant;

            enemyStatus = new Character(50,5,0);
   
        }

        else if (sceneName == "Stage2")
        {
            enemyStatus = new Character(40, 8, 0);
        }

        gamemanager.SetEnemyHealthBar(enemyStatus.hp);
        gamemanager.SetMonserHP(enemyStatus.hp.ToString());


    }

    
    void Update()
    {
        
    }

   //적의 체력차감
    public void DecreaseEnemyHP(int attackDamage)
    {
        if (!bGameEnd)
        {
            enemyStatus.hp -= attackDamage;

            if (enemyStatus.hp <= 0)
            {
                enemyStatus.hp = 0;
                gamemanager.SetMonserHP(enemyStatus.hp.ToString());
                gamemanager.UpdateEnemyHealthBar(enemyStatus.hp);
                gamemanager.ShowResultPanel();
                this.GetComponent<SpriteRenderer>().sprite = dieSprite;

                //체력이 0이면 타이머 종료
                GameObject.Find("Timer").GetComponent<Timer>().SetGameEnd();
            }
            else
            {

                gamemanager.SetMonserHP(enemyStatus.hp.ToString());
                gamemanager.UpdateEnemyHealthBar(enemyStatus.hp);

                this.GetComponent<SpriteRenderer>().sprite = hitSprite;

                Invoke("UpdateSprite", 1.0f);
            }
        }
    }

    public void UpdateSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = IdleSprite;
    }


    //적의 데미지 표시
    public int CheckEnemyDamage()
    {
        return enemyStatus.damage;
    }

    //적의 체력이 0이면 게임 종료
    public void SetGameEnd()
    {
        bGameEnd = true;
    }



   
}
