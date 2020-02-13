using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class SkillClass : MonoBehaviour
    {
        // 공용 기술
        public static bool FirstScript(Pokemon pokemon, string skillName, bool isEnemy, out string firstDialog)
        {
            if (!pokemon)
            {
                firstDialog = "";
                return false;
            }

            firstDialog = pokemon.Info._name + "는(은) " + skillName + "를(을) 시전했다!";

            if (isEnemy)
                firstDialog = "적의 " + firstDialog;

            return true;
        }

        // 튀어오르기 : 멍때리기류 갑
        public void Splash(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float probability = Random.Range(0f, 1f);

            FirstScript(myPokemon, "튀어오르기", false, out dialog);
            PokemonBattleManager.AddNextText(dialog);

            if (probability <= 0.05f)
            {
                enemyPokemon.Health = 0f;

                PokemonBattleManager.AddNextText("효과는 굉장했다!");
                PokemonBattleManager.AddNextText("이게 왜 되지? 새내기는 혼란에 빠졌다!");
            }
            else if (probability <= 0.13f)
            {
                myPokemon.Health -=  myPokemon.MaxHealth / 5f;

                PokemonBattleManager.AddNextText(myPokemon.Info._name + "는(은) 무리한 코딩으로 인해 데미지를 입었다!");
                PokemonBattleManager.AddNextText("새내기는 눈앞이 아득해질 것 같았다...");
            }
            else
            {
                PokemonBattleManager.AddNextText("그러나 아무것도 일어나지 않는다!");
            }
        }
        
        // Skills For C++
        public void OptimalizingOfLimitation(Pokemon myPokemon, Pokemon enemyPokemon)
        {

        }
    }
}