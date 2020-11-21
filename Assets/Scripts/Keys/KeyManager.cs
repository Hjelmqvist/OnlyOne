using System.Collections.Generic;
using UnityEngine;

public class KeyManager : Singleton<KeyManager>
{
    List<Key> Keys = new List<Key>();
    List<Key> UsedKeys = new List<Key>();
    List<Door> openedDoors = new List<Door>();

    void Start()
    {
        GameManager.Instance.onDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        for (int i= Keys.Count-1; Keys.Count > 0; i--)
        {
            Keys[i].GetComponent<SpriteRenderer>().enabled = true;
            Keys[i].GetComponent<Collider2D>().enabled = true;
            Keys.RemoveAt(i);
        }
        for (int i = UsedKeys.Count - 1; UsedKeys.Count > 0; i--)
        {
            UsedKeys[i].GetComponent<SpriteRenderer>().enabled = true;
            UsedKeys[i].GetComponent<Collider2D>().enabled = true;
            UsedKeys.RemoveAt(i);
        }
        openedDoors.Clear();
    }

    public void SaveProgress()
    {
        UsedKeys.Clear();
        for (int i = openedDoors.Count - 1; openedDoors.Count > 0; i--)
        {
            openedDoors[i].Disable();
            openedDoors.RemoveAt(i);
        }
    }

    public void AddKey(Key key)
    {
        Keys.Add(key);
    }

    public Key FindKey(Colors color)
    {
        foreach (Key key in Keys)
        {
            if (key.Color == color)
                return key;
        }
        return null;
    }

    public void RemoveKey(Key key, Door door)
    {
        Keys.Remove(key);
        UsedKeys.Add(key);
        openedDoors.Add(door);
    }
}
