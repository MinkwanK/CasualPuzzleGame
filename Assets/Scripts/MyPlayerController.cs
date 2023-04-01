using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾��� ������ ���� ��ũ��Ʈ. Main Camera�� �����Ǵ� ��ũ��Ʈ�̴�.
public class MyPlayerController : MonoBehaviour
{

    GameManager gamemanager;
    Camera cam;
    bool pressedFruit = false;

    GameObject firstFruit;
    GameObject secondFruit;

    const int ToFindRowAndCol = 2;
    const float MaxDistance = 1.2f;

    string firstFruitName;
    string secondFruitName;

    Vector2 firstTemp;
    Vector2 secondTemp;


    Fruit fruit;

    //�̵� ���� �� 
    public int move = 15;

    void Start()
    {

        cam = GetComponent<Camera>();
        fruit = GetComponent<Fruit>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gamemanager.SetMoveTxt(move.ToString());
        

    }


    void Update()
    {
        getFruiteInput();
    }

    //������ ��ġ�� ���� ����
    void getFruiteInput()
    {

       GetMouseInput();
       // GetMobileInput();

        
    }

     //���콺 �Է� �ޱ�
    void GetMouseInput()
    {
        //��� �̵� ��, ��Ī ��, ��� ���� �ƴҶ��� �Է��� ����
        if (Input.GetMouseButtonDown(0) && fruit.CanTouch())
        {
            Debug.Log("���콺 �Է�");
            RaycastHit2D hit = ShootRay(Input.mousePosition);
            PressFruit(hit);

          
        }
    }

    //��ġ �Է� �ޱ�
    void GetMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && move > 0)
            {

                RaycastHit2D hit = ShootRay(touch.position);
                PressFruit(hit);
            }

        }

               
    }

    //���� ���� �Լ�
    void PressFruit(RaycastHit2D hit)
    {
        if (hit.transform != null)
        {
            if (hit.transform.gameObject.CompareTag("Apple") || hit.transform.gameObject.CompareTag("Banana") || hit.transform.gameObject.CompareTag("Pear") || hit.transform.gameObject.CompareTag("Grape"))
            {
                Debug.Log("������ �����ϼ̽��ϴ�" + hit.transform.gameObject + "pressedFruit " + pressedFruit + "move:  " + move);

                //���ϳ��� �ΰ��� Ŭ���� ���
                if (pressedFruit)
                {

                    ClickFruitTwice(hit);

                }
                //���� �Ѱ��� Ŭ���� ����
                else
                {
                    pressedFruit = true;
                    firstFruit = hit.transform.gameObject;

                }

            }

        }
    }
    //Ray �߻� �Լ�
    RaycastHit2D ShootRay(Vector2 touchPos)
    {
        Ray ray = cam.ScreenPointToRay(touchPos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 60.0f);

        return hit;
    }

    
    //���õ� ���� �ΰ��� �Ÿ��� ��ȯ ������ �Ÿ����� �Ǵ��ϰ� ��Ī ���� ����
    void ClickFruitTwice(RaycastHit2D hit)
    {
        //�Ÿ� �� ������ �밢�� ��ġ �Ǵ�

        Debug.Log(Vector2.Distance(firstFruit.transform.localPosition, hit.transform.localPosition));

        if ((Vector2.Distance(firstFruit.transform.localPosition, hit.transform.localPosition) < MaxDistance))
        {
            Debug.Log("���� �ΰ� Ŭ���� ����");

           

            secondFruit = hit.transform.gameObject;

            firstFruitName = firstFruit.name;
            secondFruitName = secondFruit.name;

            //�̵� ����, UI ǥ��
            move--;
            gamemanager.SetMoveTxt(move.ToString());

            if(move == 0)
            {
                gamemanager.SetMoveTxt(move.ToString());
                gamemanager.LoseTxt();
                gamemanager.showWin();
 
            }
            fruit.ListExchangeByPlayer(firstFruitName, secondFruitName);



        }
        

        pressedFruit = false;
    }
}
   








