using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager
{
    private static WindowManager instance;

    public static  WindowManager getSingle()
    {
        if (instance == null)
        {
            instance = new WindowManager();
        }
        return instance;
    }

    private WindowManager()
    {
        mLoadwindows = new Dictionary<string, GameObject>();
        mOpenWindows = new Dictionary<string, GameObject>();
        mOpenPanel = new Dictionary<string, UIPanel>();
        mOpenDepth = new Dictionary<string, int>();
        mUILayer = LayerMask.NameToLayer("UI");

        mUIRootObj = new GameObject("UIRootObject");
        mUIRoot = mUIRootObj.AddComponent<UIRoot>();
        mUIRootPanel = mUIRootObj.AddComponent<UIPanel>();
        mUIRootObj.layer = mUILayer;

        GameObject tempCamObject = new GameObject("UICamera");
        tempCamObject.layer = mUILayer;
        tempCamObject.transform.localPosition = Vector3.back;
        tempCamObject.transform.SetParent(mUIRootObj.transform);

        mCamera = tempCamObject.AddComponent<Camera>();
        mCamera.nearClipPlane = 0;
        mCamera.orthographic = true;
        mCamera.cullingMask = (int)Mathf.Pow(2, LayerMask.NameToLayer("UI"));
        mCamera.clearFlags = CameraClearFlags.SolidColor;
        mCamera.backgroundColor = Color.black;
        mUICamera = tempCamObject.AddComponent<UICamera>();

        MaxDepth = mUIRootPanel.depth;
    }

    Dictionary<string, GameObject> mLoadwindows;
    Dictionary<string, GameObject> mOpenWindows;
    Dictionary<string, UIPanel> mOpenPanel;
    Dictionary<string, int> mOpenDepth;
    private int MaxDepth;

    GameObject mUIRootObj;
    UIRoot mUIRoot;
    UIPanel mUIRootPanel;

    private int mUILayer;
    Camera mCamera;
    UICamera mUICamera;

    /// <summary>
    /// 加载界面
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject LoadWindow(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Load path is null!!");
            return null;
        }

        GameObject tmp = Resources.Load<GameObject>(path);
        if (tmp != null)
        {
            mLoadwindows.Add(path, tmp);
            return tmp;
        }
        else
        {
            Debug.LogError("Load Window path error!!");
            return null;
        }
        
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public void OpenWindow(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        int tempIndex = path.LastIndexOf("/");
        if (tempIndex >= 0)
        {
            string name = path.Substring(tempIndex + 1);
            if (mOpenWindows.ContainsKey(name))
            {
                Debug.LogError("This window is existed!!");
                return;
            }

            GameObject tmpObj = null;
            if (mLoadwindows.ContainsKey(name))
            {
                tmpObj= mLoadwindows[name];
            }
            else
            {
                tmpObj = LoadWindow(path);
            }
            mOpenWindows.Add(name, tmpObj);
            mOpenPanel.Add(name, tmpObj.GetComponent<UIPanel>());
            mOpenDepth.Add(name, GetWindowPanel(tmpObj).depth);
            GameObject tgo = GameObject.Instantiate(tmpObj, mUIRootObj.transform);
            tgo.layer = LayerMask.NameToLayer("UI");
            MoveWindowToFront(name);
        }
        else
        {
            Debug.LogError("Open Window path error!!");
            return;
        }
    }

    /// <summary>
    /// 关闭单个窗口
    /// </summary>
    /// <param name="name"></param>
    public void CloseWindow(string name)
    {
        GameObject tmpWindow = GetWindowByName(name);
        if (tmpWindow != null)
        {
            mOpenWindows.Remove(name);
            ChangedDepth(GetWindowPanel(tmpWindow).depth);
            GameObject.Destroy(tmpWindow);

            MaxDepth--;
        }
        else
        {
            Debug.LogError("this window isn't existed");
        }
       
    }

    /// <summary>
    /// 关闭所有窗口
    /// </summary>
    public void CloseAllWindows()
    {
        if (mOpenWindows == null)
        {
            return;
        }

        Dictionary<string, GameObject> tempWindows = new Dictionary<string, GameObject>(mOpenWindows);
        Dictionary<string, GameObject>.Enumerator tempEnumerator = tempWindows.GetEnumerator();

        for (int i = 0; i < tempWindows.Count; i++)
        {
            tempEnumerator.MoveNext();

            KeyValuePair<string, GameObject> tmpKeyValue = tempEnumerator.Current;
            if (tmpKeyValue.Value != null)
            {
                GameObject.Destroy(tmpKeyValue.Value);
            }
            mOpenWindows.Remove(tmpKeyValue.Key);
        }

        MaxDepth = 0;
    }

    /// <summary>
    /// 通过名字获取窗口
    /// </summary>
    /// <param name="varWindowName"></param>
    /// <returns></returns>
    public GameObject GetWindowByName(string varWindowName)
    {
        if (string.IsNullOrEmpty(varWindowName))
        {
            return null;
        }
        GameObject obj = null;
        if (mOpenWindows.TryGetValue(varWindowName, out obj))
        {
            return obj;
        }

        return null;
    }

    /// <summary>
    /// 将窗口放到最前
    /// </summary>
    /// <param name="varWindowName"></param>
    /// <returns></returns>
    public bool MoveWindowToFront(string varWindowName)
    {
        GameObject go = GetWindowByName(varWindowName);
        return MoveWindowToFront(go, varWindowName);
    }

    public bool MoveWindowToFront(GameObject varObj, string varWindowName)
    {
        if (varObj == null)
        {
            return false;
        }

        varObj.SetActive(true);

        if (string.IsNullOrEmpty(varWindowName))
        {
            return false;
        }

        if (mOpenWindows.Count <= 1)
        {
            return false;
        }

        if(mOpenDepth.ContainsKey(varWindowName))
        {
            UIPanel tmpPanel = GetWindowPanel(varObj);
            if (tmpPanel != null)
            {
                tmpPanel.depth += MaxDepth;
            }

            MaxDepth++;
        }

        return true;
        
    }
    
    /// <summary>
    /// 获取UIpanel
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public UIPanel GetWindowPanel(GameObject go)
    {
        return go.GetComponent<UIPanel>();
    }

    /// <summary>
    /// 有窗口被关闭改变深度
    /// </summary>
    /// <param name="varDepth"></param>
    public void ChangedDepth(int varDepth)
    {
        Dictionary<string, UIPanel>.Enumerator tmp = mOpenPanel.GetEnumerator();
        for (int i = 0; i < mOpenPanel.Count; i++)
        {
            tmp.MoveNext();
            KeyValuePair<string, UIPanel> mKY = tmp.Current;
            if (varDepth < mKY.Value.depth)
            {
                mKY.Value.depth--;
            }
        }
    }
}
