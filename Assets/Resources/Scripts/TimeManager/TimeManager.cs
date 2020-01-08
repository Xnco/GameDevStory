using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    EventManager eventManager;

    // 游戏主流程的时间控制
    [SerializeField]
    private bool bGameIsPause = true;
    [SerializeField]
    private float GameTimeScale = 1.0f;

    private float PauseTime;    // 记录游戏暂停时间
    private float RestartTime;  // 游戏恢复时间

    private float CurGameTime;
    private float DayBeginSecond;

    private void Start()
    {
        eventManager = EventManager.GetSinglon();
        AddDelegate();
    }

    private void OnDestroy()
    {
        ClearDelegate();
    }

    void AddDelegate()
    {
        eventManager.RegisterMsgHandler((int)PlayerEvent.PE_GameStart, OnRes_GameStart);
    }

    void ClearDelegate()
    {
        eventManager.UnRegisterMsgHandler((int)PlayerEvent.PE_GameStart, OnRes_GameStart);
    }

    void Update()
    {
        if (!bGameIsPause)
        {
            CurGameTime += Time.deltaTime * GameTimeScale;
            if (CurGameTime - DayBeginSecond >= 1.0f)
            {
                DayBeginSecond = CurGameTime;

                BaseEvent data = new BaseEvent();
                data.pEventID = (int)PlayerEvent.PE_MainTimeKey;
                eventManager.NotifyEvent(data.pEventID, data);
            }
        }
    }

    void OnRes_GameStart(BaseEvent data)
    {
        bGameIsPause = false;
    }

    public void GamePause(bool bIsPause)
    {
        if(bIsPause)
        {
            PauseTime = Time.time;
            bGameIsPause = true;
        }
        else
        {
            RestartTime = Time.time;
            bGameIsPause = false;

            Debug.Log($"Game restart!! Pause time : {RestartTime - PauseTime}");
        }
    }
}
