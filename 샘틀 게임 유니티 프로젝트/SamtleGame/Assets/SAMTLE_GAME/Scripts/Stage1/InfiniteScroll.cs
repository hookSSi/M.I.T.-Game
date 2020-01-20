using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField]
    private int _currentIndex = 0;
    private int _count = 0;
    [SerializeField]
    private float _currentX = 0;
    [SerializeField]
    private Vector3 _nextGenPos;
    
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
        BackgroundUpdate();
    }

    private void FixedUpdate() 
    {
        BackgroundUpdate();
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
}
