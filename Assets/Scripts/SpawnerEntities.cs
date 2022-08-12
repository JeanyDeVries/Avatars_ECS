using Unity.Entities;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;


public class SpawnerEntities : MonoBehaviour
{
    [SerializeField] private int totalEntities;

    [SerializeField] private GameObject[] headPrefabs;
    [SerializeField] private GameObject[] bodyPrefabs;
    [SerializeField] private GameObject[] feetPrefabs;

    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(AvatarData),
            typeof(Translation),
            typeof(GameObject),
            typeof(LocalToWorld)
        );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(totalEntities, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);

        for (int i = 0; i < entityArray.Length; i++)
        {
            Entity entity = entityArray[i];
            entityManager.SetComponentData(entity, new AvatarData
            {
                
            });
        }

        entityArray.Dispose();
    }
}