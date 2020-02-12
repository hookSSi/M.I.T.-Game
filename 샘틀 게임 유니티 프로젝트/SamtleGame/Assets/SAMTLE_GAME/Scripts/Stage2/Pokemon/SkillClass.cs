using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class SkillClass : MonoBehaviour
    {
        // 공용 기술
        public static void FirstScript(Pokemon pokemon, string skillName, bool isEnemy, out string firstDialog)
        {
            firstDialog = pokemon.Info._name + "는(은) " + skillName + "를(을) 시전했다!";

            if (isEnemy)
                firstDialog = "적의 " + firstDialog;
        }

        // 튀어오르기 : 멍때리기류 갑
        public void Splash(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string[] dialog = PokemonBattleManager.Instance.nextScripts;
            float probability = Random.Range(0f, 1f);

            dialog = new string[3];

            FirstScript(myPokemon, "튀어오르기", false, out dialog[0]);

            if (probability <= 0.05f)
            {
                Debug.Log("이게 왜... 되지?");

                enemyPokemon.Info._health = 0f;

                dialog[1] = "효과는 굉장했다!";
                dialog[2] = "이게 왜 되지? 새내기는 혼란에 빠졌다!";
            }
            else if (probability <= 0.08f)
            {
                Debug.Log("이게 외 않되?????");

                myPokemon.Info._health -= 0.49f;

                if (myPokemon.Info._health < 0f)
                    myPokemon.Info._health = 0f;

                dialog[1] = myPokemon.Info._name + "는(은) 무리한 코딩으로 인해 데미지를 입었다!";
                dialog[2] = "새내기는 눈앞이 아득해질 것 같았다...";
            }
        }
        
        // Skills For C++
        public void OptimalizingOfLimitation(Pokemon myPokemon, Pokemon enemyPokemon)
        {

        }
    }
}