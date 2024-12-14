using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mp_trash_spawner : MonoBehaviour
{
    [SerializeField]
    private Vector3 _spawnRange1 = Vector3.zero;
    [SerializeField]
    private Vector3 _spawnRange2 = Vector3.zero;
    [SerializeField]
    private GameObject _prefab;

    private GameObject _trash;

    void Start() {
        SpawnTrash();
    }
    public void Give() {
        Destroy(_trash);
        SpawnTrash();
    }
    void SpawnTrash() {
        float _rangex = (int)((int)Random.Range(_spawnRange1.x, _spawnRange2.x) / 4.5f) * 4.5f;
        float _rangey = (int)Random.Range(_spawnRange1.y, _spawnRange2.y);
        float _rangez = (int)((int)Random.Range(_spawnRange1.z, _spawnRange2.z) / 4.5f) * 4.5f;
        Vector3 _pos = new Vector3(_rangex, _rangey, _rangez);
        _trash = Instantiate(_prefab, _pos, transform.rotation);
    }
}
