using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            UIEventListener.Get(startBtn.gameObject).onClick += OnClickLoad;
        }
    }

    void OnClickOpenCreate(GameObject go)
    {
        mCreatePlane.gameObject.SetActive(true);
        //UICamera.Notify(mInput.gameObject, "OnPress", false);
        mInput.GetComponent<UIInput>().isSelected = true;
    }

    void OnClickCreate(GameObject go)
    {
        //创建一家新的公司

        OpenMain();
    }

    void OnClickLoad(GameObject go)
    {
        // 加载存档中的公司

        OpenMain();
    }

    void OpenMain()
    {
        WindowManager.getSingle().OpenWindow("UI/UIMain");

        WindowManager.getSingle().CloseWindow("UIMain");
    }
}
