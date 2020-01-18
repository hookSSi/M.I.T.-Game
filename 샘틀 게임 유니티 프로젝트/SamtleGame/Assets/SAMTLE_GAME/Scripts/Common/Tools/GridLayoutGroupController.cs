using UnityEngine;
using UnityEngine.UI;

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

        Vector2 size = ((RectTransform)transform).rect.size;

        float width = size.x - _grid.padding.horizontal;
        float height = size.y - _grid.padding.vertical;

        width = (width - _grid.spacing.x * (_column - 1)) / _column;
        height = (height - _grid.spacing.y * (_row - 1)) / _row;

        _grid.cellSize = new Vector2(width, height);
    }
}
