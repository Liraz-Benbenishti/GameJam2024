using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    private GameObject player_movement;
    public float poop_dropping = 8.8f;
    public GameObject poopPrefab;
    private Transform environment_transform;
    private bool isPoopDropped = false;
    public Transform poop_hole;

    public AudioCueEventChannelSO playSfxEvent;
    public AudioConfigurationSO sfxConfig;
    public AudioCueSO poopingSfx;


    private void Awake()
    {
        player_movement = GameObject.FindGameObjectWithTag("Player");
        environment_transform = transform.parent;

    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position +=new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float distance_to_player = Vector3.Distance(transform.position, player_movement.transform.position);
        if(distance_to_player <= poop_dropping && !isPoopDropped)
        {
            dropPoop();
            isPoopDropped = true;
}
    }

    private void dropPoop()
    {
        Instantiate(poopPrefab, poop_hole.position, Quaternion.identity, environment_transform);
        playSfxEvent.RaisePlayEvent(poopingSfx, sfxConfig);
    }
}
