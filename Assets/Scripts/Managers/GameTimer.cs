using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GameTimer;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;
    public delegate void TimerDelegate();
    public delegate void LerpDelegate(float lerp);
    public delegate void ObjectDelegate(object parameter);
    public delegate void ObjectsDelegate(object[] parameter);



    List<Timer> triggeringTimers = new List<Timer>();
    List<Timer> allTimers = new List<Timer>();
    List<LerpTimer> allLerpTimer = new List<LerpTimer>();
    public List<LerpTimer> triggeringLerpTimer = new List<LerpTimer>();
    List<ObjectTimer> allObjectTimer = new List<ObjectTimer>();
    List<ObjectTimer> triggeringObjectTimers = new List<ObjectTimer>();
    List<ObjectsTimer> allObjectsTimer = new List<ObjectsTimer>();
    List<ObjectsTimer> triggeringObjectsTimers = new List<ObjectsTimer>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        TimerCheck();
        LerpTimerCheck();
        ObjectTimerCheck();
        ObjectsTimerCheck();
    }

    private void TimerCheck()
    {
        if (allTimers.Count != 0)
        {
            foreach (Timer timer in allTimers)
            {
                if (timer.CheckTimer(Time.deltaTime))
                {
                    triggeringTimers.Add(timer);
                }
            }
            foreach (Timer timer in triggeringTimers)
            {
                allTimers.Remove(timer);
                timer.myDelegate.Invoke();
            }
            triggeringTimers.Clear();
        }
    }

    private void ObjectTimerCheck()
    {
        if (allObjectTimer.Count != 0)
        {
            foreach (ObjectTimer timer in allObjectTimer)
            {
                if (timer.CheckTimer(Time.deltaTime))
                {
                    triggeringObjectTimers.Add(timer);
                }
            }
            foreach (ObjectTimer timer in triggeringObjectTimers)
            {
                allObjectTimer.Remove(timer);
                timer.myDelegate.Invoke(timer.invokeParam);
            }
            triggeringObjectTimers.Clear();
        }
    }

    private void ObjectsTimerCheck()
    {
        if (allObjectsTimer.Count != 0)
        {
            foreach (ObjectsTimer timer in allObjectsTimer)
            {
                if (timer.CheckTimer(Time.deltaTime))
                {
                    triggeringObjectsTimers.Add(timer);
                }
            }
            foreach (ObjectsTimer timer in triggeringObjectsTimers)
            {
                allObjectsTimer.Remove(timer);
                timer.myDelegate.Invoke(timer.invokeParam);
            }
            triggeringObjectsTimers.Clear();
        }
    }

    private void LerpTimerCheck()
    {
        if (allLerpTimer.Count != 0)
        {
            foreach (LerpTimer lerpTimer in allLerpTimer)
            {
                if (lerpTimer.ContinueLerp(Time.deltaTime))
                {
                    triggeringLerpTimer.Add(lerpTimer);
                }
            }
            foreach (LerpTimer timer in triggeringLerpTimer)
            {
                if (timer.endDelegate != null)
                    timer.endDelegate.Invoke();
                allLerpTimer.Remove(timer);
            }
            triggeringLerpTimer.Clear();
        }
    }

    private void EndLerpTimers()
    {

    }

    public void AddNewTimer(TimerDelegate mydelegate, float timer)
    {
        allTimers.Add(new Timer(timer, mydelegate));
    }

    public void AddNewTimer(TimerDelegate mydelegate, float timer, int amount)
    {
        float tmpTimer = 0;
        for (int i = 0; i < amount; i++)
        {
            allTimers.Add(new Timer(tmpTimer, mydelegate));
            tmpTimer += timer;
        }
    }

    public LerpTimer AddNewLerpTimer(LerpDelegate lerpDelegate, TimerDelegate endDelegate, float maxTime)
    {
        LerpTimer tmp = new LerpTimer(lerpDelegate, endDelegate, maxTime);
        allLerpTimer.Add(tmp);
        return tmp;
    }

    public ObjectTimer AddNewObjectTimer(ObjectDelegate objectDelegate, float timer, object _invokeParam)
    {
        ObjectTimer tmp = new ObjectTimer(timer, objectDelegate, _invokeParam);
        allObjectTimer.Add(tmp);
        return tmp;
    }

    public void AddNewObjectsTimer(ObjectsDelegate objectDelegate, float timer, object[] _invokeParam)
    {
        allObjectsTimer.Add(new ObjectsTimer(timer, objectDelegate, _invokeParam));
    }
}

public class LerpTimer
{
    public LerpDelegate myDelegate;
    public TimerDelegate endDelegate;
    float maxTime;
    float currentTime;

    public LerpTimer(LerpDelegate lerp, TimerDelegate end, float _secondsToLerp)
    {
        maxTime = _secondsToLerp;
        currentTime = 0;
        myDelegate = lerp;
        endDelegate = end;
    }

    public bool ContinueLerp(float deltaTime)
    {
        currentTime += deltaTime;
        if (currentTime >= maxTime)
        {
            myDelegate.Invoke(1);
            return true;
        }
        else
        {
            myDelegate.Invoke(currentTime / maxTime);
            return false;
        }
    }
}

public class Timer
{
    public float time;
    public TimerDelegate myDelegate;

    public Timer(float _time, TimerDelegate _myDelegate)
    {
        time = _time;
        myDelegate = _myDelegate;
    }

    public bool CheckTimer(float deltaTime)
    {
        time -= deltaTime;
        if (time <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class ObjectTimer
{
    public float time;
    public ObjectDelegate myDelegate;
    public object invokeParam;

    public ObjectTimer(float _time, ObjectDelegate _myDelegate, object _invokeParam)
    {
        time = _time;
        myDelegate = _myDelegate;
        invokeParam = _invokeParam;
    }

    public bool CheckTimer(float deltaTime)
    {
        time -= deltaTime;
        if (time <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class ObjectsTimer
{
    public float time;
    public ObjectsDelegate myDelegate;
    public object[] invokeParam;

    public ObjectsTimer(float _time, ObjectsDelegate _myDelegate, object[] _invokeParam)
    {
        time = _time;
        myDelegate = _myDelegate;
        invokeParam = _invokeParam;
    }

    public bool CheckTimer(float deltaTime)
    {
        time -= deltaTime;
        if (time <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
