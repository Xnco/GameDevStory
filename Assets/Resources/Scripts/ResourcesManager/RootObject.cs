using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDS;

public class RootObject
{
    public class cJobs
    {
        public int Num { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
    }

    public class cStaff
    {
        public int Num { get; set; }
        // 当前的职业编号
        public int JobNum { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public List<cJobs> Jobs { get; set; }
    }

    public class cGameGenre
    {
        public int Num { get; set; }
        public int Level { get; set; }
    }

    public class cGameType
    {
        public int Num { get; set; }
        public int Level { get; set; }
    }

    public class cPlatform
    {
        public int Num { get; set; }
        public int GameCount { get; set; }
    }

    public class cGame
    {
    }

    public class cFans
    {
    }

    public string Name { get; set; }
    public int CurYear { get; set; }
    public int CurMonty { get; set; }
    public int CurWeek { get; set; }
    public int CurDay { get; set; }
    public int Gold { get; set; }
    public List<cStaff> Staff { get; set; }
    public List<cGameGenre> GameGenre { get; set; }
    public List<cGameType> GameType { get; set; }
    public List<cPlatform> Platform { get; set; }
    public List<cGame> Game { get; set; }
    public List<cFans> Fans { get; set; }
}
