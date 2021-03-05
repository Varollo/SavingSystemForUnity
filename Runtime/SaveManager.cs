using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Varollo.SavingSystem
{
    public static class SaveManager
    {
        private static readonly string SaveFileName = "save";
        private static readonly string SaveFileExtension = "dat";
        private static readonly List<SaveableGameObject> _saveableGameObjects = new List<SaveableGameObject>();

        /// <summary>
        /// Returns the full save path of the desired fileId.
        /// </summary>
        /// <param name="fileId"></param>
        private static string GetFullSavePath(int fileId) => $"{Application.persistentDataPath}/{SaveFileName}{fileId}.{SaveFileExtension}";

        /// <summary>
        /// Adds a saveable game object to the list that is going to be saved.
        /// </summary>
        /// <param name="saveable"></param>
        public static void AddToSaveList(SaveableGameObject saveable)
        {
            if (!_saveableGameObjects.Contains(saveable))
            {
                _saveableGameObjects.Add(saveable);
                //Debug.Log(saveable.name);
            }
            else
            {
                //Debug.Log(saveable.name + " already in To Save list.");
            }
        }

        /// <summary>
        /// Removes a sabeable game object from the list that is going to be saved.
        /// </summary>
        /// <param name="saveable"></param>
        public static void RemoveFromSaveList(SaveableGameObject saveable)
        {
            _saveableGameObjects.Remove(saveable);
        }

        /// <summary>
        /// Saves all currently loaded GameObjects with a SaveableGameObject componnent.
        /// </summary>
        /// <param name="fileId"></param>
        public static void Save(int fileId = 0)
        {
            Dictionary<string, object> state = LoadFile(fileId);
            CaptureState(ref state);
            SaveFile(state, fileId);
        }

        /// <summary>
        /// Loads to all currently loaded GameObjects with a SaveableGameObject componnent.
        /// </summary>
        /// <param name="fileId"></param>
        public static void Load(int fileId = 0)
        {
            Dictionary<string, object> state = LoadFile(fileId);
            RestoreState(state);
        }

        /// <summary>
        /// Deletes the save file for this fileId.
        /// </summary>
        /// <param name="fileId"></param>
        public static void DeleteSave(int fileId = 0)
        {
            if (File.Exists(GetFullSavePath(fileId)))
            {
                File.Delete(GetFullSavePath(fileId));
            }
        }

        private static void SaveFile(object state, int fileId = 0)
        {
            using (FileStream stream = File.Open(GetFullSavePath(fileId), FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        /// <summary>
        /// Loads a dictionry with the saved objects.
        /// </summary>
        /// <param name="fileId"></param>
        private static Dictionary<string, object> LoadFile(int fileId = 0)
        {
            if (!File.Exists(GetFullSavePath(fileId)))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(GetFullSavePath(fileId), FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = 0;
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Loops through the list of objects to save and adds the data to a Dictionary with the ID as the key.
        /// </summary>
        /// <param name="state"></param>
        private static void CaptureState(ref Dictionary<string, object> state)
        {
            foreach (SaveableGameObject saveable in _saveableGameObjects)
            {
                state[saveable.Id] = saveable.CaptureState();
            }
        }

        /// <summary>
        /// Loops through the list of objects to save and restore the data based on a Dictionary if it contains the ID.
        /// </summary>
        /// <param name="state"></param>
        private static void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableGameObject saveable in _saveableGameObjects)
            {
                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
                else
                {
                    saveable.RestoreState(null);
                }
            }
        }
    }
}
