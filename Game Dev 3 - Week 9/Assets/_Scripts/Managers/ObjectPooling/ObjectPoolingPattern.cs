using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithhanniel.DesignPattern
{
    public class ObjectPoolingPattern : Singleton<ObjectPoolingPattern>
    {
        [SerializeField] PoolData bulletPool;
        [SerializeField] PoolData muzzleFlashPool;
       
        public enum TypeOfPool
        {
            BulletePool,
            MuzzleFlash
        }
        // Start is called before the first frame update

        private void Start()
        {
            FillThePool(bulletPool);
            FillThePool(muzzleFlashPool);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void FillThePool(PoolData poolData)
        {
            poolData.ResetThePool();

            for (int i = 0; i < poolData.poolAmount; i++)
            {
                GameObject thingToAddToThePool = Instantiate(poolData.poolItem);

                thingToAddToThePool.transform.parent = transform;

                thingToAddToThePool.SetActive(false);

                poolData.pooledObjectContainer.Add(thingToAddToThePool);
            }
        }

        public GameObject GetPoolItem(TypeOfPool poolToUse)
        {
           PoolData pool =  ScriptableObject.CreateInstance<PoolData>();
            switch (poolToUse)
            { 
                case TypeOfPool.BulletePool:
                    pool = bulletPool;
                    break;
                case TypeOfPool.MuzzleFlash:
                    pool = muzzleFlashPool;
                    break;
            
            
            }



            for (int i = 0;i < pool.pooledObjectContainer.Count; i++)
            {
                if (!pool.pooledObjectContainer[i].activeInHierarchy)
                {
                    pool.pooledObjectContainer[i].SetActive(true);
                    return pool.pooledObjectContainer[i];
                }
            }

            return null;
        }
    }
}
