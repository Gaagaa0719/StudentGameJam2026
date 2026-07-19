using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Image DegreeBar;

    [SerializeField]
    private TextMeshProUGUI DegreePointText;

    [SerializeField]
    private float currentDegreePoint = 0;

    [SerializeField]
    public float maxDegreePoint = 0;

    private void Start()
    {
        DegreePointText.text = $"{currentDegreePoint}/{maxDegreePoint}";
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
        currentDegreePoint = value;
        DegreeBar.fillAmount = Mathf.Min(currentDegreePoint / maxDegreePoint, 1);
        DegreePointText.text = $"{currentDegreePoint}/{maxDegreePoint}";
    }

    public float GetDegreePoint()
    {
        return currentDegreePoint;
    }
}
