using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : DoOnHit
{
    bool triggered = false;

    public bool Triggered { get { return triggered; } set { triggered = value; } }

    void Start()
    {
        if (triggered)
            GetComponent<Animator>().SetTrigger("Triggered");
    }

    protected override void HitEffect(GameObject go)
    {
        Player player = go.GetComponent<Player>();
        if (player && !triggered)
        {
            triggered = true;
            player.SetNewCheckpoint(this);
            GetComponent<Animator>().SetTrigger("Triggered");
        }
    }

}
