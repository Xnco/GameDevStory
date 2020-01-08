using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnClickBottom();

public class UIBottom : MonoBehaviour
{
    private Transform mLeftBtn;
    private Transform mRightBtn;

    private OnClickBottom onClickLeft;
    public OnClickBottom onClickRight;

    void Start()
    {
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

        EventManager.GetSinglon().RegisterMsgHandler((int)PlayerEvent.PE_UpdateBottom, UpdateBottom);
    }

    /// <summary>
    /// 更新 Bottom 的文本和 点击事件
    /// </summary>
    /// <param name="varData"></param>
    void UpdateBottom(BaseEvent varData)
    {
        if (varData == null)
        {
            return;
        }

        ExData<PE_UpdateBottomStruct> data = varData as ExData<PE_UpdateBottomStruct>;

        if (string.IsNullOrEmpty(data.data.left))
        {
            UIHelper.SetActive(mLeftBtn, false);
            onClickLeft = null;
        }
        else
        {
            UIHelper.SetActive(mLeftBtn, true);
            UIHelper.SetLabel(mLeftBtn, "Label", data.data.left);
            onClickLeft = data.data.onClickLeft;
        }

        if (string.IsNullOrEmpty(data.data.right))
        {
            UIHelper.SetActive(mRightBtn, false);
            onClickRight = null;
        }
        else
        {
            UIHelper.SetActive(mRightBtn, true);
            UIHelper.SetLabel(mRightBtn, "Label", data.data.right);
            onClickRight = data.data.onClickRight;
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
