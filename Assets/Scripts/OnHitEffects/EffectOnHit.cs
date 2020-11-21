using UnityEngine;

public class EffectOnHit : DoOnHit
{
    [SerializeField] ParticleSystem effectPrefab = null;
    [SerializeField] Vector2 positionOffset = new Vector2();

    protected override void HitEffect(GameObject go)
    {
        ParticleSystem effect = Instantiate(effectPrefab, (Vector2)go.transform.position + positionOffset, effectPrefab.transform.rotation);
        effect.Play();
    }
}
