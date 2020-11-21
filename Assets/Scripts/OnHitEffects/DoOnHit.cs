using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class DoOnHit : MonoBehaviour
{
    [SerializeField] bool onEnter = false, onStay = false, onExit = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (onEnter)
            HitEffect(other.gameObject);       
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (onStay)
            HitEffect(other.gameObject);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (onExit)
            HitEffect(other.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (onEnter)
            HitEffect(other.gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (onStay)
            HitEffect(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (onExit)
            HitEffect(other.gameObject);
    }

    protected abstract void HitEffect(GameObject go);
}

