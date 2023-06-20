using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
블록 매칭 스크립트

수정 필요...
*/




public class Blocks : MonoBehaviour
{
    //UI ������ ���� �Ŵ���
    GameManager gamemanager;
    Enemy monster;

    //�� ����Ʈ �迭, ��� ������ ����Ʈ
    List<GameObject>[] cols;
    List<GameObject> generators;

    GameObject firstBlockObj = null;
    GameObject secondBlockObj = null;


    bool playerTurn = false;
    bool isMoving = false;
    bool bCanTouch = false;
    bool bMatch = false;
    bool bReversed = false;
    bool bGameEnd = false;

    //��� Ÿ���� �� ����,��� �̵��ӵ�
    public int colCnt = 4;
    public float Speed = 0.5f;
    int GameObjectNameCnt = 65;

    const float ToFindRowAndCol = 2.5f;


    Vector3 firstBlcokObjTargetVec;
    Vector3 secondBlockObjTargetVec;

    public GameObject sword;
    public GameObject shield;
    public GameObject spear;
    public GameObject mana;


   // public int goalCnt = 15;
    //public GameObject goalFruit;



    Effect effect;


    void Start()
    {
        //����Ʈ ������Ʈ
        effect = GetComponent<Effect>();

        //���� ������Ʈ
        monster = GameObject.Find("Enemy").GetComponent<Enemy>();

        //UI������ ���� �Ŵ��� ������Ʈ
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();


        //����Ʈ �ʱ�ȭ �� �ʱⰪ ����
        InitialList();

    }


    void Update()
    {
        //��� �̵�
        if(isMoving)
        MoveBlockByPlayer();

        /*
        //��ǥ ���� üũ
        if(goalCnt<=0)
        {
            gamemanager.SetGoalTxt("0");
            gamemanager.showWin();
        }
        */
        
    }

    //����Ʈ �ʱ�ȭ �Լ�
    void InitialList()
    {
        //��ü ����
        cols = new List<GameObject>[colCnt];
        generators = new List<GameObject>();
        bCanTouch = true;

        //����Ʈ �ʱⰪ ����
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

        //Debug.Log("��� �� �ʱ�ȭ �Ϸ�");

    }

    //��� ��Ī ����
    void MatchLogic()
    {
       // Debug.Log("��ġ����");
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
                        colTempList.Clear();
                        col = 1;
                        colTempList.Add(cols[i][j]);
                    }
                    else
                    {   
                         DestroyListInsert(col, colTempList, destroyList);
                         col = 1;
                         colTempList.Add(cols[i][j]);

                        //�ı�����Ʈ�� �����ϰ� colTempList clear ��Ű��

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
                        rowTempList.Clear();
                        row = 1;
                        rowTempList.Add(cols[j][i]);
                    }
                    else
                    {
                       
                        DestroyListInsert(row, rowTempList, destroyList);
                        row = 1;
                        rowTempList.Add(cols[j][i]);
                        
                        //�ı�����Ʈ�� �����ϰ� rowTempList clear ��Ű��
                    }
                }

                //ó������ ������ ��ġ�ϴ� ����� ���� ���
                if(row>2)
                    DestroyListInsert(row, rowTempList, destroyList);
                else if(col>2)
                    DestroyListInsert(col, colTempList, destroyList);
            }

         //  Debug.Log("Row: " + row + "Col: " + col);

            
        }

        //�ı���ϸ���Ʈ�� ����� �ִٸ� �ı� ���� -> ��� ���
        if (destroyList.Count > 0)
        {   
            DestroyBlock(destroyList);
            
        }
        //�ı��� ����� ���ٸ�, ��Ī ���� ����
        else
        {
            bMatch = false;
            bCanTouch = true;

            //�÷��̾ ������ ����� ���� �ʴٸ� �ٽ� ��� �����·� �ǵ�����
            if (playerTurn)
            {
                bReversed = true;
                ListExchangeByPlayer(firstBlockObj.name,secondBlockObj.name);
            }
            else
                playerTurn = false;
         

        }
      
    }

    //�ı��� ��� �Է� �Լ�
    void DestroyListInsert(int matchedCnt, List<GameObject> matchedTempList, List<GameObject> destroyList)
    {

        if(matchedCnt>2)
        {
            foreach (var item in matchedTempList)
                destroyList.Add(item);
        }
        matchedTempList.Clear();
        
    }

    //��� �ı� �Լ�
    //��� �ı� �Լ����� for���� 4���� ���´�. ������ ��Ī�� for���� ���� ������ ����
    void DestroyBlock(List<GameObject> destroyList)
    {
        
        //��� �ı� ����
        foreach (var item in destroyList)
        {
           
            //��� �ı� ����Ʈ
            effect.Explosion(item.transform.position, item.transform.rotation);         
            effect.DestroySound();
            effect.SmashBambooSound();
            gamemanager.ShowPlayerSmashEffect();
           

            //��� �ı�
            Destroy(item);

        }

        
        //cols ��� ����
        for(int i=0;i<colCnt;i++)
        {
            foreach(var item in destroyList)
            cols[i].Remove(GameObject.Find(item.name));
        }

        monster.DecreaseEnemyHP(destroyList.Count);

        destroyList.Clear();
        //����� �ı��� ��, ��� ����
        StartCoroutine(GenerateBlocks());

    }

    //����� �ı��ϰ� �ٷ� ���ڸ��� ����� ä������Ѵ�.
    //���� ��� ���� ���� �Լ�
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
                    yield return new WaitForSeconds(0.1f); // 0.1�� ���
                 
                }
            }
        }

        //��� ������ �Ϸ�����Ƿ�, ����� ��Ͽ� ���� ��Ī�� �ؾ��Ѵ�. ����, bMatch true ��ȯ
   
        bMatch = true;
        playerTurn = false;

        SortBlock();
     

    }

    //����� �ı��� ��, ���� ��� ���� �Լ�
    IEnumerator InstantiateBlocks(int i)
    {


        int minValue = 0;
        int maxValue = 4;
        int randomValue = UnityEngine.Random.Range(minValue, maxValue);

        switch (randomValue)
        {
            case 0:
                GameObject temp = GameObject.Instantiate(sword, generators[i].transform.position, generators[i].transform.rotation);
                temp.name += GameObjectNameCnt;
                temp.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                cols[i].Add(temp);
                break;
            case 1:
                GameObject temp2 = GameObject.Instantiate(shield, generators[i].transform.position, generators[i].transform.rotation);
                temp2.name += GameObjectNameCnt;
                temp2.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                cols[i].Add(temp2);
                break;
            case 2:
                GameObject temp3 = GameObject.Instantiate(spear, generators[i].transform.position, generators[i].transform.rotation);
                temp3.name += GameObjectNameCnt;
                temp3.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                cols[i].Add(temp3);
                break;
            case 3:
                GameObject temp4 = GameObject.Instantiate(mana, generators[i].transform.position, generators[i].transform.rotation);
                temp4.name += GameObjectNameCnt;
                temp4.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                cols[i].Add(temp4);
                break;
        }

        yield return null;
    }


    //��� ���� �Լ�
    void SortBlock()
    {

        //��ϰ��� �̵��� �Ϸ�� ��, ���¸� �����·� ���ͽ�Ų��.
        if (playerTurn)
        {
            for (int i = 0; i < colCnt; i++)
            {
                for (int j = 0; j < colCnt; j++)
                {
                    cols[i][j].layer = 0;
                    cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 1;
                }
            }
        }

        //���� ����
        for (int i = 0; i < colCnt; i++)
        {
            //���ٽ��� �̿��� �������� ����
            cols[i].Sort((a, b) => -a.transform.position.y.CompareTo(b.transform.position.y));

            //foreach (var item in cols[i])
            //{
            //    Debug.Log($"colsitem {i}: {item}");
            //}
        }

        //���� ������ ����� ���� ���İų�, �÷��̾ ����� �̵������� ���� ��Ī���� ����
        if (bMatch)
           Invoke("MatchLogic", 0.8f);
        bReversed = false;
     //   Debug.Log("���� �Ϸ�");

        
     

    }

   
    //�÷��̾ ������ ��� �̵��� �ϱ� ��, ����Ʈ���� ������Ʈ ��ȯ
    public void ListExchangeByPlayer(string firstFruit, string secondFruit)
    {
            
        //���� ����� �̵��� �̷�� ���� �ʰ� �ִٸ�
        if (!isMoving)
        {
            //bReversed�� ��쿡�� ����� �����·� ���͵Ǿ�� �ϴ� ���
            if (!bReversed)
                playerTurn = true;
            else
                playerTurn = false;

            firstBlockObj = GameObject.Find(firstFruit);
            secondBlockObj = GameObject.Find(secondFruit);

  

            //�̵���Ű�� ����� ��� �� ã��
            long firstRow = -(int)(Mathf.Round(firstBlockObj.transform.localPosition.y - ToFindRowAndCol) );
            long firstCol = (int)(Mathf.Round(firstBlockObj.transform.localPosition.x + ToFindRowAndCol));

            long secondRow = -(int)(Mathf.Round(secondBlockObj.transform.localPosition.y - ToFindRowAndCol));
            long secondCol = (int)(Mathf.Round(secondBlockObj.transform.localPosition.x + ToFindRowAndCol));
      
            //Debug.Log("firstrow: " + firstRow + " secondrow: " + secondRow + "firstcol : " + firstCol + " secondcol: " + secondCol);
            //����Ʈ���� ������Ʈ ��ȯ

            //���� �ٸ���
            if (firstRow == secondRow)
            {
               
                cols[firstCol].Add(secondBlockObj);
                cols[secondCol].Add(firstBlockObj);

                cols[firstCol].RemoveAt((int)firstRow);
                cols[secondCol].RemoveAt((int)secondRow);


            }

            //���� ���� ���� ���� ����Ʈ���� ��ȯ�� �� �ʿ䰡 ����. �̵� ��, ���İ������� �ڵ����� ����ȴ�.
          

       //     Debug.Log("�÷��̾��� ���ÿ� ���� ��� ����Ʈ ��ȯ �Ϸ�");

                
            //����Ʈ ���� ��, �̵� �غ�
            MoveBlockSetting();
              
        }

}

    //�÷��̾ ������ ����� ���� �� �����ϴ� Setting �Լ�
    public void MoveBlockSetting()
    {
       
        //��ϰ��� �̵��� �� ���� ��� ��ϵ��� ���̾ 6, �߷°��� 0���� �ٲ۴�.
        for (int i = 0; i < colCnt; i++)
        {
            for(int j=0;j<colCnt;j++)
            {
                cols[i][j].layer = 6;
                cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }

        //�̵� ����, ��ġ ��Ȱ��ȭ,��ġȰ��ȭ
        
        
        //�÷��̾ ������ ����
        if (playerTurn)
        {
            firstBlcokObjTargetVec = secondBlockObj.transform.position;
            secondBlockObjTargetVec = firstBlockObj.transform.position;
            bMatch = true;
            bCanTouch = false;
        }
        //�÷��̾ ������ ����� Ʋ���� ��
        else
        {
            Vector3 temp = firstBlcokObjTargetVec;
            firstBlcokObjTargetVec = secondBlockObjTargetVec;
            secondBlockObjTargetVec = temp;
            bMatch = false;
            bCanTouch = true;
        }

        isMoving = true;

        effect.MoveSound();
      //  Debug.Log("�̵� �غ� �Ϸ�");

    }

    //�÷��̾ ������ ��ϵ��� �̵�
    public void MoveBlockByPlayer()
    {

        if (firstBlockObj.transform.position == firstBlcokObjTargetVec && secondBlockObj.transform.position == secondBlockObjTargetVec)
        {
            //�̵� ����
            isMoving = false;
            
            //Debug.Log("�÷��̾��� ���ÿ� ���� ��� �̵� ���� �Ϸ�");


            //�̵� �Ϸ� ��, ���� ����
             SortBlock();

        }
      
        firstBlockObj.transform.position = Vector2.MoveTowards(firstBlockObj.transform.position, firstBlcokObjTargetVec, Speed * Time.deltaTime);
        secondBlockObj.transform.position = Vector2.MoveTowards(secondBlockObj.transform.position, secondBlockObjTargetVec, Speed * Time.deltaTime);

     //   Debug.Log("�̵���...");

    }

    //������ �ҷ��� �̸� �̵��� �Ǿ� �־�� ������ �����ϴ�.
    //����Ʈ ����
    
    public bool CanTouch()
    {
        if (bCanTouch)
            return true;
        else
            return false;
    }
  








}
