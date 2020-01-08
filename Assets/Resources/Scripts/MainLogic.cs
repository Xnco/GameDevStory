using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDS;

public class MainLogic : MonoBehaviour
{
    private static MainLogic instince;
    public static MainLogic GetSingleon()
    {
        if (instince == null)
        {
            GameObject temp = new GameObject("Logic");
            instince = temp.AddComponent<MainLogic>();
            DontDestroyOnLoad(temp);
        }
        return instince;
    }

    private TimeManager timeManager;
    private WorldTime worldTime;

    // Start is called before the first frame update
    void Start()
    {
        timeManager = gameObject.AddComponent<TimeManager>();
        worldTime = WorldTime.GetSingleon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        //Debug.LogError("Error!! MainLogic can't destroy!!");
    }

    public void Pause(bool bIsPause)
    {
        if(timeManager != null)
        {
            timeManager.GamePause(bIsPause);
        }
    }
}
