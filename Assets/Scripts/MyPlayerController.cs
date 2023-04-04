using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾��� ������ ���� ��ũ��Ʈ. Main Camera�� �����Ǵ� ��ũ��Ʈ�̴�.
public class MyPlayerController : MonoBehaviour
{

  
    Camera cam;
    bool pressedBlock = false;

    GameObject firstFruit;
    GameObject secondFruit;

    const int ToFindRowAndCol = 2;
    const float MaxDistance = 1.2f;

    string firstObjectName;
    string secondObjectName;

    Vector2 firstTemp;
    Vector2 secondTemp;


    Blocks blocks;

    //�̵� ���� �� 
    public int move = 15;


   

    void Start()
    {

        cam = GetComponent<Camera>();
        blocks = GetComponent<Blocks>();

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
        if (Input.GetMouseButtonDown(0) && blocks.CanTouch())
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
            if (hit.transform.gameObject.CompareTag("Sword") || hit.transform.gameObject.CompareTag("Shield") || hit.transform.gameObject.CompareTag("Spear") || hit.transform.gameObject.CompareTag("Mana"))
            {
                Debug.Log("����� �����ϼ̽��ϴ�" + hit.transform.gameObject + "pressedFruit " + pressedBlock + "move:  " + move);

                //���ϳ��� �ΰ��� Ŭ���� ���
                if (pressedBlock)
                {

                    ClickFruitTwice(hit);

                }
                //���� �Ѱ��� Ŭ���� ����
                else
                {
                    pressedBlock = true;
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

            firstObjectName = firstFruit.name;
            secondObjectName = secondFruit.name;

            //��� ���� ����
            blocks.ListExchangeByPlayer(firstObjectName, secondObjectName);



        }
        

        pressedBlock = false;
    }
}
   








