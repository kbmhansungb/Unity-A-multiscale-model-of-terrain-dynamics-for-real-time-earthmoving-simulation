using Debris;
using System;
using System.Collections;
using System.Collections.Generic;
using TerrainEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class FreeCameraController : MonoBehaviour
{
    public FreeCamera FreeCamera;
    public RunTimeTerrainEditor terrainEditor;

    [Header("Input")]
    public InputAction ActiveCameraInputAction;
    public InputAction EditTerrainInputAction;

    private void Awake()
    {
        // Shift가 눌렸을 때만 FreeCamera를 Enable합니다.
        FreeCamera.enabled = false;
        ActiveCameraInputAction.started += ActiveFreeCamera;
        ActiveCameraInputAction.canceled += DisactiveFreeCamera;
        ActiveCameraInputAction.Enable();

        EditTerrainInputAction.performed += EditTerrain;
        EditTerrainInputAction.Enable();
    }

    private void ActiveFreeCamera(InputAction.CallbackContext obj)
    {
        Debug.Log("FreeCameraController: 카메라를 활성화 합니다.");
        FreeCamera.enabled = true;
    }

    private void DisactiveFreeCamera(InputAction.CallbackContext obj)
    {
        Debug.Log("FreeCameraController: 카메라를 비활성화 합니다.");
        FreeCamera.enabled = false;
    }

    private void EditTerrain(InputAction.CallbackContext obj)
    {
        Debug.Log("FreeCameraController: 지형을 편집합니다.");
        
        // 카메라의 레이케스트 포지션을 구합니다.
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, 1000, LayerMask.GetMask("Terrain")))
        {
            // 레이케스트가 지형에 맞았다면, 지형을 편집합니다.
            terrainEditor.TestCall(hit.point);
            //DebrisSpawnerSystem.spawnPosition.Add(new System.Numerics.Vector3(hit.point.x, hit.point.y, hit.point.z));
        }
    }
}
