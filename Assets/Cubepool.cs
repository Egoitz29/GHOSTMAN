using System.Collections.Generic;
using UnityEngine;

public class Cubepool : MonoBehaviour

{
    public static Cubepool Instance;
    public GameObject cubePrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public GameObject GetCube(Vector3 position)
    {
        if (pool.Count > 0)
        {
            GameObject cube = pool.Dequeue();
            cube.transform.position = position;
            cube.SetActive(true);
            return cube;
        }
        else
        {
            return Instantiate(cubePrefab, position, Quaternion.identity);
        }
    }

    public void ReturnCube(GameObject cube)
    {
        cube.SetActive(false);
        pool.Enqueue(cube);
    }
}

