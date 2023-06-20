using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    /*
     Enemy�� ������ �ð����� ������ �� �ֵ��� ��
     
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
