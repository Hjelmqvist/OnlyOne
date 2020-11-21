using UnityEngine;

public class AudioOnHit : DoOnHit
{
    [SerializeField] AudioSource source = null;
    [SerializeField] bool onlyPlayOnce = false;
    bool didPlay = false;

    protected override void HitEffect(GameObject go)
    {
        if (onlyPlayOnce && didPlay)
            return;

        source.Play();
        didPlay = true;
    }
}
