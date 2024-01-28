using Unity.Entities;
using UnityEngine;

namespace Debris
{
    public struct DebrisSpawner : IComponentData
    {
        public Entity Prefab;
    }

    public class DebrisSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
    }

    public class DebrisBaker : Baker<DebrisSpawnerAuthoring>
    {
        public override void Bake(DebrisSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new DebrisSpawner
            {
                // By default, each authoring GameObject turns into an Entity.
                // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}