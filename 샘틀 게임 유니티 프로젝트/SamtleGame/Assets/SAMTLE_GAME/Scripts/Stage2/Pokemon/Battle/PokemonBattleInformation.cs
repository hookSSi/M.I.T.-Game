using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MIT.SamtleGame.Stage2
{
    public class PokemonBattleInformation : MonoBehaviour
    {
        public Image _profileImage;
        public Text _nameText;
        public Text _lovesText;
        public Text _hatesText;
        public Text _traitsText;

        private void Awake()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entryCancel = new EventTrigger.Entry();
            entryCancel.eventID = EventTriggerType.Cancel;
            entryCancel.callback.AddListener((data) => { Pokemon.PokemonBattleManager.Instance.SelectAction(); });
            trigger.triggers.Add(entryCancel);

            EventTrigger.Entry entrySelect = new EventTrigger.Entry();
            entrySelect.eventID = EventTriggerType.Select;
            entrySelect.callback.AddListener((data) => { UpdateInformation(Pokemon.PokemonBattleManager.Instance._myPokemon.Info); });
            trigger.triggers.Add(entrySelect);
        }

        public void UpdateInformation(Pokemon.PokemonInfo info)
        {
            _profileImage.sprite = info._frontImage;
            _nameText.text = "플밍몬 이름 : " + info._name;

            switch(info._name)
            {
                case "C":
                    _lovesText.text = "좋아하는 것 : 새내기 괴롭히기";
                    _hatesText.text = "싫어하는 것 : 세미콜론 빠뜨리기";
                    _traitsText.text = "특징 : 컴공과에 입학한 모든 새내기들이 한 번쯤은 거쳐가는 언어로, 진화형은 C++이다. " +
                        "뭐만 하면 오류를 뿜어대어 고학년도 싫어하는 듯하다.";
                    break;
                case "Python":
                case "파이썬":
                    _lovesText.text = "좋아하는 것 : 뱀(아니다)";
                    _hatesText.text = "싫어하는 것 : 탭과 스페이스바 혼용하기";
                    _traitsText.text = "특징 : 영국 BBC 프로그램의 이름을 따왔다. " +
                        "비전공자도 조금만 배우면 업무에 쓸 수 있다. " +
                        "C교수(가명)님의 머스트 헤브 아이템.";
                    break;
                case "Java":
                    _lovesText.text = "좋아하는 것 : 커피";
                    _hatesText.text = "싫어하는 것 : 이름으로 장난치기(...)";
                    _traitsText.text = "특징 : C언어에 시달린 새내기를 한 번 더 괴롭히는 언어. " +
                        "그런데 익숙해지면 또 자바만 쓰게 된다고 하며, 한국에서 기이하게 많이 사용한다...";
                    break;
            }
        }
    }
}