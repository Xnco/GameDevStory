using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDS;

public class UIMain : MonoBehaviour {

    Transform mYear;
    Transform mMonth;
    Transform mWeek;
    Transform mDay;
    Transform mGold;

    Transform mMenu;

    EventManager manager;
    Company company;
    
    void Start () {
        company = Company.GetSingle();
        // 初始化 UI 
        Init();
        // 初始化事件
        InitEvent();
        // 初始化底部 UI
        MainBottom();

        // 开始游戏 / 时间开始流逝
        StartCoroutine(company.Timing());
    }

    void Init()
    {
        mYear = transform.Find("Top/Year");
        UIHelper.SetLabel(mYear, company.pCurYear.ToString());
        mMonth = transform.Find("Top/Month");
        UIHelper.SetLabel(mMonth, company.pCurMonth.ToString());
        mWeek = transform.Find("Top/Week");
        UIHelper.SetLabel(mWeek, company.pCurWeek.ToString());
        mDay = transform.Find("Top/Day");
        UIHelper.SetSlider(mDay, company.pCurDay/10f);
        mGold = transform.Find("Top/Gold");
        UIHelper.SetLabel(mGold, UIHelper.GetSeparatorNumber(company.pGold));

        mMenu = transform.Find("Menu");
    }

    // 注册事件
    void InitEvent()
    {
        manager = EventManager.GetSinglon();

        manager.RegisterMsgHandler((int)PlayerEvent.UpdateYear, UpdateYear);
        manager.RegisterMsgHandler((int)PlayerEvent.UpdateMonth, UpdateMonth);
        manager.RegisterMsgHandler((int)PlayerEvent.UpdateWeek, UpdateWeek);
        manager.RegisterMsgHandler((int)PlayerEvent.UpdateDay, UpdateDay);
        manager.RegisterMsgHandler((int)PlayerEvent.UpdateGold, UpdateGold);
    }

    void UpdateYear(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        UIHelper.SetLabel(mYear, data.data.ToString());
    }

    void UpdateMonth(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        UIHelper.SetLabel(mMonth, data.data.ToString());
    }

    void UpdateWeek(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        UIHelper.SetLabel(mWeek, data.data.ToString());
    }

    void UpdateDay(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        UIHelper.SetSlider(mDay, data.data/10f);
    }

    void UpdateGold(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        string text = UIHelper.GetSeparatorNumber(data.data);
        UIHelper.SetLabel(mGold, text);
    }

    // 主界面的Bottom
    void MainBottom()
    {
        ExData<PE_UpdateBottom> data = new ExData<PE_UpdateBottom>();
        data.pEventID = (int)PlayerEvent.UpdateBottom;
        data.data = new PE_UpdateBottom();
        data.data.left = "保存";
        data.data.onClickLeft = ResourcesManager.GetSingle().Sava;
        data.data.right = "菜单";
        data.data.onClickRight = OpenMenu;

        manager.NotifyEvent(data.pEventID, data);
    }

    // 打开菜单
    void OpenMenu()
    {
        UIHelper.SetActive(mMenu, true);
        Time.timeScale = 0; // 打开菜单游戏暂停
    }
	
    // 销毁解注册
    void OnDestroy()
    {
        manager.UnRegisterMsgHandler((int)PlayerEvent.UpdateYear, UpdateYear);
        manager.UnRegisterMsgHandler((int)PlayerEvent.UpdateMonth, UpdateMonth);
        manager.UnRegisterMsgHandler((int)PlayerEvent.UpdateWeek, UpdateWeek);
        manager.UnRegisterMsgHandler((int)PlayerEvent.UpdateDay, UpdateDay);
        manager.UnRegisterMsgHandler((int)PlayerEvent.UpdateGold, UpdateGold);
    }
}
