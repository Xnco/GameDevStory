using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDS
{
    public struct Platform
    {
        public int mNum;
        public string mName;
        public string mCo;       // 开发公司
        public int mLicense;    // 版权费
        public int mSales;
        public int mCost;       // 开发基础费用
        public int mDevMult; // 开发费用系数 - 影响开发游戏的费用
        public int mCreateYear;
        public int mCreateMonth;
        public int mCreateWeek;
    }

    // 游戏类型
    public struct GameGenre
    {
        public int mNum;
        public string mName;
        public int mCost;
        public List<int> mAmazings;
        public List<int> mBads;
    }

    // 游戏内容
    public struct GameType
    {
        public int mNum;
        public string mName;
        public int mCost;
    }
}

