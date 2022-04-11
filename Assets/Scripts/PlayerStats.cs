using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class holds the funds info about the play, other global player stats 
/// could also be stored and manipulated here
/// </summary>

public class PlayerStats : MonoBehaviour
{
    public int InitialFunds = 1000;
    public int steadyFundsPer5Seconds = 5;
    public static PlayerStats stats;
    public Text fundsTxt;
    private int funds;
    private float steadyFundTimer;


    private void Start() {
        funds = InitialFunds;
        stats = this;
        fundsTxt.text = "$" + funds;
    }

    /// <summary>
    ///this adds steady funds of a very small amount incase the player mismanages
    ///funds and cannot build anything, so the player gets only a very small
    ///amount to slowly recover
    /// </summary>
    private void steadyFunds() {
        steadyFundTimer += Time.deltaTime;

        if (steadyFundTimer >= steadyFundsPer5Seconds) {
            steadyFundTimer = 0;
            adjustFund(steadyFundsPer5Seconds);
        }
    }

    /// <summary>
    /// increase or decrease the funds based on purchases of units or other factors 
    /// </summary>
    /// <param name="amount"></param>
    public void adjustFund(int amount) {
        funds += amount;
        if (funds < 0) {
            funds = 0;
        }
        fundsTxt.text = "$" + funds;
    }

    public int getFunds() {
        return funds;
    }

    /// <summary>
    /// check if the player can spend x amount of money
    /// </summary>
    /// <param name="amount"></param> the amount you want to check if the player has
    /// <returns></returns>
    public bool canSpend(int amount) {
        return (funds - amount >= 0);
    }

    private void Update() {
        steadyFunds();
    }


}
