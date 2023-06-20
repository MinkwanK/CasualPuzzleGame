using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 조작을 위한 스크립트. Main Camera에 부착되는 스크립트이다.
public class MyPlayerController : MonoBehaviour
{

  
    Camera cam;
    bool pressedBlock = false;

    GameObject firstBlock;
    GameObject secondFruit; 

    const int ToFindRowAndCol = 2;
    const float MaxDistance = 1.2f;

    string firstObjectName;
    string secondObjectName;

    Vector2 firstTemp;
    Vector2 secondTemp;


    Blocks blocks;

    //이동 가능 수 
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

    //과일을 터치할 의 조작
    void getFruiteInput()
    {

       //GetMouseInput();
        GetMobileInput();

        
    }

    //마우스 입력 받기
    void GetMouseInput()
    {
        //블록 이동 중, 매칭 중, 드랍 중이 아닐때만 입력이 가능
        if (Input.GetMouseButtonDown(0) && blocks.CanTouch())
        {
            RaycastHit2D hit = ShootRay(Input.mousePosition);
            PressBlock(hit);
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
                //첫 블록은 터치로 선택
                if (!pressedBlock)
                {
                    Debug.Log("터치");
                    RaycastHit2D hit = ShootRay(touch.position);
                    PressBlock(hit);
                }
            }
            //드래그
            else if(touch.phase == TouchPhase.Moved && move > 0 )
            {
                if (pressedBlock)
                {
                    Debug.Log("드래그");
                    RaycastHit2D hit = ShootRay(touch.position);

                    if(hit.transform.gameObject != firstBlock)
                    {
                        PressBlock(hit);
                    }
                }
            }

        }

               
    }

    //과일 선택 함수
    void PressBlock(RaycastHit2D hit)
    {
        if (hit.transform != null)
        {
            if (hit.transform.gameObject.CompareTag("Cat") || hit.transform.gameObject.CompareTag("Slime") || hit.transform.gameObject.CompareTag("Skeleton") || hit.transform.gameObject.CompareTag("Devil"))
            {
                Debug.Log("블록을 선택하셨습니다" + hit.transform.gameObject + "pressedFruit " + pressedBlock + "move:  " + move);

                //지금까지 두개의 블록이 선택된 경우
                if (pressedBlock)
                {
                    ClickBlockTwice(hit);
                }
                //지금까지 하나의 블록을 선택한 경우
                else
                {
                    pressedBlock = true;
                    firstBlock = hit.transform.gameObject;

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

    void ClickBlockTwice(RaycastHit2D hit)
    {
        //거리 및 서로의 대각선 위치 판단

        Debug.Log(Vector2.Distance(firstBlock.transform.localPosition, hit.transform.localPosition));

        if ((Vector2.Distance(firstBlock.transform.localPosition, hit.transform.localPosition) < MaxDistance))
        {
            Debug.Log("과일 두개 클릭한 상태");



            secondFruit = hit.transform.gameObject;

            firstObjectName = firstBlock.name;
            secondObjectName = secondFruit.name;

            //블록 로직 수행
            blocks.ListExchangeByPlayer(firstObjectName, secondObjectName);



        }
        

        pressedBlock = false;
    }
}
   








