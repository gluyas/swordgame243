using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
	public static Player Instance { get; private set; }
	
	private Character _char;

	public GameObject Bullet;

	public AudioSource SoundRapid;
	public AudioSource SoundBurst;
	public AudioSource SoundDamage;

	public float MuzzleLength;
	public float AimDeadZone = 0.1f;
	
	public float FireRateMean;
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

	public int Hp
	{
		get { return _char.Hp; }
	}
	
	public int Ammo
	{
		get { return Mathf.FloorToInt(AmmoRaw); }
	}
	
	public float AmmoRaw { get; private set; }
	
	public float FireRate { get; private set; }

	private float _shotCarryOver;
	private bool _burstFired = false;

	private void Start()
	{
		Instance = this;
		_char = GetComponent<Character>();
		_char.OnDeath.AddListener(Kill);
		_char.OnDamage.AddListener(() => SoundDamage.Play());
		AmmoRaw = AmmoMax;
	}
	
	private void Update()
	{		
		// MOVEMENT
		_char.SetMovement(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
		
		// AMMO REGEN
		{
			var regenRate = Mathf.Clamp(AmmoRaw / AmmoMax / AmmoRegenMaxPoint, 0, 1);
			AmmoRaw += Mathf.Lerp(AmmoRegenMin, AmmoRegenMax, regenRate) * Time.deltaTime;
			if (AmmoRaw >= AmmoMax) AmmoRaw = AmmoMax;
		}
		
		// SHOOTING
		{
			var aim = GetAim();
			if (Math.Abs(aim.magnitude) > AimDeadZone)	// set aim direction
			{
				_char.Facing = new Vector3(aim.x, 0, aim.y).normalized;
			}
			
			if (Input.GetButton("FireBurst"))
			{
				if (!_burstFired)
				{
					_shotCarryOver = AmmoRaw * FireBurstSize; // fire everything
					SoundBurst.Play();
				}
				_burstFired = true;
			}
			else
			{
				_burstFired = false;
				_shotCarryOver += FireRateMean * Time.deltaTime *
				                  FireStrengthCurve.Evaluate(Input.GetAxis("FireStrength"));
				if (_shotCarryOver > AmmoRaw) _shotCarryOver = AmmoRaw;
			}
			
			var shots = Mathf.FloorToInt(_shotCarryOver);
			AmmoRaw -= shots;
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
				
				if (!_burstFired) SoundRapid.Play();
			}
		}
	}

	private Vector2 GetAim()
	{
		if (Ui.UsingMouse)
		{
			var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			var playerPlane = new Plane(Vector3.up, transform.position);
			
			float distance;
			if (playerPlane.Raycast(mouseRay, out distance))
			{
				var aim = mouseRay.GetPoint(distance) - transform.position;
				return new Vector2(aim.x, aim.z);
			}
			else
			{
				return Vector2.zero;
			}
		}
		else
		{
			return new Vector2(Input.GetAxis("AimX"), Input.GetAxis("AimY"));
		}
	}
	
	private void Kill()
	{
		_char.SetMovement(Vector3.zero);		
		this.enabled = false;
		GetComponent<Collider>().enabled = false;
	}
	
	private void OnDestroy()
	{
		Instance = null;
	}
}
