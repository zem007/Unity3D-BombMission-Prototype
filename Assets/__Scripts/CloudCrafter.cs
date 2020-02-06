using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    public int numClouds = 40;
    public GameObject[] cloudPrefabs;
    public Vector3 cloudPosMin;
    public Vector3 cloudPosMax;
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 5;
    public float cloudSpeedMult = 0.5f;
    public bool ______________________;
    public GameObject[] cloudInstances;

    void Awake()
    {
        cloudInstances = new GameObject[numClouds];
        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;
        for(int i=0; i<numClouds; i++) {
            //随机选取5个云预设中的1个，然后实例化
            int prefabNum = Random.Range(0, cloudPrefabs.Length);
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
            //云的位置
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //设置云的缩放
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //小云朵离地面近
            cPos.y = Mathf.Lerp(cloudScaleMin, cPos.y, scaleU);
            //较小的云距离较远
            cPos.z = 100 - 90*scaleU;
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            cloud.transform.parent = anchor.transform;
            cloudInstances[i] = cloud;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject cloud in cloudInstances) {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //云越大，移动速度越快
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //如果云已经位于画面左侧较远的位置
            if(cPos.x <= cloudPosMin.x) {
                cPos.x = cloudPosMax.x;
            }
            cloud.transform.position = cPos;
        }
    }
}
