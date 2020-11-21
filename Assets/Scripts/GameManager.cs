using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] float restartDelayTime = 1.5f;

    public delegate void Death();
    public event Death onDeath;

    Coroutine quickRestart = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            quickRestart = StartCoroutine(QuickRestart());
        else if (Input.GetKeyUp(KeyCode.R))
            StopCoroutine(quickRestart);

        IEnumerator QuickRestart()
        {
            yield return new WaitForSeconds(0.5f);
            onDeath?.Invoke();
        }
    }

    public void Lose(Player player)
    {
        StartCoroutine(Lose());

        IEnumerator Lose()
        {
            yield return new WaitForSeconds(restartDelayTime);
            onDeath?.Invoke();
        }
    }
}
