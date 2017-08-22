using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Swarmer : MonoBehaviour
{
	// AI property weighting
	public float Acceleration = 1f;
	public float SwarmRadius;
	
	public float AvoidanceRadius;
	
	public float AvoidanceExponent;
	public float AvoidanceWeight = 1f;
	
	public float AlignmentWeight = 1f;
	public float CohesionWeight = 1f;
	public float PursuitWeight = 1f;
	
	public float TotalWeight { get { return AlignmentWeight + CohesionWeight + AvoidanceWeight + PursuitWeight; } }

	private static readonly HashSet<Swarmer> AllSwarmers = new HashSet<Swarmer>();
	
	private Character _char;
	
	private void OnEnable ()
	{
		_char = GetComponent<Character>();
		AllSwarmers.Add(this);
	}
	
	// Update is called once per frame
	private void Update()
	{
		var swarmCentre = Vector3.zero;
		var swarmDirection = Vector3.zero;
		var swarmCount = 0;

		var avoidance = Vector3.zero;
		var avoidanceCount = 0;
		
		foreach (var other in AllSwarmers)
		{
			if (other == this) continue;

			var toOther = other.transform.position - this.transform.position;
			var distance = toOther.magnitude;
			
			if (distance <= SwarmRadius)
			{
				swarmCentre += other.transform.position;
				swarmDirection += other._char.Velocity;
				
				swarmCount += 1;

				if (distance <= AvoidanceRadius)
				{
					toOther.Normalize();
					avoidance -= toOther * Mathf.Pow(1 - distance / AvoidanceRadius, AvoidanceExponent);
					
					avoidanceCount += 1; 
					Debug.DrawLine(this.transform.position, other.transform.position, Color.red, 0, false);	
				}
				else Debug.DrawLine(this.transform.position, other.transform.position, Color.green);
			}
		}
		
		// all metrics are mapped to the range [0, 1], to simplify tweaking by the designer
		
		// COHESION
		swarmCentre /= swarmCount;
		var cohesion = (swarmCentre - this.transform.position) / SwarmRadius;
		
		// ALIGNMENT
		swarmDirection /= swarmCount;
		var alignment = swarmDirection - _char.Velocity;		// using 2*speed as scale factor means that this factor
		alignment /= 2 * _char.MovementSpeed;					// is only at max when boids move opposite directions
		if (alignment.magnitude > 1) alignment.Normalize();            

		// AVOIDANCE
		avoidance /= avoidanceCount;
		
		// PURSUIT
		var pursuit = Player.Instance.transform.position - this.transform.position;
		pursuit.Normalize();		// constant accelleration towards player
	
		// EDGE CASES
		if (swarmCount == 0)
		{
			alignment = Vector3.zero;
			cohesion = Vector3.zero;
		}
		if (avoidanceCount == 0)
		{
			avoidance = Vector3.zero;
		}
			
		// FINAL CALCULATION
		var result = CohesionWeight * cohesion + AlignmentWeight * alignment + AvoidanceWeight * avoidance +
		             PursuitWeight * pursuit;
		result *= Acceleration / TotalWeight;	// map the result from [0 - TotalWegiht] -> [0 - Acceleration]
		_char.AddMovement(result);
	}

	private void OnDestroy()
	{
		AllSwarmers.Remove(this);
	}
}
