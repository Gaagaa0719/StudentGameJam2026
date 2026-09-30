using Sakemottekoi.MainGame;
using System.Linq;
using UnityEngine;

public class Enemy : Actor
{

    [SerializeField]
    private Animator animator;

    private int currentFrame = 0;

    private void Update()
    {
        currentFrame++;
        if(currentFrame % (60 * 10) == 0)
        {
            animator.SetTrigger("animate");
        }
    }

    /// <summary>
    /// 敵キャラの選んだ酒を返す
    /// </summary>
    public AlcoholGlass SelectGlass()
    {
        var alcholGlasses = GameObject.FindGameObjectsWithTag("Glass").Select(v => v.GetComponent<AlcoholGlass>()).ToArray();
        return alcholGlasses[Random.Range(0, alcholGlasses.Length)];
    }
}
