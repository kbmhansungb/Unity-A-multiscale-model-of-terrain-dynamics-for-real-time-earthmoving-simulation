using UnityEngine;

namespace TerrainEditor
{
    public sealed class RunTimeTerrainEditor : MonoBehaviour
    {
        [SerializeField] private Terrain terrain;
        [SerializeField] private GameObject DebrisPrefab;
        private TerrainData terrainData;

        private void Awake()
        {
            terrainData = terrain.terrainData;
        }

        public void TestCall(Vector3 worldPosition)
        {
            Debug.Log("RunTimeTerrainEditor: TestCall");

            var brush = GetBrushRange(worldPosition, 1, 1);
            var heights = terrainData.GetHeights(brush.baseX, brush.baseY, brush.width, brush.height);
            for (var y = 0; y < brush.height; y++)
            {
                for (var x = 0; x < brush.width; x++)
                {
                    heights[y, x] += -0.01f * Time.deltaTime;
                }
            }
            terrainData.SetHeights(brush.baseX, brush.baseY, heights);

            // 파편을 스폰합니다.
            var debris = Instantiate(DebrisPrefab, worldPosition, Quaternion.identity);
        }

        private Vector3 WorldToTerrainPosition(Vector3 worldPosition)
        {
            var terrainPosition = worldPosition - terrain.GetPosition();
            var terrainSize = terrainData.size;
            var heightmapResolution = terrainData.heightmapResolution;

            terrainPosition = new Vector3(terrainPosition.x / terrainSize.x, terrainPosition.y / terrainSize.y, terrainPosition.z / terrainSize.z);

            return new Vector3(terrainPosition.x * heightmapResolution, 0, terrainPosition.z * heightmapResolution);
        }

        private (int baseX, int baseY, int width, int height) GetBrushRange(Vector3 worldPosition, int brushWidth, int brushHeight)
        {
            var terrainPosition = WorldToTerrainPosition(worldPosition);
            var heightmapResolution = terrainData.heightmapResolution;

            var brushPosition =  new Vector2Int((int)Mathf.Clamp(terrainPosition.x - brushWidth / 2.0f, 0.0f, heightmapResolution), (int)Mathf.Clamp(terrainPosition.z - brushHeight / 2.0f, 0.0f, heightmapResolution));

            while (heightmapResolution - (brushPosition.x + brushWidth) < 0) brushWidth--;
            while (heightmapResolution - (brushPosition.y + brushHeight) < 0) brushHeight--;

            return (brushPosition.x, brushPosition.y, brushWidth, brushHeight);
        }
    }
}