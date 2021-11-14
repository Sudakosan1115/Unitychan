using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroy : MonoBehaviour
{
    //Unityちゃん
    private GameObject unitychan;
    // Start is called before the first frame update
    void Start()
    {
        //Unityちゃんのオブジェクトを取得
        this.unitychan = GameObject.Find("unitychan");

    }

    // Update is called once per frame
    void Update()
    {
        float dis = unitychan.transform.position.z - this.transform.position.z;
        if (dis > 5)
        { Destroy(this.gameObject); }
    }
}
