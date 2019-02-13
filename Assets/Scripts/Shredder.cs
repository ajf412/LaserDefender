using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{

    // Destroy any 2D colliders that enter the shredder.
    private void OnTriggerEnter2D(Collider2D other)
    {
            Destroy(other.gameObject);
    }
}
