using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKGSpawner : MonoBehaviour
{

    public Transform[] bkgPrefabs;

    // Start is called before the first frame update
    void Start() {

        SpawnPrefab(bkgPrefabs);
    }


    //Spawn prefabs
    private void SpawnPrefab(Transform[] prefabs) {

        //go through the array of prefabs and pick a random num
        if(prefabs.Length > 0) {

            //random prefab from array
            int choosePrefab = Random.Range(0,prefabs.Length);

            // Randomize the y-axis rotation (0 or 90 degrees)
            float randomRotationY = Random.Range(0, 2) * 90f;

            // Spawn prefab
            var bkgPrefab = (Transform)Instantiate(prefabs[choosePrefab], transform.position, transform.rotation);

            // Set rotation instead of scale
            bkgPrefab.transform.eulerAngles = new Vector3(
                bkgPrefab.transform.eulerAngles.x,
                randomRotationY,
                bkgPrefab.transform.eulerAngles.z
            );
        }
    }


    //easy way to get random val
    // private static int GetRandomInverse(int _randomVal) {
    //     if(_randomVal == 0) _randomVal = -1;
    //     else _randomVal = 1;
    //     return _randomVal;
    // }


    // Update is called once per frame
    void Update()
    {

    }




}
