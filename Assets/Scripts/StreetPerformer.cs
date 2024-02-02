using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetPerformer : MonoBehaviour
{
    [SerializeField] Animator Moves;

    private void OnCollisionStay(Collision OBJ)
    {
        if (OBJ.gameObject.CompareTag("Player"))
        {
            Moves.Play("Slipping");
        }
    }
}
