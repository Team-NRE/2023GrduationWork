public class Minion : ObjectController
{
    private void Update() 
    {
        UpdateObjectAction();
    }

    protected override void Attack()
    {
        base.Attack();


    }
    protected override void Death()
    {
        base.Death();


    }

    protected override void Move()
    {
        base.Move();


    }

    protected override void Summon()
    {
        base.Summon();


    }

    private void UpdateObjectAction()
    {
        
    }
}
