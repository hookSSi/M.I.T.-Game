using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
            UpdateText();
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

        public void AddEvents()
        {
            // Submit 이벤트(아이템 사용), Cancel 이벤트(뒤로 가기)
            PokemonBattleManager manager = PokemonBattleManager.Instance;

            EventTrigger trigger = GetComponent<EventTrigger>();

            EventTrigger.Entry entrySubmit = new EventTrigger.Entry();
            entrySubmit.eventID = EventTriggerType.Submit;
            entrySubmit.callback.AddListener((data) => { manager._itemManager.UseItem(gameObject); });
            trigger.triggers.Add(entrySubmit);

            EventTrigger.Entry entryCancel = new EventTrigger.Entry();
            entryCancel.eventID = EventTriggerType.Cancel;
            entryCancel.callback.AddListener((data) => { manager.SelectAction(); });
            trigger.triggers.Add(entryCancel);
        }
    }

    // 인게임 아이템 이벤트들
    public class Items
    {
        private static void FirstScript(out string firstDialog)
        {
            firstDialog = "새내기는 "
                + PokemonBattleManager.Instance._itemManager._previousItemName + "를(을) 사용했다!";
        }

        // 전 부회장의 3신기
        public void TheTrinity(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            // 확률에 따라 플레이어의 체력이 1/3이 되거나 적의 체력이 1/3이 되거나 아무 일도 일어나지 않는다.
            int nextEffect = Random.Range(1, 4);
            float damage;

            FirstScript(out dialog);
            PokemonBattleManager.AddNextText(dialog);

            switch (nextEffect)
            {
                case 1:
                    damage = enemyPokemon.MaxHealth * 0.34f;
                    enemyPokemon.Health -= damage;

                    PokemonBattleManager.AddNextText("제 1신기 폭풍을 부르는 선풍기로 인해 " + enemyPokemon.Info._name + "이(가) 피해를 입었다!");
                    break;
                case 2:
                    damage = myPokemon.MaxHealth * 0.34f;
                    myPokemon.Health -= damage;

                    PokemonBattleManager.AddNextText("제 2신기 음성 인식 되는 플라즈마볼이 잘못 작동해 " + myPokemon.Info._name + "이(가) 반동을 입었다...");
                    break;
                case 3:
                    PokemonBattleManager.AddNextText("제 3신기 워터 컨트리뷰터로 인해 물바다가 되었다...");

                    if (enemyPokemon.Info._name == "오래된 동방컴")
                    {
                        damage = enemyPokemon.MaxHealth * 0.2f;
                        enemyPokemon.Health = Mathf.Max(0.1f, enemyPokemon.Health - damage);
                        PokemonBattleManager.AddNextText("오래된 동방컴의 키보드에 물이 들어가 삐걱거리고 있다!");
                    }
                    break;
            }
        }

        // 노사람즈 스카이 확장팩(노맨즈스카이 아니다)
        public void NoHumansSky(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            myPokemon.Health -= 15f;

            FirstScript(out dialog);
            PokemonBattleManager.AddNextText(dialog);
            PokemonBattleManager.AddNextText(myPokemon.Info._name +
                "이(가) 게임을 구동하다가 타격을 입었다. 새내기가 이딴 것도 게임이냐며 화를 낸다...");
        }

        // 마시면 큰일나는 보드카
        public void DangerousVodka(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            myPokemon.Health += 40f;
            myPokemon._status = Pokemon.StatusEffect.AttackDown;
            myPokemon._effectCount = 2;

            FirstScript(out dialog);
            PokemonBattleManager.AddNextText(dialog);
            PokemonBattleManager.AddNextText(myPokemon.Info._name + "는(은) 정신이 헤롱헤롱해졌다... 컴퓨터에서 알싸한 향이 난다. " +
                myPokemon.Info._name + "의 다음 기술의 위력이 감소했다!");
        }

        // MIT산 보드겜
        public void MITsBoardGame(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            myPokemon.Health += 20f;
            enemyPokemon.Health += 20f;

            FirstScript(out dialog);
            PokemonBattleManager.AddNextText(dialog);
            PokemonBattleManager.AddNextText("즐겁게 놀아서 " + myPokemon.Info._name + "와(과) " + enemyPokemon.Info._name +
                "의 체력이 회복되었다... 그런데 우리 뭐하고 있었더라? 그보다 왜 보드게임이 여기에?");
        }

        // 평범한 포켓볼
        public void Pokeball(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;

            FirstScript(out dialog);
            PokemonBattleManager.AddNextText(dialog);
            PokemonBattleManager.AddNextText("여긴 포켓몬 세계가 아니라서 " +
                enemyPokemon.Info._name + "을 사로잡을 수는 없다...");
        }
    }
}