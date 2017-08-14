using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
	public Text AmmoCount;

	public Player Player;
	
	// Update is called once per frame
	void Update ()
	{
		AmmoCount.text = Player.Ammo.ToString();
	}
}
