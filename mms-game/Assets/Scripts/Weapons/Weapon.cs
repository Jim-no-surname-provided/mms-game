using UnityEngine;

public abstract class Weapon : MonoBehaviour, DamageDealer
{
    public abstract void Use(float angle);
    private SpriteRenderer spriteRenderer;
    public virtual void Flip()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipY = !spriteRenderer.flipY;
    }
    public virtual void Flip(bool b)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipY = b;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return true;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}