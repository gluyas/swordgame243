using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject Spawner;

	public float SpawnRadiusMin;
	public float SpawnRadiusMax;
	public float SpawnTimeInitial;
	public float SpawnTimeIncrement;

	private float _spawnTime;
	private float _nextSpawn;
	
	private void Start()
	{
		_spawnTime = SpawnTimeInitial;
	}

	// Update is called once per frame
	private void Update ()
	{
		_nextSpawn -= Time.deltaTime;
		if (_nextSpawn <= 0)
		{
			var spawnRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
			var spawnVector = Vector3.forward * Random.Range(SpawnRadiusMin, SpawnRadiusMax);

			Instantiate(Spawner).GetComponent<Spawner>().Init(spawnRotation * spawnVector);
			_spawnTime *= SpawnTimeIncrement;
			_nextSpawn += _spawnTime;
		}
	}
}
