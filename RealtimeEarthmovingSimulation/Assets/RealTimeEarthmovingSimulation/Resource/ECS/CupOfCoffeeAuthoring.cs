using UnityEngine;
using Unity.Entities;

[DisallowMultipleComponent]
public class CupOfCoffeeAuthoring : MonoBehaviour
{

    [Range(0, 1)] public float Fill = 0.5f;
    [Min(0)] public int Intensity = 5;
    public GameObject InstantiateOnDestroyed;

    public class Baker : Baker<CupOfCoffeeAuthoring>// <<< HERE IS THIS BAKER<COMPONENT> CLASS
    {
        public override void Bake(CupOfCoffeeAuthoring authoring)
        {
            Entity entity = this.GetEntity(authoring, TransformUsageFlags.Dynamic);
            // note on TransformUsageFlags: https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.TransformUsageFlags.html#fields

            AddComponent<IsCoffee>(entity);
            AddComponent(entity, new Fill
            {
                Value = authoring.Fill,
            });
            AddComponent(entity, new Intensity
            {
                Value = authoring.Intensity,
            });
            AddComponent(entity, new InstantiateOnDestroyed
            {
                Prefab = this.GetEntity(authoring.InstantiateOnDestroyed, TransformUsageFlags.Dynamic),
            });
        }
    }

}

public struct IsCoffee : IComponentData { }
public struct Fill : IComponentData
{
    public float Value;
}
public struct Intensity : IComponentData
{
    public int Value;
}
public struct InstantiateOnDestroyed : IComponentData
{
    public Entity Prefab;
}