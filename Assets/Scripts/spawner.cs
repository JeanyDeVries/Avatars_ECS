using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;

public class spawner : MonoBehaviour
{
    [SerializeField] private int totalEntities;

    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(AvatarData),
            typeof(Translation)
        );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(totalEntities, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);

        for (int i = 0; i < entityArray.Length; i++)
        {
            Entity entity = entityArray[i];
            entityManager.SetComponentData(entity, new AvatarData
            {
                head = Random.Range(0, 5),
                body = Random.Range(0, 5),
                feet = Random.Range(0, 5)
            });
        }

        entityArray.Dispose();
    }
}
