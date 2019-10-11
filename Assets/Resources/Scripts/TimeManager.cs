using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    EventManager eventManager;

    // 游戏主流程的时间控制
    [SerializeField]
    private bool bMainIsPause = false;

    private float mainKeyTime;      // 关键时间, 游戏内过1s
    private float mainRealTime;     // 游戏真实时间
    private float mainPauseTime;    // 记录游戏暂停时间
    private float mainRestartTime;  // 游戏恢复时间

    void Start()
    {
        eventManager = EventManager.GetSinglon();
    }

    void Update()
    {
        if (!bMainIsPause)
        {
            mainRealTime += Time.deltaTime;
            if (mainRealTime - mainKeyTime >= 1)
            {
                mainKeyTime++;

                BaseEvent data = new BaseEvent();
                data.pEventID = (int)PlayerEvent.PE_MainTimeKey;
                eventManager.NotifyEvent(data.pEventID, data);
            }
        }
    }

    public void GamePause()
    {
        mainPauseTime = Time.time;
        bMainIsPause = true;
    }

    public void GameRestart()
    {
        mainRestartTime = Time.time;
        bMainIsPause = false;

        Debug.Log($"Game restart!! Pause time : {mainRestartTime - mainPauseTime}");
    }
}
