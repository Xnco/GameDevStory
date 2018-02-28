using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDS
{
    public class Staff
    {
        private string mName;     // 名字
        private Job mCurJob;     // 职位
        private List<Job> mJobs;    // 职业生涯
        private float mSalary;      // 薪水
        private float mPower;      // 体力

        // 基本四维
        private float mProgram;     // 编程
        private float mMaxProgram;
        private float mScenario;    // 脚本
        private float mMaxScenario;
        private float mGraphics;    // 图像
        private float mMaxGraphics;
        private float mSound;        // 声音
        private float mMaxSound;

        // 隐性属性
        private float mTalent;      // 天赋, 隐藏四维加成
        private float mDiligent;    // 勤奋程度, 影响工作频率
        private float mEffect;   // 效率, 影响工作速度
        private int mStrata;    // 阶层, 影响招人渠道

        public float pProgram
        {
            get
            {
                // 显示能力为 人物能力 + 职业加成能力 + ...
                return mProgram + mCurJob.mInfo.mProgram;
            }
        }
    }

}
