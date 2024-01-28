using Unity.Entities;
using Unity.Burst;

namespace Debris
{
    [BurstCompile]
    public partial struct DebrisSpawnerSystem : ISystem
    {
        //public static List<Vector3> spawnPosition = new List<Vector3>();

        public void OnCreate(ref SystemState state) 
        { 
        }

        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Queries for all Spawner components. Uses RefRW because this system wants
            // to read from and write to the component. If the system only needed read-only
            // access, it would use RefRO instead.
            foreach (RefRW<DebrisSpawner> spawner in SystemAPI.Query<RefRW<DebrisSpawner>>())
            {
                ProcessSpawner(ref state, spawner);
            }
        }

        private void ProcessSpawner(ref SystemState state, RefRW<DebrisSpawner> spawner)
        {
            //foreach(Vector3 position in spawnPosition)
            //{
            //    // Spawns a new entity and positions it at the spawner.
            //    Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
            //    // LocalPosition.FromPosition returns a Transform initialized with the given position.
            //    state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(position.X, position.Y, position.Z));
            //}
            //spawnPosition.Clear();
        }
    }
}