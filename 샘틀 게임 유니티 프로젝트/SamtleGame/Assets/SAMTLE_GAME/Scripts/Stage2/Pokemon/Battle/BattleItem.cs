using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    [System.Serializable]
    public class BattleItem : MonoBehaviour
    {
        public string _itemName;
        public string _itemCaption;
        public int _itemCount;
        public ItemType _itemType;
        public BattleEvent _itemEvent;

        public enum ItemType
        {
            Consume, Pokeball, Etc
        }

        [SerializeField] private Text _itemNameText;
        [SerializeField] private Text _itemCountText;

        public BattleItem(string itemName, string itemCaption, int itemCount, ItemType itemType, BattleEvent itemEvent)
        {
            SetValues(itemName, itemCaption, itemCount, itemType, itemEvent);
        }

        public void SetValues(string itemName, string itemCaption, int itemCount, ItemType itemType, BattleEvent itemEvent)
        {
            _itemName = itemName;
            _itemCaption = itemCaption;
            _itemCount = itemCount;
            _itemType = itemType;
            _itemEvent = itemEvent;
        }

        public void SetCount(int newCount)
        {
            _itemCount += newCount;
        }

        public void UpdateText()
        {
            _itemNameText.text = _itemName;
            _itemCountText.text = _itemCount.ToString();
        }
    }

    // 인게임 아이템 이벤트들
    public class Items
    {
        private static string UsingScript()
        {
            var battleManager = PokemonBattleManager.Instance;
            return "새내기는 " + battleManager._itemManager._previousItemName + "를(을) 사용했다!";
        }

        // 전 부회장의 3신기
        public static void TheTrinity(Pokemon myPokemon, Pokemon enemyPokemon, out string[] dialog)
        {
            var battleManager = PokemonBattleManager.Instance;
            var uiManager = PokemonBattleManager.Instance._uiManager;
            // 확률에 따라 플레이어의 체력이 1/3이 되거나 적의 체력이 1/3이 되거나 아무 일도 일어나지 않는다.
            int nextEffect = Random.Range(1, 4);

            float currentHealth;
            float damage = 100f / 3f;

            dialog = new string[2];
            dialog[0] = UsingScript();

            switch (nextEffect)
            {
                case 1:
                    currentHealth = myPokemon.Info._health;
                    currentHealth -= damage;

                    if (currentHealth < 0f)
                        currentHealth = 0f;

                    myPokemon.Info._health = currentHealth;

                    dialog[1] = "제 1신기로 인해 " + myPokemon.Info._name +
                        "가 피해를 입었다...";
                    break;
                case 2:
                    currentHealth = enemyPokemon.Info._health;
                    currentHealth -= damage;

                    if (currentHealth < 0f)
                        currentHealth = 0f;

                    enemyPokemon.Info._health = currentHealth;

                    dialog[1] = "제 2신기로 인해 " + enemyPokemon.Info._name +
                        "가 피해를 입었다!";
                    break;
                case 3:
                    dialog[1] = "제 3신기로 인해 분위기가 스산해졌다...";
                    break;
            }
        }
    }
}