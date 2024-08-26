using System;
using UnityEngine;

namespace JonathonOH.UnitySystemsManagement
{
    public interface IGameSystem
    {
        bool IsInstantiated();
        void OnInstantiation();
    }

    public class GameSystem<T> : MonoBehaviour, IGameSystem
    {
        private bool instantiated = false;
        private const string systemPrefabName = "Systems";
        private static T instance;
        public static T Instance { get => GetInstance(); }

        private static T GetInstance()
        {
            if (instance is null) CreateSystems();
            return instance;

        }

        public bool IsInstantiated()
        {
            return instantiated;
        }

        private static bool DoesSystemObjectExist()
        {
            return GameObject.Find(systemPrefabName) != null;
        }

        private static void CreateSystems()
        {
            if (DoesSystemObjectExist()) return;

            GameObject systems = Instantiate(GetSystemPrefab());
            systems.name = systemPrefabName;

            // Instantiate all the systems
            foreach (Transform child in systems.transform)
            {
                if (!child.gameObject.activeInHierarchy) continue;
                if (!child.TryGetComponent(out IGameSystem system)) continue;
                if (system.IsInstantiated()) continue;

                system.OnInstantiation();
                if (!system.IsInstantiated()) Debug.LogError($"Could not start {system} GameSystem!");
            }
        }

        private static GameObject GetSystemPrefab()
        {
            GameObject[] prefabs = Resources.LoadAll<GameObject>("");
            GameObject systemPrefab = null;

            foreach (GameObject prefab in prefabs)
            {
                if (prefab.name != systemPrefabName) continue;
                systemPrefab = prefab;
                break;
            }

            if (systemPrefab is null) Debug.LogError("Could not find prefab: " + systemPrefabName);
            return systemPrefab;
        }

        public void OnInstantiation()
        {
            instance = GetComponent<T>();
            AwakeSystem();
            instantiated = true;
        }

        protected virtual void AwakeSystem() { }

        /// <summary>
        /// Empty method. Just a point to wake the system up without other side effects.
        /// </summary>
        public static void PromptLoad()
        {
            CreateSystems();
        }
    }
}