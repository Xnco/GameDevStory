using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDS;

public class UIMain : MonoBehaviour {

    Transform YearLabel;
    Transform MonthLabel;
    //Transform WeekLabel;
    Transform DayLabel;
    Transform GoldLabel;

    Transform mMenu;
    Transform mSecondMenu;

    EventManager manager;

    private WorldTime worldTime;
    private Company company;

    void Start () {

        worldTime = WorldTime.GetSingleon();
        company = Company.GetSingleon();

        // 初始化 UI 
        Init();
        // 初始化事件
        AddDelegate();
        // 初始化底部 UI
        MainBottom();
    }

    // 销毁解注册
    void OnDestroy()
    {
        ClearDelegate();
    }

    void Init()
    {
        YearLabel = transform.Find("Top/Year");
        MonthLabel = transform.Find("Top/Month");
        //WeekLabel = transform.Find("Top/Week");
        DayLabel = transform.Find("Top/Day");
        GoldLabel = transform.Find("Top/Gold");
        mMenu = transform.Find("Menu");

        //List<string> menulist = new List<string>() {"Develop", "Staff", "Action", "Info" , "System" };
        UpdateYear(worldTime.pCurYear);
        UpdateMonth(worldTime.pCurMonth);
        UpdateDay(worldTime.pCurDay);
        UpdateGold(company.pGold);

        // 开发按钮
        Transform dev = mMenu.Find("Grid/Develop");
        if (dev != null)
        {
            Transform second = dev.Find("Menu");
            if (second != null)
            {
                UIEventListener.Get(dev.gameObject).onClick += (go) => {
                    UIHelper.SetActive(mSecondMenu, false);

                    mSecondMenu = second; // 接受二级菜单
                    UIHelper.SetActive(second, true);
                };

                Transform newGame = second.Find("NewGame");
                if (newGame != null)
                {
                    // 打开新游戏界面
                    //UIEventListener.Get(newGame.gameObject).onClick += (go) => 
                }
                Transform outsourcing = second.Find("Outsourcing");
                if (outsourcing != null)
                {
                    // 打开外包界面
                    //UIEventListener.Get(outsourcing.gameObject).onClick += (go) => 
                }
            }
        }
    }

    // 注册事件
    void AddDelegate()
    {
        manager = EventManager.GetSinglon();

        manager.RegisterMsgHandler((int)PlayerEvent.PE_UpdateYear, OnRes_UpdateYear);
        manager.RegisterMsgHandler((int)PlayerEvent.PE_UpdateMonth, OnRes_UpdateMonth);
        manager.RegisterMsgHandler((int)PlayerEvent.PE_UpdateWeek, OnRes_UpdateWeek);
        manager.RegisterMsgHandler((int)PlayerEvent.PE_UpdateDay, OnRes_UpdateDay);
        manager.RegisterMsgHandler((int)PlayerEvent.PE_UpdateGold, OnRes_UpdateGold);
    }

    void ClearDelegate()
    {
        manager.UnRegisterMsgHandler((int)PlayerEvent.PE_UpdateYear, OnRes_UpdateYear);
        manager.UnRegisterMsgHandler((int)PlayerEvent.PE_UpdateMonth, OnRes_UpdateMonth);
        manager.UnRegisterMsgHandler((int)PlayerEvent.PE_UpdateWeek, OnRes_UpdateWeek);
        manager.UnRegisterMsgHandler((int)PlayerEvent.PE_UpdateDay, OnRes_UpdateDay);
        manager.UnRegisterMsgHandler((int)PlayerEvent.PE_UpdateGold, OnRes_UpdateGold);
    }

    void OnRes_UpdateYear(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        UpdateYear(data.data);
    }

    void OnRes_UpdateMonth(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        UpdateMonth(data.data);
    }

    void OnRes_UpdateWeek(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        UpdateWeek(data.data);
    }

    void OnRes_UpdateDay(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        UpdateDay(data.data);
    }

    void OnRes_UpdateGold(BaseEvent varData)
    {
        if (varData == null) return;

        ExData<int> data = varData as ExData<int>;
        string text = UIHelper.GetSeparatorNumber(data.data);
        UpdateGold(data.data);
    }

    void UpdateYear(int year)
    {
        if(YearLabel != null)
            UIHelper.SetLabel(YearLabel, year.ToString());
    }

    void UpdateMonth(int month)
    {
        if (MonthLabel != null)
            UIHelper.SetLabel(MonthLabel, month.ToString());
    }

    void UpdateWeek(int week)
    {
        //UIHelper.SetLabel(WeekLabel, week.ToString());
    }

    void UpdateDay(int day)
    {
        if(DayLabel != null)
            UIHelper.SetLabel(DayLabel, day.ToString());
    }

    void UpdateGold(int gold)
    {
        if(GoldLabel != null)
            UIHelper.SetLabel(GoldLabel, gold.ToString());
    }

    /// <summary>
    /// 主界面的最开始的Bottom
    /// </summary>
    void MainBottom()
    {
        ExData<PE_UpdateBottomStruct> data = new ExData<PE_UpdateBottomStruct>();
        data.pEventID = (int)PlayerEvent.PE_OpenWindow_UpdateBottom;
        data.data = new PE_UpdateBottomStruct();
        data.data.bIsReplace = false;
        data.data.left = "保存";
        data.data.onClickLeft = ResourcesManager.GetSingle().Sava;
        data.data.right = "菜单";
        data.data.onClickRight = OpenMenu;

        manager.NotifyEvent(data.pEventID, data);
    }

    /// <summary>
    /// 菜单的Bottom
    /// </summary>
    void MenuBottom()
    {
        ExData<PE_UpdateBottomStruct> data = new ExData<PE_UpdateBottomStruct>();
        data.pEventID = (int)PlayerEvent.PE_OpenWindow_UpdateBottom;
        data.data = new PE_UpdateBottomStruct();
        data.data.bIsReplace = false;
        data.data.left = "保存";
        data.data.onClickLeft = ResourcesManager.GetSingle().Sava;
        data.data.right = "返回";
        data.data.onClickRight = CloseMenu;

        manager.NotifyEvent(data.pEventID, data);
    }

    void OpenMenu()
    {
        UIHelper.SetActive(mMenu, true);

        // 打开菜单游戏暂停
        MainLogic.GetSingleon().Pause(true);

        // 打开菜单后 更新底部
        MenuBottom();
    }

    void CloseMenu()
    {
        if (mSecondMenu == null)
        {
            UIHelper.SetActive(mMenu, false);
            manager.NotifyEvent((int)PlayerEvent.PE_CloseWindow_UpdateBottm, null);
            MainLogic.GetSingleon().Pause(false);
        }
        else
        {
            UIHelper.SetActive(mSecondMenu, false);
            mSecondMenu = null;
        }
    }
}
