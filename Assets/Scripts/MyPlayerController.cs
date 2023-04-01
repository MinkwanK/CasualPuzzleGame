using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 조작을 위한 스크립트. Main Camera에 부착되는 스크립트이다.
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

    //이동 가능 수 
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

    //과일을 터치할 떄의 조작
    void getFruiteInput()
    {

       GetMouseInput();
       // GetMobileInput();

        
    }

     //마우스 입력 받기
    void GetMouseInput()
    {
        //블록 이동 중, 매칭 중, 드랍 중이 아닐때만 입력이 가능
        if (Input.GetMouseButtonDown(0) && fruit.CanTouch())
        {
            Debug.Log("마우스 입력");
            RaycastHit2D hit = ShootRay(Input.mousePosition);
            PressFruit(hit);

          
        }
    }

    //터치 입력 받기
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

    //과일 선택 함수
    void PressFruit(RaycastHit2D hit)
    {
        if (hit.transform != null)
        {
            if (hit.transform.gameObject.CompareTag("Apple") || hit.transform.gameObject.CompareTag("Banana") || hit.transform.gameObject.CompareTag("Pear") || hit.transform.gameObject.CompareTag("Grape"))
            {
                Debug.Log("과일을 선택하셨습니다" + hit.transform.gameObject + "pressedFruit " + pressedFruit + "move:  " + move);

                //과일끼리 두개가 클릭된 경우
                if (pressedFruit)
                {

                    ClickFruitTwice(hit);

                }
                //과일 한개를 클릭한 상태
                else
                {
                    pressedFruit = true;
                    firstFruit = hit.transform.gameObject;

                }

            }

        }
    }
    //Ray 발사 함수
    RaycastHit2D ShootRay(Vector2 touchPos)
    {
        Ray ray = cam.ScreenPointToRay(touchPos);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 60.0f);

        return hit;
    }

    
    //선택된 과일 두개의 거리가 교환 가능한 거리인지 판단하고 매칭 로직 시작
    void ClickFruitTwice(RaycastHit2D hit)
    {
        //거리 및 서로의 대각선 위치 판단

        Debug.Log(Vector2.Distance(firstFruit.transform.localPosition, hit.transform.localPosition));

        if ((Vector2.Distance(firstFruit.transform.localPosition, hit.transform.localPosition) < MaxDistance))
        {
            Debug.Log("과일 두개 클릭한 상태");

           

            secondFruit = hit.transform.gameObject;

            firstFruitName = firstFruit.name;
            secondFruitName = secondFruit.name;

            //이동 감소, UI 표시
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
   








