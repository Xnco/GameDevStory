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

    public struct JobInfo
    {
        private int mCurLevel;     // 等级
        private float mCurExp;    // 当前职业经验       

        public int mNum;    // 职业编号
        public string mName;   
        public int mMaxLevel;   // 最高级
        // 技能的所有等级信息
        public Dictionary<int, JobLevelUpInfo> mAllLevelInfo;

        public int pCurLevel
        {
            get
            {
                return mCurLevel;
            }
            set
            {
                mCurLevel = value > mMaxLevel ? mMaxLevel : value;
            }
        }

        public float pCurExp
        {
            get
            {
                return mCurExp;
            }
            set
            {
                if (value >= pNeedExp && pCurLevel != mMaxLevel)
                {
                    pCurLevel++;
                    mCurExp = 0;
                }
                else
                {
                    mCurExp = value;
                }
            }
        }

        public float pNeedExp
        {
            get { return mAllLevelInfo[pCurLevel].mNeedExp; }
        }

        public float pProgram
        {
            get { return mAllLevelInfo[pCurLevel].mProgram; }
        }

        public float pScenario
        {
            get { return mAllLevelInfo[pCurLevel].mScenario; }
        }

        public float pGraphics
        {
            get { return mAllLevelInfo[pCurLevel].mGraphics; }
        }

        public float pSound
        {
            get { return mAllLevelInfo[pCurLevel].mSound; }
        }
    }

    public struct JobLevelUpInfo
    {
        // 升级需要经验
        public int mLevel;
        public float mNeedExp;

        public float mProgram;     // 编程加成
        public float mScenario;    // 脚本加成
        public float mGraphics;    // 图像加成
        public float mSound;        // 声音加成
    }

}