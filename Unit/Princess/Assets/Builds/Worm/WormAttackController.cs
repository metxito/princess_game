using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAttackController : MonoBehaviour
{
    public SpriteRenderer render;
    public float powerAttackBasic = 1f;
    public float powerAttackMedium = 1.5f;
    public float powerAttackHard = 2f;


    [ColorUsage(true, false)]
    public Color wormBasicColor;
    [ColorUsage(true, false)]
    public Color wormMediumColor;
    [ColorUsage(true, false)]
    public Color wormHardColor;


    private float powerAttack;
    private Color wormColor;

    public void SetLevel(int level){
        switch(level){
            case 1:
                powerAttack = powerAttackMedium;
                wormColor = wormMediumColor;
                break;
            case 2:
                powerAttack = powerAttackHard;
                wormColor = wormHardColor;
                break;
            default:
                powerAttack = powerAttackBasic;
                wormColor = wormBasicColor;
                break;
        }

        render.color = wormColor;
    }


}
