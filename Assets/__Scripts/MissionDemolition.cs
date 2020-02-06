using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode {
        idle,
        playing,
        levelEnd
    }

public class MissionDemolition : MonoBehaviour
{
    public static MissionDemolition S;

    public GameObject[] castles;
    public GUIText gtLevel;
    public GUIText gtScore;
    public Vector3 castlePos;

    public bool _________________________;
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Slingshot";

    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel() 
    {
        //如果有城堡，则清除原有的城堡
        if(castle != null) {
            Destroy(castle);
        }
        //清除原有的炮弹
        GameObject gos = GameObject.FindGameObjectWithTag("Projectile");
        Destroy(gos);
        // foreach (GameObject pTemp in gos)
        // {
        //     Destroy(pTemp);
        // }
        //实例化新城堡
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;
        //重置摄像头的位置
        SwitchView("Both");
        ProjectileLine.S.Clear();
        //重置目标状态
        Goal.goalMet = false;
        showGT();
        mode = GameMode.playing;
    }

    //设置文字界面
    void showGT() 
    {
        gtLevel.text = "Level: " + (level + 1) + " of" + levelMax;
        gtScore.text = "Shots Taken: " + shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        showGT();
        //检查是否已经完成该级别,如果达成则改变mode，并进入下一等级
        if(mode == GameMode.playing && Goal.goalMet) {
            mode = GameMode.levelEnd;
            SwitchView("Both");
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel() 
    {
        level++;
        if(level == levelMax) {
            level = 0;
        }
        StartLevel();
    }

    void OnGUI() {
        //在屏幕顶端绘制界面按钮，用于切换视图
        Rect buttonRect = new Rect((Screen.width/2)-50, 10, 100, 24);
        switch (showing)
        {
            case "Castle":
                if(GUI.Button(buttonRect, "查看城堡")) {
                    SwitchView("Castle");
                }
                break;
            case "Both":
                if(GUI.Button(buttonRect, "查看全部")) {
                    SwitchView("Both");
                }
                break;
            case "Slingshot":
                if(GUI.Button(buttonRect, "查看弹弓")) {
                    SwitchView("Slingshot");
                }
                break;
        }
    }

    public static void SwitchView(string eView) 
    {
        S.showing = eView;
        switch (S.showing) {
            case "Slingshot":
                FollowCam.S.poi = null;
                break;
            case "Castle":
                FollowCam.S.poi = S.castle;
                break;
            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
        }
    }

    public static void ShotFired() 
    {
        S.shotsTaken++;
    }
}
