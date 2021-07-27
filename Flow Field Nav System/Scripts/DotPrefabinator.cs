using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

[GenerateAuthoringComponent]
public struct  DotPrefabinator : IComponentData
{
    public Entity Dootprefab;
    public Entity TargetPrefab;
    public Entity TestObjectPrefab;
   
}


//
//[DisallowMultipleComponent]
//[RequiresEntityConversion]
//public class CameraTagAuthoring : MonoBehaviour, IConvertGameObjectToEntity
//{
//    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
//    {
//
//        //dstManager.AddComponentData(entity, new CameraTag());
//        dstManager.AddComponent(entity, typeof(CopyTransformToGameObject));
//    }
//}
