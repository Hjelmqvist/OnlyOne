using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : DoOnHit
{
    [SerializeField] GameObject endScreen = null;
    // Start is called before the first frame update
    protected override void HitEffect(GameObject go)
    {
        Time.timeScale = 0;
        endScreen.SetActive(true);
    }
}
