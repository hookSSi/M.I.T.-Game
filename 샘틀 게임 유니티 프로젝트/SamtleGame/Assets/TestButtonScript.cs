using UnityEngine;

// 테스트 전용 스크립트 : Merge 시 삭제 예정
public class TestButtonScript : MonoBehaviour
{
    public int newHP;
    public int maxHp = 100;
    public bool useAnimation;
    public PokemonBattleUIManager uimanager;

    public void OnClickEnemy()
    {
        // Debug.Log("눌러따");
        uimanager.UpdateEnemyHpUI(newHP, maxHp, useAnimation);
    }

    public void OnClickPlayer()
    {
        uimanager.UpdatePlayerHpUI(newHP, maxHp, useAnimation);
    }

    public void OnClickPlayerExp()
    {
        uimanager.UpdatePlayerExpUI(newHP, maxHp, useAnimation);
    }
}
