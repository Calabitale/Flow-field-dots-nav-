using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]//TODO Need to figure out how to convert just the data that I need with these spawnpoints all I need at the moment is the position and scale of them, so just need to convert that into an entity
public class SpawnPointAuthoring : IComponentData { }
