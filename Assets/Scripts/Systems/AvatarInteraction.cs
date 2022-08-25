using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class AvatarInteraction : SystemBase
{
    protected override void OnUpdate()
    {
        float time = Time.DeltaTime;

        Entities.ForEach((ref Translation translation, ref AvatarData data) =>
        {
            translation.Value.z += data.movingSpeed * time;
        }).ScheduleParallel();
    }
}
