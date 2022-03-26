using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> pooledObjects;

    public GameObject objectToPool;
    public int amountToPool;

    // Start is called before the first frame update
    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++){
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
            if(tmp.CompareTag("Soldier")){
                if(i<amountToPool/2) tmp.AddComponent<Attackers>();
                else tmp.AddComponent<Defenders>();
            }
        }
    }

    public GameObject GetPooledObject(bool value){
        int startPoint = 0;
        if (!value) startPoint = amountToPool/2;
        for(int i = startPoint; i < amountToPool; i++){
            if(!pooledObjects[i].activeInHierarchy){
                return pooledObjects[i];
            }
        }
        return null;
    }

}
