using UnityEngine;

public class ShockwaveItem : ItemAttributes
{
    private bool frostwave = false;

    void Start()
    {
        if (itemManager.GetAttributes().Find(attribute => attribute.GetType() == typeof(IceItem)) != null)
        {
            ApplyIceBuff();
        }
    }

    void Update()
    {

    }

    public override bool AffectedByFire() => true;

    public override bool AffectedByIce() => true;

    public bool getIceBuff() => frostwave;

    public override void ApplyIceBuff()
    {
        frostwave = true;
    }
}
