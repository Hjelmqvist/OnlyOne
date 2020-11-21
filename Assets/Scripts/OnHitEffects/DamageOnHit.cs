using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageOnHit : DoOnHit
{
    void Start()
    {
        GameManager.Instance.onDeath += OnPlayerDeath;
    }

    void OnDestroy()
    {
        GameManager.Instance.onDeath -= OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        col.enabled = true;
    }

    //Destroy(this);
    protected override void HitEffect(GameObject go)
    {
        Player player = go.GetComponent<Player>();
        if (player)
            player.Die();
    }
}
