using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



/**
 *森君へ！！！！！！！！！！！！！
 * 変数名わかりやすく変えてくれるのはありがたいですが、
 * その変数が何の変数かコメントを残しておいてください
 * 次に作業するときにわかりません
 * 
 * ぶん殴るぞ
 * 
 * やってみろ
 * 
 * 今から一緒に
 * 
 * これから一緒に
 * 
 * 殴りに行こうか！！！
 * yeah
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

    //ボタン
    GameObject Room1FButton;    //1階モード
    GameObject Room2FButton;    //2階モード
    GameObject RoomCreateButton;    //部屋作成モード
    GameObject RoomDeleteButton;  //部屋削除モード
    GameObject toile2;          //トイレ検索モード

    private RaycastHit hit;
    
    //変数定義
    string startObjectName = "";
    string endObjectName = "";
    string delObjectName = "";
    string wc2 = "";    //トイレの変数(タグ判定のため)
    string kit2 = "";   //キッチンの変数(タグ判定のため)
    string bus2 = "";   //お風呂の変数(タグ判定のため)

    //3次元の配列を置く
    int[,,] field = new int[70, 10, 40];
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
    public GameObject SecondGrands;
    List<Transform> SecondGrandList = new List<Transform>();
    Transform[] SecondGrandArray;



    // Start is called before the first frame update
    void Start()
    {
        SecondGrandArray = SecondGrandList.ToArray();
        /*
         * 地面生成の処理(森)
         * -----------------------------------------------------------------
         */
        // 1と0を代入
        // 初期化
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int k = 0; k < field.GetLength(2); k++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        field[i, j, k] = 1;
                    }
                    else
                    {
                        field[i, j, k] = 0;
                    }
                }

            }
        }

        //各インデックスに代入された値をもとに生成するorしない

        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                for (int k = 0; k < field.GetLength(2); k++)
                {
                    //インデックスの値が1の時、cubeを生成
                    if (field[i, j, k] == 1)
                    {
                        GameObject wall = Instantiate(grand);
                        // wallの名前の変更 countによって一つ一つに番号をふる
                        wall.name = "grand" + count;
                        wall.transform.position = new Vector3(i, j, k);
                        count++;
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

                                        //選択した座標を配列の要素番号に代入からのその上に床オブジェクトを配置
                                        field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] = 1;

                                        //インデックスの値が1の時、cubeを生成
                                        if (field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] == 1)
                                        {
                                            //Assetsからmasuを取得
                                            GameObject floor = Instantiate(masutile);
                                            //オブジェクト生成時に"masu番号"に名前変更
                                            floor.name = "masutile" + count;
                                            floor.transform.position = new Vector3(tmp.x, tmp.y + 1, tmp.z);
                                            count++;
                                            floor.tag = "1fRoom" + FirstfRoomCount;
                                        }
                                    }
                                }
                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        if (field[(int)tmp.x + 1, (int)tmp.y + 1, (int)tmp.z] == 0 || field[(int)tmp.x - 1, (int)tmp.y + 1, (int)tmp.z] == 0 || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z + 1] == 0 || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z - 1] == 0)
                                        {
                                            // 選択した座標を配列の要素番号に代入からのその上に壁オブジェクトを配置
                                            field[(int)tmp.x, (int)tmp.y + 2, (int)tmp.z] = 1;
                                            field[(int)tmp.x, (int)tmp.y + 3, (int)tmp.z] = 1;
                                            field[(int)tmp.x, (int)tmp.y + 4, (int)tmp.z] = 1;

                                            //インデックスの値が1の時、cubeを生成
                                            for (int u = 2; u <= 4; u++)
                                            {
                                                if (field[(int)tmp.x, (int)tmp.y + u, (int)tmp.z] == 1)
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

                                        //選択した座標を配列の要素番号に代入からのその上に床オブジェクトを配置
                                        field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] = 1;

                                        //インデックスの値が1の時、cubeを生成
                                        if (field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z] == 1)
                                        {
                                            //Assetsからmasuを取得
                                            GameObject floor = Instantiate(masutile);
                                            //オブジェクト生成時に"masu番号"に名前変更
                                            floor.name = "masutile" + count;
                                            floor.transform.position = new Vector3(tmp.x, tmp.y + 1, tmp.z);
                                            count++;
                                            floor.tag = "2fRoom" + SecondRoomCount;
                                            SecondRoomList.Add(floor);
                                        }
                                    }
                                }
                                for (int s = 0; tmp3.z >= s; s++)
                                {
                                    tmp.z = tmp1.z - s;

                                    for (int t = 0; tmp3.x >= t; t++)
                                    {
                                        tmp.x = tmp1.x + t;
                                        tmp.y = tmp1.y;

                                        if (field[(int)tmp.x + 1, (int)tmp.y + 1, (int)tmp.z] == 0 || field[(int)tmp.x - 1, (int)tmp.y + 1, (int)tmp.z] == 0 || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z + 1] == 0 || field[(int)tmp.x, (int)tmp.y + 1, (int)tmp.z - 1] == 0)
                                        {
                                            // 選択した座標を配列の要素番号に代入からのその上に壁オブジェクトを配置
                                            field[(int)tmp.x, (int)tmp.y + 2, (int)tmp.z] = 1;
                                            field[(int)tmp.x, (int)tmp.y + 3, (int)tmp.z] = 1;
                                            field[(int)tmp.x, (int)tmp.y + 4, (int)tmp.z] = 1;

                                            //インデックスの値が1の時、cubeを生成
                                            for (int u = 2; u <= 4; u++)
                                            {
                                                if (field[(int)tmp.x, (int)tmp.y + u, (int)tmp.z] == 1)
                                                {
                                                    //Assetsからmasuを取得
                                                    GameObject wall = Instantiate(masuwall);

                                                    //オブジェクト生成時に"masu番号"に名前変更
                                                    wall.name = "masuwall" + count;
                                                    wall.transform.position = new Vector3(tmp.x, tmp.y + u, tmp.z);
                                                    count++;
                                                    wall.tag = "2fRoom" + SecondRoomCount;
                                                    SecondRoomList.Add(wall);
                                                    

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

                    field[(int)DelTagPos.x, (int)DelTagPos.y, (int)DelTagPos.z] = 0;

                    Destroy(gameObj);
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



                // 2階の地面があった場所の配列の値を戻すため0と1を代入
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(2); j++)
                    {
                        if (field[i, 4, j] == 1)
                        {
                            field[i, 4, j] = 0;
                        }
                        if (field[i, 4, j] == 2)
                        {
                            field[i, 4, j] = 1;
                        }
                    }
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
                //1と0を代入
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(2); j++)
                    {
                        if (field[i, 4, j] == 1)
                        {
                            field[i, 4, j] = 2;
                        }
                        else if (field[i, 4, j] == 0)
                        {
                            field[i, 4, j] = 1;
                        }
                    }
                }

                //各インデックスに代入された値をもとに生成するorしない

                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(2); j++)
                    {
                        //インデックスの値が1の時、cubeを生成
                        if (field[i, 4, j] == 1)
                        {
                            GameObject wall = Instantiate(secondgrand);
                            // wallの名前の変更 countによって一つ一つに番号をふる
                            wall.name = "secondgrand" + count;
                            wall.transform.position = new Vector3(i, 4, j);
                            count++;
                            wall.transform.parent = SecondGrands.transform;

                        }//インデックスの値が2の時、cubeを生成
                        else if (field[i, 4, j] == 2)
                        {
                            GameObject wall = Instantiate(blackcube);
                            //wallの名前の変更 countによって一つ一つに番号をふる
                            wall.name = "blackcube" + blackcubecount;
                            wall.transform.position = new Vector3(i, 4, j);
                            blackcubecount++;
                            wall.transform.parent = SecondGrands.transform;
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
                // 2階で作った非表示のオブジェクトを表示する
                foreach (GameObject HidSecondObj in SecondRoomList)
                {
                    HidSecondObj.SetActive(true);
                }
            }
            // 2階モードの時
            else if (SecondFloorButton.activeSelf == true)
            {
                // 2階の地面の非表示
                SecondGrandArray = SecondGrands.GetComponentsInChildren<Transform>();
                foreach (GameObject GrandHiddn in SecondGrandArray)
                {
                    GrandHiddn.SetActive(false);
                }
                // 一度非表示にした壁の高い場所を表示
                foreach (GameObject WallTopActive in WallTopList)
                {
                    WallTopActive.SetActive(true);
                }
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
                    Debug.Log("入った1111");
                    SecondGrandArray = SecondGrands.GetComponentsInChildren<Transform>();
                    foreach (GameObject GrandHiddn in SecondGrandList.gameObject)
                    {
                        GrandHiddn.SetActive(true);
                        Debug.Log("入った");
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
