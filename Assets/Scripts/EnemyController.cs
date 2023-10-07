using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator enemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();

        //playerAnimator.SetFloat("Forward", isPlayerMoving ? 1f : 0f);
        //enemyAnimator.SetBool("walking", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
