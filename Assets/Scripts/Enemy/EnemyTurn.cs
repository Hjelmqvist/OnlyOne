using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyTurn : MonoBehaviour
{
    SpriteRenderer sr = null;
    Player player = null;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = Player.Instance;
    }

    void Update()
    {
        sr.flipX = player.transform.position.x > transform.position.x;
    }
}
