using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnityChanController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;
    //Unityちゃんを移動させるコンポーネントを入れる（追加）
    private Rigidbody myRigidbody;
    //前方向の速度（追加）
    private float velocityZ = 16f;
    //横方向の速度（追加）
    private float velocityX = 10f;
    //上方向の速度（追加）
    private float velocityY = 10f;
    //左右の移動できる範囲（追加）
    private float movableRange = 3.4f;
    //動きを減速させる係数（追加）
    private float coefficient = 0.99f;
    //ゲーム終了の判定（追加）
    private bool isEnd = false;
    //ゲーム終了時に表示するテキスト（追加）
    private GameObject stateText;
    //スコアを表示するテキスト（追加）
    private GameObject scoreText;
    //得点（追加）
    private int score = 0;
    //左ボタン押下の判定（追加）
    private bool isLButtonDown = false;
    //右ボタン押下の判定（追加）
    private bool isRButtonDown = false;
    //ジャンプボタン押下の判定（追加）
    private bool isJButtonDown = false;
    // Use this for initialization

    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //cornPrefabを入れる
    public GameObject conePrefab;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;

    void Start()
    {
        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();
        //走るアニメーションを開始
        this.myAnimator.SetFloat("Speed", 1);
        //Rigidbodyコンポーネントを取得（追加）
        this.myRigidbody = GetComponent<Rigidbody>();
        //シーン中のstateTextオブジェクトを取得（追加）
        this.stateText = GameObject.Find("GameResultText");
        //シーン中のscoreTextオブジェクトを取得（追加）
        this.scoreText = GameObject.Find("ScoreText");
    }
    void Update()
    {
        //ゲーム終了ならUnityちゃんの動きを減衰する（追加）
        if (this.isEnd)
        {
            this.velocityZ *= this.coefficient;
            this.velocityX *= this.coefficient;
            this.velocityY *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }
        //横方向の入力による速度（追加）
        float inputVelocityX = 0;
        //上方向の入力による速度（追加）
        float inputVelocityY = 0;
        //Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる（追加）
        if ((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x)
        {
            //左方向への速度を代入
            inputVelocityX = -this.velocityX;
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange)
        {
            //右方向への速度を代入
            inputVelocityX = this.velocityX;
        }
        //ジャンプしていない時にスペースまたはボタンが押されたらジャンプする（追加）
        if ((Input.GetKeyDown(KeyCode.Space) || this.isJButtonDown) && this.transform.position.y < 0.5f)
        {
            //ジャンプアニメを再生
            this.myAnimator.SetBool("Jump", true);
            //上方向への速度を代入
            inputVelocityY = this.velocityY;
        }
        else
        {
            //現在のY軸の速度を代入
            inputVelocityY = this.myRigidbody.velocity.y;
        }
        //Jumpステートの場合はJumpにfalseをセットする（追加）
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }
        //Unityちゃんに速度を与える（変更）
        this.myRigidbody.velocity = new Vector3(inputVelocityX, inputVelocityY, velocityZ);
    }
    //トリガーモードで他のオブジェクトと接触した場合の処理（追加）
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
                        cone.transform.position = new Vector3(4 * j, cone.transform.position.y, this.transform.position.z + 50);
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
                            coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, this.transform.position.z + 50+offsetZ);
                        }
                        else if (7 <= item && item <= 9)
                        {
                            //車を生成
                            Debug.Log("車生成");
                            GameObject car = Instantiate(carPrefab);
                            car.transform.position = new Vector3(posRange * j, car.transform.position.y, this.transform.position.z + 50 + offsetZ);
                        }
                    }
                }
            }
        }
        //障害物に衝突した場合
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
            //stateTextにGAME OVERを表示（追加）
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }
        //ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            //stateTextにGAME CLEARを表示（追加）
            this.stateText.GetComponent<Text>().text = "CLEAR!!";
        }
        //コインに衝突した場合
        if (other.gameObject.tag == "CoinTag")
        {
            // スコアを加算(追加)
            this.score += 10;
            //ScoreText獲得した点数を表示(追加)
            this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";
            //パーティクルを再生（追加）
            GetComponent<ParticleSystem>().Play();
            //接触したコインのオブジェクトを破棄
            Destroy(other.gameObject);
        }
        //障害物に衝突した場合（追加）
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
        }
        //ゴール地点に到達した場合（追加）
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
        }
        //コインに衝突した場合（追加）
        if (other.gameObject.tag == "CoinTag")
        {
            //接触したコインのオブジェクトを破棄（追加）
            Destroy(other.gameObject);
        }
    }
    //ジャンプボタンを押した場合の処理（追加）
    public void GetMyJumpButtonDown()
    {
        this.isJButtonDown = true;
    }
    //ジャンプボタンを離した場合の処理（追加）
    public void GetMyJumpButtonUp()
    {
        this.isJButtonDown = false;
    }
    //左ボタンを押し続けた場合の処理（追加）
    public void GetMyLeftButtonDown()
    {
        this.isLButtonDown = true;
    }
    //左ボタンを離した場合の処理（追加）
    public void GetMyLeftButtonUp()
    {
        this.isLButtonDown = false;
    }
    //右ボタンを押し続けた場合の処理（追加）
    public void GetMyRightButtonDown()
    {
        this.isRButtonDown = true;
    }
    //右ボタンを離した場合の処理（追加）
    public void GetMyRightButtonUp()
    {
        this.isRButtonDown = false;
    }
}