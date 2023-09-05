using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Player : CharacterBase
{
    private void Awake()
    {
        SetupHealthBar();
    }

    private new void Update()
    {
        base.Update();
        Knockdown();
    }

    public override void Knockdown()
    {
        if(currentHP <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
}
