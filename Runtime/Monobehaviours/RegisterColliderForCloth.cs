using System.Collections.Generic;
using UnityEngine;

namespace EventObjects.Monobehaviours
{
    /// <summary>
    /// Add to a GameObject with a collider assign the ClothColliders field to
    /// have a cloth object and colliders find each other through the asset.
    /// Use in conjunction with SyncCollidersToCloth.
    /// </summary>
    public class RegisterColliderForCloth : MonoBehaviour
    {
        public ClothCollidersWithEvent ClothColliderRegistry;

        public List<CapsuleCollider> Capsules;
        public List<SerializableClothSphereColliderPair> Spheres;

        private void Start()
        {
            ClothColliderRegistry.AddCapsules(Capsules);
            ClothColliderRegistry.AddSpherePairs(Spheres);
            ClothColliderRegistry.FinishModifications();
        }


    }
}
