using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// People on the town on Combat Mode will only listen to their 
/// Militar Brain... 
/// 
/// Steps:
/// 1 - Get out of building if in one, and it sets it self in Idle
/// 2 - If engage on fight by an enenmy or enemy in proximity, will fight back 
/// 3 - When enemies are all death will go back to house.
/// 4 - When in house, will reset Brain state
/// 5 - Will not listen to MilitarBrain anymore
/// 
/// The person will be listening to user commands once in ready to fight, so will go where directed and will fight 
/// if enemy in proximity
public class CombatBrain : MilitarBrain
{

    public void Update()
    {
        base.Update();
    }
}

