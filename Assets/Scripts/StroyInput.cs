using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//게임의 스토리 진행을 위한 스토리 스크립트
public class StroyInput : MonoBehaviour
{

    int cnt = 0;
    GameManager gamemanager;

    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

   
    void Update()
    {
        //터치를 통해 스토리 진행
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                switch (cnt)
                {
                    case 0:
                        gamemanager.UpdatePrologueText("모험가들의 경험치를 위해 몬스터들은 매일을 눈물로 보냈어요...");
                        cnt++;
                        break;
                    case 1:
                        gamemanager.UpdatePrologueText("고블린A는 친구들을 위해 복수를 다짐했어요!");
                        cnt++;
                        break;
                    case 2:
                        gamemanager.UpdatePrologueText("오늘부터 친구들은 누구도 괴롭히지 못해요!");
                        cnt++;
                        break;
                    case 3:
                        UpdatePrologueCharactersSprite();
                        cnt++;
                        break;
                    case 4:
                        SceneManager.LoadScene("StageScene");
                        break;

                }
            }
        }
    }

    public void UpdatePrologueCharactersSprite()
    {

        Sprite goblinSprite = Resources.Load<Sprite>("Sprite/GoblinA");
        GameObject.Find("Goblin_Flower").GetComponent<SpriteRenderer>().sprite = goblinSprite;

        Sprite slimeSprite = Resources.Load<Sprite>("Images/Weapon/Slime_32");
        GameObject.Find("Slime_32_hurt").GetComponent<SpriteRenderer>().sprite = slimeSprite;

        Sprite catSprite = Resources.Load<Sprite>("Images/Weapon/Cat32");
        GameObject.Find("Cat_Cry").GetComponent<SpriteRenderer>().sprite = catSprite;

    }
}
