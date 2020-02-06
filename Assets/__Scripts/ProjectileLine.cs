using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    public static ProjectileLine S;
    public float minDist = 0.1f;
    public bool ________________________;
    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;

    void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    public GameObject poi {
        get {
            return(_poi);
        }
        set {
            _poi = value;
            if(_poi != null) {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //添加一个点
        //如果该点与上一个点的距离不够远，返回
        Vector3 pt = _poi.transform.position;
        if(points.Count > 0 && (pt -lastPoint).magnitude < minDist) {
            return;
        }
        //对于发射点
        if(points.Count == 0) {
            Vector3 launchPos = Slingshot.S.launchPoint.transform.position;
            Vector3 launchPosDiff = pt - launchPos;
            // 添加一个线条帮助瞄准
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        } else {
            points.Add(pt);
            line.SetVertexCount(points.Count);
            line.SetPosition(points.Count-1, lastPoint);
            line.enabled = true;
        }
    }

    //返回最近添加点的位置
    public Vector3 lastPoint{
        get {
            if(points == null) {
                return(Vector3.zero);
            }
            return(points[points.Count - 1]);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(poi == null) {
            //如果兴趣点不存在，就找出一个
            if(FollowCam.S.poi != null) {
                if(FollowCam.S.poi.tag == "Projectile") {
                    poi = FollowCam.S.poi;
                } else {
                    return; //未找到兴趣点
                }
            } else {
                return;
            }
        }
        AddPoint();
        if(poi.GetComponent<Rigidbody>().IsSleeping()) {
            poi = null;
        }
    }
}
