using System.Collections;
using System.Collections.Generic;
using TerrainSystem;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;

    float activeTime = 0.0f;

    public void FixedUpdate()
    {
        // y kill
        if (transform.position.y < -100)
            Destroy(gameObject);

        // 3�� �̻� �������� ������ ����
        if (activeTime > 3.0f)
            Passive();

        if (m_rigidbody.velocity.magnitude > 0.5f)
        {
            activeTime = 0.0f;
        }
        else
        {
            activeTime += Time.deltaTime;
        }
    }

    private void Passive()
    {
        RunTimeTerrainSystem terrainsystem = Terrain.activeTerrain.GetComponent<RunTimeTerrainSystem>();
        terrainsystem.TestPassiveSoil(transform.position);

        Destroy(gameObject);
    }
}
