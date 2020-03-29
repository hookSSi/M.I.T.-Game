using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class SkillClass : MonoBehaviour
    {
        private static int _count = 0;
        PokemonBattleManager _manager;

        public void Init()
        {
            _count = 0;
            _manager = PokemonBattleManager.Instance;
        }

        /*
         * 공용 기술
         */

        /// 스킬 시전 대사
        public static bool FirstScript(Pokemon pokemon, string skillName, out string firstDialog)
        {
            Debug.Log("I play the FirstScript!");

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

        private static float GetDamage(Pokemon myPokemon, Pokemon enemyPokemon, float damage)
        {
            if (myPokemon._status == Pokemon.StatusEffect.AttackDown)
                damage *= 0.66f;

            return damage;
        }

        /// 뇌정지(생각을 멈추었다) : 발버둥 같은 스킬
        public void StopThinking(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;

            myPokemon.Health -= myPokemon.MaxHealth * 0.1f;
            enemyPokemon.Health -= enemyPokemon.MaxHealth * 0.1f;

            FirstScript(myPokemon, "뇌정지", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            _manager._dialogueController.AddNextPage(myPokemon.Info._name + "는(은) 생각을 포기하였다!", true);
        }

        /// 튀어오르기 : 멍때리기류 갑
        public void Splash(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float probability = Random.value;

            FirstScript(myPokemon, "튀어오르기", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);

            if (probability <= 0.05f)
            {
                enemyPokemon.Health -= enemyPokemon.MaxHealth / 2f;

                _manager._dialogueController.AddNextPage("효과는 굉장했다!");
                _manager._dialogueController.AddNextPage("이게 왜 되지? 신입생은 혼란에 빠졌다!", true);
            }
            else if (probability <= 0.13f)
            {
                myPokemon.Health -=  myPokemon.MaxHealth / 5f;

                _manager._dialogueController.AddNextPage(myPokemon.Info._name + "는(은) 무리한 코딩으로 인해 데미지를 입었다!");
                _manager._dialogueController.AddNextPage("신입생은 눈앞이 아득해질 것 같았다...", true);
            }
            else
            {
                _manager._dialogueController.AddNextPage("그러나 아무것도 일어나지 않는다!", true);
            }
        }
        
        /*
         * Skills For C++
         */

        /// 극한의 최적화
        public void OptimalizingOfLimitation(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = GetDamage(myPokemon, enemyPokemon, 10 + 20 * Mathf.Min(_count, 3));
            _count++;

            enemyPokemon.Health -= damage;
            myPokemon.Health += damage * 0.1f;

            FirstScript(myPokemon, "극한의 최적화", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            if (_count == 1)
                _manager._dialogueController.AddNextPage("효과는 미미했다...");
            if (_count >= 3)
                _manager._dialogueController.AddNextPage("효과는 굉장했다!");
            _manager._dialogueController.AddNextPage("해당 기술의 위력이 조금 강해졌다!", true);
        }

        /*
         * Skills For Python
         */

        /// 이지 투 유즈
        public void EasyToUse(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = GetDamage(myPokemon, enemyPokemon, 30f);
            enemyPokemon.Health -= damage;

            FirstScript(myPokemon, "이지 투 유즈", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            _manager._dialogueController.AddNextPage("심플 이즈 베스트!", true);
        }

        /*
         * Skills For Java
         */

        /// Java를 Java
        public void GrabTheJava(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = GetDamage(myPokemon, enemyPokemon, (enemyPokemon.MaxHealth + myPokemon.MaxHealth) * 0.2f);
            enemyPokemon.Health -= damage;

            _manager._uiManager._effect.SetAnim("Blizzard", true);

            FirstScript(myPokemon, "Java를 Java", out dialog);
            _manager._dialogueController.AddNextPage(dialog + " '자바'를 자바라!");
            _manager._dialogueController.AddNextPage("서릿발이 날리기 시작했다...", true);
            if (enemyPokemon.Health <= 0f)
                _manager._dialogueController.AddNextPage(enemyPokemon.Info._name + "는(은) 드립을 참지 못하고 죽음을 택하였다!");
            _manager._dialogueController.AddNextPage("효과는 굉장했다!", true);
        }

        /* 
        *  Skills For 신입생
        */

        /// 쓰다듬기
        public void TouchTouch(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = GetDamage(myPokemon, enemyPokemon, 0);

            enemyPokemon.Health -= damage;
            myPokemon.Health -= damage * 0.1f;

            FirstScript(myPokemon, "쓰담쓰담!", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            _manager._dialogueController.AddNextPage("고양이는 손길을 피했다...");
            _manager._dialogueController.AddNextPage("밥을 줘야할거 같다!", true);
        }

        /// 밥주기
        public void FeedACat(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = GetDamage(myPokemon, enemyPokemon, 50);

            enemyPokemon.Health -= damage;
            myPokemon.Health += damage * 0.1f;

            FirstScript(myPokemon, "쓰담쓰담!", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            _manager._dialogueController.AddNextPage("고양이가 먹는 모습을 보니...");
            _manager._dialogueController.AddNextPage("맘이 편해진다.", true);
        }

        /// 소리지르기
        public void Crying(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = GetDamage(myPokemon, enemyPokemon, 299);

            enemyPokemon.Health -= damage;

            FirstScript(myPokemon, "으아ㅏ아아앙아아아!", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            _manager._dialogueController.AddNextPage("고양이는 기겁했다!", true);
        }
        

        /*
         * Skills For 오래된 동방컴
         */

        /// 돌돌돌
        public void StoneStoneStone(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            // 그냥 돌돌돌... 도는 소리
            string dialog;
            int rand = Random.Range(-1, 2);
            myPokemon.Health += rand * myPokemon.MaxHealth / 10f;

            FirstScript(myPokemon, "마지막 팬소리", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);

            switch (rand)
            {
                case -1:
                    _manager._dialogueController.AddNextPage("돌돌돌... " + myPokemon.Info._name + "는(은) 힘겹게 돌아가고 있다.");
                    _manager._dialogueController.AddNextPage(myPokemon.Info._name + "의 체력이 약간 감소하였다!", true);
                    break;
                case 0:
                    _manager._dialogueController.AddNextPage("돌돌돌... " + myPokemon.Info._name + "는(은) 힘겹게 돌아가고 있다.", true);
                    break;
                case 1:
                    _manager._dialogueController.AddNextPage("돌돌돌... " + myPokemon.Info._name + "는(은) 힘겹게 돌아가고 있다.");
                    _manager._dialogueController.AddNextPage(myPokemon.Info._name + "의 체력이 약간 회복하였다!", true);
                    break;
            }
        }

        /*
         * Skills For 민지의 고양이
         */

        /// 심쿵사
        public void DeadByHeartPonding(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = GetDamage(myPokemon, enemyPokemon, 30f);

            enemyPokemon.Health -= damage;

            _manager._uiManager._effect.SetAnim("HeartAttack", true);

            FirstScript(myPokemon, "심쿵사", out dialog);
            _manager._dialogueController.AddNextPage(dialog);
            _manager._dialogueController.AddNextPage (myPokemon.Info._name + "는(은) 귀엽게 하악질을 한다.", true);
            _manager._dialogueController.AddNextPage("효과는 굉장했다! " + enemyPokemon.Info._name + "는(은) 정신을 차릴 수 없다!", true);
        }

        /// 집사 간택
        public void BeChosen(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            float damage = GetDamage(myPokemon, enemyPokemon, 20f);

            enemyPokemon.Health -= damage;

            FirstScript(myPokemon, "집사 간택", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            _manager._dialogueController.AddNextPage("신입생은 " + myPokemon.Info._name + "에게 간택당해 정신을 차릴 수 없다!");

            if (enemyPokemon.Info._name != "신입!신입!")
                _manager._dialogueController.AddNextPage(enemyPokemon.Info._name + "이(가) 신입생을 한심하게 쳐다 본다...", true);
            else
                _manager._dialogueController.AddNextPage(enemyPokemon.Info._name + "은 행복사하기 직전이다...", true);
        }

        /// 터줏대감
        public void MasterMeow(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;

            enemyPokemon._status = Pokemon.StatusEffect.AttackDown;
            enemyPokemon._effectCount = 5;

            FirstScript(myPokemon, "터줏대감", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            _manager._dialogueController.AddNextPage(enemyPokemon.Info._name + "이(가) 기가 눌렸다... 5턴간 위력이 감소했다.", true);
        }

        /// 낮잠
        public void SleepAfternoon(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;

            _manager._uiManager._effect.SetAnim("Sleep", false);

            FirstScript(myPokemon, "낮잠", out dialog);
            _manager._dialogueController.AddNextPage(dialog, true);
            _manager._dialogueController.AddNextPage(myPokemon.Info._name + "는 나른하게 " + enemyPokemon.Info._name + "을 쳐다보았다.", true);
        }
    }
}