using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    public enum TerrainModificationAction
    {
        Raise,
        Lower,
        Flatten,
        Sample,
        SampleAverage,
    }

    public sealed class RunTimeTerrainEditController : MonoBehaviour
    {
        public int brushWidth;
        public int brushHeight;

        [Range(0.001f, 0.1f)]
        public float strength;

        public TerrainModificationAction modificationAction;
        private UnityEngine.Terrain _targetTerrain;
        private float _sampledHeight;


        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    if (hit.transform.TryGetComponent(out UnityEngine.Terrain terrain)) _targetTerrain = terrain;

                    switch (modificationAction)
                    {
                        case TerrainModificationAction.Raise:

                            _targetTerrain.RaiseTerrain(hit.point, strength, brushWidth, brushHeight);

                            break;

                        case TerrainModificationAction.Lower:

                            _targetTerrain.LowerTerrain(hit.point, strength, brushWidth, brushHeight);

                            break;

                        case TerrainModificationAction.Flatten:

                            _targetTerrain.FlattenTerrain(hit.point, _sampledHeight, brushWidth, brushHeight);

                            break;

                        case TerrainModificationAction.Sample:

                            _sampledHeight = _targetTerrain.SampleHeight(hit.point);

                            break;

                        case TerrainModificationAction.SampleAverage:

                            _sampledHeight = _targetTerrain.SampleAverageHeight(hit.point, brushWidth, brushHeight);

                            break;
                    }
                }
            }
        }
    }

}