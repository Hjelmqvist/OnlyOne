using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    Colors color = Colors.None;

    public Colors Color { get { return color; } private set { color = value; } }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            KeyManager.Instance.AddKey(this);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

public enum Colors { None, Blue, Green, Red, Yellow}
