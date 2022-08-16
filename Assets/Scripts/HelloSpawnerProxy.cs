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

    public static Entity[] avatarEntitiesHeads;
    public GameObject[] avatarPrefabsHeads;

    public static Entity[] avatarEntitiesBodies;
    public GameObject[] avatarPrefabsBodies;

    public static Entity[] avatarEntitiesFeet;
    public GameObject[] avatarPrefabsFeet;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        avatarEntitiesHeads = new Entity[totalEntities];
        avatarEntitiesBodies = new Entity[totalEntities];
        avatarEntitiesFeet = new Entity[totalEntities];


        for (int i = 0; i < totalEntities; i++)
        {
            int randomNumber = Random.Range(0, avatarPrefabsBodies.Length);

            Entity avatarBodyEntity = conversionSystem.GetPrimaryEntity(avatarPrefabsBodies[randomNumber]);
            avatarEntitiesBodies[i] = avatarBodyEntity;
            Entity spawnedBodyEntity = dstManager.Instantiate(avatarBodyEntity);
            AddComponentData(dstManager, spawnedBodyEntity, true, 0f);

            randomNumber = Random.Range(0, avatarPrefabsHeads.Length);
            Entity avatarHeadEntity = conversionSystem.GetPrimaryEntity(avatarPrefabsHeads[randomNumber]);
            avatarEntitiesHeads[i] = avatarHeadEntity;
            Entity spawnedHeadEntity = dstManager.Instantiate(avatarHeadEntity);
            AddComponentData(dstManager, spawnedHeadEntity, false, 0.75f);

            randomNumber = Random.Range(0, avatarPrefabsFeet.Length);
            Entity avatarFeetEntity = conversionSystem.GetPrimaryEntity(avatarPrefabsFeet[randomNumber]);
            avatarEntitiesFeet[i] = avatarFeetEntity;
            Entity spawnedFeetEntity = dstManager.Instantiate(avatarFeetEntity);
            AddComponentData(dstManager, spawnedFeetEntity, false, -0.9f);


            dstManager.AddComponentData(spawnedHeadEntity, new Parent { Value = spawnedBodyEntity });
            dstManager.AddComponentData(spawnedFeetEntity, new Parent { Value = spawnedBodyEntity });
            dstManager.AddComponentData(spawnedHeadEntity, new LocalToParent());
            dstManager.AddComponentData(spawnedFeetEntity, new LocalToParent());
        }
    }

    public void AddComponentData(EntityManager dstManager, Entity entity, bool isBody, float height)
    {
        dstManager.AddComponent(entity, typeof(AvatarData));
        dstManager.AddComponent(entity, typeof(Translation));
        dstManager.AddComponent(entity, typeof(LocalToWorld));
        dstManager.SetComponentData(entity, new AvatarData
        {

        });
        dstManager.SetComponentData(entity, new LocalToWorld
        {
            Value = new float4x4(
                rotation: quaternion.identity,
                translation: new float3(0, 0, 0))
        });
        if(isBody)
        {
            dstManager.SetComponentData(entity, new Translation
            {
                Value = new float3(
                    Random.Range(0, 100), height, Random.Range(0, 100))
            });
        }
        else
        {
            dstManager.SetComponentData(entity, new Translation
            {
                Value = new float3(
                    Random.Range(0, 0), height, Random.Range(0, 0))
            });
        }
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        for (int i = 0; i < avatarPrefabsBodies.Length; i++)
        {
            referencedPrefabs.Add(avatarPrefabsBodies[i]);
        }

        for (int i = 0; i < avatarPrefabsHeads.Length; i++)
        {
            referencedPrefabs.Add(avatarPrefabsHeads[i]);
        }

        for (int i = 0; i < avatarPrefabsFeet.Length; i++)
        {
            referencedPrefabs.Add(avatarPrefabsFeet[i]);
        }
    }
}
