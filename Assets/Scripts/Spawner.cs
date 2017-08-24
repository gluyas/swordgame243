using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject WeedSpawn;
	public int WeedSpawnCount;
	public float WeedSpawnRadiusMax;
	public float WeedSpawnRadiusMin;
	
	public GameObject EnemySpawn;
	public int EnemySpawnCount;
	public float EnemySpawnInterval;
	public float InitialSpawnInterval;

	private AudioSource _spawnSound;
	
	private float _nextWaveTime;
	private int _remainingWeeds;

	public void Init(Vector3 pos)
	{
		this.transform.position = pos;
		for (var i = 0; i < WeedSpawnCount; i++)
		{
			var spawnRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
			var spawnVector = Vector3.forward * Random.Range(WeedSpawnRadiusMin, WeedSpawnRadiusMax);
			
			var weed = Instantiate(WeedSpawn, spawnRotation * spawnVector, spawnRotation);
			weed.GetComponent<Character>().OnDeath.AddListener(() => OnWeedDestroy(weed));
		}
		_nextWaveTime = InitialSpawnInterval;
		_remainingWeeds = WeedSpawnCount;
		_spawnSound = GetComponent<AudioSource>();
	}

	private void Update()
	{
		_nextWaveTime -= Time.deltaTime;
		if (_nextWaveTime <= 0)	// spawn wave of enemies
		{
			for (var i = 0; i < EnemySpawnCount; i++)
			{
				Instantiate(EnemySpawn, this.transform.position, Quaternion.identity);
			}
			_nextWaveTime += EnemySpawnInterval;	// reset this way stops compound rounding error due to frame timings
			_spawnSound.Play();
		}
	}

	private void OnWeedDestroy(GameObject weed)
	{
		Destroy(weed);
		_remainingWeeds--;
		if (_remainingWeeds <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
