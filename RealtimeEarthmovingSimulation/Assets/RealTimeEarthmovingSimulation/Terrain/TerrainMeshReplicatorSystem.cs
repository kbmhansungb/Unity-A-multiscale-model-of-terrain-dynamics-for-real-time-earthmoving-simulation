using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Debris
{
    [BurstCompile]
    public partial struct TerrainMeshReplicatorSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
        }
    }
}