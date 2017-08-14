﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float Speed;
	public float LifeTime;
	
	private Vector3 _velocity;
	private float _time;
	
	public void Init(Vector3 position, Vector3 direction)
	{
		_velocity = direction;
		_velocity.Normalize();
		_velocity *= Speed;
		transform.position = position;
		transform.rotation *= Quaternion.FromToRotation(Vector3.forward, _velocity);
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
}