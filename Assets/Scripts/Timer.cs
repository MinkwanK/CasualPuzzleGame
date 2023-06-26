using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    /*
    적의 공격을 위한 타이머 스크립트
     
     */



    float timer = 6.0f;
    const float minTime = 0.0f;

    Effect effect;
    Enemy enemy;
    Player player;
    GameManager gamemanager;

    bool bGameEnd = false;

    void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        effect = GameObject.Find("Main Camera").GetComponent<Effect>();
        player = GameObject.Find("Player").GetComponent<Player>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

      
    }

    // Update is called once per frame
    void Update()
    {
        if (!bGameEnd)
        {
            timer -= Time.deltaTime;
            gamemanager.UpdateTimerTxt(((int)timer).ToString());

            if (timer <= minTime)
            {
                player.DecreasePlayerHP(enemy.CheckEnemyDamage());
                timer = 6.0f;

                effect.SmashSoundByEnemy();
                gamemanager.ShowEnemySmashEffect();
            }
        }
        //플레이어의 체력이 0이 된다 -> 타이머 종료 -> 적 공격 종료
        else
        {
            enemy.SetGameEnd();
        }


    }

    public void SetGameEnd()
    {
        bGameEnd = true;
    }

   

}
