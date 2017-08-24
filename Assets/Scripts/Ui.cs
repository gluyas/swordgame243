using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
	public Image AmmoBar;
	public Text TimeDisplay;

	private static Player Player { get {return Player.Instance;} }

	public static bool UsingMouse { get; private set; }

	private bool _isDead;
	private float _elapsedTime;

	private void Start()
	{
		Player.GetComponent<Character>().OnDeath.AddListener(() => _isDead = true);
	}

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote)) UsingMouse = !UsingMouse;
		if (!_isDead)
		{
			_elapsedTime += Time.deltaTime;
			TimeDisplay.text = _elapsedTime.ToString(".0000");
			
			var ammo = Player.AmmoRaw / Player.AmmoMax;
			AmmoBar.rectTransform.localScale = new Vector3(ammo, 1, 1);
			AmmoBar.color = Color.Lerp(Color.red, Color.white, ammo / Player.AmmoRegenMaxPoint);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
