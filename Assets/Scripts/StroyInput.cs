using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StroyInput : MonoBehaviour
{

    int cnt = 0;
    GameObject[] longSwords;
    GameManager gamemanager;
    // Start is called before the first frame update
    void Start()
    {
        longSwords = new GameObject[4];

        for (int i=0;i<4;i++)
        {
            //longSwords[i] = new GameObject();
            longSwords[i] = GameObject.Find("Long_Sword" + i);
            longSwords[i].GetComponent<SpriteRenderer>().enabled = false;
        }

        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (cnt)
            {
                case 0:
                    gamemanager.UpdatePrologueText("더 이상 참을 수 없던 몬스터들은 고블린A를 찾아갔어요!");
                    cnt++;
                    break;
                case 1:
                    gamemanager.UpdatePrologueText("숲 속의 흔한 고블린A는 상처입은 몬스터들의 모습을 보고 마음이 아팠어요.");
                    cnt++;
                    break;
                case 2:
                    gamemanager.UpdatePrologueText("고블린A는 다짐했어요. 다시는 플레이어들이 소중한 친구들을 괴롭히지 못하게 만들 것이라고요!");
                    cnt++;
                    break;
                case 3:
                    gamemanager.UpdatePrologueText("고블린A를 중심으로 몬스터들의 반란은 시작됐습니다!");
                    cnt++;
                    break;
                case 4:
                    UpdatePrologueCharactersSprite();
                    cnt++;
                    break;
                case 5:
                    SceneManager.LoadScene("StageScene");
                    break;

            }
        }
    }

    public void UpdatePrologueCharactersSprite()
    {

        for (int i = 0; i < 4; i++)
        {
            longSwords[i].GetComponent<SpriteRenderer>().enabled = true;
        }
        Sprite goblinSprite = Resources.Load<Sprite>("Sprite/GoblinA");
        GameObject.Find("Goblin_Flower").GetComponent<SpriteRenderer>().sprite = goblinSprite;

        Sprite slimeSprite = Resources.Load<Sprite>("Images/Weapon/Slime_32");
        GameObject.Find("Slime_32_hurt").GetComponent<SpriteRenderer>().sprite = slimeSprite;

        Sprite demonSprite = Resources.Load<Sprite>("Images/Weapon/Devil_32");
        GameObject.Find("Devil_32_hurt").GetComponent<SpriteRenderer>().sprite = demonSprite;

        Sprite skeletonSprite = Resources.Load<Sprite>("Images/Weapon/Skeleton_32");
        GameObject.Find("Skeleton_cry").GetComponent<SpriteRenderer>().sprite = skeletonSprite;

        Sprite catSprite = Resources.Load<Sprite>("Images/Weapon/Cat32");
        GameObject.Find("Cat_Cry").GetComponent<SpriteRenderer>().sprite = catSprite;




    }
}
