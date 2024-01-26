using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class DebrisController : MonoBehaviour
{
    public Renderer DebrisRenderer;
    public Mesh DebrisMesh;
    public List<Material> DebrisMaterials;

    private EntityManager m_entityManager;
    private List<Entity> m_entities = new List<Entity>();

    private void Start()
    {
        m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //for (int i = 0; i < 100000; i++)
        //{
        //    var newEntity = m_entityManager.CreateEntity();
        //    m_entityManager.AddComponentData(newEntity, new Debris());
        //    //m_entityManager.SetSharedComponentManaged(newEntity, new RenderMesh(DebrisRenderer, DebrisMesh, DebrisMaterials));

        //    m_entities.Add(newEntity);
        //    Debug.Log($"SpawnController: CreateEntity: {m_entities}");
        //}
    }
}
