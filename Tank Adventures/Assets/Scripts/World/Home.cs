namespace World
{
    public class Home : World
    {
        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }

        protected override void OnUpdate()
        { }

        protected override void LevelReached()
        {
            manager.GManager.Events.OnZoneStart?.Invoke();
            manager.GManager.Events.OnFreeZoneReached?.Invoke();
        }
    }
}