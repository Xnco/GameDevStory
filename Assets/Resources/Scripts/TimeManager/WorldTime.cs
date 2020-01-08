using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDS
{
    public class WorldTime
    {
        private WorldTime()
        {
            manager = EventManager.GetSinglon();
            AddDelegate();
        }

        ~WorldTime()
        {
            ClearDelegates();
        }

        private static WorldTime instance;
        public static WorldTime GetSingleon()
        {
            if (instance == null)
            {
                instance = new WorldTime();
            }
            return instance;
        }

        private EventManager manager;

        private void AddDelegate()
        {
            manager.RegisterMsgHandler((int)PlayerEvent.PE_MainTimeKey, Timing);
        }

        private void ClearDelegates()
        {
            manager.UnRegisterMsgHandler((int)PlayerEvent.PE_MainTimeKey, Timing);
        }

        private int CurDay;
        private int CurMonth;
        private int CurYear;

        private int CurWeek;

        public int pCurDay
        {
            get
            {
                return CurDay;
            }
            set
            {
                CurDay = value;
                // todo:每个月天数不同
                if (CurDay > 30)
                {
                    CurDay = 1; // 重置为1号
                    pCurMonth++;
                }

                // todo:根据当前日期推算具体星期
                //pCurWeek = 

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateDay;
                data.data = CurDay;
                manager.NotifyEvent(data.pEventID, data);
            }
        }

        public int pCurWeek
        {
            get
            {
                return CurWeek;
            }

            set
            {
                CurWeek = value;

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateWeek;
                data.data = CurWeek;
                manager.NotifyEvent(data.pEventID, data);
            }
        }

        public int pCurMonth
        {
            get
            {
                return CurMonth;
            }
            set
            {
                CurMonth = value;
                if (CurMonth > 12)
                {
                    CurMonth = 1; // 重置为1月
                    pCurYear++;
                }

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateMonth;
                data.data = CurMonth;
                manager.NotifyEvent(data.pEventID, data);
            }
        }

        public int pCurYear
        {
            get
            {
                return CurYear;
            }
            set
            {
                CurYear = value;

                ExData<int> data = new ExData<int>();
                data.pEventID = (int)PlayerEvent.PE_UpdateYear;
                data.data = CurYear;
                manager.NotifyEvent(data.pEventID, data);
            }
        }

        public void Timing(BaseEvent data)
        {
            pCurDay++;
        }
    }

}
