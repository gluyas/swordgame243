using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component for objects to move around within the world
/// Provides a simpler movement model than Unity's physics engine.
/// </summary>
public class Character : MonoBehaviour
{
	public float MovementSpeed;
	public int MaxHp;

	public Vector3 Velocity { get; private set; }
	public int Hp { get; private set; }

	private Vector3 _facing = Vector3.forward;

	private void Start()
	{
		Hp = MaxHp;
	}

	public Vector3 Facing
	{
		get { return _facing; }
		set
		{
			_facing = value.normalized;
			transform.rotation = Quaternion.FromToRotation(Vector3.forward, _facing);
		}
	}

	/// <summary>
	/// Set the Character's horizontal movement
	/// </summary>
	/// <param name="movement">Vector to move in, as a coefficient of the Character's movement speed</param>
	/// <param name="truncate">If true, then the new speed will not exceed MovemenSpeed</param>
	public void SetMovement(Vector3 movement, bool truncate = true)
	{
		if (truncate && movement.magnitude > 1)
		{
			movement.Normalize();
		}
		movement *= MovementSpeed;
		Velocity = movement;
	}
	
	/// <summary>
	/// Adjust the Character's movement. Effectively acceleration
	/// </summary>
	/// <param name="delta">Vector to adjust movment in, as a coefficient of the Character's movement speed</param>
	/// <param name="truncate">If true, then the new speed will not exceed MovemenSpeed</param>
	public void AddMovement(Vector3 delta, bool truncate = true)
	{
		SetMovement(Velocity + delta, truncate);	
	}

	public void Damage(int damage)
	{
		Hp -= damage;
		if (Hp <= 0)
		{
			Destroy(this.gameObject);
		}
	}
	
	private void Update()
	{
		this.transform.position += Velocity * Time.deltaTime;
	}
}
