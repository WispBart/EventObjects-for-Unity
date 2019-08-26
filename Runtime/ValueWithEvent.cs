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
using UnityEngine;
using UnityEngine.Events;

namespace EventObjects
{
    public abstract class BaseValueWithEvent : ScriptableObject
    {
        /// <summary>
        /// Invoke the event.
        /// </summary>
        public abstract void Invoke();
        public abstract bool IsInitialized { get; protected set; }
        public abstract void Reset();
        public abstract void Init();

        /// <summary>
        /// You can override this to return true to always invoke OnChange events,
        /// even if the new value is the same as the old one.
        /// </summary>
        protected virtual bool InvokeWithIdenticalValue => false;

    }
    
    public abstract class ValueWithEvent<T,TY> : BaseValueWithEvent where TY : UnityEvent<T>, new()
    {
        public TY OnChange = new TY();

        public T InitialValue;

        public override bool IsInitialized { get; protected set; }

        [SerializeField] private T _value;
        public T Value
        {
            get
            {
                if (!IsInitialized) Init();
                return _value;
            }
            set => SetValue(value);
        }

        /// <summary>
        /// Set a new value and invoke the change event if it is different.
        /// </summary>
        public virtual void SetValue(T x)
        {
            if (!IsInitialized) IsInitialized = true;

            if (!InvokeWithIdenticalValue)
            {
                if (x == null && _value == null) return;
                if (x != null && x.Equals(_value)) return;
            }
            _value = x;
            OnChange.Invoke(x);
        }

        /// <summary>
        /// Calls OnChange with the current value.
        /// </summary>
        public override void Invoke()
        {
            OnChange.Invoke(Value);
        }

        /// <summary>
        /// Sets the initial value and marks the object initialized.
        /// </summary>
        public override void Init()
        {
            _value = InitialValue;
            IsInitialized = true;
        }

        /// <summary>
        /// Get current value of the object and add a listener.
        /// </summary>
        /// <param name="action">Action that should happen on value change</param>
        public virtual T GetValueAndAddListener(UnityAction<T> action)
        {
            if (!IsInitialized) Init();
            OnChange.AddListener(action);
            return Value;
        }

        public void RemoveListener(UnityAction<T> action)
        {
            OnChange.RemoveListener(action);
        }

        /// <summary>
        /// Reset the object's fields.
        /// </summary>
        public override void Reset()
        {
            _value = default(T);
            IsInitialized = false;
        }

        // In the editor, reset IsInitialized value when changing playmode.
#if UNITY_EDITOR
        void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeStateChange;
        }

        void OnDisable()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeStateChange;

        }

        private void PlayModeStateChange(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode || state == UnityEditor.PlayModeStateChange.ExitingEditMode) Reset();
        }
#endif
        
    }
    
    [Serializable] public class BoolEvent : UnityEvent<bool>{}
    [Serializable] public class IntEvent : UnityEvent<int>{}
    [Serializable] public class FloatEvent : UnityEvent<float>{}
}
