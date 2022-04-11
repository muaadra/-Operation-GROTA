
/// <summary>
/// This class is for the player attack units
/// </summary>

public class SoldierControls : PlayerUnit
{

    public override void Start() {
        base.Start();
    }

    /// <summary>
    /// overrides attack method, see base method
    /// </summary>
    public override void attack() {
        attackParticleEffect.SetActive(true);
        base.attack();
    }


}
