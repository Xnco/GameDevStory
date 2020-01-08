using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIBottom : MonoBehaviour
{
    private Transform mLeftBtn;
    private Transform mRightBtn;

    private UnityAction onClickLeft;
    public UnityAction onClickRight;

    Stack<PE_UpdateBottomStruct> FunctionStack;

    EventManager eventManager;

    void Start()
    {
        FunctionStack = new Stack<PE_UpdateBottomStruct>();
        mLeftBtn = transform.Find("Left");
        if (mLeftBtn != null)
        {
            UIEventListener.Get(mLeftBtn.gameObject).onClick += OnClickLeft;
        }

        mRightBtn = transform.Find("Right");
        if (mRightBtn != null)
        {
            UIEventListener.Get(mRightBtn.gameObject).onClick += OnClickRight;
        }

        eventManager = EventManager.GetSinglon();
        eventManager.RegisterMsgHandler((int)PlayerEvent.PE_OpenWindow_UpdateBottom, OpenWindow_UpdateBottom);
        eventManager.RegisterMsgHandler((int)PlayerEvent.PE_CloseWindow_UpdateBottm, CloseWindow_UpdateBottom);
    }

    private void OnDestroy()
    {
        eventManager.UnRegisterMsgHandler((int)PlayerEvent.PE_OpenWindow_UpdateBottom, OpenWindow_UpdateBottom);
        eventManager.UnRegisterMsgHandler((int)PlayerEvent.PE_CloseWindow_UpdateBottm, CloseWindow_UpdateBottom);
    }

    // bIsReplace: ture为替换界面模式, 将上个同级界面关闭, 底部菜单也一同替换
    void OpenWindow_UpdateBottom(BaseEvent varData)
    {
        if (varData == null)
        {
            return;
        }

        ExData<PE_UpdateBottomStruct> data = varData as ExData<PE_UpdateBottomStruct>;
        if(data.data.bIsReplace && FunctionStack.Count != 0)
        {
            FunctionStack.Pop();
        }
        FunctionStack.Push(data.data);

        UpdateBottom(FunctionStack.Peek());
    }

    void CloseWindow_UpdateBottom(BaseEvent varData)
    {
        if(FunctionStack.Count == 0)
        {
            return;
        }

        FunctionStack.Pop();
        UpdateBottom(FunctionStack.Peek());
    }

    void UpdateBottom(PE_UpdateBottomStruct varData)
    {
        if (string.IsNullOrEmpty(varData.left))
        {
            UIHelper.SetActive(mLeftBtn, false);
            onClickLeft = null;
        }
        else
        {
            UIHelper.SetActive(mLeftBtn, true);
            UIHelper.SetLabel(mLeftBtn, "Label", varData.left);
            onClickLeft = varData.onClickLeft;
        }

        if (string.IsNullOrEmpty(varData.right))
        {
            UIHelper.SetActive(mRightBtn, false);
            onClickRight = null;
        }
        else
        {
            UIHelper.SetActive(mRightBtn, true);
            UIHelper.SetLabel(mRightBtn, "Label", varData.right);
            onClickRight = varData.onClickRight;
        }
    }

    void OnClickLeft(GameObject go)
    {
        if (onClickLeft != null)
        {
            onClickLeft();
        }
    }

    void OnClickRight(GameObject go)
    {
        if (onClickRight != null)
        {
            onClickRight();
        }
    }

}
