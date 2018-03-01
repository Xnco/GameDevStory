using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void OnLoadWindowFinish(GameObject obj);
/// <summary>
/// 窗口管理器.
/// </summary>
public class WindowManager 
{
    public static int ScreenHeight=667;
    private GameObject mUIRoot;//UIRoot;
    private GameObject mCameraObj;//相机物体;
    private GameObject mWindowRoot;//窗口根节点;
   
    private Dictionary<string,GameObject> mAllWindows;//保存已经打开过的窗口:
    //记录窗口最大深度;
    private int CurMaxDepth=0;

	private static WindowManager mManager;
	public static WindowManager GetSingle()
	{
		if (mManager == null)
		{
			mManager = new WindowManager();
		}
		return mManager;
	}
	private WindowManager()
	{
        mAllWindows = new Dictionary<string, GameObject>();
       
		//创建uiRoot;
		mUIRoot = new GameObject("UIRoot");
		UIRoot uiroot= mUIRoot.AddComponent<UIRoot>();
		uiroot.scalingStyle = UIRoot.Scaling.Flexible;
        uiroot.maximumHeight = ScreenHeight;
        uiroot.minimumHeight = ScreenHeight;
		mUIRoot.AddComponent<UIPanel>();

		//创建相机;
		mCameraObj = new GameObject("Camera");
		mCameraObj.transform.SetParent(mUIRoot.transform);
        mCameraObj.AddComponent<AudioListener>();
		Camera camara = mCameraObj.AddComponent<Camera>();
        camara.clearFlags = CameraClearFlags.SolidColor;
        camara.backgroundColor = Color.black;
		camara.orthographic = true;
		camara.orthographicSize = 1;
        camara.nearClipPlane = -10;
		UICamera uiCamera = mCameraObj.AddComponent<UICamera>();

		//创建窗口根节点;
		mWindowRoot = new GameObject("WindowRoot");
		mWindowRoot.transform.SetParent(mUIRoot.transform);

        MonoBehaviour.DontDestroyOnLoad(mUIRoot);
	}

	//打开窗口,放到WindowRoot下面;
	public GameObject OpenWindow(string windowPath)
	{
        //是否打开了这个窗口;
        if (mAllWindows.ContainsKey(windowPath))
        {
            GameObject win = mAllWindows[windowPath];
            if (win!=null)
            {
                //获取自己所占用的Panel深度;
                int offset = GetWindowDepth(win);
                //将自己前面的窗口往后移动offset单位;
                CurMaxDepth= MoveToBack(win, offset);
                //将自己往前移动.
                MoveWinToFront(win);
            }
            return win;
        }
        GameObject obj = Resources.Load<GameObject>(windowPath);
        if (obj == null)
        {
            Debug.Log("加载窗口失败:" + windowPath);
            return null;
        }
        Transform cloneTran = obj.transform.GetChild(1);
        GameObject winObj = GameObject.Instantiate<GameObject>(cloneTran.gameObject);
        winObj.transform.SetParent(mWindowRoot.transform);
        winObj.transform.localScale = Vector3.one;
        winObj.SetActive(true);
        //将自己移动到最前面;
        MoveWinToFront(winObj);
        //记录打开的窗口;
        mAllWindows.Add(windowPath,winObj);
        return winObj;
	}

    //打开窗口;
    public void OpenWindow(string windowPath,OnLoadWindowFinish fun)
    {
        //是否打开了这个窗口;
        if (mAllWindows.ContainsKey(windowPath))
        {
            GameObject win = mAllWindows[windowPath];
            if (win!=null)
            {
                //获取自己所占用的Panel深度;
                int offset = GetWindowDepth(win);
                //将自己前面的窗口往后移动offset单位;
                CurMaxDepth= MoveToBack(win, offset);
                //将自己往前移动.
                MoveWinToFront(win);

            }
            if(fun!=null)
                fun(win);
            return;
        }
        GameObject obj = Resources.Load<GameObject>(windowPath);
        if (obj == null)
        {
            Debug.Log("加载窗口失败:"+windowPath);
            if (fun != null)
                fun(null);
            return;
        }
        Transform cloneTran = obj.transform.GetChild(1);
        GameObject winObj = GameObject.Instantiate<GameObject>(cloneTran.gameObject);
        winObj.transform.SetParent(mWindowRoot.transform);
        winObj.transform.localScale = Vector3.one;
        winObj.SetActive(true);
        //将自己移动到最前面;
        MoveWinToFront(winObj);
        //记录打开的窗口;
        if(!mAllWindows.ContainsKey(windowPath))
            mAllWindows.Add(windowPath,winObj);
        if(fun!=null)
            fun(winObj);
        

    }
        
    //获取一个窗口的占用的深度.
    private int GetWindowDepth(GameObject winObj)
    {
        return GetWinsowMaxDepth(winObj)-GetWinsowMinDepth(winObj)+1;
    }

    //获取最大深度;
    public int GetWinsowMaxDepth(GameObject winObj)
    {
        //获取到这个窗口的所有panel;
        UIPanel[] panels = winObj.GetComponentsInChildren<UIPanel>();
        if (panels != null)
        {
            Array.Sort(panels,(x, y) => x.depth.CompareTo(y.depth));
            int max=panels[panels.Length-1].depth;
            return max;

        }
        return 0;
    }

    //获取最小深度;
    public int GetWinsowMinDepth(GameObject winObj)
    {
        //获取到这个窗口的所有panel;
        UIPanel[] panels = winObj.GetComponentsInChildren<UIPanel>();
        if (panels != null)
        {
            Array.Sort(panels,(x, y) => x.depth.CompareTo(y.depth));
            //找出最大和最小深度的pannel.
            int min=panels[0].depth;
            return min ;

        }
        return 0;
    }

    //将某个窗口前的所有窗口往后移动固定的深度;
    private int MoveToBack(GameObject winObj,int depth)
    {
        //获取当前窗口的最大深度;
        int max = GetWinsowMaxDepth(winObj);//10
        int tmpMax=CurMaxDepth-depth;
        //让所有大于这个深度的Panel往后移动depth.
        foreach (var item in mAllWindows)
        {
            UIPanel[] panels = item.Value.GetComponentsInChildren<UIPanel>();
            if (panels != null)
            {
                foreach (var panel in panels) 
                {
                    if (panel.depth > max)
                    {
                        panel.depth -= depth;
                        //记录所有窗口中panel的最大深度;
                        if (panel.depth > tmpMax)
                        {
                            tmpMax = panel.depth;
                        }
                    }

                   
                }
            }
        }

        return tmpMax;
    }

    //将当前要打开的窗口移动到最前方.
    private void MoveWinToFront(GameObject WinObj)
    {
        //当前窗口的最小深度.
        int minDepth = GetWinsowMinDepth(WinObj);
        //需要移动的量.
        int offset = CurMaxDepth - minDepth+1;
        UIPanel[] panels = WinObj.GetComponentsInChildren<UIPanel>();
        foreach (var panel in panels)
        {
            panel.depth += offset;
            //更新最大深度;
            if (CurMaxDepth < panel.depth)
            {
                CurMaxDepth = panel.depth;
            }
        }
    }
   
    //关闭窗口;
    public void CloseWindow(string windowPath)
    {
        GameObject tempobj;
        mAllWindows.TryGetValue(windowPath, out tempobj);
        if (tempobj!=null) 
        {
            tempobj.SetActive(false);
            mAllWindows.Remove(windowPath);
        }
    }
    //关闭所有窗口;
    public void CloseAllWindow()
    {
        foreach (var item in mAllWindows)
        {
            GameObject.Destroy(item.Value);
        }
        mAllWindows.Clear();
    }
}
