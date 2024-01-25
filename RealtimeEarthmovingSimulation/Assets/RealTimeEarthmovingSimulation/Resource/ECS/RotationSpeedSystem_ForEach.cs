using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class RotationSpeedSystem_ForEach : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = UnityEngine.Time.deltaTime;

        Entities
            .WithName("CupOfCoffeeAuthoring")
            // �׻� ref ������ in �� �־���Ѵ�.
            .ForEach((in IsCoffee rotationSpeed) =>
            {
                //rotation.Value = math.mul(
                //        math.normalize(rotation.Value),
                //        quaternion.AxisAngle(math.up(), rotationSpeed.RadiansPerSecond * deltaTime));
            }).ScheduleParallel();
    }
}