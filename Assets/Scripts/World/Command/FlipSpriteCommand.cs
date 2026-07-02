using Fungus;
using UnityEngine;

[CommandInfo("Character", "Flip Sprite", "Flip sprite renderer")]
public class FlipSpriteCommand: Command
{
    public SpriteRenderer spriteRenderer;

    public bool flip;

    public override void OnEnter()
    {
        spriteRenderer.flipX = flip;

        Continue();
    }
}
