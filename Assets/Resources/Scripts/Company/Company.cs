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
            RemoveDelegates();
        }

        private EventManager manager;

        private void AddDelegates()
        {
            manager.RegisterMsgHandler((int)PlayerEvent.PE_MainTimeKey, Timing);
        }

        private void RemoveDelegates()
        {
            manager.UnRegisterMsgHandler((int)PlayerEvent.PE_MainTimeKey, Timing);
        }

        public string mName;
        private int mGold;
        private int mCurYear;
        private int mCurMonth;
        private int mCurWeek;
        private int mCurDay;

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

        public int pCurYear
        {
            get
            {
                return mCurYear;
            }
            set
            {
                mCurYear = value;

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateYear;
                data.data = mCurYear;
                manager.NotifyEvent(data.pEventID, data);
            }
        }

        public int pCurMonth
        {
            get
            {
                return mCurMonth;
            }
            set
            {
                mCurMonth = value;
                if (mCurMonth > 12)
                {
                    mCurMonth = 1;
                    pCurYear++;
                }

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateMonth;
                data.data = mCurMonth;
                manager.NotifyEvent(data.pEventID, data);
            }
        }

        public int pCurWeek
        {
            get
            {
                return mCurWeek;
            }

            set
            {
                mCurWeek = value;
                if (mCurWeek > 4)
                {
                    mCurWeek = 1;
                    pCurMonth++;
                }

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateWeek;
                data.data = mCurWeek;
                manager.NotifyEvent(data.pEventID, data);
            }
        }

        public int pCurDay
        {
            get
            {
                return mCurDay;
            }

            set
            {
                mCurDay = value;
                if (mCurDay > 10)
                {
                    mCurDay = 1;
                    pCurWeek++;
                }

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateDay;
                data.data = mCurDay;
                manager.NotifyEvent(data.pEventID, data);
            }
        }

        // 公司游戏进程 -> MainTime++ = 一天
        public void Timing(BaseEvent data)
        {
            pCurDay++;
        }
    }
}