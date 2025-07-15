using UnityEngine;

namespace CF.Player {
public class MoveCommand : Command
{
    public Vector2 playerInput;

    public MoveCommand(Vector2 _input) 
    {
        playerInput = _input;
    }    

    public override void Execute()
    {
        
    }
}
}
