using System.Collections;
using UnityEngine;

namespace TerrainSystem
{
    public interface IRunTimeTerrainSystem
    {
        /// <summary>
        /// 지정한 범위의 Height보다 높은 Terrain의 지형을 활성화합니다.
        /// </summary>
        public void ActiveSoil(int baseX, int baseY, float[,] heights);

        /// <summary>
        /// 지정한 범위안의 활성화된 흙을 비활성화 합니다.
        /// </summary>
        public void PassiveSoil(int baseX, int baseY, float[,] heights);

        /// <summary>
        /// 터레인의 높이를 가져옵니다.
        /// </summary>
        public float[,] GetHeights(int baseX, int baseY, int width, int height);

        /// <summary>
        /// 터레인의 지형을 수정합니다.
        /// </summary>
        public void SetHeights(int baseX, int baseY, float[,] heights);
    }

    public sealed class RunTimeTerrainSystem : MonoBehaviour, IRunTimeTerrainSystem
    {
        [SerializeField] private Terrain terrain;

        /// <summary>
        /// 파편의 크기입니다.
        /// </summary>
        [SerializeField] private float debrisSize = 0.05f;
        [SerializeField] private GameObject DebrisPrefab;

        private TerrainData terrainData;

        /// <summary>
        /// 가로, 세로 높이가 정사각형인 모델이여야 함
        /// </summary>
        private float terrainTexelSize;

        private void Awake()
        {
            terrainData = terrain.terrainData;
            terrainTexelSize = terrainData.size.x / terrainData.heightmapResolution;
        }

        public void TestActiveSoil(Vector3 worldPosition)
        {
            Debug.Log("RunTimeTerrainEditor: TestActiveSoil");

            var brush = GetBrushRange(worldPosition, 5, 5);
            var heights = terrainData.GetHeights(brush.baseX, brush.baseY, brush.width, brush.height);
            for (var y = 0; y < brush.height; y++)
            {
                for (var x = 0; x < brush.width; x++)
                {
                    heights[x, y] += -1f / terrainData.size.y;
                }
            }
            ActiveSoil(brush.baseX, brush.baseY, heights);
        }

        public void TestPassiveSoil(Vector3 worldPosition)
        {
            Debug.Log("RunTimeTerrainEditor: TestPassiveSoil");

            // TODO: 어떻게 배분하는지 공식 보고 수정 필요
            float[,] spread =
            {
                { 0.05f, 0.05f, 0.05f },
                { 0.05f, 0.6f, 0.05f },
                { 0.05f, 0.05f, 0.05f }
            };

            var brush = GetBrushRange(worldPosition, 3, 3);
            var heights = terrainData.GetHeights(brush.baseX, brush.baseY, brush.width, brush.height);
            float amount = debrisSize / terrainData.size.y;
            for (int y = 0; y < brush.height; y++)
            {
                for (int x = 0; x < brush.width; x++)
                {
                    heights[x, y] += spread[x, y] * amount;
                }
            }
            PassiveSoil(brush.baseX, brush.baseY, heights);
        }

        #region IRunTimeTerrainSystem
        public void ActiveSoil(int baseX, int baseY, float[,] heights)
        {
            // TODO: 생성 알고리즘을 수정해야 함.
            // heigts[y, x]

            var sourceHeights = GetHeights(baseX - 1, baseY - 1, heights.GetLength(1) + 2, heights.GetLength(0) + 2);
            SetHeights(baseX, baseY, heights);
            heights = GetHeights(baseX - 1, baseY - 1, heights.GetLength(1) + 2, heights.GetLength(0) + 2);

            Vector3[,] sources = new Vector3[sourceHeights.GetLength(0), sourceHeights.GetLength(1)];
            Vector3[,] targets = new Vector3[sourceHeights.GetLength(0), sourceHeights.GetLength(1)];
            
            for (var y = 0; y < sourceHeights.GetLength(1); y++)
                for (var x = 0; x < sourceHeights.GetLength(0); x++)
                {
                    sources[x, y] = TerrainPositionToWorld(new Vector3(baseX + x + 0.5f - 1, sourceHeights[y, x], baseY + y + 0.5f - 1));
                    targets[x, y] = TerrainPositionToWorld(new Vector3(baseX + x + 0.5f - 1, heights[y, x], baseY + y + 0.5f - 1));
                }

            float step = debrisSize / terrainTexelSize;
            for (float j = 0; j < sources.GetLength(1) - 1; j += step)
                for (float i = 0; i < sources.GetLength(0) - 1; i += step)
                {
                    int x = (int)i;
                    int y = (int)j;

                    float x_ratio = i - x;
                    float y_ratio = j - y;

                    Vector3 source = Vector3.Lerp(Vector3.Lerp(sources[x, y], sources[x + 1, y], x_ratio), Vector3.Lerp(sources[x, y + 1], sources[x + 1, y + 1], x_ratio), y_ratio);
                    Vector3 target = Vector3.Lerp(Vector3.Lerp(targets[x, y], targets[x + 1, y], x_ratio), Vector3.Lerp(targets[x, y + 1], targets[x + 1, y + 1], x_ratio), y_ratio);

                    while (target.y + 0.001f < source.y)
                    {
                        // 잔해가 생성될 위치 오프셋과, 회전값을 랜덤으로 생성합니다.
                        //var offset = Random.insideUnitSphere * step * 0.5f - new Vector3(step, -step * 0.5f, step);
                        //var rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

                        //var debris = Instantiate(DebrisPrefab, source + offset, rotation);
                        var debris = Instantiate(DebrisPrefab, source, Quaternion.identity);
                        source.y -= step;
                    }
                }
        }

        public void PassiveSoil(int baseX, int baseY, float[,] heights)
        {
            StartCoroutine(SmoothPassiveSoil(baseX, baseY, heights));
        }

        public float[,] GetHeights(int baseX, int baseY, int width, int height)
        {
            return terrainData.GetHeights(baseX, baseY, width, height);
        }

        public void SetHeights(int baseX, int baseY, float[,] heights)
        {
            terrainData.SetHeights(baseX, baseY, heights);
        }
        #endregion

        private IEnumerator SmoothPassiveSoil(int baseX, int baseY, float[,] heights, float smooth = 1.0f)
        {
            var deltaHeights = GetHeights(baseX, baseY, heights.GetLength(1), heights.GetLength(0));
            for (var y = 0; y < heights.GetLength(0); y++)
            {
                for (var x = 0; x < heights.GetLength(1); x++)
                {
                    deltaHeights[x, y] = heights[x, y] - deltaHeights[x, y];
                }
            }

            while (smooth > 0.0f)
            {
                float spend = Mathf.Min(Time.deltaTime, smooth);
                smooth -= spend;

                var currentHeights = GetHeights(baseX, baseY, heights.GetLength(1), heights.GetLength(0));
                for (var y = 0; y < heights.GetLength(0); y++)
                {
                    for (var x = 0; x < heights.GetLength(1); x++)
                    {
                        currentHeights[x, y] += deltaHeights[x, y] * spend;
                    }
                }
                SetHeights(baseX, baseY, currentHeights);

                yield return null;
            }
        }

        /// <summary>
        /// 월드 포지션을 터레인 포지션으로 반환합니다.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        private Vector3 WorldToTerrainPosition(Vector3 worldPosition)
        {
            var terrainSize = terrainData.size;
            var heightmapResolution = terrainData.heightmapResolution;

            var terrainPosition = worldPosition - terrain.GetPosition();
            terrainPosition = new Vector3(terrainPosition.x / terrainSize.x, terrainPosition.y / terrainSize.y, terrainPosition.z / terrainSize.z);
            terrainPosition = new Vector3(terrainPosition.x * heightmapResolution, terrainPosition.y, terrainPosition.z * heightmapResolution);
            return terrainPosition;
        }

        /// <summary>
        /// 터레인 포지션을 월드 포지션으로 변환합니다.
        /// </summary>
        /// <param name="terrainPosition"></param>
        /// <returns></returns>
        private Vector3 TerrainPositionToWorld(Vector3 terrainPosition)
        {
            var terrainSize = terrainData.size;
            var heightmapResolution = terrainData.heightmapResolution;

            var worldPosition = new Vector3(terrainPosition.x / heightmapResolution, terrainPosition.y, terrainPosition.z / heightmapResolution);
            worldPosition = new Vector3(worldPosition.x * terrainSize.x, worldPosition.y * terrainSize.y, worldPosition.z * terrainSize.z) + terrain.GetPosition();
            return worldPosition;
        }

        /// <summary>
        /// 브러시 범위를 반환합니다.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="brushWidth"></param>
        /// <param name="brushHeight"></param>
        /// <returns></returns>
        private (int baseX, int baseY, int width, int height) GetBrushRange(Vector3 worldPosition, int brushWidth, int brushHeight)
        {
            var terrainPosition = WorldToTerrainPosition(worldPosition);
            var heightmapResolution = terrainData.heightmapResolution;

            var brushPosition =  new Vector2Int(
                Mathf.RoundToInt(Mathf.Clamp(terrainPosition.x - brushWidth / 2.0f, 0.0f, heightmapResolution)), 
                Mathf.RoundToInt(Mathf.Clamp(terrainPosition.z - brushHeight / 2.0f, 0.0f, heightmapResolution)));

            while (heightmapResolution - (brushPosition.x + brushWidth) < 0) brushWidth--;
            while (heightmapResolution - (brushPosition.y + brushHeight) < 0) brushHeight--;

            return (brushPosition.x, brushPosition.y, brushWidth, brushHeight);
        }
    }
}