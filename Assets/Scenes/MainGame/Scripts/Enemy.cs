using Sakemottekoi.MainGame;
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
}
