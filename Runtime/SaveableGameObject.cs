using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Varollo.SavingSystem
{
    public class SaveableGameObject : MonoBehaviour
    {
        [SerializeField] private bool addToSaveList = true;
        [SerializeField] private bool removeFromSaveList = true;
        [Space]
        [SerializeField] private string id;
        public string Id { get => id; private set => id = value; }
        public void GenerateId() => Id = Guid.NewGuid().ToString();

        /// <summary>
        /// When object is loaded add it to the list of GameObjects that are gonna be saved.
        /// </summary>
        private void Awake()
        {
            if (addToSaveList)
            {
                SaveManager.AddToSaveList(this);
            }
        }

        /// <summary>
        /// When object is destroyed remove it from the list of GameObjects that are gonna be saved.
        /// </summary>
        private void OnDestroy()
        {
            if (removeFromSaveList)
            {
                SaveManager.RemoveFromSaveList(this);
            }
        }

        /// <summary>
        /// Captures the state of all ISaveable componnents in this GameObject.
        /// </summary>
        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        /// <summary>
        /// Loops through the ISaveable componnents in this GameObject and restore the states based on saved data.
        /// </summary>
        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)state;

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                if (state == null)
                {
                    saveable.OnNullState();
                }
                else if (stateDictionary.TryGetValue(saveable.GetType().ToString(), out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}
