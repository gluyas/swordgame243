using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DamageTrigger : MonoBehaviour
{
	public int Damage = 1;
	public bool DeleteOnTrigger = true;
	
	private void OnTriggerEnter(Collider other)
	{
		var target = other.gameObject.GetComponent<Character>();	
		if (target != null)
		{
			target.Damage(Damage);
			if (DeleteOnTrigger) Destroy(this.gameObject);
		}
	}
}
