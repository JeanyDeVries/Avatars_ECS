using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class HelloSpawnerProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{

    public static Entity[] towerEntities;
    public GameObject[] towerPrefabs;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        towerEntities = new Entity[towerPrefabs.Length];
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            Entity towerPrefabEntity = conversionSystem.GetPrimaryEntity(towerPrefabs[i]);
            towerEntities[i] = towerPrefabEntity;
        }
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            referencedPrefabs.Add(towerPrefabs[i]);
        }
    }
}
