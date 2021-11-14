using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemGenerator : MonoBehaviour
{
    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //cornPrefabを入れる
    public GameObject conePrefab;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("接触");
        // LineオブジェクトのLineTagを識別    
        if (other.gameObject.tag == "LineTag")
        {
            Debug.Log("座標取得");
            // 接触したオブジェクトの座標を取得
            Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
            // 取得したZ座標の80m先から、50m先までにアイテムを生成
            for (float i = other.transform.position.z + 80f; i < other.transform.position.z + 130f; i += 15)
            {
                Debug.Log("アイテム選択");
                //どのアイテムを出すのかをランダムに設定
                int num = Random.Range(1, 11);
                if (num <= 2)
                {
                    //コーンをx軸方向に一直線に生成
                    for (float j = -1; j <= 1; j += 0.4f)
                    {
                        Debug.Log("コーン生成");
                        GameObject cone = Instantiate(conePrefab);
                        cone.transform.position = new Vector3(4 * j, cone.transform.position.y, i);
                    }
                }
                else
                {
                    //レーンごとにアイテムを生成
                    for (int j = -1; j <= 1; j++)
                    {
                        Debug.Log("アイテム選択");
                        //アイテムの種類を決める
                        int item = Random.Range(1, 11);
                        //アイテムを置くZ座標のオフセットをランダムに設定
                        int offsetZ = Random.Range(-5, 6);
                        //60%コイン配置:30%車配置:10%何もなし
                        if (1 <= item && item <= 6)
                        {
                            //コインを生成
                            Debug.Log("コイン生成");
                            GameObject coin = Instantiate(coinPrefab);
                            coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, i + offsetZ);
                        }
                        else if (7 <= item && item <= 9)
                        {
                            //車を生成
                            Debug.Log("車生成");
                            GameObject car = Instantiate(carPrefab);
                            car.transform.position = new Vector3(posRange * j, car.transform.position.y, i + offsetZ);
                        }
                    }
                }
            }
        }
    }
}