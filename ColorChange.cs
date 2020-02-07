using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private RaycastHit hit;
    string CreateObjName; // 生成したオブジェクトの名前
    GameObject CreateObjTag; // 生成したオブジェクトのタグ
    Material mat;
    Material materi;
    // Start is called before the first frame update
    void Start()
    {
        materi = Resources.Load<Material>("Materials/orenji");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                CreateObjName = hit.collider.gameObject.name;
                CreateObjTag = GameObject.Find(CreateObjName);
                mat = CreateObjTag.GetComponent<Renderer>().material;
                CreateObjTag.GetComponent<Renderer>().material = materi;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                CreateObjTag.GetComponent<Renderer>().material = mat;

            }
        }
    }
}
