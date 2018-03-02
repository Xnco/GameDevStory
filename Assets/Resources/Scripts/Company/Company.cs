using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDS
{
    public class Company
    {
        private static Company instance;

        public static Company GetSingle()
        {
            if (instance == null)
            {
                instance = new Company();
            }
            return instance;
        }

        private Company()
        {

        }

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
}