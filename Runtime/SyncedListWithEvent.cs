using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventObjects
{
    public abstract class SyncedListWithEvent<T> : ValueWithEvent<List<T>, SyncedListEvent<T>> where T : new()
    {
        public CollectionModificationEvent<T> OnAdd = new CollectionModificationEvent<T>();
        public CollectionModificationEvent<T> OnRemove = new CollectionModificationEvent<T>();

        
        public void Add(T item)
        {
            Value.Add(item);
            OnAdd.Invoke(new[] {item});
        }
        public void Remove(T item)
        {
            Value.Remove(item);
            OnRemove.Invoke(new[] {item});
        }

        public void Clear()
        {
            Value.Clear();
        }

        public void AddRange(IEnumerable<T> items)
        {
            Value.AddRange(items);
            OnAdd.Invoke(items);
        }

        public void RemoveAll(Predicate<T> match)
        {
            var matching = Value.FindAll(match);
            Value.RemoveAll(match);
            OnRemove.Invoke(matching);
        }

        public void CompleteModifications()
        {
            OnChange.Invoke(Value);
        }
        
    }
    public class CollectionModificationEvent<T> : UnityEvent<IEnumerable<T>> where T : new () { }
    
    public class SyncedListEvent<T> : UnityEvent<List<T>> { }
}