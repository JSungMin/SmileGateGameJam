using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour {

    public GameObject Mon;
    public List<GameObject> Mons;

    void Start()
    {

        Mons = new List<GameObject>();
        LoadObject(Mon, Mons);
        GetPooledObject(Mons).SetActive(true);
        Debug.Log(GetPooledObject(Mons));
        
    }

    private void LoadObject(GameObject Obj, List<GameObject> Objs)
    {
        foreach(Transform child in Obj.transform)
        {
            if (child != null)
                Objs.Add(child.gameObject);
        }
    }

    public GameObject GetPooledObject(List<GameObject> Objs)
    {
        for(int i = 0; i < Objs.Count; i++)
        {
            if (!Objs[i].activeInHierarchy)
                return Objs[i];
        }
        return null;
    }

    public void SetPooledObject(GameObject Obj)
    {
        Obj.SetActive(false);
    }
    
}
