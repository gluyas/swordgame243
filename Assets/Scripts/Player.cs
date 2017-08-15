using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
	private Character _char;

	public GameObject Bullet;

	public float MuzzleLength;
	public float AimDeadZone = 0.1f;
	
	public float FireRate;
	public float FireBurstSize;
	public AnimationCurve FireStrengthCurve = AnimationCurve.Linear(0, 0, 1, 1);
	
	public float SpreadBase;
	public float SpreadIncreaseFactor;
	public float JitterBase;
	public float JitterIncreaseFactor;
	
	public int AmmoMax;
	public float AmmoRegenMax;
	public float AmmoRegenMin;
	public float AmmoRegenMaxPoint;
	
	public int Ammo
	{
		get { return Mathf.FloorToInt(_ammo); }
	}

	private float _ammo;
	private float _shotCarryOver;
	private bool _burstFired = false;

	private void Start()
	{
		_char = GetComponent<Character>();
		_ammo = AmmoMax;
	}
	
	private void Update()
	{
		// MOVEMENT
		_char.SetMovement(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		
		// AMMO REGEN
		{
			var regenRate = Mathf.Clamp(_ammo / AmmoMax / AmmoRegenMaxPoint, 0, 1);
			_ammo += Mathf.Lerp(AmmoRegenMin, AmmoRegenMax, regenRate) * Time.deltaTime;
			if (_ammo >= AmmoMax) _ammo = AmmoMax;
		}
		
		// SHOOTING
		{	
			var aim = new Vector2(Input.GetAxis("AimX"), Input.GetAxis("AimY"));
			if (Math.Abs(aim.magnitude) > AimDeadZone)	// set aim direction
			{
				_char.Facing = new Vector3(aim.x, 0, aim.y).normalized;;
			}
			
			if (Input.GetButton("FireBurst"))
			{
				if (!_burstFired) _shotCarryOver = _ammo * FireBurstSize;		// fire everything
				_burstFired = true;
			}
			else
			{
				_burstFired = false;
				Debug.Log(Input.GetAxis("FireStrength"));
				_shotCarryOver += FireRate * Time.deltaTime *
				                  FireStrengthCurve.Evaluate(Input.GetAxis("FireStrength"));
				if (_shotCarryOver > _ammo) _shotCarryOver = _ammo;
			}
			
			var shots = Mathf.FloorToInt(_shotCarryOver);
			_ammo -= shots;
			_shotCarryOver -= shots;	// carry unfired 'half-shots' over between frames

			var spread = SpreadBase;
			var jitter = JitterBase;
			for (var i = 0; i < shots; i++)
			{
				var bullet = Instantiate(Bullet).GetComponent<Bullet>();
				var aimSpread = Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.up) * _char.Facing;
				var posJitter = transform.position + aimSpread * Random.Range(MuzzleLength, jitter);
				bullet.Init(posJitter, aimSpread, _char);
	
				spread += SpreadBase * SpreadIncreaseFactor;
				jitter += JitterBase * JitterIncreaseFactor;
			}
		}
	}
}
