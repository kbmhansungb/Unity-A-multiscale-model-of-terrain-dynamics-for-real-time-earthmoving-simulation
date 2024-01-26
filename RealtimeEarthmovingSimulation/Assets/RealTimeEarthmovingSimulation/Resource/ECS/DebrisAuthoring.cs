using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Debris : IComponentData
{
    public Entity Prefab;
    public float3 SpawnPosition;
    public float NextSpawnTime;
    public float SpawnRate;
}

class DebrisAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float SpawnRate;
}

class DebrisBaker : Baker<DebrisAuthoring>
{
    public override void Bake(DebrisAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new Debris
        {
            // By default, each authoring GameObject turns into an Entity.
            // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            SpawnPosition = authoring.transform.position,
            NextSpawnTime = 0.0f,
            SpawnRate = authoring.SpawnRate
        });
    }
}