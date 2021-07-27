using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


public struct TimeTickEvent : IComponentData { }

[AlwaysUpdateSystem]
public class TimeTickSystem : SystemBase
{
    //1.0f is about equal to 1 second 0.5 is about half a second
    private const float TICK_TIMER_MAX = 0.5f;

    private int tick;
    private float tickTimer;

    protected override void OnCreate()
    {
        //I'm guessing this conflicts with AlwaysupdateSystem
        //RequireSingletonForUpdate<PauseSystemsCompTag>();
    }

    protected override void OnStartRunning()
    {
        tick = 0;
        tickTimer = 0.0f;
    }

    //To get rid of the inconsistent spawning at the beginning I can delay spawning or this for a couple of seconds perhaps
    protected override void OnUpdate()
    {
        if (HasSingleton<TimeTickEvent>())
        {
            EntityManager.DestroyEntity(GetSingletonEntity<TimeTickEvent>());
        }
        tickTimer += Time.DeltaTime;
        if (tickTimer >= TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;
            //Debug.Log("tick" + tick);
            if (!HasSingleton<TimeTickEvent>())
            {
                EntityManager.CreateEntity(typeof(TimeTickEvent));

            }

        }

    }

}