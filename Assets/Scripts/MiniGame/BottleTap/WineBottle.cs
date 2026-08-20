using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WineBottle: MonoBehaviour, InputActions.IMiniGameActions
{
    [Header("元の角度に戻ろうとする速度")]
    [SerializeField] private float returnSpeed = 1;

    [Header("タップした際に一度に傾ける角度")]
    [SerializeField] private float tiltOnce = 0.5f;

    [Header("ワインが流れ始める閾値")]
    [SerializeField] private float wineFlowThreshold = 10f;

    [Header("傾きの最大値")]
    [SerializeField] private float maxTilt = 25f;

    [Header("ワイン")]
    [SerializeField] private GameObject wine;

    [Header("ワインのスポーン位置")]
    [SerializeField] private Transform wineSpawnPos;

    private bool isTilting = false;
    private InputActions actions;

    private float CurrentTilt
    {
        get{ return transform.rotation.eulerAngles.z; }
        set { transform.rotation = Quaternion.Euler(0, 0, value); }
    }

    public void Init()
    {
        CurrentTilt = 0;
        if(actions != null) actions.Enable();
    }

    private void Awake()
    {
        actions = new InputActions();
        actions.MiniGame.SetCallbacks(this);
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void Update()
    {
        SpawnWine();
        ReturnOrigin();
    }
    public void OnMainAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if(CurrentTilt >= maxTilt) return;

        StopAllCoroutines();
        StartCoroutine(Tilt());
    }

    public void ReturnOrigin()
    {
        if (isTilting) return;
        if (CurrentTilt <= 0) return;

        CurrentTilt -= returnSpeed * Time.deltaTime;
        if(CurrentTilt < 0 || CurrentTilt > maxTilt) CurrentTilt = 0;
    }

    public void SpawnWine()
    {
        if (CurrentTilt < wineFlowThreshold) return;

        int a = Mathf.Max(3, (int)(maxTilt - CurrentTilt));
        if (Time.frameCount % a == 0) Instantiate(wine, wineSpawnPos.position, Quaternion.identity);
    }

    public IEnumerator Tilt()
    {
        isTilting = true;
        float animateTime = 0.2f;
        float elapssedTime = 0f;
        Quaternion target = transform.rotation * Quaternion.Euler(0, 0, tiltOnce);

        while (elapssedTime < animateTime)
        {
            elapssedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, target, elapssedTime/animateTime);

            if (CurrentTilt >= maxTilt) {
                transform.rotation = Quaternion.Euler(0, 0, maxTilt);
                break;
            }

            yield return null;
        }
        isTilting = false;
    }
}
