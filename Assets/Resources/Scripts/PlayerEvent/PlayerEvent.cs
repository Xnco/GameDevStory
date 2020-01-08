using UnityEngine;
using System.Collections;

public enum PlayerEvent
{
    // 全能游戏设计师
    // Bottom
    PE_OpenWindow_UpdateBottom, // 打开界面_更新底部菜单
    PE_CloseWindow_UpdateBottm, // 关闭界面_更新底部菜单

    // Global Event
    PE_GameStart,
    PE_GamePause,     // bool

    // Time
    PE_MainTimeKey,

    // Main
    PE_UpdateYear, 
    PE_UpdateMonth,
    PE_UpdateWeek,
    PE_UpdateDay,
    PE_UpdateGold,
}
