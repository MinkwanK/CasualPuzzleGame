using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 조작을 위한 스크립트. Main Camera에 부착되는 스크립트이다.
public class MyPlayerController : MonoBehaviour
{


    Camera cam;
    bool pressedBlock = false;

    GameObject _firstBlock;
    GameObject _secondBlock;

    BlockInfo _firstBlockInfo;
    BlockInfo _secondBlockInfo;

    Blocks blocks;



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

    //마우스 입력 받기 (디버기용)
    void GetMouseInput()
    {
        //블록 이동 중, 매칭 중, 드랍 중이 아닐때만 입력이 가능
        if (Input.GetMouseButtonDown(0))
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

            if (touch.phase == TouchPhase.Began)
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
            else if (touch.phase == TouchPhase.Moved)
            {
                if (pressedBlock)
                {
                    Debug.Log("드래그");
                    RaycastHit2D hit = ShootRay(touch.position);

                    if (hit.transform.gameObject != _firstBlock)
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
                Debug.Log("블록을 선택하셨습니다" + hit.transform.gameObject + "pressedFruit " + pressedBlock );

                //지금까지 두개의 블록이 선택된 경우
                if (pressedBlock)
                {
                    _secondBlock = hit.transform.gameObject;
                    FindLocationInLinkedList();
                    ClickBlockTwice(hit);
                }
                //지금까지 하나의 블록을 선택한 경우
                else
                {

                    _firstBlock = hit.transform.gameObject;
                    FindLocationInLinkedList();
                    pressedBlock = true;

                }

            }

        }
    }

    //선택한 블록이 게임 내에서 n행 n열에 있는지를 탐색한다.
    void FindLocationInLinkedList()
    {
        //첫 번째 블록
        if (!pressedBlock)
        {
            _firstBlockInfo = _firstBlock.GetComponent<BlockInfo>();

            for (int i = 0; i < blocks.colCnt; ++i)
            {
                if (blocks.cols[_firstBlockInfo._ColPos][i] == _firstBlock)
                {
                    _firstBlockInfo._RowPos = i;
                }
            }
        }
        //두 번째 블록
        else
        {
            _secondBlockInfo = _secondBlock.GetComponent<BlockInfo>();

            for (int i = 0; i < blocks.colCnt; ++i)
            {
                if (blocks.cols[_secondBlockInfo._ColPos][i] == _secondBlock)
                {
                    _secondBlockInfo._RowPos = i;
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

    //두 블록이 교환 가능한지 확인하고, 가능하다면 교환 실시
    void ClickBlockTwice(RaycastHit2D hit)
    {
        //두 블록의 행이 같을 때
        if (_firstBlockInfo._RowPos == _secondBlockInfo._RowPos)
        {
            if (_firstBlockInfo._ColPos - 1 == _secondBlockInfo._ColPos || _firstBlockInfo._ColPos + 1 == _secondBlockInfo._ColPos)
            {
                Debug.Log("블록 교환 수행");
                blocks._playerTurn = true;
                blocks.BlockExchangeByPlayer(_firstBlockInfo,_secondBlockInfo);
            }

        }
        //두 블록의 열이 같을 때
        else if (_firstBlockInfo._ColPos == _secondBlockInfo._ColPos)
        {
            if (_secondBlockInfo._RowPos == _firstBlockInfo._RowPos - 1 || _secondBlockInfo._RowPos == _firstBlockInfo._RowPos + 1)
            {
                Debug.Log("블록 교환 수행");
                blocks._playerTurn = true;
                blocks.BlockExchangeByPlayer(_firstBlockInfo, _secondBlockInfo);


            }
        }


        pressedBlock = false;

    }
}



   








