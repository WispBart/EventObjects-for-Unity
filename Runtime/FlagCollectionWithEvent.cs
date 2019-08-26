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

using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace EventObjects
{
    /// <summary>
    /// Class for registering multiple callers and returning a single bool value.
    /// For example, if you want to know if there is a menu, you can let each menu
    /// in your game Register with an instance of this class and their 'enabled'
    /// state. Client code should then listen to the OnChange event and check the
    /// Or() value.
    /// </summary>
    [CreateAssetMenu(menuName = "EventObjects/State Flag Object")]
    public class FlagCollectionWithEvent : ScriptableObject
    {
        public BoolEvent OnChangeAnd;
        public BoolEvent OnChangeOr;
        
        private Dictionary<Object, bool> _states = new Dictionary<Object, bool>();

        private bool _and = false;
        private bool _or = false;


        /// <summary>
        /// Return AND of all registered states.
        /// </summary>
        /// <returns>True if all registered objects are 'true'.</returns>
        public bool And() => _and;
        /// <summary>
        /// Return OR of all registered states.
        /// </summary>
        /// <returns>True if any registered object is 'true'.</returns>
        public bool Or() => _or;

        /// <summary>
        /// Force a recalculation. You should not need to call this.
        /// </summary>
        public void ForceRecalc() => Recalc();
        
        private void Recalc()
        {
            bool newAnd = GetAnd();
            bool newOr = GetOr();
            bool andChanged = newAnd != _and;
            bool orChanged = newOr != _or;
            _and = newAnd;
            _or = newOr;
            if (andChanged) OnChangeAnd.Invoke(_and);
            if (orChanged) OnChangeOr.Invoke(_or);
        }
    
        /// <summary>
        /// Adds the object to the collection. RemoveStateObject must be called to prevent leaks.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="newValue"></param>
        public void AddStateObject(Object obj, bool newValue)
        {
            if (_states.ContainsKey(obj))
            {
                if (_states[obj] != newValue)
                {
                    _states[obj] = newValue;
                    Recalc();
                }
            }
            else
            {
                _states.Add(obj, newValue);
                Recalc();
            }
        }

        /// <summary>
        /// Removes the Object from the list. Must be called to prevent leaks.
        /// </summary>
        /// <param name="obj">A UnityObject that has been registered with the FlagCollection object.</param>
        public void RemoveStateObject(Object obj)
        {
            _states.Remove(obj);
            Recalc();
        }
    
        private bool GetAnd()
        {
            foreach (KeyValuePair<Object,bool> keyValuePair in _states)
            {
                if (!keyValuePair.Value)
                {
                    return false;
                }
            }
            return true;
        }

        private bool GetOr()
        {
            foreach (KeyValuePair<Object,bool> keyValuePair in _states)
            {
                if (keyValuePair.Value)
                {
                    return true;
                }
            }
            return false;
        }

        [ContextMenu("Debug And")] public void Test_DebugAnd() => Debug.Log($"Value is: {And()}", this);
        [ContextMenu("Debug Or")] public void Test_DebugOr() => Debug.Log($"Value is: {Or()}", this);

    }
}
