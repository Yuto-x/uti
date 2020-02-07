using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloor3d : MonoBehaviour
{
    public GameObject Ni1FButton;
    public GameObject Ni2FButton;
    public GameObject San2FButton;

    public void OnClick()
    {
        this.gameObject.SetActive(false);
        San2FButton.gameObject.SetActive(true);
        Ni1FButton.gameObject.SetActive(false);
        Ni2FButton.gameObject.SetActive(true);
    }
}
