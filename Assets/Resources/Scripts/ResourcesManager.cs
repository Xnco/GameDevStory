using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GDS;

public class ResourcesManager
{
    private static ResourcesManager instance;

    public static ResourcesManager GetSingle()
    {
        if (instance == null)
        {
            instance = new ResourcesManager();
        }
        return instance;
    }

    private ResourcesManager()
    {
        LoadGameInfo();
        LoadPlatform();
        LoadJobInfo();
        LoadStaffInfo();
    }

    Dictionary<int, GameGenre> mAllGenre;
    Dictionary<int, GameType> mAllType;
    Dictionary<int, Platform> mAllPlatform;
    Dictionary<int, JobInfo> mAllJob;
    Dictionary<int, Staff> mAllStaff;

    // 读取游戏类型和内容
    public void LoadGameInfo()
    {
        mAllGenre = new Dictionary<int, GameGenre>();
        mAllType = new Dictionary<int, GameType>();

#if UNITY_EDITOR
        string path = Application.dataPath + "/StreamingAssets/Xml/GameInfo.xml";
#elif UNITY_ANDROID
        string path = Application.streamingAssetsPath + "/Xml/GameInfo.xml";
#endif

        XmlDocument xd = new XmlDocument();
        xd.Load(path);

        #region 得到所有的'类型'
        XmlNodeList glist = xd.SelectNodes("GameInfo/GerenList/Geren");
        foreach (XmlNode gxn in glist)
        {
            GameGenre gg = new GameGenre();
            gg.mNum = int.Parse(gxn.SelectSingleNode("Num").InnerText);
            gg.mName = gxn.SelectSingleNode("Name").InnerText;
            gg.mCost = int.Parse(gxn.SelectSingleNode("Cost").InnerText);

            // 杰作列表
            gg.mAmazings = new List<int>();
            XmlNodeList amazing = gxn.SelectNodes("Amazing/Num");
            foreach (XmlNode tmpa in amazing)
            {
                int num = int.Parse(tmpa.InnerText);
                gg.mAmazings.Add(num);
            }

            // 糟糕组合列表
            gg.mBads = new List<int>();
            XmlNodeList bad = gxn.SelectNodes("Bad/Num");
            foreach (XmlNode tmpb in bad)
            {
                int num = int.Parse(tmpb.InnerText);
                gg.mBads.Add(num);
            }

            mAllGenre.Add(gg.mNum, gg);
        }
        #endregion

        #region 得到所有的'内容'

        XmlNodeList tlist = xd.SelectNodes("GameInfo/TypeList/Type");
        foreach (XmlNode txn in tlist)
        {
            GameType gt = new GameType();
            gt.mNum = int.Parse(txn.SelectSingleNode("Num").InnerText);
            gt.mName = txn.SelectSingleNode("Name").InnerText;
            gt.mCost = int.Parse(txn.SelectSingleNode("Cost").InnerText);

            mAllType.Add(gt.mNum, gt);
        }
        #endregion
    }

    // 读取平台固定信息
    public void LoadPlatform()
    {
        mAllPlatform = new Dictionary<int, Platform>();

#if UNITY_EDITOR
        string path = Application.dataPath + "/StreamingAssets/Xml/Platform.xml";
#elif UNITY_ANDROID
        string path = Application.streamingAssetsPath + "/Xml/Platform.xml";
#endif

        XmlDocument xd = new XmlDocument();
        xd.Load(path);

        XmlNodeList plist = xd.SelectNodes("Platforms/Platform");
        foreach (XmlNode pxn in plist)
        {
            Platform tmpP = new Platform();
            tmpP.mNum = int.Parse(pxn.SelectSingleNode("Num").InnerText);
            tmpP.mName =  pxn.SelectSingleNode("Name").InnerText;
            tmpP.mCo = pxn.SelectSingleNode("Co").InnerText;
            tmpP.mLicense = int.Parse(pxn.SelectSingleNode("License").InnerText);
            tmpP.mSales = int.Parse(pxn.SelectSingleNode("Sales").InnerText);
            tmpP.mCost = int.Parse(pxn.SelectSingleNode("Cost").InnerText);
            tmpP.mDevMult = int.Parse(pxn.SelectSingleNode("DevMult").InnerText);

            string[] times = pxn.SelectSingleNode("Time").InnerText.Split('-');
            tmpP.mCreateYear = int.Parse(times[0]);
            tmpP.mCreateMonth = int.Parse(times[1]);
            tmpP.mCreateWeek = int.Parse(times[2]);

            mAllPlatform.Add(tmpP.mNum, tmpP);
        }
    }

    // 读取职业固定信息
    public void LoadJobInfo()
    {
        mAllJob = new Dictionary<int, JobInfo>();

#if UNITY_EDITOR
        string path = Application.dataPath + "/StreamingAssets/Xml/Job.xml";
#elif UNITY_ANDROID
        string path = Application.streamingAssetsPath + "/Xml/Job.xml";
#endif

        XmlDocument xd = new XmlDocument();
        xd.Load(path);

        XmlNodeList jlist = xd.SelectNodes("JobList/Job");
        foreach (XmlNode jxn in jlist)
        {
            JobInfo ji = new JobInfo();
            ji.mNum = int.Parse(jxn.SelectSingleNode("Num").InnerText);
            ji.mName = jxn.SelectSingleNode("Name").InnerText;
            ji.mMaxLevel = int.Parse(jxn.SelectSingleNode("MaxLv").InnerText);

            ji.mAllLevelInfo = new Dictionary<int, JobLevelUpInfo>();
            XmlNodeList alljn = jxn.SelectNodes("Info/Lv");
            foreach (XmlNode tmpI in alljn)
            {
                JobLevelUpInfo jnlu = new JobLevelUpInfo();
                jnlu.mLevel = int.Parse(tmpI.SelectSingleNode("Level").InnerText);
                jnlu.mNeedExp = int.Parse(tmpI.SelectSingleNode("Exp").InnerText);
                jnlu.mProgram = int.Parse(tmpI.SelectSingleNode("Program").InnerText);
                jnlu.mScenario = int.Parse(tmpI.SelectSingleNode("Scenario").InnerText);
                jnlu.mGraphics = int.Parse(tmpI.SelectSingleNode("Graphics").InnerText);
                jnlu.mSound = int.Parse(tmpI.SelectSingleNode("Sound").InnerText);

                ji.mAllLevelInfo.Add(jnlu.mLevel, jnlu);
            }

            mAllJob.Add(ji.mNum, ji);
        }
    }

    // 读取员工固定信息
    public void LoadStaffInfo()
    {
        mAllStaff = new Dictionary<int, Staff>();

#if UNITY_EDITOR
        string path = Application.dataPath + "/StreamingAssets/Xml/Staff.xml";
#elif UNITY_ANDROID
        string path = Application.streamingAssetsPath + "/Xml/Staff.xml";
#endif

        XmlDocument xd = new XmlDocument();
        xd.Load(path);

        XmlNodeList slist = xd.SelectNodes("StaffList/Staff");
        foreach (XmlNode sxn in slist)
        {
            Staff staff = new Staff();

            staff.mNum = int.Parse(sxn.SelectSingleNode("Num").InnerText);
            staff.mName = sxn.SelectSingleNode("Name").InnerText;

            staff.mSalary = float.Parse(sxn.SelectSingleNode("Salary").InnerText);
            staff.mPower = float.Parse(sxn.SelectSingleNode("Power").InnerText);

            staff.mProgram = int.Parse(sxn.SelectSingleNode("Program").InnerText);
            staff.mMaxProgram = int.Parse(sxn.SelectSingleNode("MaxProgram").InnerText);
            staff.mScenario = int.Parse(sxn.SelectSingleNode("Scenario").InnerText);
            staff.mMaxScenario = int.Parse(sxn.SelectSingleNode("MaxScenario").InnerText);
            staff.mGraphics = int.Parse(sxn.SelectSingleNode("Graphics").InnerText);
            staff.mMaxGraphics = int.Parse(sxn.SelectSingleNode("MaxGraphics").InnerText);
            staff.mSound = int.Parse(sxn.SelectSingleNode("Sound").InnerText);
            staff.mMaxSound = int.Parse(sxn.SelectSingleNode("MaxSound").InnerText);

            staff.mTalent = float.Parse( sxn.SelectSingleNode("Talent").InnerText);
            staff.mDiligent = float.Parse(sxn.SelectSingleNode("Diligent").InnerText);
            staff.mEffect = float.Parse(sxn.SelectSingleNode("Effect").InnerText);
            staff.mStrata = int.Parse(sxn.SelectSingleNode("Strata").InnerText);

            mAllStaff.Add(staff.mNum, staff);
        }

    }

    public void LoadNewSava()
    {
        string path = Application.dataPath + "/StreamingAssets/Json/NewSavs.json";
    }
}

public class JsonParser
{
    public string Name;
}