using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] SmallTrashPrefabs;
    [SerializeField] private GameObject[] BigTrashPrefabs;
    
    [SerializeField] private Vector2 BottomLeft;
    [SerializeField] private Vector2 TopRight;
    [SerializeField] private float MinimumBigTrashDistance;
    [SerializeField] private float MinimumSmallTrashDistance;
    [SerializeField] private float DefaultHeight;

    void Awake()
    {
        CreateTrash(MinimumBigTrashDistance, BigTrashPrefabs);
        CreateTrash(MinimumSmallTrashDistance, SmallTrashPrefabs);
    }

    void CreateTrash(float minDistance, GameObject[] prefabs)
    {
        var positions = Gists.FastPoissonDiskSampling.Sampling(BottomLeft, TopRight, minDistance);
        foreach (var trashPos in positions)
        {
            int trashIndex = Random.Range(0, prefabs.Length - 1);
            CreateTrash(prefabs[trashIndex], trashPos, trashIndex == 1);
        }
    }

    void CreateTrash(GameObject trashPrefab, Vector2 pos, bool fixedRotation=false)
    {
        Quaternion rotation = fixedRotation ? Quaternion.identity : Random.rotation;
        Instantiate(trashPrefab, new Vector3(pos.x, DefaultHeight, pos.y), rotation, transform);
    }
}
