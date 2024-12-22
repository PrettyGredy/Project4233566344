//Author: Small Hedge Games
//Date: 05/04/2024

namespace SHG.AnimatorCoder
{
    /// <summary> Complete list of all animation state names </summary>
    public enum Animations
    {
        //Change the list below to your animation state names
        Idle,
        AttackX1,
        AttackX2,
        AttackX3,
        AttackX4,
        Run,
        Roll,
        Die,
        SpAttack1,
        SpAttack2,
        SpAttack3,
        RESET  //Keep Reset
    }

    /// <summary> Complete list of all animator parameters </summary>
    public enum Parameters
    {
        //Change the list below to your animator parameters
        IsEndState,
        GROUNDED,
        FALLING
    }
}


