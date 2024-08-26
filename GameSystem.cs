using System;
using System.Collections.Generic;
using UnityEngine;

namespace JonathonOH.UnityTools.SystemsManagement
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
        public static List<GameObject> gameSystems;

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
            gameSystems = new List<GameObject>();

            GameObject systems = Instantiate(GetSystemPrefab());
            systems.name = systemPrefabName;

            // Instantiate all the systems
            foreach (Transform child in systems.transform)
            {
                if (!child.gameObject.activeInHierarchy) continue;
                if (!child.TryGetComponent(out IGameSystem system)) continue;
                if (system.IsInstantiated()) continue;

                system.OnInstantiation();
                if (system.IsInstantiated())
                {
                    gameSystems.Add(child.gameObject);
                }
                else
                {
                    Debug.LogError($"Could not start {system} GameSystem!");
                }
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

        public static void PromptLoad() { CreateSystems(); }

        public static T Get()
        {
            PromptLoad();
            foreach (GameObject systemObject in gameSystems)
            {
                if (systemObject.TryGetComponent(out T system)) return system;
            }
            throw new KeyNotFoundException($"Could not find a system of type ${typeof(T)}");
        }
    }
}