using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
    전투 씬에서의 적의 모습과 상태 변화를 나타내는 스크립트 
 */
public class Enemy : MonoBehaviour
{
    
    enum enumEnemyType
    {
        peasant = 1,
        manAtArms,
    }

    struct Character
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
    Character enemy;
    bool bGameEnd = false;

    public Sprite IdleSprite;
    public Sprite hitSprite;

    private enumEnemyType enemyType;


    
    void Start()
    {

        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        string sceneName = SceneManager.GetActiveScene().name;
        
        if(sceneName == "Stage1")
        {
            enemyType = enumEnemyType.peasant;

            enemy = new Character(50,5,0);
   
        }

        else if (sceneName == "Stage2")
        {
            enemy = new Character(40, 8, 0);
        }

        gamemanager.SetEnemyHealthBar(enemy.hp);
        gamemanager.SetMonserHP(enemy.hp.ToString());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //적의 체력을 깎는 함수
    public void DecreaseEnemyHP(int attackDamage)
    {
        if (!bGameEnd)
        {
            enemy.hp -= attackDamage;

            if(enemy.hp <=0)
            {
                enemy.hp = 0;
                gamemanager.showWin();

                //타이머 중지시키기
                GameObject.Find("Timer").GetComponent<Timer>().SetGameEnd();
            }

            gamemanager.SetMonserHP(enemy.hp.ToString());
            gamemanager.UpdateEnemyHealthBar(enemy.hp);

            this.GetComponent<SpriteRenderer>().sprite = hitSprite;

            Invoke("UpdateSprite", 1.0f);
        }
    }

    public void UpdateSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = IdleSprite;
    }


    //적의 데미지를 반환하는 함수
    public int CheckEnemyDamage()
    {
        return enemy.damage;
    }

    //게임 종료 변수를 true로 바꿔주는 변수
    public void SetGameEnd()
    {
        bGameEnd = true;
    }



   
}
