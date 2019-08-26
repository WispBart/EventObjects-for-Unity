using UnityEngine;

namespace EventObjects.Monobehaviours
{
    /// <summary>
    /// Add to a cloth component and assign the ClothColliders field to have a cloth object
    /// and colliders find each other through the asset.
    /// Use in conjunction with RegisterColliderForCloth.
    /// </summary>
    [RequireComponent(typeof(Cloth))]
    public class SyncCollidersToCloth : MonoBehaviour
    {
        public ClothCollidersWithEvent ClothColliders;
        private Cloth _cloth;

        private bool _dirty;
    
        /// <summary>
        /// Immediately update colliders;
        /// </summary>
        public void ForceUpdateColliders()
        {
            if (_cloth == null) { Debug.LogWarning("Can not update colliders before Awake has been called"); }

            _cloth.capsuleColliders = ClothColliders.Value.Capsules.ToArray();
            _cloth.sphereColliders = ClothColliders.Value.Spheres.ConvertAll((x) => x.ToRuntime()).ToArray();
            _dirty = false;
        }
    
        private void Awake()
        {
            _cloth = GetComponent<Cloth>();
        }
    
        private void OnEnable()
        {
            ClothColliders.OnChange.AddListener(QueueUpdate);
            _dirty = true; // Only force update if the list isn't empty.
        }

        private void OnDisable()
        {
            ClothColliders.OnChange.RemoveListener(QueueUpdate);
        }

        private void Update()
        {
            if (_dirty) ForceUpdateColliders();
        }

        private void QueueUpdate(ClothColliderSetup latestItems)
        {
            _dirty = true;
        }
    
    
    
    }
}
