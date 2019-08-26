//
// EventObjects - A scriptable-object based messaging system for Unity
//
// Copyright (c) 2019 Bart Heijltjes (Wispfire)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventObjects
{
    /// <summary>
    /// This EventObject can be used to pair a collider to a cloth physics components,
    /// without them directly referencing each other.
    /// </summary>
    [CreateAssetMenu(menuName = "EventObjects/ClothColliderSetup", fileName = "New ClothColliderSetup")]
    public class ClothCollidersWithEvent : ValueWithEvent<ClothColliderSetup, ColliderListEvent>
    {
        public void AddCapsule(CapsuleCollider collider, bool triggerOnChange = false)
        {
            Value.Capsules.Add(collider);
            if (triggerOnChange) FinishModifications();
        }
        
        public void AddCapsules(IEnumerable<CapsuleCollider> colliders, bool triggerOnChange = false)
        {
            Value.Capsules.AddRange(colliders);
            if (triggerOnChange) FinishModifications();
        }
        
        public void RemoveCapsule(CapsuleCollider collider, bool triggerOnChange = false)
        {
            Value.Capsules.Remove(collider);
            if (triggerOnChange) FinishModifications();
        }

        public void AddSpherePair(SerializableClothSphereColliderPair colliders, bool triggerOnChange = false)
        {
            Value.Spheres.Add(colliders);
            if (triggerOnChange) FinishModifications();
        }
        
        public void AddSpherePairs(IEnumerable<SerializableClothSphereColliderPair> colliders, bool triggerOnChange = false)
        {
            Value.Spheres.AddRange(colliders);
            if (triggerOnChange) FinishModifications();
        }
        
        public void RemoveSpherePair(SerializableClothSphereColliderPair collider, bool triggerOnChange = false)
        {
            Value.Spheres.Remove(collider);
            if (triggerOnChange) FinishModifications();
        }

        /// <summary>
        /// Triggers OnChange event.
        /// </summary>
        public void FinishModifications()
        {
            OnChange.Invoke(Value);
        }
        

    }
    

    [Serializable]
    public class ColliderListEvent : UnityEvent<ClothColliderSetup> { }

    [Serializable]
    public class ClothColliderSetup
    {
        public List<CapsuleCollider> Capsules = new List<CapsuleCollider>();
        public List<SerializableClothSphereColliderPair> Spheres = new List<SerializableClothSphereColliderPair>();
    }

    [Serializable]
    public class SerializableClothSphereColliderPair
    {
        public SphereCollider A;
        public SphereCollider B;

        public ClothSphereColliderPair ToRuntime()
        {
            return new ClothSphereColliderPair(A, B);
        } 
    }

}



