using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Character _char; 

	private void Start()
	{
		_char = GetComponent<Character>();
	}
	
	private void Update()
	{
		_char.SetMovement(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		
	}
}
