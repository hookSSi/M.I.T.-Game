using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MIT.SamtleGame.Tools;

public struct ChangeImageEvent
{
    public int _id;
    public int _targetSpriteIndex;

    public ChangeImageEvent(int targetSpriteIndex, int id = 0)
    {
        _id = id;
        _targetSpriteIndex = targetSpriteIndex;
    }

    static ChangeImageEvent e;

    public static void Trigger(int targetSpriteIndex, int id = 0)
    {
        e._id = id;
        e._targetSpriteIndex = targetSpriteIndex;
        EventManager.TriggerEvent(e);
    }
}

[RequireComponent(typeof(Image))]
public class AnimImage : MonoBehaviour, EventListener<ChangeImageEvent>
{
    [Header("Identification")]
    public int _id;

    [Header("Sprites")]
    [Tooltip("애니메이션 스프라이트")]
    public Sprite[] _sprites;
    public int _defaultSpriteIndex;

    [InspectorButton("NextImage")]
    public bool _nextImage;
    [InspectorButton("PrevImage")]
    public bool _prevImage;
    [InspectorButton("Reset")]
    public bool _reset;

    private Image _image;
    private int _currentSpriteIndex;

    private void Reset()
    {
        ChangeImageEvent.Trigger(_defaultSpriteIndex, _id);
    }

    private void NextImage()
    {
        ChangeImageEvent.Trigger(_currentSpriteIndex + 1, _id);
    }

    private void PrevImage()
    {
        ChangeImageEvent.Trigger(_currentSpriteIndex - 1, _id);
    }

    private void Initialization()
    {
        _image = GetComponent<Image>();
        _image.sprite = _sprites[_defaultSpriteIndex];
    }

    private void Start() 
    {
        Initialization();
    }

    private void ChangeImage(int targetIndex, int id)
    {
        if (id != _id)
        {
            return;
        }

        if (targetIndex < 0)
            _currentSpriteIndex = _sprites.Length + targetIndex;
        else if (targetIndex >= _sprites.Length)
            _currentSpriteIndex = targetIndex % _sprites.Length;
        else
            _currentSpriteIndex = targetIndex;

        _image.sprite = _sprites[_currentSpriteIndex];
    }

    public void SpriteInitialize(int size)
    {
        _sprites = new Sprite[size];
    }

    public void AddNext(Sprite newSprite, int index)
    {
        if (index >= 0 && index < _sprites.Length)
            _sprites[index] = newSprite;
    }

    public virtual void OnEvent(ChangeImageEvent changeImageEvent)
    {
        _currentSpriteIndex = changeImageEvent._targetSpriteIndex;
        ChangeImage(_currentSpriteIndex, changeImageEvent._id);
    }

    private void OnEnable() 
    {
        this.EventStartListening<ChangeImageEvent>();
    }

    private void OnDisable() 
    {
        this.EventStopListening<ChangeImageEvent>();
    }
}
