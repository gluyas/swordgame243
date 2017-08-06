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
	
	public Vector3 Velocity { get; private set; }

	/// <summary>
	/// Set the Character's horizontal movement
	/// </summary>
	/// <param name="movement">Vector to move in, as a coefficient of the Character's movement speed</param>
	/// <param name="truncate">If true, then the new speed will not exceed MovemenSpeed</param>
	public void SetMovement(Vector2 movement, bool truncate = true)
	{
		if (truncate && movement.magnitude > 1)
		{
			movement.Normalize();
		}
		movement *= MovementSpeed;
		Velocity = new Vector3(movement.x, Velocity.y, movement.y);
	}
	
	/// <summary>
	/// Adjust the Character's movement. Effectively acceleration
	/// </summary>
	/// <param name="delta">Vector to adjust movment in, as a coefficient of the Character's movement speed</param>
	/// <param name="truncate">If true, then the new speed will not exceed MovemenSpeed</param>
	public void AddMovement(Vector2 delta, bool truncate = true)
	{
		SetMovement(new Vector2(Velocity.x + delta.x, Velocity.z + delta.y), truncate);	
	}
	
	private void Update()
	{
		this.transform.position += Velocity * Time.deltaTime;
	}
}
