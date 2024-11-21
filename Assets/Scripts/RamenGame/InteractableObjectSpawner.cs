using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractableObjectSpawner : ObjectPool<InteractableObject>
{
    [SerializeField] private InteractableObject[] _prefabs;
    [SerializeField] private SpawnArea _spawnArea;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _interval = 1;
    //[SerializeField] private PickTheFruitGameController _game;
    [SerializeField] private Transform _parent;
    
    private IEnumerator _spawnCoroutine;
    private List<InteractableObject> _spawnedObjects = new List<InteractableObject>();

    private void Awake()
    {
        for (int i = 0; i <= _poolCapacity; i++)
        {
            ShuffleArray();

            foreach (var prefab in _prefabs)
            {
                Initalize(prefab, _parent);
            }
        }
    }

    private void OnEnable()
    {
       // _game.FruitCatched += ReturnToPool;
    }

    private void OnDisable()
    {
       // _game.FruitCatched -= ReturnToPool;
    }
    
    public void EnableSpawn()
    {
        if (_spawnCoroutine == null)
            _spawnCoroutine = StartSpawning();

        StartCoroutine(_spawnCoroutine);
    }

    public void StopSpawning()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
            ReturnAllObjectsToPool();
        }
    }

    private IEnumerator StartSpawning()
    {
        WaitForSeconds interval = new WaitForSeconds(_interval);
        
        while (enabled)
        {
            int randomIndex = Random.Range(0, _prefabs.Length);
            InteractableObject prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out InteractableObject @object, prefabToSpawn))
            {
                @object.RectTransform.position = _spawnArea.GetRandomXPositionToSpawn();
                _spawnedObjects.Add(@object);
                @object.EnableMovement();
            }

            yield return interval;
        }
    }

    public void ReturnToPool(InteractableObject @object)
    {
        if (@object == null)
            return;
        
        PutObject(@object);

        if (_spawnedObjects.Contains(@object))
            _spawnedObjects.Remove(@object);
    }

    private void ReturnAllObjectsToPool()
    {
        if (_spawnedObjects.Count <= 0)
            return;

        List<InteractableObject> objectsToReturn = new List<InteractableObject>(_spawnedObjects);
        foreach (var @object in objectsToReturn)
        {
            ReturnToPool(@object);
        }
    }

    private void ShuffleArray()
    {
        for (int i = 0; i < _prefabs.Length - 1; i++)
        {
            InteractableObject temp = _prefabs[i];
            int randomIndex = Random.Range(0, _prefabs.Length);
            _prefabs[i] = _prefabs[randomIndex];
            _prefabs[randomIndex] = temp;
        }
    }
}