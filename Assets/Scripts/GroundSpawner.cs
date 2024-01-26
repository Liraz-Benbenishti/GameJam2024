using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject[] prefab;
    public Transform firstGroundSpawnLocation;
    public Transform location_to_spawn_ground;
    private GameObject currentGround;
    private GameObject nextGround;
    public float xOffset = 500;

    // Start is called before the first frame update
    void Start()
    {
        currentGround = showGround(firstGroundSpawnLocation);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGround.transform.position.x <= xOffset)
        {
            var temp = currentGround;
            nextGround = showGround();
            currentGround = nextGround;

            Destroy(temp.gameObject, 50f);
        }
    }

    public GameObject showGround(Transform newGroundLocation = null)
    {
        int random_index = Random.Range(0, prefab.Length);
        var ground_prefab = prefab[random_index];
        var ground = Instantiate(ground_prefab);

        ground.transform.position = (newGroundLocation ?? location_to_spawn_ground).position;

        return ground;

    }


}
