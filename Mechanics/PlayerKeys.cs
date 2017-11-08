﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TankProject
{
    class PlayerKeys
    {
        public Keys Forward, Backward, Left, Right, Shoot,
            HatchetOpen, TurretLeft, TurretRight, CannonUp, CannonDown;

        internal PlayerKeys(Keys forward, Keys backward, Keys left, Keys right, Keys shoot,
            Keys hatchetOpen, Keys turretLeft, Keys turretRight, Keys cannonUp, Keys cannonDown)
        {
            this.Forward = forward;
            this.Backward = backward;
            this.Left = left;
            this.Right = right;
            this.Shoot = shoot;
            this.HatchetOpen = hatchetOpen;
            this.TurretLeft = turretLeft;
            this.TurretRight = turretRight;
            this.CannonUp = cannonUp;
            this.CannonDown = cannonDown;
        }
    }
}
