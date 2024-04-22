using UnityEngine;

namespace Gameplay
{
    public class Trash : MonoBehaviour
    {
        public TrashData data;

        public void Disappear()
        {
            // could replace this with a collision disable & animation/particles
            Destroy(gameObject);
        }
    }
}