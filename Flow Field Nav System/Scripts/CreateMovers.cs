using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class CreateMovers : SystemBase
{
    protected override void OnUpdate()
    {
       
        //var dootprefab = GetSingleton<DotPrefabinator>();

        //var baked = GetSingleton<BezierGraphSpawner>();
        ////ref var booked = ref baked.BezzyGraphcomp.Value;

        var boo = GetSingleton<DotPrefabinator>();

        

        if (!HasSingleton<TestMoveojbectTag>())
        {
            Entity toto = EntityManager.Instantiate(boo.Dootprefab);
            EntityManager.AddComponent<TestMoveojbectTag>(toto);
            EntityManager.SetComponentData(toto, new Translation { Value = new float3(10, 1, 0) });
            EntityManager.SetName(toto, "TestMoveObject");
            //Debug.Log("Why the fuck isnt this working");
        }

        //if(!HasSingleton<TestTargetEntity>())
        //{
        //    Entity dodo = EntityManager.Instantiate(boo.TargetPrefab);
        //    EntityManager.AddComponent<TestTargetEntity>(dodo);
        //    //var tempvals = EntityManager.GetComponentData<Translation>(dodo);
        //    //EntityManager.SetComponentData(dodo, new Translation { Value = new float3(tempvals.Value.x, 5, tempvals.Value.z) });
        //    EntityManager.SetName(dodo, "TestTargetEntity");

        //}


        Enabled = false;

    }
}

public struct TestMoveojbectTag : IComponentData { }

public struct TestTargetEntity : IComponentData { }