using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //在检视面板中设置的全局变量
    public static Slingshot S;
    public GameObject prefabProjectile;
    public float velocityMult = 4f;
    public bool _____________________________;
    //动态设置的全局变量
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //计算launchPos到mousePos3D间的坐标差
        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude){
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //将projectile移动到新位置
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0)){
            aimingMode = false;
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
            FollowCam.S.poi = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
        }
    }

    void OnMouseEnter()
    {
        // print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        // print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        // projectile.rigidbody.isKinematic = true;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
