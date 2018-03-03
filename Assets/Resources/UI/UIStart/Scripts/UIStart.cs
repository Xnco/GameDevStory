using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDS;

public class UIStart : MonoBehaviour
{

    Transform mCreatePlane;
    Transform mInput;

    void Start()
    {
        mCreatePlane = transform.Find("CreateCompany");
        mInput = mCreatePlane.Find("Input");
        Transform mBack = mCreatePlane.Find("Back");
        if (mBack != null)
        {
            UIEventListener.Get(mBack.gameObject).onClick += (go)=> mCreatePlane.gameObject.SetActive(false);
        }

        Transform mOK = mCreatePlane.Find("OK");
        if (mOK != null)
        {
            UIEventListener.Get(mOK.gameObject).onClick += OnClickCreate;
        }

        Transform startBtn = transform.Find("StartBtn");
        if (startBtn != null)
        {
            UIEventListener.Get(startBtn.gameObject).onClick += OnClickOpenCreate;
        }

        Transform loadBtn = transform.Find("LoadBtn");
        if (loadBtn != null)
        {
            UIEventListener.Get(loadBtn.gameObject).onClick += OnClickLoad;
        }

        // 按右边就退出游戏
        ExData<PE_UpdateBottom> data = new ExData<PE_UpdateBottom>();
        data.data = new PE_UpdateBottom();
        data.pEventID = (int)PlayerEvent.UpdateBottom;
        data.data.right = "退出游戏";
        data.data.onClickRight = Quit;

        EventManager.GetSinglon().NotifyEvent(data.pEventID, data);
    }

    void OnClickOpenCreate(GameObject go)
    {
        mCreatePlane.gameObject.SetActive(true);
        //UICamera.Notify(mInput.gameObject, "OnPress", false);
        mInput.GetComponent<UIInput>().isSelected = true;
    }

    void OnClickCreate(GameObject go)
    {
        // 创建一家新的公司 -> 名字是新的, 加载默认存档
        ResourcesManager.GetSingle().LoadNewSava(true);
        // 新游戏，公司名为新的 
        Company.GetSingle().mName = mInput.GetComponent<UIInput>().text;

        OpenMain();
    }

    void OnClickLoad(GameObject go)
    {
        // 加载存档中的公司 
        ResourcesManager.GetSingle().LoadNewSava(false);
        OpenMain();
    }

    void OpenMain()
    {
        WindowManager.GetSingle().OpenWindow("UI/UIMain/UIMain");

        WindowManager.GetSingle().CloseWindow("UI/UIStart/UIStart");
    }

    void Quit()
    {
        Application.Quit();
    }
}
