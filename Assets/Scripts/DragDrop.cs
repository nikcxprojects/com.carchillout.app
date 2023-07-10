using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IEndDragHandler, IDragHandler
{
    [SerializeField] private Vector2 _startPosition;
    [SerializeField] private Sprite _sprite;
    public Sprite sprite => _sprite;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.isRemember)
            return;

        gameObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.transform.position = _startPosition;
        if (GameManager.instance.currentCar == null)
            return;

        GameManager.instance.currentCar.TryPlace(_sprite);
    }

}
