using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Colors color = Colors.None;
    [SerializeField] AudioSource source = null;

    void Start()
    {
        GameManager.Instance.onDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        GetComponent<Collider2D>().enabled = true;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Disable()
    {
        GameManager.Instance.onDeath -= OnPlayerDeath;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            Key key = KeyManager.Instance.FindKey(color);
            if (key != null)
            {
                source.Play();
                GetComponent<Collider2D>().enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                KeyManager.Instance.RemoveKey(key, this);
            }
        }
    }
}
