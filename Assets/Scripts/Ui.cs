using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
	public Image AmmoBar;

	public Player Player;
	
	// Update is called once per frame
	void Update ()
	{
		var ammo = Player.AmmoRaw / Player.AmmoMax;
		AmmoBar.rectTransform.localScale = new Vector3(ammo, 1, 1);
		AmmoBar.color = Color.Lerp(Color.red, Color.white, ammo / Player.AmmoRegenMaxPoint);
	}
}
