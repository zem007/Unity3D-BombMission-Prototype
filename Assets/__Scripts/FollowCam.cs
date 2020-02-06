using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static FollowCam S;
    public float easing = 0.5f; //用来平滑摄像头的Lerp插值
    public Vector2 minXY; //用来限制摄像机镜头不落到地面之外，限制在xy正方向上
    public bool __________________________;
    public GameObject poi; //这里的poi实际上是画面的中的发射出去的小球，从Slingshot中传入
    public float camZ;

    void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() // 不用Update是因为此计算机的帧率性能远高于物理模型调用的50FPS
    {
        Vector3 destination;
        if(poi == null) {
            destination = Vector3.zero;
        } else {
            destination = poi.transform.position;
            if(poi.tag == "Projectile") {
                if(poi.GetComponent<Rigidbody>().IsSleeping()) {
                    poi = null;
                    return; //在下一次更新时
                }
            }
        }

        //限定x y的最小值
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(this.transform.position, destination, easing);
        destination.z = camZ;
        this.transform.position = destination;
        //随高度放大摄像头视野
        this.GetComponent<Camera>().orthographicSize = destination.y + 10; //最小视野10
    }
}
