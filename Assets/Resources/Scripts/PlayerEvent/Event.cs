﻿/*----------------------------------------------------------------
            // Copyright © 2015 NCSpeedLight
            // 
            // FileName: Event.cs
			// Describle:事件基类
			// Created By:  meixuan.fu
			// Date&Time:  2016/1/19 10:03:15
            // Modify History:
            //
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

public struct PE_UpdateBottom
{
    public string left;
    public OnClickBottom onClickLeft;

    public string right;
    public OnClickBottom onClickRight;
}

