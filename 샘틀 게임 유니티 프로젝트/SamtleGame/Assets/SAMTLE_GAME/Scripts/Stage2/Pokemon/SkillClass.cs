﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class SkillClass : MonoBehaviour
    {
        private static int count = 0;

        private void Start()
        {
            count = 0;
        }

        // 공용 기술
        // 스킬 시전 대사
        public static bool FirstScript(Pokemon pokemon, string skillName, out string firstDialog)
        {
            if (!pokemon)
            {
                firstDialog = "";
                return false;
            }

            bool isEnemy = (pokemon == PokemonBattleManager.Instance._enemyPokemon);

            firstDialog = pokemon.Info._name + "는(은) " + skillName + "를(을) 시전했다!";

            if (isEnemy)
                firstDialog = "적의 " + firstDialog;

            return true;
        }

        // 뇌정지(생각을 멈추었다) : 발버둥 같은 스킬
        public void StopThinking(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;

            myPokemon.Health -= myPokemon.MaxHealth * 0.1f;
            enemyPokemon.Health -= enemyPokemon.MaxHealth * 0.1f;

            FirstScript(myPokemon, "뇌정지", out dialog);
            PokemonBattleManager.AddNextText(dialog);
            PokemonBattleManager.AddNextText(myPokemon.Info._name + "는(은) 생각을 포기하였다!");
        }

        // 튀어오르기 : 멍때리기류 갑
        public void Splash(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float probability = Random.value;

            FirstScript(myPokemon, "튀어오르기", out dialog);
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
        // 극한의 최적화
        public void OptimalizingOfLimitation(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = enemyPokemon.MaxHealth * (0.1f + 0.2f * count++);

            enemyPokemon.Health -= damage;
            myPokemon.Health += damage * 0.2f;

            FirstScript(myPokemon, "극한의 최적화", out dialog);
            PokemonBattleManager.AddNextText(dialog);
            if (count == 1)
                PokemonBattleManager.AddNextText("효과는 미미했다...");
            if (count >= 3)
                PokemonBattleManager.AddNextText("효과는 굉장했다!");
        }

        // Skills For Python
        // 이지 투 유즈
        public void EasyToUse(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            enemyPokemon.Health -= enemyPokemon.MaxHealth * 0.3f;

            FirstScript(myPokemon, "이지 투 유즈", out dialog);
            PokemonBattleManager.AddNextText(dialog);
            PokemonBattleManager.AddNextText("심플 이즈 베스트!");
        }

        // Skills For Java
        // Java를 Java
        public void GrabTheJava(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            enemyPokemon.Health -= myPokemon.MaxHealth / 2f;

            FirstScript(myPokemon, "Java를 Java", out dialog);
            PokemonBattleManager.AddNextText(dialog);
            PokemonBattleManager.AddNextText("'자바'를 자바라!");
            PokemonBattleManager.AddNextText("서릿발이 날리기 시작했다...");
            if (enemyPokemon.Health <= 0f)
                PokemonBattleManager.AddNextText(enemyPokemon.Info._name + "는(은) 죽음을 택하였다!");
            PokemonBattleManager.AddNextText("효과는 굉장했다!");
        }

        // Skills For 오래된 동방컴
        // 돌돌돌
        public void StoneStoneStone(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            // 그냥 돌돌돌... 도는 소리
            string dialog;
            int rand = Random.Range(-1, 2);
            myPokemon.Health += rand * myPokemon.MaxHealth / 10f;

            FirstScript(myPokemon, "마지막 팬소리", out dialog);
            PokemonBattleManager.AddNextText(dialog);
            PokemonBattleManager.AddNextText("돌돌돌... " + myPokemon.Info._name + "는(은) 힘겹게 돌아가고 있다.");

            switch (rand)
            {
                case -1:
                    PokemonBattleManager.AddNextText(myPokemon.Info._name + "의 체력이 약간 감소하였다!");
                    break;
                case 1:
                    PokemonBattleManager.AddNextText(myPokemon.Info._name + "의 체력이 약간 회복하였다!");
                    break;
            }
        }

        // Skills For 민지의 고양이
        // 심쿵사
        
        // 집사 간택

        // 터줏대감

        // 손가락 흔들기
    }
}