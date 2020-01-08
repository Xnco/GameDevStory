using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBegin : MonoBehaviour {

	// Use this for initialization
	void Start () {

        MainLogic.GetSingleon();

        WindowManager.GetSingle().OpenBottom("UI/UIBottom/UIBottom");

        // 初始化游戏
        // 加载本地固定的数据 
        ResourcesManager.GetSingle();
        WindowManager.GetSingle().OpenWindow("UI/UIStart/UIStart");
        Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
