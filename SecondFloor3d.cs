using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloor3d : MonoBehaviour
{
    public GameObject Ni1FButton;
    public GameObject Ni2FButton;
    public GameObject San1FButton;

    public void OnClick()
    {
        this.gameObject.SetActive(false);
        San1FButton.gameObject.SetActive(true);
        Ni1FButton.gameObject.SetActive(true);
        Ni2FButton.gameObject.SetActive(false);
    }
}
