using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    }

    List<GameGenre> mAllGenre;
    List<GameType> mAllType;
    List<Platform> mAllPlatform;

    // 读取游戏类型和内容
    public void LoadGameInfo()
    {
        mAllGenre = new List<GameGenre>();
        mAllType = new List<GameType>();

#if UNITY_EDITOR
        string path = Application.dataPath + "/StreamingAssets/Xml/GameInfo.xml";
#elif UNITY_ANDROID
        string path = Application.streamingAssetsPath + "/Xml/GameInfo.xml";
#endif

        XmlDocument xd = new XmlDocument();
        xd.Load(path);

        #region 得到所有的类型
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

            mAllGenre.Add(gg);
        }
        #endregion

        #region 得到所有的内容
        XmlNodeList tlist = xd.SelectNodes("TypeList/Type");
        foreach (XmlNode txn in tlist)
        {
            GameType gt = new GameType();
            gt.mNum = int.Parse(txn.SelectSingleNode("Num").InnerText);
            gt.mName = txn.SelectSingleNode("Name").InnerText;
            gt.mCost = int.Parse(txn.SelectSingleNode("Cost").InnerText);

            mAllType.Add(gt);
        }
        #endregion
    }

    // 读取平台信息
    public void LoadPlatform()
    {
        mAllPlatform = new List<Platform>();

#if UNITY_EDITOR
        string path = Application.dataPath + "/StreamingAssets/Xml/Platform.xml";
#elif UNITY_ANDROID
        string path = Application.streamingAssetsPath + "/Xml/GameInfo.xml";
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
            tmpP.mDevCost = int.Parse(pxn.SelectSingleNode("DevCost").InnerText);

            string[] times = pxn.SelectSingleNode("Time").InnerText.Split('-');
            tmpP.mCreateYear = int.Parse(times[0]);
            tmpP.mCreateMonth = int.Parse(times[1]);
            tmpP.mCreateWeek = int.Parse(times[2]);

            mAllPlatform.Add(tmpP);
        }
    }

    //public void Load
}