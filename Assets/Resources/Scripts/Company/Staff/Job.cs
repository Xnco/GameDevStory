using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDS
{
    public enum JobType
    {
        Coder = 101, // 程序员
        Writer,        // 脚本策划
        Designer,   // 图像师
        Snd_Eng,  // 音效师
        Director,    // 总监
        Producer,  // 制片人
        Hardware, // 硬件工程师
        Hacker,     // 黑客
    }

    public class Job
    {
        public JobType mType;
        public int mLevel;     // 等级
        public float mCurExp;    // 当前职业经验          
        public int mMaxLevel;   // 最高级
        public JobLevelUpInfo mInfo; // 职业信息
    }

    public struct JobInfo
    {
        // 职业每级对应的升级的信息
        Dictionary<int, JobLevelUpInfo> mAllLevelInfo;
    }

    public struct JobLevelUpInfo
    {
        // 升级需要经验
        public float mNeedExp;

        public float mProgram;     // 编程加成
        public float mScenario;    // 脚本加成
        public float mGraphics;    // 图像加成
        public float mSound;        // 声音加成
    }

}