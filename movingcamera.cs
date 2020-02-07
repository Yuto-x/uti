using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingcamera : MonoBehaviour
{
    public Camera camera; // 使用するカメラ
    public GameObject target; // an object to follow
    public Vector3 offset; // offset form the target object

    [SerializeField] private float distance = 1.0f; // distance from following object
    [SerializeField] private float polarAngle = 45.0f; // angle with y-axis
    [SerializeField] private float azimuthalAngle = 45.0f; // angle with x-axis

    [SerializeField] private float minDistance = 1.0f;
    [SerializeField] private float maxDistance = 30.0f;
    [SerializeField] private float minPolarAngle = 5.0f;
    [SerializeField] private float maxPolarAngle = 75.0f;
    [SerializeField] private float mouseXSensitivity = 5.0f;
    [SerializeField] private float mouseYSensitivity = 5.0f;
    [SerializeField] private float scrollSensitivity = 1.0f;

    private Vector3 baseCameraPos; // 基準となるカメラの座標
    private float basePinchDistance = 0f;// 基準となるピンチ時の指と指の距離
    private float currentPinchDistance = 0f;
    private bool isPinchStart = true; // ピンチスタートしたかを管理するフラグ

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //回転移動
        if (Input.GetMouseButton(0))
        {
            
            updateAngle(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        }
        updateDistance(Input.GetAxis("Mouse ScrollWheel"));

        var lookAtPos = target.transform.position + offset;
        updatePosition(lookAtPos);
        transform.LookAt(lookAtPos);
    }

    //回転する量を指定
    void updateAngle(float x, float y)
    {
        x = azimuthalAngle - x * mouseXSensitivity;
        azimuthalAngle = Mathf.Repeat(x, 360);

        y = polarAngle + y * mouseYSensitivity;
        polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
    }

    //拡大・縮小
    void updateDistance(float scroll)
    {
        if (Input.touchCount == 2)
        {
            if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Ended)
            {
                isPinchStart = true;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
            {
                
                if (isPinchStart)
                {
                    isPinchStart = false;

                    basePinchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                    baseCameraPos = camera.transform.localPosition;
                }
            }
            currentPinchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            scroll = ((basePinchDistance - currentPinchDistance) - scroll * scrollSensitivity) / 10; //分母の数字を大きくすると縮小が小さくなる
            distance = Mathf.Clamp(scroll, minDistance, maxDistance);
        }

        
    }

    void updatePosition(Vector3 lookAtPos)
    {
        var da = azimuthalAngle * Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + distance * Mathf.Cos(dp),
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }

}