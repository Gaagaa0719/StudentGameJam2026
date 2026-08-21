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

    [SerializeField]
    public GameObject HealthyLiver;

    [SerializeField]
    public GameObject DamagedLiver;

    [SerializeField]
    public GameObject PoorLiver;

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

        float surplus = currentDegreePoint % maxDegreePoint;
        DegreeBar.fillAmount = surplus / maxDegreePoint;

        DegreePointText.text = $"{currentDegreePoint}/{maxDegreePoint}";

        switch ((int)(currentDegreePoint / maxDegreePoint))
        {
            case 0:
                HealthyLiver.SetActive(true);
                DamagedLiver.SetActive(false);
                PoorLiver.SetActive(false);
                break;
            case 1:
                HealthyLiver.SetActive(false);
                DamagedLiver.SetActive(true);
                PoorLiver.SetActive(false);
                break;
            default:
                HealthyLiver.SetActive(false);
                DamagedLiver.SetActive(false);
                PoorLiver.SetActive(true);
                break;

        }
    }

    public float GetDegreePoint()
    {
        return currentDegreePoint;
    }
}
