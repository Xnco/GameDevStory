using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDS
{
    public class Company
    {
        private static Company instance;

        public static Company GetSingleon()
        {
            if (instance == null)
            {
                instance = new Company();
            }
            return instance;
        }

        private Company()
        {
            mStaffList = new List<Staff>();
            mGame = new List<Game>();
            mGenre = new List<GameGenre>();
            mGameType = new List<GameType>();
            mPlatform = new List<Platform>();

            manager = EventManager.GetSinglon();
            AddDelegates();
        }

        ~Company()
        {
            ClearDelegates();
        }

        private EventManager manager;

        private void AddDelegates()
        {
            //manager.RegisterMsgHandler((int)PlayerEvent.PE_MainTimeKey, Timing);
        }

        private void ClearDelegates()
        {
            //manager.UnRegisterMsgHandler((int)PlayerEvent.PE_MainTimeKey, Timing);
        }

        public string mName;
        private int mGold;

        public List<Staff> mStaffList;
        public List<Game> mGame;
        public List<GameGenre> mGenre;
        public List<GameType> mGameType;
        public List<Platform> mPlatform;

        public int pGold
        {
            get
            {
                return mGold;
            }
            set
            {
                mGold = value;

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateGold;
                data.data = mGold;
                manager.NotifyEvent(data.pEventID, data);
            }
        }
    }
}