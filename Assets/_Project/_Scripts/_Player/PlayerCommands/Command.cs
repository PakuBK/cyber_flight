namespace CF.Player {
public abstract class Command
{
    public bool isExecuted = false;

    public abstract void Execute();

}
}
