public class Pigeon : Enemy
{
    protected override void Initialization()
    {
        base.Initialization();

        _enemySpeed = 2.0f;
    }
}
