using UnityEngine;
using UnityEngine.UI;

namespace MIT.SamtleGame.Tools
{
    // Grid Layout Group에서 Cell Size를 자동으로 조절합니다.(스크립트 부착만 하면 됨)
    public class GridLayoutGroupController : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _grid;

        [SerializeField] private int _row;
        [SerializeField] private int _column;

        private void Awake()
        {
            DynamicCell();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            DynamicCell();
        }
#endif

        public void DynamicCell()
        {
            if (_row == 0 || _column == 0)
                return;

            if (!_grid)
            {
                _grid = GetComponent<GridLayoutGroup>();

                if (!_grid)
                    return;
            }

            float rectwidth = Mathf.Abs(((RectTransform)transform).rect.width);
            float rectheight = Mathf.Abs(((RectTransform)transform).rect.height);
            // Debug.Log("너비/높이 " + rectwidth + "/" + rectheight);

            float width = rectwidth - _grid.padding.horizontal;
            float height = rectheight - _grid.padding.vertical;

            width = (width - _grid.spacing.x * (_column - 1)) / _column;
            height = (height - _grid.spacing.y * (_row - 1)) / _row;

            _grid.cellSize = new Vector2(width, height);
        }
    }
}