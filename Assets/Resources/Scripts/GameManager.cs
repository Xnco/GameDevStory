using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int mYear;
    private int mMonth;
    private int mWeek;
    private int mDay;

    void Awake()
    {
        // 初始化时间
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(TimeManager());
    }

    IEnumerator TimeManager()
    {
        while (true)
        {
            mDay++;
            yield return new WaitForSeconds(1);
        }
    }

    public int pDay
    {
        get
        {
            return mDay;
        }
        set
        {
            mDay = value;
            if (mDay > 7)
            {
                mDay = 1;
                pWeek++;
            }
        }
    }

    public int pWeek
    {
        get { return mWeek; }
        set
        {
            mWeek = pWeek;
            if (mWeek > 4)
            {
                mWeek = 1;
                pMonth++;
            }
        }
    }

    public int pMonth
    {
        get
        {
            return mMonth;
        }
        set
        {
            mMonth = value;
            if (mMonth > 12)
            {
                mMonth = 1;
                pYear++;
            }
        }
    }

    public int pYear
    {
        get { return mYear; }
        set
        {
            mYear = value;
        }
    }
}