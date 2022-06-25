using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BattleState { start, playerturn, enemyturn, won, lost}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject Enemyprefab;

    public Transform PlayerBattleState;
    public Transform EnemyBattleState;

   [SerializeField] private GameObject[] choicesbat;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHud playerHUD;
    public BattleHud enemyHUD;

    public BattleState state;

    void Start()
    {
        state = BattleState.start;
        StartCoroutine(SetupBatle());
    }

    IEnumerator SetupBatle()
    {
       GameObject playerGo = Instantiate(playerPrefab, PlayerBattleState);
       playerUnit = playerGo.GetComponent<Unit>();


       GameObject enemyGo = Instantiate(Enemyprefab, EnemyBattleState);
       enemyUnit = enemyGo.GetComponent<Unit>();

        dialogueText.text = "A Wild " + enemyUnit.unitName + " approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.playerturn;
        PlayerTurn();
        EventSystem.current.SetSelectedGameObject(choicesbat[0].gameObject);
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.won;
            EndBattle();
        }
        else
        {
            state = BattleState.enemyturn;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + "attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.lost;
            EndBattle();
        }
        else
        {
            state = BattleState.playerturn;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.won)
        {
            dialogueText.text = "You won the Battle!";
        }
        else if(state == BattleState.lost)
        {
            dialogueText.text = "You were defeated.";
        }
    }

    private void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(15);
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        state = BattleState.enemyturn;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.playerturn)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.playerturn)
            return;

        StartCoroutine(PlayerHeal());
    }
}
