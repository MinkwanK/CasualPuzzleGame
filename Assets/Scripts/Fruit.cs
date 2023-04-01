using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��� ����Ʈ�� �����ϰ� ��Ī����,����������� ����ִ� ��ũ��Ʈ
//��Ī ��, ��� ���϶��� �̵���ų �� ���� �������Ѵ�.

public class Fruit : MonoBehaviour
{
    //UI ������ ���� �Ŵ���
    GameManager gamemanager;

    //�� ����Ʈ �迭, ��� ������ ����Ʈ
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

    //��� Ÿ���� �� ����,��� �̵��ӵ�
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
        //����Ʈ ������Ʈ
        effect = GetComponent<Effect>();

        //UI������ ���� �Ŵ��� ������Ʈ
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.SetGoalTxt(goalCnt.ToString());
        //����Ʈ �ʱ�ȭ �� �ʱⰪ ����
        InitialList();

    }


    void Update()
    {
        //��� �̵�
        if(isMoving)
        MoveBlockByPlayer();

        //��ǥ ���� üũ
        if(goalCnt<=0)
        {
            gamemanager.SetGoalTxt("0");
            gamemanager.showWin();
        }

        
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

        Debug.Log("��� �� �ʱ�ȭ �Ϸ�");

    }

    //��� ��Ī ����
    //�࿡�� ������ ������ ��Ī��Ű�� ���� ������ �ϳ��� ������ �̻��� ���� 
    void MatchLogic()
    {
        Debug.Log("��ġ����");
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
             
                   
                       //�ı�����Ʈ�� �����ϰ� rowTempList clear ��Ű��
                        
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
                        //�ı�����Ʈ�� �����ϰ� rowTempList clear ��Ű��
                    }
                }
            }

           Debug.Log("Row: " + row + "Col: " + col);

            //��Ī�Ǵ� ����� ������ �ı���ϸ���Ʈ�� �����ϱ�
            DestroyListInsert(row, col, rowTempList, colTempList,destroyList);
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

            //�ٽ� ��� �����·� �ǵ�����
            if (playerTurn)
            {
                bReversed = true;
                ListExchangeByPlayer(firstFruitObj.name,secondFruiObj.name);
            }
            else
                playerTurn = false;
         

        }
      
    }

    //�ı��� ��� �Է� �Լ�
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

    //��� �ı� �Լ�
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
                //��ǥ ���� �� ����, UI ǥ��
                goalCnt--;
                gamemanager.SetGoalTxt(goalCnt.ToString());
            }

            effect.Explosion(item.transform.position, item.transform.rotation);
            GameObject.Destroy(GameObject.Find(item.name));
            effect.DestroySound();
            
        }


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


    //��� ���� �Լ�
    void SortBlock()
    {

        //��ϰ��� �̵��� �Ϸ�� ��, ���¸� �����·� ���ͽ�Ų��.
        for (int i = 0; i < colCnt; i++)
        {
            for (int j = 0; j < colCnt; j++)
            {
                cols[i][j].layer = 0;
                cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }

        //���� ����
        for (int i = 0; i < colCnt; i++)
        {
            //���ٽ��� �̿��� �������� ����
            cols[i].Sort((a, b) => -a.transform.position.y.CompareTo(b.transform.position.y));
            foreach (var item in cols[i])
            {
                Debug.Log($"colsitem {i}: {item}");
            }
        }

        //���� ������ ����� ���� ���İų�, �÷��̾ ����� �̵������� ���� ��Ī���� ����
        if (bMatch)
            Invoke("MatchLogic", 1.0f);
        bReversed = false;
        Debug.Log("���� �Ϸ�");

        
     

    }

   
    //�÷��̾ ������ ��� �̵��� �ϱ� ��, ����Ʈ���� ������Ʈ ��ȯ
    public void ListExchangeByPlayer(string firstFruit, string secondFruit)
    {
            
        //���� ����� �̵��� �̷�� ���� �ʰ� �ִٸ�
        if (!isMoving)
        {

            if (!bReversed)
                playerTurn = true;
            else
                playerTurn = false;

            firstFruitObj = GameObject.Find(firstFruit);
            secondFruiObj = GameObject.Find(secondFruit);

  

            //�̵���Ű�� ����� ��� �� ã��
            long firstRow = -(int)(Mathf.Round(firstFruitObj.transform.localPosition.y - ToFindRowAndCol) );
            long firstCol = (int)(Mathf.Round(firstFruitObj.transform.localPosition.x + ToFindRowAndCol));

            long secondRow = -(int)(Mathf.Round(secondFruiObj.transform.localPosition.y - ToFindRowAndCol));
            long secondCol = (int)(Mathf.Round(secondFruiObj.transform.localPosition.x + ToFindRowAndCol));
      
            Debug.Log("firstrow: " + firstRow + " secondrow: " + secondRow + "firstcol : " + firstCol + " secondcol: " + secondCol);
            //����Ʈ���� ������Ʈ ��ȯ

            //���� �ٸ���
            if (firstRow == secondRow)
            {
               
                cols[firstCol].Add(secondFruiObj);
                cols[secondCol].Add(firstFruitObj);

                cols[firstCol].RemoveAt((int)firstRow);
                cols[secondCol].RemoveAt((int)secondRow);


            }

            //���� ���� ���� ���� ����Ʈ���� ��ȯ�� �� �ʿ䰡 ����. �̵� ��, ���İ������� �ڵ����� ����ȴ�.
          

            Debug.Log("�÷��̾��� ���ÿ� ���� ��� ����Ʈ ��ȯ �Ϸ�");

                
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
            firstFruitObjTargetVec = secondFruiObj.transform.position;
            secondFruiObjTargetVec = firstFruitObj.transform.position;
            bMatch = true;
            bCanTouch = false;
        }
        //�÷��̾ ������ ����� Ʋ���� ��
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
        Debug.Log("�̵� �غ� �Ϸ�");

    }

    //�÷��̾ ������ ��ϵ��� �̵�
    public void MoveBlockByPlayer()
    {

        if (firstFruitObj.transform.position == firstFruitObjTargetVec && secondFruiObj.transform.position == secondFruiObjTargetVec)
        {
            //�̵� ����
            isMoving = false;
            
            Debug.Log("�÷��̾��� ���ÿ� ���� ��� �̵� ���� �Ϸ�");


            //�̵� �Ϸ� ��, ���� ����
             SortBlock();

        }
      
        firstFruitObj.transform.position = Vector2.MoveTowards(firstFruitObj.transform.position, firstFruitObjTargetVec, Speed * Time.deltaTime);
        secondFruiObj.transform.position = Vector2.MoveTowards(secondFruiObj.transform.position, secondFruiObjTargetVec, Speed * Time.deltaTime);

        Debug.Log("�̵���...");

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
