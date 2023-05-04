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
                    gamemanager.UpdatePrologueText("�� �̻� ���� �� ���� ���͵��� ���A�� ã�ư����!");
                    cnt++;
                    break;
                case 1:
                    gamemanager.UpdatePrologueText("�� ���� ���� ���A�� ��ó���� ���͵��� ����� ���� ������ ���;��.");
                    cnt++;
                    break;
                case 2:
                    gamemanager.UpdatePrologueText("���A�� �����߾��. �ٽô� �÷��̾���� ������ ģ������ �������� ���ϰ� ���� ���̶���!");
                    cnt++;
                    break;
                case 3:
                    gamemanager.UpdatePrologueText("���A�� �߽����� ���͵��� �ݶ��� ���۵ƽ��ϴ�!");
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
