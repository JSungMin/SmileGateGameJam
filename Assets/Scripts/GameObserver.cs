using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObserver : MonoBehaviour {

    Camera camera;
    
    public Text[] Combo_Txt;

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

    IEnumerator ComboShake(Text Obj)
    {
        var delay = 0.2f;

        var firstPos = Obj.rectTransform.position;
        
        while(delay > 0f)
        {
            delay -= Time.deltaTime;
            Obj.rectTransform.position = Random.insideUnitCircle * 5;
            TextGhost();
        }

        Obj.rectTransform.position = firstPos;

        yield return null;
    }
    
    public void TextGhost()
    {
        for(int i = 1; i < Combo_Txt.Length; i++)
        {
            Combo_Txt[i].rectTransform.position = Combo_Txt[i - 1].rectTransform.position;
            Combo_Txt[i].text = Combo_Txt[i - 1].text;
        }
    }

    IEnumerator ComboManage()
    {
        int currentCombo = 0;
            
        while (true)
        {
            if (ComboTimer.GetInstance.combo > currentCombo)
            {
                Combo_Txt[0].gameObject.SetActive(true);
                currentCombo++;
                Combo_Txt[0].text = "COMBO " + currentCombo;
                StartCoroutine(ComboShake(Combo_Txt[0]));
            }
            else if (ComboTimer.GetInstance.combo == 0)
            {
                currentCombo = 0;
                Combo_Txt[0].text = null;
                Combo_Txt[0].gameObject.SetActive(false);
            }

            yield return null;
        }
    }

}
