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

    private InputActions actions;

    private float CurrentTilt
    {
        get{ return transform.rotation.eulerAngles.z; }
        set { transform.rotation = Quaternion.Euler(0, 0, value); }
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

        StartCoroutine(AddTilt(tiltOnce));
    }

    public void ReturnOrigin()
    {

    }

    public void SpawnWine()
    {
        if (CurrentTilt < wineFlowThreshold) return;

        int a = Mathf.Max(3, (int)(maxTilt - CurrentTilt));
        if (Time.frameCount % a == 0) Instantiate(wine, wineSpawnPos.position, Quaternion.identity);
    }

    public IEnumerator AddTilt(float tilt)
    {
        float animateTime = 0.2f;
        float elapssedTime = 0f;
        Quaternion target = transform.rotation * Quaternion.Euler(0, 0, tilt);

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
    }
}
