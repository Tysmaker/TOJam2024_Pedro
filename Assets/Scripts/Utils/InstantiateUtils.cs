using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class InstantiateUtils
    {
        public static GameObject InstantiatePrefab(GameObject prefab, Vector3? position = null, Quaternion? rotation = null, Transform parent = null, string name = null)
        {
            GameObject instance = GameObject.Instantiate(prefab);

            if (rotation != null)
            {
                instance.transform.rotation = rotation.Value;
            }
            if (position != null)
            {
                instance.transform.position = position.Value;
            }
            if (parent != null)
            {
                instance.transform.SetParent(parent);
            }
            if (name != null)
            {
                instance.name = name;
            }
            return instance;
        }
    }
}