using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MoveToAndBack : MonoBehaviour
{
    [SerializeField] Vector2 secondPosOffset = new Vector2();
    [SerializeField] float movementSpeed = 1;
    [SerializeField] bool dieOnReached = false;
    [SerializeField] bool stopOnReached = false;

    SpriteRenderer sr = null;
    bool goingBack = false;
    Vector2 firstPos = new Vector2();
    Vector2 secondPos = new Vector2();

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        firstPos = transform.position;
        secondPos = (Vector2)transform.position + secondPosOffset;
    }

    void Update()
    {
        Vector2 target = new Vector2();
        if (goingBack)
            target = firstPos;
        else
            target = secondPos;

        transform.position = Vector2.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            goingBack = !goingBack;
            if (dieOnReached)
                Destroy(gameObject);
            if (stopOnReached)
                Destroy(this);
        }

        sr.flipX = target.x > transform.position.x;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + secondPosOffset);
    }
}
