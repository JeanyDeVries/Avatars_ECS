using UnityEngine;
using Unity.Entities;

public class AvatarInteraction : SystemBase
{
    protected override void OnUpdate()
    {
        float time = Time.DeltaTime;
        Entities.ForEach((ref AvatarData avatarData) =>
        {
            avatarData.body += (int)(1f * time);
        }).Schedule();
    }
}
