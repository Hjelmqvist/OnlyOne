using UnityEngine;

public class SpawnOnHit : DoOnHit
{
    [SerializeField] GameObject prefab = null;
    [SerializeField] Vector2 spawnpoint = new Vector2();
    bool canSpawn = true;
    GameObject spawnedInObject = null;

    void Start()
    {
        GameManager.Instance.onDeath += Instance_onDeath;
    }

    private void Instance_onDeath()
    {
        canSpawn = true;
        if (spawnedInObject)
            Destroy(spawnedInObject);
    }

    protected override void HitEffect(GameObject go)
    {
        Player player = go.GetComponent<Player>();
        if (player && canSpawn)
        {
            spawnedInObject = Instantiate(prefab, spawnpoint, prefab.transform.rotation);
            canSpawn = false;
        }    
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(spawnpoint, Vector3.one);
    }
}
