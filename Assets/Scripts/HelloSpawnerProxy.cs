using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

[RequiresEntityConversion]
public class HelloSpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    [SerializeField] private int totalEntities;

    public static Entity[] avatarEntities;
    public GameObject[] avatarPrefabs;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        avatarEntities = new Entity[totalEntities];
        for (int i = 0; i < totalEntities; i++)
        {
            int randomNumber = Random.Range(0, avatarPrefabs.Length);
            Entity avatarEntity = conversionSystem.GetPrimaryEntity(avatarPrefabs[randomNumber]);
            avatarEntities[i] = avatarEntity;
            Entity spawnedEntity = dstManager.Instantiate(avatarEntity);


            dstManager.AddComponent(spawnedEntity, typeof(AvatarData));
            dstManager.AddComponent(spawnedEntity, typeof(Translation));
            dstManager.AddComponent(spawnedEntity, typeof(LocalToWorld));
            dstManager.SetComponentData(spawnedEntity, new AvatarData
            {

            });
            dstManager.SetComponentData(spawnedEntity, new Translation
            {
                Value = new float3(
                    Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5))
            });
            dstManager.SetComponentData(spawnedEntity, new LocalToWorld
            {
                Value = new float4x4(
                    rotation: quaternion.identity,
                    translation: new float3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5)))
            });

            Debug.Log(spawnedEntity);
        }
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        for (int i = 0; i < avatarPrefabs.Length; i++)
        {
            referencedPrefabs.Add(avatarPrefabs[i]);
        }
    }
}
