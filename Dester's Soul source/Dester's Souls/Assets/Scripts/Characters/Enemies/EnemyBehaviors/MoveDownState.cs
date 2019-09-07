using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class MoveDownState : ZombieMoveState
    {
        public override void ZombieMove(Zombie context)
        {
            if (context.DirectionHorizontal)
            {
                if (!context.MoveDirection(Character.Directions.left))
                {
                    context.MovementState = new MoveUpState();
                }
            }
            else
            {
                if (!context.MoveDirection(Character.Directions.down))
                {
                    context.MovementState = new MoveUpState();
                }
            }
        }
    }

