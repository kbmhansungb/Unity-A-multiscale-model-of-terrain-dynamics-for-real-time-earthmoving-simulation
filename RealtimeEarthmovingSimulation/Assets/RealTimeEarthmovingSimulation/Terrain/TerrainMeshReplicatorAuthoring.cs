using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Debris
{
    public struct TerrainMeshReplicator : IComponentData
    {
    }

    public class TerrainMeshReplicatorAuthoring : MonoBehaviour
    {
#if UNITY_EDITOR
        [ContextMenu("Bake")]
        public void Bake()
        {
            var mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>($"Assets/RealTimeEarthmovingSimulation/Terrain/TerrainMesh.asset");
            if (mesh == null)
            {
                mesh = new Mesh();
                AssetDatabase.CreateAsset(mesh, $"Assets/RealTimeEarthmovingSimulation/Terrain/TerrainMesh.asset");
            }

            CreateTerrainMesh(mesh);
        }
#endif

        public void CreateTerrainMesh(Mesh mesh)
        {
            var resolution = 100;
            float[,] heights = new float[resolution, resolution];

            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    heights[i, j] = Random.Range(0.0f, 0.3f);
                }
            }

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            
            // height에 따라 버텍스를 생성합니다.
            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    vertices.Add(new Vector3(i, heights[i, j], j));
                }
            }

            // 버텍스를 이용해 삼각형을 생성합니다.
            for (int i = 0; i < resolution - 1; i++)
            {
                for (int j = 0; j < resolution - 1; j++)
                {
                    triangles.Add(i * resolution + j);
                    triangles.Add(i * resolution + j + 1);
                    triangles.Add((i + 1) * resolution + j);
                    triangles.Add((i + 1) * resolution + j);
                    triangles.Add(i * resolution + j + 1);
                    triangles.Add((i + 1) * resolution + j + 1);
                }
            }

            mesh.name = "TerrainMesh";
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }

    public class TerrainMeshReplicatorBaker : Baker<TerrainMeshReplicatorAuthoring>
    {
        public override void Bake(TerrainMeshReplicatorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new TerrainMeshReplicator
            {
            });
        }
    }
}