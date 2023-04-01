using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//블록 리스트를 관리하고 매칭로직,드랍로직등이 담겨있는 스크립트
//매칭 중, 드랍 중일때는 이동시킬 수 없게 만들어야한다.

public class Fruit : MonoBehaviour
{
    //UI 관리용 게임 매니저
    GameManager gamemanager;

    //열 리스트 배열, 블록 생성기 리스트
    List<GameObject>[] cols;
    List<GameObject> generators;

    GameObject firstFruitObj = null;
    GameObject secondFruiObj = null;

    bool playerTurn = false;
    bool isMoving = false;
    bool bCanTouch = false;
    bool bMatch = false;
    bool bMatchSuccess = false;
    bool bReversed = false;

    //블록 타일의 열 길이,블록 이동속도
    public int colCnt = 4;
    public float Speed = 0.5f;
    int GameObjectNameCnt = 65;

    const float ToFindRowAndCol = 2.5f;


    Vector3 firstFruitObjTargetVec;
    Vector3 secondFruiObjTargetVec;

    public GameObject apple;
    public GameObject banana;
    public GameObject pear;
    public GameObject grape;


    public int goalCnt = 15;
    public GameObject goalFruit;



    Effect effect;


    void Start()
    {
        //이펙트 컴포넌트
        effect = GetComponent<Effect>();

        //UI관리용 게임 매니저 컴포넌트
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.SetGoalTxt(goalCnt.ToString());
        //리스트 초기화 및 초기값 삽입
        InitialList();

    }


    void Update()
    {
        //블록 이동
        if(isMoving)
        MoveBlockByPlayer();

        //목표 점수 체크
        if(goalCnt<=0)
        {
            gamemanager.SetGoalTxt("0");
            gamemanager.showWin();
        }

        
    }

    //리스트 초기화 함수
    void InitialList()
    {
        //객체 생성
        cols = new List<GameObject>[colCnt];
        generators = new List<GameObject>();
        bCanTouch = true;

        //리스트 초기값 삽입
        for (int i = 0; i < colCnt; i++)
        {
            cols[i] = new List<GameObject>();
            GameObject col = GameObject.Find("Cols" + (i + 1));
            GameObject generator = GameObject.Find("Generators");
           
            for (int j = 0; j < col.transform.childCount; j++)
            {
                cols[i].Add(col.transform.GetChild(j).gameObject);
                generators.Add(generator.transform.GetChild(j).gameObject);
            }

        }

        Debug.Log("행과 열 초기화 완료");

    }

    //블록 매칭 로직
    //행에서 왼쪽의 세개를 매칭시키면 가장 오른쪽 하나가 터지는 이상한 현상 
    void MatchLogic()
    {
        Debug.Log("매치로직");
        List<GameObject> destroyList = new List<GameObject>();

        for (int i = 0; i < colCnt; i++)
        {

            List<GameObject> rowTempList = new List<GameObject>();
            List<GameObject> colTempList = new List<GameObject>();
   

            int row = 1;
            int col = 1;

            string colTag = cols[i][0].tag;
            string rowTag = cols[0][i].tag;

            colTempList.Add(cols[i][0]);
            rowTempList.Add(cols[0][i]);
      

            for (int j = 1; j < colCnt; j++)
            {
                if (colTag == cols[i][j].tag)
                {           
                    col++;
                    colTempList.Add(cols[i][j]);
                }
                else
                {
                    colTag = cols[i][j].tag;
                  
                    if (col < 3)
                    {
                        col = 1;
                        colTempList.Clear();
                        colTempList.Add(cols[i][j]);
                    }
                    else
                    {
             
                   
                       //파괴리스트에 삽입하고 rowTempList clear 시키기
                        
                    }

                }

                if(rowTag == cols[j][i].tag)
                {
                    row++;
                    rowTempList.Add(cols[j][i]);
                }
                else
                {
                    rowTag = cols[j][i].tag;
      
                    if (row < 3)
                    {
                        row = 1;
                        rowTempList.Clear();
                        rowTempList.Add(cols[j][i]);
                    }
                    else
                    {
                        //파괴리스트에 삽입하고 rowTempList clear 시키기
                    }
                }
            }

           Debug.Log("Row: " + row + "Col: " + col);

            //매칭되는 블록이 있으면 파괴블록리스트에 삽입하기
            DestroyListInsert(row, col, rowTempList, colTempList,destroyList);
        }

        //파괴블록리스트에 블록이 있다면 파괴 수행 -> 블록 드롭
        if (destroyList.Count > 0)
        {   
            DestroyBlock(destroyList);
            
        }
        //파괴할 블록이 없다면, 매칭 로직 종료
        else
        {

            bMatch = false;
            bCanTouch = true;

            //다시 블록 원상태로 되돌리기
            if (playerTurn)
            {
                bReversed = true;
                ListExchangeByPlayer(firstFruitObj.name,secondFruiObj.name);
            }
            else
                playerTurn = false;
         

        }
      
    }

    //파괴할 블록 입력 함수
    void DestroyListInsert(int row, int col, List<GameObject> rowTempList, List<GameObject> colTempList,List<GameObject> destroyList)
    {
        if (row >= 3 || col >= 3)
        {
            if (row >= 3)
            {
                foreach (var item in rowTempList)
                    destroyList.Add(item);
            }
            if (col >= 3)
            {
                foreach (var item in colTempList)
                    destroyList.Add(item);
            }
            rowTempList.Clear();
            colTempList.Clear();
        }
    }

    //블록 파괴 함수
    void DestroyBlock(List<GameObject> destroyList)
    {
        for(int i=0;i<colCnt;i++)
        {
            foreach (var item in destroyList)
                cols[i].Remove(GameObject.Find(item.name));
        }

        foreach (var item in destroyList)
        {
            if (item.tag == goalFruit.tag)
            {
                //목표 과일 수 감소, UI 표기
                goalCnt--;
                gamemanager.SetGoalTxt(goalCnt.ToString());
            }

            effect.Explosion(item.transform.position, item.transform.rotation);
            GameObject.Destroy(GameObject.Find(item.name));
            effect.DestroySound();
            
        }


        destroyList.Clear();


        //블록을 파괴한 뒤, 블록 생성
        StartCoroutine(GenerateBlocks());

    }

    //블록을 파괴하고 바로 빈자리에 블록을 채워줘야한다.
    //랜덤 블록 생성 세팅 함수
    IEnumerator GenerateBlocks()
    {
        for(int i=0;i<colCnt;i++)
        {
            if(cols[i].Count < colCnt)
            {
                int GenerateBlockCnt = colCnt - cols[i].Count;
                UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

                for (int j = 0; j < GenerateBlockCnt; j++)
                {
                    StartCoroutine(InstantiateBlocks(i));
                    yield return new WaitForSeconds(0.1f); // 0.1초 대기
                 
                }
            }
        }

        //드랍 로직이 완료됐으므로, 드랍된 블록에 대한 매칭을 해야한다. 따라서, bMatch true 전환
   
        bMatch = true;
        playerTurn = false;

        SortBlock();
     

    }

    //블록이 파괴된 후, 랜덤 블록 생성 함수
    IEnumerator InstantiateBlocks(int i)
    {


        int minValue = 0;
        int maxValue = 4;
        int randomValue = UnityEngine.Random.Range(minValue, maxValue);

        switch (randomValue)
        {
            case 0:
                GameObject temp = GameObject.Instantiate(apple, generators[i].transform.position, generators[i].transform.rotation);
                temp.name += GameObjectNameCnt;
                temp.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                cols[i].Add(temp);
                break;
            case 1:
                GameObject temp2 = GameObject.Instantiate(banana, generators[i].transform.position, generators[i].transform.rotation);
                temp2.name += GameObjectNameCnt;
                temp2.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                cols[i].Add(temp2);
                break;
            case 2:
                GameObject temp3 = GameObject.Instantiate(pear, generators[i].transform.position, generators[i].transform.rotation);
                temp3.name += GameObjectNameCnt;
                temp3.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                cols[i].Add(temp3);
                break;
            case 3:
                GameObject temp4 = GameObject.Instantiate(grape, generators[i].transform.position, generators[i].transform.rotation);
                temp4.name += GameObjectNameCnt;
                temp4.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                cols[i].Add(temp4);
                break;
        }

        yield return null;
    }


    //블록 정렬 함수
    void SortBlock()
    {

        //블록간의 이동이 완료된 후, 상태를 원상태로 복귀시킨다.
        for (int i = 0; i < colCnt; i++)
        {
            for (int j = 0; j < colCnt; j++)
            {
                cols[i][j].layer = 0;
                cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }

        //정렬 시작
        for (int i = 0; i < colCnt; i++)
        {
            //람다식을 이용한 오름차순 정렬
            cols[i].Sort((a, b) => -a.transform.position.y.CompareTo(b.transform.position.y));
            foreach (var item in cols[i])
            {
                Debug.Log($"colsitem {i}: {item}");
            }
        }

        //새로 생성된 블록이 나온 직후거나, 플레이어가 블록을 이동시켰을 때만 매칭로직 실행
        if (bMatch)
            Invoke("MatchLogic", 1.0f);
        bReversed = false;
        Debug.Log("정렬 완료");

        
     

    }

   
    //플레이어가 선택한 블록 이동을 하기 전, 리스트간의 오브젝트 교환
    public void ListExchangeByPlayer(string firstFruit, string secondFruit)
    {
            
        //현재 블록의 이동이 이루어 지지 않고 있다면
        if (!isMoving)
        {

            if (!bReversed)
                playerTurn = true;
            else
                playerTurn = false;

            firstFruitObj = GameObject.Find(firstFruit);
            secondFruiObj = GameObject.Find(secondFruit);

  

            //이동시키는 블록의 행과 열 찾기
            long firstRow = -(int)(Mathf.Round(firstFruitObj.transform.localPosition.y - ToFindRowAndCol) );
            long firstCol = (int)(Mathf.Round(firstFruitObj.transform.localPosition.x + ToFindRowAndCol));

            long secondRow = -(int)(Mathf.Round(secondFruiObj.transform.localPosition.y - ToFindRowAndCol));
            long secondCol = (int)(Mathf.Round(secondFruiObj.transform.localPosition.x + ToFindRowAndCol));
      
            Debug.Log("firstrow: " + firstRow + " secondrow: " + secondRow + "firstcol : " + firstCol + " secondcol: " + secondCol);
            //리스트간의 오브젝트 교환

            //열이 다를때
            if (firstRow == secondRow)
            {
               
                cols[firstCol].Add(secondFruiObj);
                cols[secondCol].Add(firstFruitObj);

                cols[firstCol].RemoveAt((int)firstRow);
                cols[secondCol].RemoveAt((int)secondRow);


            }

            //같은 열에 있을 때는 리스트간의 교환을 할 필요가 없다. 이동 후, 정렬과정에서 자동으로 수행된다.
          

            Debug.Log("플레이어의 선택에 의한 블록 리스트 교환 완료");

                
            //리스트 갱신 후, 이동 준비
            MoveBlockSetting();
              
        }

}

    //플레이어가 선택한 블록을 대입 및 저장하는 Setting 함수
    public void MoveBlockSetting()
    {
       
        //블록간의 이동을 할 때는 모든 블록들의 레이어를 6, 중력값을 0으로 바꾼다.
        for (int i = 0; i < colCnt; i++)
        {
            for(int j=0;j<colCnt;j++)
            {
                cols[i][j].layer = 6;
                cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }

        //이동 시작, 터치 비활성화,매치활성화
        
        
        //플레이어가 조작한 직후
        if (playerTurn)
        {
            firstFruitObjTargetVec = secondFruiObj.transform.position;
            secondFruiObjTargetVec = firstFruitObj.transform.position;
            bMatch = true;
            bCanTouch = false;
        }
        //플레이어가 조작한 블록이 틀렸을 때
        else
        {
            Vector3 temp = firstFruitObjTargetVec;
            firstFruitObjTargetVec = secondFruiObjTargetVec;
            secondFruiObjTargetVec = temp;
            bMatch = false;
            bCanTouch = true;
        }

        isMoving = true;

        effect.MoveSound();
        Debug.Log("이동 준비 완료");

    }

    //플레이어가 선택한 블록들의 이동
    public void MoveBlockByPlayer()
    {

        if (firstFruitObj.transform.position == firstFruitObjTargetVec && secondFruiObj.transform.position == secondFruiObjTargetVec)
        {
            //이동 종료
            isMoving = false;
            
            Debug.Log("플레이어의 선택에 의한 블록 이동 수행 완료");


            //이동 완료 후, 정렬 시작
             SortBlock();

        }
      
        firstFruitObj.transform.position = Vector2.MoveTowards(firstFruitObj.transform.position, firstFruitObjTargetVec, Speed * Time.deltaTime);
        secondFruiObj.transform.position = Vector2.MoveTowards(secondFruiObj.transform.position, secondFruiObjTargetVec, Speed * Time.deltaTime);

        Debug.Log("이동중...");

    }

    //정렬을 할려면 미리 이동이 되어 있어야 정렬이 가능하다.
    //리스트 정렬
    
    public bool CanTouch()
    {
        if (bCanTouch)
            return true;
        else
            return false;
    }
  








}
