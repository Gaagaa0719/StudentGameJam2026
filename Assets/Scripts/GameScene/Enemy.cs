using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Image DegreeBar;

    [SerializeField]
    private TextMeshProUGUI DegreePointText;

    [SerializeField]
    private float currentDegreePoint = 0;

    [SerializeField]
    public float maxDegreePoint = 0;

    [SerializeField]
    private Animator animator;

    private int currentFrame = 0;

    private void Start()
    {
        DegreePointText.text = $"{currentDegreePoint}/{maxDegreePoint}";
    }

    private void Update()
    {
        currentFrame++;
        if(currentFrame % (60 * 10) == 0)
        {
            animator.SetTrigger("animate");
        }
    }

    public void AddDegreePoint(float value)
    {
        SetDegreePoint(currentDegreePoint += value);
    }

    public void RemoveDegreePoint(float value)
    {
        SetDegreePoint(currentDegreePoint -= value);
    }

    public void SetDegreePoint(float value)
    {
        currentDegreePoint = Mathf.Min(value, maxDegreePoint);

        DegreeBar.fillAmount = currentDegreePoint / maxDegreePoint;

        DegreePointText.text = $"{currentDegreePoint}/{maxDegreePoint}";
    }

    public float GetDegreePoint()
    {
        return currentDegreePoint;
    }
}
