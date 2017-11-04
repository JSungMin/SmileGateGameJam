using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public GameObject Mon;
    public List<GameObject> Mons;

    void Start()
    {

        Mons = new List<GameObject>();
        LoadObject(Mon, Mons);
        for(int i = 0; i < Mons.Count; i++)
        {
            GetPooledObject(Mons);
        }

    }

    private void LoadObject(GameObject Obj, List<GameObject> Objs)
    {
        foreach (Transform child in Obj.transform)
        {
            if (child != null)
                Objs.Add(child.gameObject);
        }
    }

    public GameObject GetPooledObject(List<GameObject> Objs)
    {
        for (int i = 0; i < Objs.Count; i++)
        {
            if (!Objs[i].activeInHierarchy)
            {
                Objs[i].SetActive(true);
				Objs [i].GetComponent<Enemy> ().spanwer = this;
				GameObserver.GetInstance.enemiesList.Add(Objs[i].GetComponent<Enemy>());
                
                return Objs[i];
            }
        }
        return null;
    }

    public void SetPooledObject(GameObject Obj)
    {
        Obj.transform.position = Mon.transform.position;
        GameObserver.instance.enemiesList.Remove(Obj.GetComponent<Enemy>());
		for (int i = 0; i < Obj.GetComponent<Enemy>().mats.Length;i++)
		{
			var mat = Obj.GetComponent<Enemy> ().mats [i];
			mat.SetColor ("_Black", Color.black);
			mat.SetColor ("_Color", Color.white);
		}
		Obj.GetComponent<Enemy> ().acInfo.ResetFlags ();
		Obj.SetActive(false);
    }

}
