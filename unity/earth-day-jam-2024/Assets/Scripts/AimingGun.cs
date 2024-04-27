using UnityEngine;

public class AimingGun : MonoBehaviour
{
    [SerializeField] private GameObject Target;

    private void Update()
    {
        if (Target.activeInHierarchy)
            transform.LookAt(Target.transform);
    }
}
