using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class CubeController : MonoBehaviour
{
    public GameObject prefab;
    GameObject cube;
    public List<GameObject> cubes;

    void Start()
    {

        for (var i = 30; i < 54; i++)
        {
            cube = Instantiate(prefab);
            cube.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(0.0f, 10.0f), Random.Range(-5.0f, 5.0f));
            cube.GetComponent<OscillatorController>().noteNumber = i;
            cubes.Add(cube);
        }

    }

}
