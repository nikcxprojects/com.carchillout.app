using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Car : MonoBehaviour
{
    [SerializeField] private Image _renderer;
    [SerializeField] private Sprite _initializedSprite;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private AudioClip _clipPlace;
    private bool _isOver = false;
    public bool isOver => _isOver;
    public void InitializeCar(Sprite sprite)
    {
        _initializedSprite = sprite;
        _renderer.sprite = sprite;
        StartCoroutine(SetEmptySprite());
    }
    
    public void ResetCar()
    {
        _initializedSprite = null;
        _renderer.sprite = _emptySprite;
        _isOver = false;
    }

    private IEnumerator SetEmptySprite()
    {
        yield return new WaitForSeconds(10.0f);
        _renderer.sprite = _emptySprite;
    }

    public void TryPlace(Sprite sprite)
    {
        AudioManager.instance.PlayAudio(_clipPlace);
        if (sprite != _initializedSprite)
        {
            if (AudioManager.instance.isVibration)
                Vibration.Vibrate();

            return;
        }

        _renderer.sprite = _initializedSprite;
        _isOver = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.currentCar = this;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.instance.currentCar = null;
    }
}
