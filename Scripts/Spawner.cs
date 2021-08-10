using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [HideInInspector] public static Action<bool> StartGame;

    [SerializeField] private GameObject _humanoidPrefabs;
    [SerializeField] private int _enemiesCount;
    [SerializeField] private int _humanoidsOnStartCount = 50;
    [SerializeField] private Material[] _skinsMaterial;
    [SerializeField] private Material _playerMaterial;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _playerStartPos;

    private void Start()
    {
        GameObject player = Instantiate(_playerPrefab, _playerStartPos.position, Quaternion.Euler(0, 180, 0));
        player.GetComponent<Leader>().Init(_playerMaterial.color);
        FollowPlayer.Instance.SetTarget(player.transform);
    }


    public void StartSpawn()
    {
        StartCoroutine(SpawnNewHumanoid());
        SpawnEnemies();
    }

    IEnumerator SpawnNewHumanoid()
    {
        PlayerMovement.StartGame.Invoke(true);
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(_humanoidPrefabs, new Vector3(UnityEngine.Random.Range(-Plane.Width, Plane.Width), 1, UnityEngine.Random.Range(-Plane.Height, Plane.Height)), Quaternion.identity);
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < _enemiesCount; i++)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(UnityEngine.Random.Range(-Plane.Width, Plane.Width), 1, UnityEngine.Random.Range(-Plane.Height, Plane.Height)), Quaternion.identity);
            newEnemy.GetComponent<Leader>().Init(_skinsMaterial[i].color);
        }
    }

    public void SpawnHumanoidsOnStart()
    {
        for (int i = 0; i < _humanoidsOnStartCount; i++)
        {
            Instantiate(_humanoidPrefabs, new Vector3(UnityEngine.Random.Range(-Plane.Width, Plane.Width), 1, UnityEngine.Random.Range(-Plane.Height, Plane.Height)), Quaternion.identity);
        }
    }
}
