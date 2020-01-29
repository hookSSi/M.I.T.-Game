using System.Collections;
using UnityEngine;

using MIT.SamtleGame.Stage1;

public class InfiniteScroll : MonoBehaviour
{
    private float _currentX = 0;
    private Vector3 _nextGenPos;
    private int _currentIndex = 0;

    [Header("스테이지 정보")]
    [SerializeField]
    private int _count = 0;
    [Tooltip("복도의 끝 카운트")]
    public int _endCount = 3;
    
    [Header("메인 카메라")]
    public Transform _camera;
    public float _range = 5;

    [Header("다른 background 정보")]
    public Transform[] _background;
    public float _backgroundWidth = 34;

    private void Initilization()
    {
        _nextGenPos = _background[_currentIndex].transform.position;
    }

    private void Start() 
    {
        Initilization();
        StartCoroutine(BackgroundUpdateRoutine());
    }

    private void BackgroundUpdate() 
    {
        Vector3 currentBackgroundPos = _background[_currentIndex].transform.position;
        _currentX = currentBackgroundPos.x;

        currentBackgroundPos.x -= _backgroundWidth;
        _nextGenPos = currentBackgroundPos;

        if( _camera.transform.position.x < ( _currentX - _range ) )
        {
            _count += 1;
            _currentIndex = (_currentIndex + 1) % _background.Length;
            _background[_currentIndex].transform.position = _nextGenPos;
        }
    }

    IEnumerator BackgroundUpdateRoutine()
    {
        while(true)
        {
            if(_count >= _endCount)
            {
                SpawnerEvent.Trigger(SpawnerState.Pasue);

                if(GameManager._totalEnemyCount == 0)
                {
                    PasueGameEvent.Trigger();
                    yield break;
                }
            }
            else
            {
                BackgroundUpdate();
            }

            yield return new WaitForFixedUpdate();
        }        
    }
}
