using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam2dChan1 : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject SanCamera_1;
    public GameObject MainChanvas;
    public GameObject SanChanvas_1;

    public void OnClick()
    {
        MainCamera.gameObject.SetActive(true);
        SanCamera_1.gameObject.SetActive(false);
        MainChanvas.gameObject.SetActive(true);
        SanChanvas_1.gameObject.SetActive(false);
    }
}
