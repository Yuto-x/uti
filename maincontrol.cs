using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



/**
 * 変数名わかりやすく変えてくれるのはありがたいですが、
 * その変数が何の変数かコメントを残しておいてください
 * 次に作業するときにわかりません
 */
public class maincontrol : MonoBehaviour
{

    GameObject[] ArrayList;

    //プレハブ
    public GameObject grand;    //1階のマス
    public GameObject masutile; //床
    public GameObject masuwall; //壁
    public GameObject secondgrand;  //2階のマス
    public GameObject blackcube;  //既存の壁
    public GameObject kara; // 空のオブジェクト
    public GameObject kaidanin; //階段
    public GameObject door; //ドア

    //ボタン
    GameObject Room1FButton;    //1階モード
    GameObject Room2FButton;    //2階モード
    GameObject RoomCreateButton;    //部屋作成モード
    GameObject RoomDeleteButton;  //部屋削除モード
    GameObject toile2;          //トイレ検索モード
    GameObject stepup;          //階段下から上モード
    GameObject doorButton;          //ドアモード

    private RaycastHit hit;

    //変数定義
    string wallObjName = "";
    string startObjectName = "";
    string endObjectName = "";
    string delObjectName = "";
    string wc2 = "";    //トイレの変数(タグ判定のため)
    string kit2 = "";   //キッチンの変数(タグ判定のため)
    string bus2 = "";   //お風呂の変数(タグ判定のため)

    GameObject ClickObj; // クリックしたしたオブジェクト
    GameObject firstObj; // 最初にクリックした壁を保持

    //3次元の配列を置く
    public GameObject[,,] field = new GameObject[70, 10, 40];
    int count = 0;
    

    //クリック用の変数
    Vector3 tmp = new Vector3(0, 0, 0);
    Vector3 tmp1 = new Vector3(0, 0, 0);
    Vector3 tmp2 = new Vector3(0, 0, 0);
    Vector3 tmp3 = new Vector3(0, 0, 0);

    //オブジェクトやタグの番号付けの変数
    int blackcubecount = 0;
    int FirstfRoomCount = 1;
    int SecondRoomCount = 1;

    bool SecondStart = true;
    bool SecondGrandDel = false;

    private List<GameObject> WallTopList = new List<GameObject>();
    private List<GameObject> SecondRoomList = new List<GameObject>();

    //オブジェクト削除用変数
    Vector3 DelTagPos = new Vector3(0, 0, 0);
    string ObjDelName = "";

    GameObject[] SecondGrandDelTag;
    GameObject[] deletetag;
    GameObject[] Jyusetsutag;

    // 視点切り替え使用変数
    public GameObject MainCanvas; // メインカメラ
    public GameObject FirstFloorButton; // 2階に切り替えるボタン
    public GameObject SecondFloorButton; // 1階に切り替えるボタン
    bool CamJudg = false;
    [SerializeField] GameObject SecondGrands;
    bool Floor1Judg = false;
    bool Floor2Judg = false;

    // 親
    public GameObject grands;

    //ドア実行用変数
    int doorsu = 0;

    bool han = true;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * 地面生成の処理(森)
         * -----------------------------------------------------------------
         */
        //各インデックスをもとに地面を生成する
        // 初期化
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int k = 0; k < field.GetLength(2); k++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        GameObject Grand = Instantiate(grand);
                        // wallの名前の変更 countによって一つ一つに番号をふる
                        Grand.name = "grand" + count;
                        Grand.transform.position = new Vector3(i, j, k);
                        field[i, j, k] = Grand;
                        Grand.transform.parent = grands.transform;
                        count++;
                    }
                    else
                    {
                        field[i, j, k] = kara;
                    }
                }

            }
        }
        /*
         * (地面生成処理)
         * -----------------------------------------------------------------
         */

    }

    // Update is called once per frame
    void Update()
    {
        //ボタンの読み込み
        Room1FButton = GameObject.Find("1F");
        Room2FButton = GameObject.Find("2F");
        RoomCreateButton = GameObject.Find("roomcreateOff");
        RoomDeleteButton = GameObject.Find("roomdeleteOff");
        stepup = GameObject.Find("kaidanupoff");
        doorButton = GameObject.Find("dooroff");


        //ボタン下のオブジェクトに反応させない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        

        //部屋生成
        if (RoomCreateButton != null)
        {

            if(Room1FButton != null)
            {
                //ボタンを押したとき
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //スタートの座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        startObjectName = hit.collider.gameObject.name;

                        tmp1 = GameObject.Find(startObjectName).transform.position;

                        // 括弧内の文字列が入っているとY座標を下げる
                        if (startObjectName.Contains("masuwall"))
                        {
                            tmp1.y = tmp1.y - 4;
                        }
                        else if(startObjectName.Contains("masutile"))
                        {

                        }
                        Debug.Log(tmp1);

                    }

                }
                //ボタンを離した処理
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //エンドの座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        endObjectName = hit.collider.gameObject.name;

                        tmp2 = GameObject.Find(endObjectName).transform.position;


                        // 括弧内の文字列が入っているとオブジェクト生成の処理に入らない
                        if (startObjectName.Contains("masutile") || endObjectName.Contains("masutile"))
                        {

                        }
                        else
                        {
                            if (tmp1.x <= tmp2.x || tmp1.z >= tmp2.z)
                            {
                                tmp3.x = tmp2.x - tmp1.x;
                                tmp3.z = tmp1.z - tmp2.z;

                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        //Assetsからmasuを取得
                                        GameObject floor = Instantiate(masutile);
                                        //オブジェクト生成時に"masu番号"に名前変更
                                        floor.name = "masutile" + count;
                                        floor.transform.position = new Vector3(tmp.x, tmp.y + 1, tmp.z);
                                        count++;
                                        floor.tag = "1fRoom" + FirstfRoomCount;
                                        field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] = floor;
                                    }
                                }
                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        if (field[(int)tmp.x + 1, (int)tmp.y + 1, (int)tmp.z] == kara || field[(int)tmp.x - 1, (int)tmp.y + 1, (int)tmp.z] == kara || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z + 1] == kara || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z - 1] == kara)
                                        {
                                            // 選択した座標を配列の要素番号に代入からのその上に壁オブジェクトを配置
                                            field[(int)tmp.x, (int)tmp.y + 2, (int)tmp.z] = masuwall;
                                            field[(int)tmp.x, (int)tmp.y + 3, (int)tmp.z] = masuwall;
                                            field[(int)tmp.x, (int)tmp.y + 4, (int)tmp.z] = masuwall;

                                            //インデックスの値が1の時、cubeを生成
                                            for (int u = 2; u <= 4; u++)
                                            {
                                                if (field[(int)tmp.x, (int)tmp.y + u, (int)tmp.z].name.Contains("wall"))
                                                {
                                                    //Assetsからmasuを取得
                                                    GameObject wall = Instantiate(masuwall);

                                                    //オブジェクト生成時に"masu番号"に名前変更
                                                    wall.name = "masuwall" + count;
                                                    wall.transform.position = new Vector3(tmp.x, tmp.y + u, tmp.z);
                                                    count++;
                                                    wall.tag = "1fRoom" + FirstfRoomCount;
                                                    // 1番上の壁をListに格納
                                                    if (u == 4)
                                                    {
                                                        WallTopList.Add(wall);
                                                    }
                                                    field[(int)tmp.x, (int)tmp.y + u, (int)tmp.z] = wall;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    FirstfRoomCount++;
                }
            }else if (Room2FButton != null) //2階の時
            {

                
                //ボタンを押したとき
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //スタートの座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        startObjectName = hit.collider.gameObject.name;

                        tmp1 = GameObject.Find(startObjectName).transform.position;

                        // 括弧内の文字列が入っているとY座標を下げる
                        if (startObjectName.Contains("masuwall"))
                        {
                            tmp1.y = tmp1.y - 4;
                        }
                        else if (startObjectName.Contains("masutile"))
                        {

                        }


                    }

                }
                //ボタンを離した処理
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    //エンドの座標を取得
                    if (Physics.Raycast(ray, out hit))
                    {
                        endObjectName = hit.collider.gameObject.name;

                        tmp2 = GameObject.Find(endObjectName).transform.position;


                        // 括弧内の文字列が入っているとオブジェクト生成の処理に入らない
                        if (startObjectName.Contains("masutile") || endObjectName.Contains("masutile"))
                        {

                        }
                        else
                        {
                            if (tmp1.x <= tmp2.x || tmp1.z >= tmp2.z)
                            {
                                tmp3.x = tmp2.x - tmp1.x;
                                tmp3.z = tmp1.z - tmp2.z;

                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        //Assetsからmasuを取得
                                        GameObject floor = Instantiate(masutile);
                                        //オブジェクト生成時に"masu番号"に名前変更
                                        floor.name = "masutile" + count;
                                        floor.transform.position = new Vector3(tmp.x, tmp.y + 1, tmp.z);
                                        count++;
                                        floor.tag = "2fRoom" + SecondRoomCount;
                                        SecondRoomList.Add(floor);
                                        field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] = floor;
                                    }
                                }
                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        if (field[(int)tmp.x + 1, (int)tmp.y + 1, (int)tmp.z] == kara || field[(int)tmp.x - 1, (int)tmp.y + 1, (int)tmp.z] == kara || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z + 1] == kara || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z - 1] == kara)
                                        {
                                            // 選択した座標を配列の要素番号に代入からのその上に壁オブジェクトを配置
                                            field[(int)tmp.x, (int)tmp.y + 2, (int)tmp.z] = masuwall;
                                            field[(int)tmp.x, (int)tmp.y + 3, (int)tmp.z] = masuwall;
                                            field[(int)tmp.x, (int)tmp.y + 4, (int)tmp.z] = masuwall;

                                            //インデックスの値が1の時、cubeを生成
                                            for (int u = 2; u <= 4; u++)
                                            {
                                                if (field[(int)tmp.x, (int)tmp.y + u, (int)tmp.z].name.Contains("wall"))
                                                {
                                                    //Assetsからmasuを取得
                                                    GameObject wall = Instantiate(masuwall);

                                                    //オブジェクト生成時に"masu番号"に名前変更
                                                    wall.name = "masuwall" + count;
                                                    wall.transform.position = new Vector3(tmp.x, tmp.y + u, tmp.z);
                                                    count++;
                                                    wall.tag = "2fRoom" + SecondRoomCount;
                                                    SecondRoomList.Add(wall);
                                                    field[(int)tmp.x, (int)tmp.y + u, (int)tmp.z] = wall;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    SecondRoomCount++;
                }
            }

            
        }

        //削除する
        if (RoomDeleteButton != null)
        {
            //ボタンを押した処理
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //座標を取得
                if (Physics.Raycast(ray, out hit))
                {
                    delObjectName = hit.collider.gameObject.name;

                    tmp = GameObject.Find(delObjectName).transform.position;

                }
                GameObject tagname = GameObject.Find(delObjectName);

                deletetag = GameObject.FindGameObjectsWithTag(tagname.tag);

                foreach (GameObject gameObj in deletetag)
                {
                    ObjDelName = gameObj.name;
                    DelTagPos = GameObject.Find(ObjDelName).transform.position;

                    field[(int)DelTagPos.x, (int)DelTagPos.y, (int)DelTagPos.z] = kara;

                    Destroy(gameObj);
                }
            }
        }

        //階段処理
        if (stepup != null)
        {
            

            //ボタンを押した処理
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //スタートの座標を取得
                if (Physics.Raycast(ray, out hit))
                {
                    startObjectName = hit.collider.gameObject.name;

                    tmp = GameObject.Find(startObjectName).transform.position;
                }
                field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] = kaidanin;
                field[(int)tmp.x, (int)tmp.y + 2, (int)tmp.z + 1] = kaidanin;
                field[(int)tmp.x, (int)tmp.y + 3, (int)tmp.z + 2] = kaidanin;
                field[(int)tmp.x, (int)tmp.y + 4, (int)tmp.z + 3] = kaidanin;
                
                for (int i = 0; i <= 4; i++)
                {
                    if (field[(int)tmp.x, (int)tmp.y + i + 1, (int)tmp.z + i].name.Contains("kaidan"))
                    {
                        Debug.Log("格納前"+field[(int)tmp.x, (int)tmp.y + i + 1, (int)tmp.z + i]);
                        //Assetsからstepを取得
                        GameObject kaidan = Instantiate(kaidanin);

                        //オブジェクト生成時に"masu番号"に名前変更
                        kaidan.name = "kaidan" + count;
                        kaidan.transform.position = new Vector3(tmp.x, tmp.y + i + 1, tmp.z + i);
                        kaidan.transform.Rotate(0.0f, 0.0f, 0.0f);
                        field[(int)tmp.x, (int)tmp.y + i + 1, (int)tmp.z + i] = kaidan;
                        Debug.Log("格納後"+field[(int)tmp.x, (int)tmp.y + i + 1, (int)tmp.z + i]);

                        count++;
                    }
                    
                    if (field[(int)tmp.x, 5 , (int)tmp.z + i].name.Contains("tile"))
                    {
                        field[(int)tmp.x, 5 , (int)tmp.z + i].SetActive(true);
                        Debug.Log("下の処理"+field[(int)tmp.x, 5 , (int)tmp.z + i]);
                    }
                    
                }
            }
        }

        //ドア処理
        if (doorButton != null)
        {

            Debug.Log("入った");
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("押された");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    for (doorsu = 0; doorsu < 2; doorsu++)
                    {
                        // クリックしたオブジェクトの名前を取得
                        wallObjName = hit.collider.gameObject.name;
                        // stringからGameObjectに
                        ClickObj = GameObject.Find(wallObjName);
                        // クリックしたオブジェクトの座標を取得
                        tmp = ClickObj.transform.position;

                        field[(int)tmp.x, (int)tmp.y - 1 - doorsu, (int)tmp.z].SetActive(false);
                        
                    }

                    Instantiate(door).transform.position = new Vector3(tmp.x, tmp.y - 2, tmp.z);
                    //ここにタグ付け

                }
                
                

            }


        }
        
        //1Fと2Fに切り替えを行う
        if (Room1FButton != null)
        {
            
            //部屋の出力、壁の出力、削除等を入力
            if (SecondGrandDel == true)//2階の地面の削除、1階の高い部分を出力
            {
                SecondGrandDel = false;
                SecondStart = true;

                toile2 = GameObject.FindWithTag("wc2");
                if(toile2 != null)
                {
                    toile2.SetActive(false);
                }
                // 2階の地面の削除

                foreach (GameObject GrandDel in SecondGrandDelTag)
                {

                    Destroy(GrandDel);
                }

                // 一度非表示にした壁の高い場所を表示
                foreach (GameObject WallTopActive in WallTopList)
                {
                    WallTopActive.SetActive(true);
                }

                //2階の部屋を隠す
                foreach (GameObject hiddenObj in SecondRoomList.ToArray())
                {
                    if (hiddenObj == null)
                    {
                        SecondRoomList.Remove(hiddenObj);
                    }

                }
                foreach (GameObject hiddenObjTes in SecondRoomList)
                {
                    hiddenObjTes.SetActive(false);
                }
            }
        }

        else if (Room2FButton != null)
        {
            //2F地盤の出力、部屋の出力、壁の出力、削除等を入力
            /*
             * 2階の地面生成の処理
             * -----------------------------------------------------------------
             */
            if (SecondStart == true)
            {
                SecondStart = false;
                SecondGrandDel = true; // 1階の処理で1回目に入らなかった処理に入れる

                
                if (toile2 != null)
                {
                    toile2.SetActive(true);
                }

                foreach (GameObject delObj in WallTopList.ToArray())
                {
                    if (delObj == null)
                    {
                        WallTopList.Remove(delObj);
                    }

                }
                foreach (GameObject delObjTes in WallTopList)
                {
                    delObjTes.SetActive(false);
                }
                //各インデックスに代入された値をもとに生成するorしない

                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(2); j++)
                    {
                        //インデックスの値が1の時、cubeを生成
                        if (field[i, 4, j].name.Contains("kara"))
                        {
                            GameObject secondGrand = Instantiate(secondgrand);
                            // wallの名前の変更 countによって一つ一つに番号をふる
                            secondGrand.name = "secondgrand" + count;
                            secondGrand.transform.position = new Vector3(i, 4, j);
                            count++;
                            secondGrand.transform.parent = SecondGrands.transform;

                        }//インデックスの値が2の時、cubeを生成
                        else if (field[i, 4, j].name.Contains("wall"))
                        {
                            GameObject blackCube = Instantiate(blackcube);
                            //wallの名前の変更 countによって一つ一つに番号をふる
                            blackCube.name = "blackcube" + blackcubecount;
                            blackCube.transform.position = new Vector3(i, 4, j);
                            blackcubecount++;
                            blackCube.transform.parent = SecondGrands.transform;
                        }
                    }
                }
                // 配列に括弧内で指定したタグが付いているオブジェクトを格納
                SecondGrandDelTag = GameObject.FindGameObjectsWithTag("2fgrand");

                //2階の部屋を表示
                foreach (GameObject hiddenObj in SecondRoomList.ToArray())
                {
                    if (hiddenObj == null)
                    {
                        SecondRoomList.Remove(hiddenObj);
                    }

                }
                foreach (GameObject hiddenObjTes in SecondRoomList)
                {
                    hiddenObjTes.SetActive(true);
                }
            }
        }
        /*
         * 01/31 松瀬 カメラ切り替え後の処理追加
         * 
         * 2階が表示されない、2階の地面が削除されない
         */
        // 3D切り替えボタンを押したとき
        if (MainCanvas.activeSelf == false)
        {
            CamJudg = true;
            // 1階モードのとき
            if (FirstFloorButton.activeSelf == true)
            {
                if (Floor1Judg == true)
                {
                    Floor1Judg = false;
                    // 2階で作ったオブジェクトを非表示にする
                    foreach (GameObject HidSecondObj in SecondRoomList)
                    {
                        HidSecondObj.SetActive(false);
                    }
                    
                }
                Floor2Judg = true;
            }
            // 2階モードの時
            else if (SecondFloorButton.activeSelf == true)
            {
                if (Floor2Judg == true)
                {
                    Floor2Judg = false;
                    // 2階で作ったオブジェクトを非表示にする
                    foreach (GameObject HidSecondObj in SecondRoomList)
                    {
                        HidSecondObj.SetActive(true);
                    }
                }
                // 2階の地面の非表示
                foreach (Transform GrandHiddn in SecondGrands.transform)
                {
                    GameObject HidObj = GrandHiddn.gameObject;
                    HidObj.SetActive(false);
                }
                // 一度非表示にした壁の高い場所を表示
                foreach (GameObject WallTopActive in WallTopList)
                {
                    WallTopActive.SetActive(true);
                }
                Floor1Judg = true;
            }
        }
        // 3Dモードから2Dモードに戻ったとき
        else if (MainCanvas.activeSelf == true)
        {
            // 一度3Dモードに切り替えないと入らない
            if (CamJudg == true)
            {
                CamJudg = false; // 処理に入り続けないようにするためfalseに
                // 1階モードのとき
                if (FirstFloorButton.activeSelf == true)
                {
                    // 2階で作ったオブジェクトを非表示にする
                    foreach (GameObject HidSecondObj in SecondRoomList)
                    {
                        HidSecondObj.SetActive(false);
                    }
                }
                // 2階モードの時
                else if (SecondFloorButton.activeSelf == true)
                {
                    // 2階の地面を表示する   
                    foreach (Transform GrandHiddn in SecondGrands.transform)
                    {
                        GameObject HidObj = GrandHiddn.gameObject;
                        HidObj.SetActive(true);
                    }
                    // 一度非表示にした壁の高い場所を非表示
                    foreach (GameObject WallTopActive in WallTopList)
                    {
                        WallTopActive.SetActive(false);
                    }
                }
            }
        }
    }
}
