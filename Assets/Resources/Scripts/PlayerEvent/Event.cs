
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

public class BaseEvent
{
    protected int mEventID;
    public int pEventID
    {
        get { return mEventID; }
        set { mEventID = value; }
    }
}

public class ExData<T> : BaseEvent
{
    public T data;
}

public struct PE_UpdateBottomStruct
{
    public bool bIsReplace;

    public string left;
    public UnityAction onClickLeft;

    public string right;
    public UnityAction onClickRight;
}

