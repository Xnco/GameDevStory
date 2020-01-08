using System.IO;
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

    Company company;
    WorldTime worldTime;
    Dictionary<int, GameGenre> mAllGenre;
    Dictionary<int, GameType> mAllType;
    Dictionary<int, Platform> mAllPlatform;
    Dictionary<int, JobInfo> mAllJob;
    Dictionary<int, Staff> mAllStaff;

#if UNITY_EDITOR
    string savapath = Application.dataPath + "/StreamingAssets/Json/{0}.json";
#elif UNITY_ANDROID
        string savapath = Application.streamingAssetsPath + "/Json/{0}.json";
#endif

    /// <summary>
    /// 读取游戏类型和内容
    /// </summary>
    public void LoadGameInfo()
    {
        company = Company.GetSingleon();
        worldTime = WorldTime.GetSingleon();
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

    // 加载存档
    public void LoadNewSava(bool isnewgame)
    {
        string path = string.Format(savapath, isnewgame ? AppConst.initSavaPath : AppConst.savaPath);

        StreamReader sr = new StreamReader(path);
        string jsontext = sr.ReadToEnd();   // 获取 Json 
        RootObject sava = JsonConvert.DeserializeObject<RootObject>(jsontext);

        company.pGold = sava.Gold;
        company.mName = sava.Name;

        worldTime.pCurYear = sava.CurYear;
        worldTime.pCurMonth = sava.CurMonty;
        worldTime.pCurWeek = sava.CurWeek;
        worldTime.pCurDay = sava.CurDay;

        // 读取平台信息
        foreach (RootObject.cPlatform item in sava.Platform)
        {
            Platform tmpP;
            if(mAllPlatform.TryGetValue(item.Num, out tmpP))
            {
                tmpP.mGameCount = item.GameCount;
                company.mPlatform.Add(tmpP);
            }
        }

        // 读取游戏类型
        foreach (RootObject.cGameGenre item in sava.GameGenre)
        {
            GameGenre tmpP;
            if (mAllGenre.TryGetValue(item.Num, out tmpP))
            {
                tmpP.mLevel = item.Level;
                company.mGenre.Add(tmpP);
            }
        }

        // 读取游戏内容
        foreach (RootObject.cGameType item in sava.GameType)
        {
            GameType tmpP;
            if (mAllType.TryGetValue(item.Num, out tmpP))
            {
                tmpP.mLevel = item.Level;
                company.mGameType.Add(tmpP);
            }
        }

        // 读取员工信息
        foreach (RootObject.cStaff item in sava.Staff)
        {
            Staff staff;
            if (mAllStaff.TryGetValue(item.Num, out staff))
            {
                staff.mCurJob = mAllJob[item.JobNum];
                staff.mCurLv = item.Level;
                staff.mCurExp = item.Exp;

                // 一个员工有n个职业
                foreach (var varJob in item.Jobs)
                {
                    JobInfo tmpjob;
                    if (mAllJob.TryGetValue(varJob.Num, out tmpjob))
                    {
                        tmpjob.mCurLevel = varJob.Level;
                        tmpjob.mCurExp = varJob.Exp;

                        staff.mJobs.Add(tmpjob);
                    }
                }

                company.mStaffList.Add(staff);
            }
        }

        // 读取游戏信息 - 暂无
        //company.mGame = new List<Game>();
        //foreach (var item in sava.Game)
        //{
        //    Game game = new Game();

        //    company.mGame.Add(game);
        //}

        // 读取粉丝信息 - 暂无
       
    }

    // 保存
    public void Sava()
    {
        string path = string.Format(savapath, "sava");

        RootObject root = new RootObject();
        // 保存基本信息
        root.Name = company.mName;
        root.Gold = company.pGold;

        root.CurYear = worldTime.pCurYear;
        root.CurMonty = worldTime.pCurMonth;
        root.CurWeek = worldTime.pCurWeek;
        root.CurDay = worldTime.pCurDay;

        // 保存员工信息
        root.Staff = new List<RootObject.cStaff>();
        foreach (var item in company.mStaffList)
        {
            RootObject.cStaff staff = new RootObject.cStaff();
            // 保存基本信息
            staff.Num = item.mNum;
            staff.JobNum = item.mCurJob.mNum;
            staff.Level = item.mCurLv;
            staff.Exp = item.mCurExp;

            // 保存员工信息
            staff.Jobs = new List<RootObject.cJobs>();
            foreach (var jobitem in item.mJobs)
            {
                RootObject.cJobs job = new RootObject.cJobs();
                job.Num = jobitem.mNum;
                job.Level = jobitem.pCurLevel;
                job.Exp = jobitem.pCurExp;

                staff.Jobs.Add(job);
            }

            root.Staff.Add(staff);
        }

        // 保存游戏类型
        root.GameGenre = new List<RootObject.cGameGenre>();
        foreach (var item in company.mGenre)
        {
            RootObject.cGameGenre genre = new RootObject.cGameGenre();
            genre.Num = item.mNum;
            genre.Level = item.mLevel;

            root.GameGenre.Add(genre);
        }

        // 保存游戏内容
        root.GameType = new List<RootObject.cGameType>();
        foreach (var item in company.mGameType)
        {
            RootObject.cGameType type = new RootObject.cGameType();
            type.Num = item.mNum;
            type.Level = item.mLevel;

            root.GameType.Add(type);
        }

        // 保存平台信息
        root.Platform = new List<RootObject.cPlatform>();
        foreach (var item in company.mPlatform)
        {
            RootObject.cPlatform platform = new RootObject.cPlatform();
            platform.Num = item.mNum;
            platform.GameCount = item.mGameCount;
        }

        // 保存游戏信息 - 暂无
        //root.Game = new List<RootObject.cGame>();
        //foreach (var item in company.mGame)
        //{
        //    RootObject.cGame game = new RootObject.cGame();
            
        //}

        // 保存粉丝信息 - 暂无

        string jsontext = JsonConvert.SerializeObject(root);
        File.WriteAllText(path, jsontext);
    }
}

