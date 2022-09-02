using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

[RequiresEntityConversion]
public class SpawnerEntities : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    #region REFERENCES
    [SerializeField] private int totalEntities;

    public static Entity[] avatarEntitiesHeads;

    public GameObject[] avatarPrefabsHeads;

    public static Entity[] avatarEntitiesBodies;
    public GameObject[] avatarPrefabsBodies;

    public static Entity[] avatarEntitiesFeet;
    public GameObject[] avatarPrefabsFeet;
    #endregion

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        avatarEntitiesHeads = new Entity[totalEntities];
        avatarEntitiesBodies = new Entity[totalEntities];
        avatarEntitiesFeet = new Entity[totalEntities];

        for (int i = 0; i < totalEntities; i++)
        {
            Entity bodyEntity = InstatiateEntity(dstManager, conversionSystem, avatarPrefabsBodies, avatarEntitiesBodies, i);
            Entity headEntity = InstatiateEntity(dstManager, conversionSystem, avatarPrefabsHeads, avatarEntitiesHeads, i);
            Entity feetEntity = InstatiateEntity(dstManager, conversionSystem, avatarPrefabsFeet, avatarEntitiesFeet, i);

            AddComponentData(dstManager, bodyEntity, true, 0f);
            AddComponentData(dstManager, headEntity, false, 0.75f);
            AddComponentData(dstManager, feetEntity, false, -0.9f);

            dstManager.AddComponentData(headEntity, new Parent { Value = bodyEntity });
            dstManager.AddComponentData(feetEntity, new Parent { Value = bodyEntity });
            dstManager.AddComponentData(headEntity, new LocalToParent());
            dstManager.AddComponentData(feetEntity, new LocalToParent());
        }
    }

    public Entity InstatiateEntity(EntityManager dstManager, GameObjectConversionSystem conversionSystem, GameObject[] prefabs, Entity[] entities, int i)
    {
        int randomNumber = Random.Range(0, prefabs.Length);

        Entity avatarEntity = conversionSystem.GetPrimaryEntity(prefabs[randomNumber]);
        entities[i] = avatarEntity;
        Entity spawnedEntity = dstManager.Instantiate(avatarEntity);

        return spawnedEntity;
    }

    public void AddComponentData(EntityManager dstManager, Entity entity, bool isBody, float height)
    {
        dstManager.AddComponent(entity, typeof(AvatarData));
        dstManager.AddComponent(entity, typeof(Translation));
        dstManager.AddComponent(entity, typeof(LocalToWorld));
        dstManager.SetComponentData(entity, new LocalToWorld
        {
            Value = new float4x4(
                rotation: quaternion.identity,
                translation: new float3(0, 0, 0))
        });
        if(isBody)
        {
            dstManager.AddComponent(entity, typeof(AABB));

            float3 position = new float3(
                    Random.Range(0, 100), height, Random.Range(0, 100));

            dstManager.SetComponentData(entity, new Translation
            {
                Value = position
            });
            dstManager.SetComponentData(entity, new AvatarData
            {
                movingSpeed = Random.Range(1, 5)
            });
            dstManager.SetComponentData(entity, new AABB
            {
                max = position + 0.5f,
                min = position - 0.5f,
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
