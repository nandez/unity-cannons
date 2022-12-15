using UnityEngine;

public class MouseCursorController : MonoBehaviour
{
    [SerializeField] private Texture2D normalCursorSprite;
    [SerializeField] private Texture2D attackCursorSprite;

    public CursorType Type { get; set; } = CursorType.Normal;


    void Update()
    {
        if (Type == CursorType.Normal)
        {
            Cursor.SetCursor(normalCursorSprite, new Vector2(normalCursorSprite.width / 2, normalCursorSprite.height / 2), CursorMode.Auto);
        }
        else if (Type == CursorType.Attack)
        {
            Cursor.SetCursor(attackCursorSprite, new Vector2(attackCursorSprite.width / 2, attackCursorSprite.height / 2), CursorMode.Auto);
        }
    }

    public enum CursorType
    {
        Normal,
        Attack
    }
}

