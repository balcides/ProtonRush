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

            int choosePrefab = Random.Range(0,prefabs.Length + 1);

            //random x and z scale
            int randomX = Random.Range(0,2);
            int randomZ = Random.Range(0,2);

            randomX = GetRandomInverse(randomX);
            randomZ = GetRandomInverse(randomZ);

            //spawn prefab
            var bkgPrefab = (Transform) Instantiate(prefabs[choosePrefab],transform.position,transform.rotation);
            bkgPrefab.transform.localScale = new Vector3(randomX,1,randomZ);
        }
    }


    //easy way to get random val
    private static int GetRandomInverse(int _randomVal) {
        if(_randomVal == 0) _randomVal = -1;
        else _randomVal = 1;
        return _randomVal;
    }


    // Update is called once per frame
    void Update()
    {

    }




}
