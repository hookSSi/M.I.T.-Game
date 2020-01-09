using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        Start,
        Introduction,
        SelectAction, SelectSkill, SelectItem,
        Act,
        AfterAct,
        End
    }

    private Pokemon.Pokemon _myPokemon;
    private Pokemon.Pokemon _enemyPokemon;

    public BattleState _state { get; private set; }

    /*
    public bool _isGameOver
    {
        get { return _myPokemon.hp <= 0 || _enemyPokemon.hp <= 0; }
    }
    */
    
    // 전투 시작
    public void StartBattle(Pokemon.Pokemon myPokemon, Pokemon.Pokemon enemyPokemon)
    {
        _myPokemon = myPokemon;
        _enemyPokemon = enemyPokemon;

        _state = BattleState.Start;

        Debug.Log("배틀 시작!");
        SelectAction();
    }
    
    // 행동 선택
    private void SelectAction()
    {
        _state = BattleState.SelectAction;
    }

    // 스킬 선택창
    public void SelectSkill()
    {
        _state = BattleState.SelectSkill;
    
        // UI Update
        Debug.Log("스킬 선택하기...");
    }

    // 아이템 선택창
    public void SelectItem()
    {
        _state = BattleState.SelectItem;

        // UI Update
        Debug.Log("아이템 선택하기...");
    }

    // 스킬 사용
    public void UseSkill(int select)
    {
        _state = BattleState.Act;
        _myPokemon.UseSkill(select);
    }

    // 아이템 사용
    private void UseItem()
    {
        _state = BattleState.Act;
        Debug.Log("아이템 사용!");
    }

    // 포켓몬 교체(아무 기능 없음)
    public void SelectPokemon()
    {
        Debug.Log("이건 포켓몬이 아닌걸.");
    }
    
    // 도주(아무 기능 없음)
    public void Escape()
    {
        Debug.Log("안돼! 이번 학기 학점을 이렇게 버릴 수 없어!");
    }
}
