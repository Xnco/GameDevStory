using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Company 
{
    public string mName;
    private int mPopularity;
    private int mGold;

    private List<Staff> mStaffList;
    private List<Game> mGame;
    private List<GameGenre> mGenre;
    private List<GameType> mGameType;
    private List<Platform> mPlatform;

    private int TopSales;
}
