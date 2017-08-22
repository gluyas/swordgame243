using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public static readonly HashSet<Bullet> All = new HashSet<Bullet>();
	
	public float Speed;
	public float LifeTime;

	public bool InheritVelocity = true;
	
	private Vector3 _velocity;
	private float _time;

	private Character _owner;
	
	public void Init(Vector3 position, Vector3 direction, Character owner = null)
	{
		_velocity = direction;
		_velocity.Normalize();
		_velocity *= Speed;

		if (owner != null)
		{
			_owner = owner;
			if (InheritVelocity) _velocity += owner.Velocity;
		}	
		
		transform.position = position;
		transform.rotation *= Quaternion.FromToRotation(Vector3.forward, _velocity);

		All.Add(this);
	}

	private void Update()
	{
		transform.position += Time.deltaTime * _velocity;
		_time += Time.deltaTime;
		if (_time >= LifeTime) Destroy(this.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		var target = other.gameObject.GetComponent<Character>();
		if (target != null)
		{
			target.Damage(1);
			Destroy(this.gameObject);
		}
	}
	
	private void OnDestroy()
	{
		All.Remove(this);
	}
}
