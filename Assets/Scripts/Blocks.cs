using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
블록 매칭 스크립트
*/



public class Blocks : MonoBehaviour
{

    GameManager gamemanager;
    Enemy enemy;

    //C#의 List는 C++의 Vector와 유사하다.
    public List<GameObject>[] cols;     //블록들이 모인 열 리스트 배열

    //교환할 블록의 정보 변수
    BlockInfo _firstBlockInfo;
    BlockInfo _secondBlockInfo;

    //교환할 블록의 위치 변수
    Vector3 _firstPos;
    Vector3 _secondPos;

    GameObject[] generators;     //블록 생성기

    GameObject _firstBlock = null;
    GameObject _secondBlock = null;

    public bool _playerTurn = false; 
    bool _goBack = false;      //플레이어가 블록 교환을 수행했는지, 안했는지?
    bool _canMove = false;   //블록이 이동 가능한 상태인지?
    bool _canMatch = false;  //매칭을 진행할지 말지?
    public bool _canTouch = false; //터치 가능한 상태인지?


    public int colCnt = 4;      //열의 개수 + 1
    public float Speed = 0.5f;  //블록 이동 속도
    int GameObjectNameCnt = 60; 


    public GameObject _cat;
    public GameObject _skeleton;
    public GameObject _devil;
    public GameObject _slime;

    Effect effect;


    void Start()
    {

        effect = GetComponent<Effect>();
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();


        InitialList();

    }


    void Update()
    {

        if(_canMove)
        {
            MoveBlockByPlayer();
        }

        if(_canMatch)
        {
            MatchLogic();
        }

    }


    //맵 내의 블록들을 각 열 리스트에 담는다.
    //public 변수로 만들어 직접 드래그 하여 오브젝트를 넣을 수 있지만 함수를 사용하여 단순화 O(n^2)
    void InitialList()
    {
        //사이즈 colCnt의 연결리스트 배열 생성
        cols = new List<GameObject>[colCnt];
        generators = new GameObject[colCnt];

   
        //Cols 리스트 배열에 맵 안의 블록들을 채워주기
        for (int i = 0; i < colCnt; i++)
        {
            cols[i] = new List<GameObject>();
            GameObject colObject = GameObject.Find("Cols" + (i+1));
            GameObject generatorGameObject = GameObject.Find("Generators");

            generators[i] = generatorGameObject.transform.GetChild(i).gameObject;

            for (int j = 0; j < colObject.transform.childCount; j++)
            {
                cols[i].Add(colObject.transform.GetChild(j).gameObject);
            }

        }   
        _canTouch = true;

       // PrintCols();
    }

    void PrintCols()
    {
        for(int i = 0; i<colCnt;i++)
        {
            foreach (var item in cols[i])
                Debug.Log(item.name);
        }
    }

    //블록 교환 
    public void BlockExchangeByPlayer(BlockInfo firstBlockInfo, BlockInfo secondBlockInfo)
    {
        if (_canTouch || _goBack)
        {
            _canTouch = false;

            //전역 변수에 값 갱신
            _firstPos = firstBlockInfo.gameObject.transform.position;
            _secondPos = secondBlockInfo.gameObject.transform.position;

            _firstBlock = firstBlockInfo.gameObject;
            _secondBlock = secondBlockInfo.gameObject;

            _firstBlockInfo = firstBlockInfo;
            _secondBlockInfo = secondBlockInfo;

            //블록 열 리스트간의 데이터 교환
            GameObject temp = cols[firstBlockInfo._ColPos][firstBlockInfo._RowPos];
            cols[firstBlockInfo._ColPos][firstBlockInfo._RowPos] = cols[secondBlockInfo._ColPos][secondBlockInfo._RowPos];
            cols[secondBlockInfo._ColPos][secondBlockInfo._RowPos] = temp;

            int tempPos = firstBlockInfo._ColPos;
            firstBlockInfo._ColPos = secondBlockInfo._ColPos;
            secondBlockInfo._ColPos = tempPos;

            MoveBlockSetting();
        }
       
    }


    //블록을 이동시키기 전에, Layer 설정 및 중력 해제
    public void MoveBlockSetting()
    {
        for (int i = 0; i < colCnt; i++)
        {
            for (int j = 0; j < colCnt; j++)
            {
                cols[i][j].layer = 6;
                cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
        effect.MoveSound();
        _canMove = true;
    }


    //매칭 성공 후에, 블록 추락을 위해 블록 세팅
    public void FallBlockSetting()
    {
        for (int i = 0; i < colCnt; i++)
        {
            for (int j = 0; j < cols[i].Count; j++)
            {
                cols[i][j].layer = 0;
                cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }

       
    }


    //블록 이동 함수
    public void MoveBlockByPlayer()
    {

        GameObject firstBlock = _firstBlock;
        GameObject secondBlock = _secondBlock;

        _firstBlock.transform.position = Vector3.MoveTowards(firstBlock.transform.position, _secondPos, Speed * Time.deltaTime);
        _secondBlock.transform.position = Vector3.MoveTowards(secondBlock.transform.position, _firstPos, Speed * Time.deltaTime);

        if(_firstBlock.transform.position == _secondPos && _secondBlock.transform.position == _firstPos)
        {
            Debug.Log("이동완료");
            _canMove = false;

            if (_playerTurn && !_goBack)
            {
                _canTouch = false;
                MatchLogic();
            }
            else if(_playerTurn && _goBack)
            {
                _playerTurn = false;
                _goBack = false;
                _canMatch = false;
                _canTouch = true;
            }
        }

     
    }

    //매칭 로직
    //이중 for문에서 Cols의 인덱스를 이용하여 행과 열을 한꺼번에 조사한다.
    void MatchLogic()
    {

        //Dictionary는 중복된 Key를 허용하지 않는다. 따라서 열이나 행에서 삭제할 블록이 중복되도 하나만 들어가게 된다.
        Dictionary<String, GameObject> destroyDic = new Dictionary<String, GameObject>();


        for(int i = 0; i<colCnt;i++)
        {
            
            string colTag = cols[i][0].tag;
            string rowTag = cols[0][i].tag;

            Dictionary<String, GameObject> destroyTempDic_Col = new Dictionary<string, GameObject>();
            Dictionary<String, GameObject> destroyTempDic_Row = new Dictionary<string, GameObject>();

            int colMatchedCount = 0;
            int rowMatchedCount = 0;

            for(int j = 0; j<colCnt; j++)
            {
                //열 매칭
                if(colTag == cols[i][j].tag)
                {    
                    ++colMatchedCount;
                    destroyTempDic_Col.Add(cols[i][j].name, cols[i][j]);

                    if (colMatchedCount >= 3 && j == colCnt-1)
                    {
                        InsertDestroyDic(destroyDic, destroyTempDic_Col);
                    }
                }
                else
                {
                    colTag = cols[i][j].tag;
                    if(colMatchedCount < 3)
                    {
                        destroyTempDic_Col.Clear();
                    }
                    else
                    {
                        InsertDestroyDic(destroyDic,destroyTempDic_Col);
                    }

                    colMatchedCount = 1;
                    destroyTempDic_Col.Add(cols[i][j].name, cols[i][j]);
                }

                //행 매칭

                if(cols[j][i].tag == rowTag)
                {
                    ++rowMatchedCount;
                    destroyTempDic_Row.Add(cols[j][i].name, cols[j][i]);

                    if (rowMatchedCount >= 3 && j == colCnt - 1)
                    {
                        InsertDestroyDic(destroyDic, destroyTempDic_Row);
                    }
                }
                else
                {
                    rowTag = cols[j][i].tag;
                    if (rowMatchedCount < 3)
                    {
                        destroyTempDic_Row.Clear();
                    }
                    else
                    {
                        InsertDestroyDic(destroyDic, destroyTempDic_Row);
                    }

                    rowMatchedCount = 1;
                    destroyTempDic_Row.Add(cols[j][i].name, cols[j][i]);
                }
            }
        }

        
        _canMatch = false; //매칭이 끝난 뒤에는 자동 매칭을 끄고, 파괴 블록 딕셔너리를 확인하고 다시 킨다.
        DestroyBlocks(destroyDic);

    }

    //임시 파괴 딕셔너리에 저장된 블록을 파괴 딕셔너리로 옮겨준다.
    void InsertDestroyDic(Dictionary<String,GameObject> destroyDic, Dictionary<String,GameObject> destroyTempDic)
    {
        //O(n)
        
        foreach (var item in destroyTempDic)
        {
            destroyDic.Add(item.Key, item.Value);
        }
        

        destroyTempDic.Clear();
    }


    //블록 파괴
    void DestroyBlocks(Dictionary<String, GameObject> destroyDic)
    {

        //열과 행의 블록 노드 삭제 O(n^3)
        for (int i = 0; i < colCnt; i++)
        {
            foreach (var item in destroyDic)
            {
                cols[i].Remove(item.Value);
            }

        }

        //게임 내 블록 파괴
        foreach (var item in destroyDic)
        {

            effect.Explosion(item.Value.gameObject.transform.position, item.Value.gameObject.transform.rotation);
            effect.DestroySound();
            effect.SmashBambooSound();
            gamemanager.ShowPlayerSmashEffect();
            Destroy(item.Value);
        }

        enemy.DecreaseEnemyHP(destroyDic.Count);

        //적의 체력을 확인하고 0 이하이면 터치, 매칭 종료 -> 적의 체력이 0 이하가 되면 타이머도 자동 종료
        if (enemy.enemyStatus.hp <= 0)
        {
            _canTouch = false;
            _canMatch = false;
        }
        else
        {
            //파괴할 블록이 있다면 파괴
            if (destroyDic.Count > 0)
            {
                _playerTurn = false;
                _goBack = false;
                FallBlockSetting();
                StartCoroutine(GenerateBlocks());
            }
            //없다면 블록 원상 복귀
            else
            {
                if (_playerTurn)
                {
                    _goBack = true; //매칭 실패로 플레이어 블록 교환 false
                    BlockExchangeByPlayer(_firstBlockInfo, _secondBlockInfo);
                }
                _canMatch = false;
                _canTouch = true;
                StartCoroutine(setCanTouch());
            }
        }

        destroyDic.Clear();


        Debug.Log("삭제완료");
    }

    IEnumerator setCanTouch()
    {
        yield return new WaitForSeconds(1.0f);
        _canTouch = true;
    }

    //블록 파괴가 끝나고, 각 열들을 확인 후에 블록 보충이 필요하면 보충 실시
    IEnumerator GenerateBlocks()
    {
        
        for(int i = 0; i < colCnt; i++)
        {
            int generateNum = colCnt  - cols[i].Count;

            if (generateNum > 0)
            {
                for (int j = 0; j < generateNum; j++)
                {
                    StartCoroutine(InstantiateBlocks(i, j, generateNum));
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        //블록 스폰이 다 끝난 뒤에 매칭 시작
        yield return new WaitForSeconds(0.5f);
        _canMatch = true;
    }

    //RandomValue에 따라 블록 생성
    IEnumerator InstantiateBlocks(int i, int j, int generateNum)
    {
        int minValue = 0;
        int maxValue = 4;
        int randomValue = UnityEngine.Random.Range(minValue, maxValue);

        switch (randomValue)
        {
            case 0:
                GameObject temp = GameObject.Instantiate(_cat, generators[i].transform.position, generators[i].transform.rotation);
                temp.name += GameObjectNameCnt;
                temp.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                temp.GetComponent<BlockInfo>()._ColPos = i;
                cols[i].Add(temp);
                break;
            case 1:
                GameObject temp2 = GameObject.Instantiate(_skeleton, generators[i].transform.position, generators[i].transform.rotation);
                temp2.name += GameObjectNameCnt;
                temp2.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                temp2.GetComponent<BlockInfo>()._ColPos = i;
                cols[i].Add(temp2);
                break;
            case 2:
                GameObject temp3 = GameObject.Instantiate(_devil, generators[i].transform.position, generators[i].transform.rotation);
                temp3.name += GameObjectNameCnt;
                temp3.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                temp3.GetComponent<BlockInfo>()._ColPos = i;
                cols[i].Add(temp3);
                break;
            case 3:
                GameObject temp4 = GameObject.Instantiate(_slime, generators[i].transform.position, generators[i].transform.rotation);
                temp4.name += GameObjectNameCnt;
                temp4.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
                GameObjectNameCnt++;
                temp4.GetComponent<BlockInfo>()._ColPos = i;
                cols[i].Add(temp4);
                break;
        }

        yield return null;
    }
    //public class Blocks : MonoBehaviour
    //{

    //    GameManager gamemanager;
    //    Enemy monster;


    //    List<GameObject>[] cols;
    //    List<GameObject> generators;

    //    GameObject firstBlockObj = null;
    //    GameObject secondBlockObj = null;


    //    bool playerTurn = false;
    //    bool isMoving = false;
    //    bool bCanTouch = false;
    //    bool bMatch = false;
    //    bool bReversed = false;
    //    bool bGameEnd = false;


    //    public int colCnt = 4;
    //    public float Speed = 0.5f;
    //    int GameObjectNameCnt = 65;

    //    const float ToFindRowAndCol = 2.5f;


    //    Vector3 firstBlcokObjTargetVec;
    //    Vector3 secondBlockObjTargetVec;

    //    public GameObject sword;
    //    public GameObject shield;
    //    public GameObject spear;
    //    public GameObject mana;

    //    Effect effect;


    //    void Start()
    //    {

    //        effect = GetComponent<Effect>();


    //        monster = GameObject.Find("Enemy").GetComponent<Enemy>();


    //        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();


    //        InitialList();

    //    }


    //    void Update()
    //    {

    //        if(isMoving)
    //        MoveBlockByPlayer();


    //    }


    //    void InitialList()
    //    {

    //        cols = new List<GameObject>[colCnt];
    //        generators = new List<GameObject>();

    //        bCanTouch = true;


    //        for (int i = 0; i < colCnt; i++)
    //        {
    //            cols[i] = new List<GameObject>();
    //            GameObject col = GameObject.Find("Cols" + (i + 1));
    //            GameObject generator = GameObject.Find("Generators");

    //            for (int j = 0; j < col.transform.childCount; j++)
    //            {
    //                cols[i].Add(col.transform.GetChild(j).gameObject);
    //                generators.Add(generator.transform.GetChild(j).gameObject);
    //            }

    //        }



    //    }

    //    //��� ��Ī ����
    //    void MatchLogic()
    //    {
    //       // Debug.Log("��ġ����");
    //        List<GameObject> destroyList = new List<GameObject>();

    //        for (int i = 0; i < colCnt; i++)
    //        {

    //            List<GameObject> rowTempList = new List<GameObject>();
    //            List<GameObject> colTempList = new List<GameObject>();


    //            int row = 1;
    //            int col = 1;

    //            string colTag = cols[i][0].tag;
    //            string rowTag = cols[0][i].tag;

    //            colTempList.Add(cols[i][0]);
    //            rowTempList.Add(cols[0][i]);


    //            for (int j = 1; j < colCnt; j++)
    //            {
    //                if (colTag == cols[i][j].tag)
    //                {           
    //                    col++;
    //                    colTempList.Add(cols[i][j]);
    //                }
    //                else
    //                {
    //                    colTag = cols[i][j].tag;

    //                    if (col < 3)
    //                    {
    //                        colTempList.Clear();
    //                        col = 1;
    //                        colTempList.Add(cols[i][j]);
    //                    }
    //                    else
    //                    {   
    //                         DestroyListInsert(col, colTempList, destroyList);
    //                         col = 1;
    //                         colTempList.Add(cols[i][j]);

    //                        //�ı�����Ʈ�� �����ϰ� colTempList clear ��Ű��

    //                    }

    //                }

    //                if(rowTag == cols[j][i].tag)
    //                {
    //                    row++;
    //                    rowTempList.Add(cols[j][i]);
    //                }
    //                else
    //                {
    //                    rowTag = cols[j][i].tag;

    //                    if (row < 3)
    //                    {      
    //                        rowTempList.Clear();
    //                        row = 1;
    //                        rowTempList.Add(cols[j][i]);
    //                    }
    //                    else
    //                    {

    //                        DestroyListInsert(row, rowTempList, destroyList);
    //                        row = 1;
    //                        rowTempList.Add(cols[j][i]);

    //                        //�ı�����Ʈ�� �����ϰ� rowTempList clear ��Ű��
    //                    }
    //                }

    //                //ó������ ������ ��ġ�ϴ� ����� ���� ���
    //                if(row>2)
    //                    DestroyListInsert(row, rowTempList, destroyList);
    //                else if(col>2)
    //                    DestroyListInsert(col, colTempList, destroyList);
    //            }

    //         //  Debug.Log("Row: " + row + "Col: " + col);


    //        }

    //        //�ı���ϸ���Ʈ�� ����� �ִٸ� �ı� ���� -> ��� ���
    //        if (destroyList.Count > 0)
    //        {   
    //            DestroyBlock(destroyList);

    //        }
    //        //�ı��� ����� ���ٸ�, ��Ī ���� ����
    //        else
    //        {
    //            bMatch = false;
    //            bCanTouch = true;

    //            //�÷��̾ ������ ����� ���� �ʴٸ� �ٽ� ��� �����·� �ǵ�����
    //            if (playerTurn)
    //            {
    //                bReversed = true;
    //                ListExchangeByPlayer(firstBlockObj.name,secondBlockObj.name);
    //            }
    //            else
    //                playerTurn = false;


    //        }

    //    }

    //    //�ı��� ��� �Է� �Լ�
    //    void DestroyListInsert(int matchedCnt, List<GameObject> matchedTempList, List<GameObject> destroyList)
    //    {

    //        if(matchedCnt>2)
    //        {
    //            foreach (var item in matchedTempList)
    //                destroyList.Add(item);
    //        }
    //        matchedTempList.Clear();

    //    }

    //    //��� �ı� �Լ�
    //    //��� �ı� �Լ����� for���� 4���� ���´�. ������ ��Ī�� for���� ���� ������ ����
    //    void DestroyBlock(List<GameObject> destroyList)
    //    {

    //        //��� �ı� ����
    //        foreach (var item in destroyList)
    //        {

    //            //��� �ı� ����Ʈ
    //            effect.Explosion(item.transform.position, item.transform.rotation);         
    //            effect.DestroySound();
    //            effect.SmashBambooSound();
    //            gamemanager.ShowPlayerSmashEffect();


    //            //��� �ı�
    //            Destroy(item);

    //        }


    //        //cols ��� ����
    //        for(int i=0;i<colCnt;i++)
    //        {
    //            foreach(var item in destroyList)
    //            cols[i].Remove(GameObject.Find(item.name));
    //        }

    //        monster.DecreaseEnemyHP(destroyList.Count);

    //        destroyList.Clear();
    //        //����� �ı��� ��, ��� ����
    //        StartCoroutine(GenerateBlocks());

    //    }

    //    //����� �ı��ϰ� �ٷ� ���ڸ��� ����� ä������Ѵ�.
    //    //���� ��� ���� ���� �Լ�
    //    IEnumerator GenerateBlocks()
    //    {
    //        for(int i=0;i<colCnt;i++)
    //        {
    //            if(cols[i].Count < colCnt)
    //            {
    //                int GenerateBlockCnt = colCnt - cols[i].Count;
    //                UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

    //                for (int j = 0; j < GenerateBlockCnt; j++)
    //                {
    //                    StartCoroutine(InstantiateBlocks(i));
    //                    yield return new WaitForSeconds(0.1f); // 0.1�� ���

    //                }
    //            }
    //        }

    //        //��� ������ �Ϸ�����Ƿ�, ����� ��Ͽ� ���� ��Ī�� �ؾ��Ѵ�. ����, bMatch true ��ȯ

    //        bMatch = true;
    //        playerTurn = false;

    //        SortBlock();


    //    }

    //    //����� �ı��� ��, ���� ��� ���� �Լ�
    //    IEnumerator InstantiateBlocks(int i)
    //    {


    //        int minValue = 0;
    //        int maxValue = 4;
    //        int randomValue = UnityEngine.Random.Range(minValue, maxValue);

    //        switch (randomValue)
    //        {
    //            case 0:
    //                GameObject temp = GameObject.Instantiate(sword, generators[i].transform.position, generators[i].transform.rotation);
    //                temp.name += GameObjectNameCnt;
    //                temp.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
    //                GameObjectNameCnt++;
    //                cols[i].Add(temp);
    //                break;
    //            case 1:
    //                GameObject temp2 = GameObject.Instantiate(shield, generators[i].transform.position, generators[i].transform.rotation);
    //                temp2.name += GameObjectNameCnt;
    //                temp2.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
    //                GameObjectNameCnt++;
    //                cols[i].Add(temp2);
    //                break;
    //            case 2:
    //                GameObject temp3 = GameObject.Instantiate(spear, generators[i].transform.position, generators[i].transform.rotation);
    //                temp3.name += GameObjectNameCnt;
    //                temp3.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
    //                GameObjectNameCnt++;
    //                cols[i].Add(temp3);
    //                break;
    //            case 3:
    //                GameObject temp4 = GameObject.Instantiate(mana, generators[i].transform.position, generators[i].transform.rotation);
    //                temp4.name += GameObjectNameCnt;
    //                temp4.transform.parent = GameObject.Find("Cols" + (i + 1)).transform;
    //                GameObjectNameCnt++;
    //                cols[i].Add(temp4);
    //                break;
    //        }

    //        yield return null;
    //    }


    //    //��� ���� �Լ�
    //    void SortBlock()
    //    {

    //        //��ϰ��� �̵��� �Ϸ�� ��, ���¸� �����·� ���ͽ�Ų��.
    //        if (playerTurn)
    //        {
    //            for (int i = 0; i < colCnt; i++)
    //            {
    //                for (int j = 0; j < colCnt; j++)
    //                {
    //                    cols[i][j].layer = 0;
    //                    cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 1;
    //                }
    //            }
    //        }

    //        //���� ����
    //        for (int i = 0; i < colCnt; i++)
    //        {
    //            //���ٽ��� �̿��� �������� ����
    //            cols[i].Sort((a, b) => -a.transform.position.y.CompareTo(b.transform.position.y));

    //            //foreach (var item in cols[i])
    //            //{
    //            //    Debug.Log($"colsitem {i}: {item}");
    //            //}
    //        }

    //        //���� ������ ����� ���� ���İų�, �÷��̾ ����� �̵������� ���� ��Ī���� ����
    //        if (bMatch)
    //           Invoke("MatchLogic", 0.8f);
    //        bReversed = false;
    //     //   Debug.Log("���� �Ϸ�");




    //    }


    //    //�÷��̾ ������ ��� �̵��� �ϱ� ��, ����Ʈ���� ������Ʈ ��ȯ
    //    public void ListExchangeByPlayer(string firstFruit, string secondFruit)
    //    {

    //        //���� ����� �̵��� �̷�� ���� �ʰ� �ִٸ�
    //        if (!isMoving)
    //        {
    //            //bReversed�� ��쿡�� ����� �����·� ���͵Ǿ�� �ϴ� ���
    //            if (!bReversed)
    //                playerTurn = true;
    //            else
    //                playerTurn = false;

    //            firstBlockObj = GameObject.Find(firstFruit);
    //            secondBlockObj = GameObject.Find(secondFruit);



    //            //�̵���Ű�� ����� ��� �� ã��
    //            long firstRow = -(int)(Mathf.Round(firstBlockObj.transform.localPosition.y - ToFindRowAndCol) );
    //            long firstCol = (int)(Mathf.Round(firstBlockObj.transform.localPosition.x + ToFindRowAndCol));

    //            long secondRow = -(int)(Mathf.Round(secondBlockObj.transform.localPosition.y - ToFindRowAndCol));
    //            long secondCol = (int)(Mathf.Round(secondBlockObj.transform.localPosition.x + ToFindRowAndCol));

    //            //Debug.Log("firstrow: " + firstRow + " secondrow: " + secondRow + "firstcol : " + firstCol + " secondcol: " + secondCol);
    //            //����Ʈ���� ������Ʈ ��ȯ

    //            //���� �ٸ���
    //            if (firstRow == secondRow)
    //            {

    //                cols[firstCol].Add(secondBlockObj);
    //                cols[secondCol].Add(firstBlockObj);

    //                cols[firstCol].RemoveAt((int)firstRow);
    //                cols[secondCol].RemoveAt((int)secondRow);


    //            }

    //            //���� ���� ���� ���� ����Ʈ���� ��ȯ�� �� �ʿ䰡 ����. �̵� ��, ���İ������� �ڵ����� ����ȴ�.


    //       //     Debug.Log("�÷��̾��� ���ÿ� ���� ��� ����Ʈ ��ȯ �Ϸ�");


    //            //����Ʈ ���� ��, �̵� �غ�
    //            MoveBlockSetting();

    //        }

    //}

    //    //�÷��̾ ������ ����� ���� �� �����ϴ� Setting �Լ�
    //    public void MoveBlockSetting()
    //    {

    //        //��ϰ��� �̵��� �� ���� ��� ��ϵ��� ���̾ 6, �߷°��� 0���� �ٲ۴�.
    //        for (int i = 0; i < colCnt; i++)
    //        {
    //            for(int j=0;j<colCnt;j++)
    //            {
    //                cols[i][j].layer = 6;
    //                cols[i][j].GetComponent<Rigidbody2D>().gravityScale = 0;
    //            }
    //        }

    //        //�̵� ����, ��ġ ��Ȱ��ȭ,��ġȰ��ȭ


    //        //�÷��̾ ������ ����
    //        if (playerTurn)
    //        {
    //            firstBlcokObjTargetVec = secondBlockObj.transform.position;
    //            secondBlockObjTargetVec = firstBlockObj.transform.position;
    //            bMatch = true;
    //            bCanTouch = false;
    //        }
    //        //�÷��̾ ������ ����� Ʋ���� ��
    //        else
    //        {
    //            Vector3 temp = firstBlcokObjTargetVec;
    //            firstBlcokObjTargetVec = secondBlockObjTargetVec;
    //            secondBlockObjTargetVec = temp;
    //            bMatch = false;
    //            bCanTouch = true;
    //        }

    //        isMoving = true;

    //        effect.MoveSound();
    //      //  Debug.Log("�̵� �غ� �Ϸ�");

    //    }

    //    //�÷��̾ ������ ��ϵ��� �̵�
    //    public void MoveBlockByPlayer()
    //    {

    //        if (firstBlockObj.transform.position == firstBlcokObjTargetVec && secondBlockObj.transform.position == secondBlockObjTargetVec)
    //        {
    //            //�̵� ����
    //            isMoving = false;

    //            //Debug.Log("�÷��̾��� ���ÿ� ���� ��� �̵� ���� �Ϸ�");


    //            //�̵� �Ϸ� ��, ���� ����
    //             SortBlock();

    //        }

    //        firstBlockObj.transform.position = Vector2.MoveTowards(firstBlockObj.transform.position, firstBlcokObjTargetVec, Speed * Time.deltaTime);
    //        secondBlockObj.transform.position = Vector2.MoveTowards(secondBlockObj.transform.position, secondBlockObjTargetVec, Speed * Time.deltaTime);

    //     //   Debug.Log("�̵���...");

    //    }

    //    //������ �ҷ��� �̸� �̵��� �Ǿ� �־�� ������ �����ϴ�.
    //    //����Ʈ ����

    //    public bool CanTouch()
    //    {
    //        if (bCanTouch)
    //            return true;
    //        else
    //            return false;
    //    }









}
