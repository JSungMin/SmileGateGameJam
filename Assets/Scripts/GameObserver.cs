using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObserver : MonoBehaviour {
    
    public Text Combo_Txt;

    public Image Hp_Bar;
    public Image Rage_Bar;

    public List<EnemySpawn> spawner;
    public EnemySpawn boss_spawner;

    public float period;
    public float boss_period;
  
    public static GameObserver instance;
    public static GameObserver GetInstance
    {
        get
        {
            if (null == instance)
            {
                instance = GameObject.FindObjectOfType<GameObserver>();
                if (null == instance)
                    Debug.LogError("Can't Find GameObserver");
            }
            return instance;
        }
    }
    
    public List<Enemy> enemiesList = new List<Enemy>();

    void Start()
    {
        StartCoroutine(ComboManage());
        StartCoroutine(BarManage());
        StartCoroutine(SummonEnemy());
        StartCoroutine(SummonBoss());
    }

    void Update()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].UpdateDistanceToPlayer();
        }
    }
    
    IEnumerator SummonEnemy()
    {
        while(true)
        {
            if(enemiesList.Count <= 20)
            {
                for (int i = 0; i < spawner.Count; i++)
                {
                    spawner[i].GetPooledObject(spawner[i].Mons);
                }
            }

            yield return new WaitForSeconds(period);
        }
    }

    IEnumerator SummonBoss()
    {
        while (true)
        {
            yield return new WaitForSeconds(boss_period);

            boss_spawner.GetPooledObject(boss_spawner.Mons);
        }
    }

    IEnumerator BarManage()
    {
        while(true)
        {
            Hp_Bar.fillAmount = (float)Player.GetInstance.acInfo.hp / 100f;
            Rage_Bar.fillAmount = (float)Player.GetInstance.acInfo.mp / 10f;

            yield return null;
        }
    }

    IEnumerator ComboManage()
    {
        int currentCombo = 0;
            
        while (true)
        {
            if (ComboTimer.GetInstance.combo > currentCombo)
            {
                Combo_Txt.gameObject.SetActive(true);
                currentCombo++;
                Combo_Txt.text = "COMBO " + currentCombo;
            }
            else if (ComboTimer.GetInstance.combo == 0)
            {
                currentCombo = 0;
                Combo_Txt.text = "";
                Combo_Txt.gameObject.SetActive(false);
            }

            yield return null;
        }
    }

}
