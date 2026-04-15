using UnityEngine;
using UnityEngine.EventSystems;

public class UIChangeCursorOnOver : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D _cursor;
    [SerializeField] private Vector2 _cursorHotspot;
    [SerializeField] private Texture2D _defaultCursor;
    [SerializeField] private Vector2 _defaultCursorHotspot;



    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(_cursor,_cursorHotspot,CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(_defaultCursor,_defaultCursorHotspot,CursorMode.Auto);
    }

}
