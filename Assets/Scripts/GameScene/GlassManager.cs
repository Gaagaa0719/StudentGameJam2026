using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GlassManager : MonoBehaviour
{
    public static GlassManager instance;
    public static GlassManager GetInstance() { return instance; }

    [SerializeField]
    private GameObject GlassPrefab;

    [SerializeField]
    public Vector3 EnemyGlassLocation;

    [SerializeField]
    public Vector3 PlayerGlassLocation;

    public GameObject PlayerGlass, EnemyGlass;

    void Start()
    {
        instance = this;
    }

    public void Init()
    {
        SafeDestroy(ref PlayerGlass);
        SafeDestroy(ref EnemyGlass);

        PlayerGlass = Instantiate(GlassPrefab);
        PlayerGlass.tag = "PlayerGlass";
        PlayerGlass.transform.position = PlayerGlassLocation;


        EnemyGlass = Instantiate(GlassPrefab);
        EnemyGlass.transform.position = EnemyGlassLocation;
    }

    public IEnumerator SwapGlass()
    {
        StartCoroutine(PlayerGlass.GetComponent<Glass>().MoveToOtherSide());
        Coroutine coroutine = StartCoroutine(EnemyGlass.GetComponent<Glass>().MoveToOtherSide());
        yield return coroutine;
    }

    public void SafeDestroy(ref GameObject gameObject)
    {
        Destroy(gameObject);
        gameObject = null;
    }
}
