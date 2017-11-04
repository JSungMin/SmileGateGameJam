using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObserver : MonoBehaviour {
    
    public Text Combo_Txt;

    public Image Hp_Bar;
    public Image Skill_Bar;

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
    }

    void Update()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].UpdateDistanceToPlayer();
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
                Combo_Txt.text = null;
                Combo_Txt.gameObject.SetActive(false);
            }

            yield return null;
        }
    }

}
