using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Unused code, maybe will be finished in the future, Instatiating MonoBehaviour problem  using new
/// </summary>
public class TimerManager
{
    static Dictionary<int,Timer> timers = null;
    static Dictionary<string, int> reserveList = null;
    static int numberOfTimers;
    static int timerIndex;
    public TimerManager()
    {
        if(timers==null)
        {
            timers = new Dictionary<int,Timer>();
            reserveList = new Dictionary<string, int>();
            numberOfTimers = 0;
            timerIndex = 0;
        }
    }
    public void setNewTimer(float delay,Action toDo)
    {
        while (timers.ContainsKey(timerIndex))
            timerIndex++;
        numberOfTimers++;
        timers.Add(numberOfTimers,new Timer(delay, toDo, timerIndex));

    }
    public void setNewTimer(float delay, Action toDo,string timerName)
    {
        if(!reserveList.ContainsKey(timerName))
        {
            numberOfTimers++;
            while (timers.ContainsKey(timerIndex))
                timerIndex++;
            reserveList.Add(timerName, timerIndex);
            timers.Add(numberOfTimers, new Timer(delay, toDo, timerIndex));
        }
    }
    public void resetTimer(string timerName,float delay)
    {
        if(reserveList.ContainsKey(timerName))
            timers[reserveList[timerName]].reset(delay);
    }
    public void stopTimer(string timerName)
    {
        if (reserveList.ContainsKey(timerName))
            timers[reserveList[timerName]].stop();
    }
    public bool timerExists(string timerName)
    {
        return reserveList.ContainsKey(timerName);
    }
    public void unreserveTimer(string timerName)
    {
        if (reserveList.ContainsKey(timerName))
        {
            timers[reserveList[timerName]].destroy();
            reserveList.Remove(timerName);
        }
            
    }
    public static void deleteTimer(string timerName)
    {
        if (reserveList.ContainsKey(timerName))
        {
            timers.Remove(reserveList[timerName]);
            numberOfTimers--;
            if (numberOfTimers == 0) timerIndex = 0;
        }
            

    }
    public bool isTimerActive(string timerName)
    {
        if(reserveList.ContainsKey(timerName))
            return timers[reserveList[timerName]].isActive;
        return false;
    }
    public static void deleteTimer(int index)
    {
        if (reserveList.ContainsValue(index))
            return;
        if(timers.ContainsKey(index))
        {
            numberOfTimers--;
            timers.Remove(index);

            if (numberOfTimers == 0) timerIndex = 0;
        }
        
    }

}
public class Timer : MonoBehaviour
{
    
    // Start is called before the first frame update 
    protected float delay = 0f;
    bool timerSet = false;
    public bool isActive {get =>timerSet;}
    int timerIndex;
    protected Action toDo;
    
    // Update is called once per frame
    void Update()
    {
        update();
        OnTick();
        
    }
    public Timer(float delay, Action toDo, int timerIndex)
    {
        this.timerIndex = timerIndex;
        timerSet = true;
        this.toDo = toDo;
    }
    public void reset(float delay)
    {
        this.delay = delay;
    }
    public void stop()
    {
        timerSet = false;
    }
    public void start()
    {
        timerSet = true;
    }
    protected virtual void OnTick()
    {
        if (delay <= 0f)
        {
            toDo();
            Destroy(this);
            //destroy();
        }
    }
    void update()
    {
        if (timerSet) this.delay -= Time.deltaTime;
    }
    public void destroy()
    {
        TimerManager.deleteTimer(timerIndex);
        Destroy(this);
    }
}
public class OneTimeTimer : Timer
{
    public OneTimeTimer(float delay, Action toDo)
        : base(delay, toDo, 0) { }

    protected override void OnTick() { }

    public void OnTickDestroy()
    {
        if (delay <= 0f)
        {
            toDo();
            Destroy(this);
        }
    }
    
}
