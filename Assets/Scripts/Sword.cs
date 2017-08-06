using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	void Update() 
	{
		// spin sword model
		var swordDirection = new Vector3(Input.GetAxisRaw("AimX"), 0, Input.GetAxisRaw("AimY"));
		transform.rotation = Quaternion.FromToRotation(Vector3.forward, swordDirection);
	}
}
