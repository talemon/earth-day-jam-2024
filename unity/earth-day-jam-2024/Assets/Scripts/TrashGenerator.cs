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
            CreateTrash(prefabs[Random.Range(0, prefabs.Count() - 1)], trashPos);
        }
    }

    void CreateTrash(GameObject trashPrefab, Vector2 pos)
    {
        Instantiate(trashPrefab, new Vector3(pos.x, DefaultHeight, pos.y), Random.rotation, transform);
    }
}
