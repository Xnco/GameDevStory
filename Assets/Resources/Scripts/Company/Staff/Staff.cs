using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDS
{
    public class Staff
    {
        public int mNum;        // 编号
        public string mName;     // 名字
        public JobInfo mCurJob;     // 职位
        public List<JobInfo> mJobs;    // 职业生涯
        public float mSalary;      // 薪水
        public float mPower;      // 体力
        public int mCurLv;  // 当前等级 
        public int mCurExp;    // 当前经验

        // 基本四维
        public float mProgram;     // 编程
        public float mMaxProgram;
        public float mScenario;    // 脚本
        public float mMaxScenario;
        public float mGraphics;    // 图像
        public float mMaxGraphics;
        public float mSound;        // 声音
        public float mMaxSound;

        // 隐性属性
        public float mTalent;      // 天赋, 隐藏四维加成
        public float mDiligent;    // 勤奋程度, 影响工作频率
        public float mEffect;   // 效率, 影响工作速度
        public int mStrata;    // 阶层, 影响招人渠道

        public float pProgram
        {
            get
            {
                // 显示能力为 人物能力 + 职业加成能力 + ...
                return mProgram + mCurJob.pProgram;
            }
        }

        public Staff()
        {
            mJobs = new List<JobInfo>();
        }
    }

}
