using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
    ���� �������� ���� ����� ���� ��ȭ�� ��Ÿ���� ��ũ��Ʈ 
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
    public Sprite dieSprite;

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

    //���� ü���� ��� �Լ�
    public void DecreaseEnemyHP(int attackDamage)
    {
        if (!bGameEnd)
        {
            enemy.hp -= attackDamage;

            if (enemy.hp <= 0)
            {
                gamemanager.SetMonserHP(enemy.hp.ToString());
                gamemanager.UpdateEnemyHealthBar(enemy.hp);
                enemy.hp = 0;
                gamemanager.ShowResultPanel();
                this.GetComponent<SpriteRenderer>().sprite = dieSprite;
                //Ÿ�̸� ������Ű��
                GameObject.Find("Timer").GetComponent<Timer>().SetGameEnd();
            }
            else
            {

                gamemanager.SetMonserHP(enemy.hp.ToString());
                gamemanager.UpdateEnemyHealthBar(enemy.hp);

                this.GetComponent<SpriteRenderer>().sprite = hitSprite;

                Invoke("UpdateSprite", 1.0f);
            }
        }
    }

    public void UpdateSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = IdleSprite;
    }


    //���� �������� ��ȯ�ϴ� �Լ�
    public int CheckEnemyDamage()
    {
        return enemy.damage;
    }

    //���� ���� ������ true�� �ٲ��ִ� ����
    public void SetGameEnd()
    {
        bGameEnd = true;
    }



   
}
