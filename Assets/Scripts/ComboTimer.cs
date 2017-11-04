using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTimer : MonoBehaviour {
	public static ComboTimer instance;
	public float comboThresholdTime = 1f;
	public int combo = 0;
	public float comboTimer = 0f;
	public event InteractFunc OnComboOccur;
	public event InteractFunc OnComboReset;

	public static ComboTimer GetInstance
	{
		get{
			if (null == instance) {
				instance = GameObject.FindObjectOfType<ComboTimer> ();
			}
			return instance;
		}
	}
	public void OnEnable ()
	{
		instance = this;
		StartCoroutine (ComboChecker());
	}
	IEnumerator ComboChecker ()
	{
		while (true)
		{
			if (comboTimer <= 0f)
			{
				combo = 0;
				comboTimer = 0f;
				if (null != OnComboReset)
					OnComboReset.Invoke ();
			}
			comboTimer -= Time.deltaTime;
			yield return null;
		}
	}

	public void AddCombo (int count)
	{
		combo += count;
		comboTimer = comboThresholdTime;
		if (null != OnComboOccur)
			OnComboOccur.Invoke ();
	}
}
