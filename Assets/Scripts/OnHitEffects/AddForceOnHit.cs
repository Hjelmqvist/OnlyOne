using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AddForceOnHit : DoOnHit
{
    [SerializeField] Vector3 force = new Vector3();
    [SerializeField] Animator anim = null;

    protected override void HitEffect(GameObject go)
    {
        Player player = go.GetComponent<Player>();
        if (player)
        {
            player.RB2D.velocity = force;
            if (anim)
                anim.SetTrigger("Triggered");
        }
            
    }
}
