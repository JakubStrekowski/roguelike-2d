using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class MoveUpState : ZombieMoveState
    {
        public override void ZombieMove(Zombie context)
        {
            if (context.DirectionHorizontal)
            {
                if (!context.MoveDirection(Character.Directions.right))
                {
                    context.MovementState = new MoveDownState();
                }
            }
            else
            {
                if (!context.MoveDirection(Character.Directions.up))
                {
                    context.MovementState = new MoveDownState();
                }
            }
        }
    }

