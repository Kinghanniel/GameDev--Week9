using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithhanniel
{
    [CreateAssetMenu(fileName = "Pool", menuName = "Scriptable Objects/Pool")]
    public class PoolData : ScriptableObject
    {

        public List<GameObject> pooledObjectContainer = new List<GameObject>();
        [SerializeField] public int poolAmount = 40;
        [SerializeField] public GameObject poolItem;
        // Start is called before the first frame update
        

        public void ResetThePool()
        {
            pooledObjectContainer.Clear();
        }
    }
}
