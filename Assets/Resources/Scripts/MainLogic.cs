using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDS;

public class MainLogic : MonoBehaviour
{
    private TimeManager timeManager;
    private WorldTime worldTime;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        timeManager = gameObject.AddComponent<TimeManager>();
        worldTime = WorldTime.GetSingleon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
