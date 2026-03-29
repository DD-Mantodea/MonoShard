using Mantodea.Contents.Animations;
using Microsoft.Xna.Framework;
using MonoShard.Contents.GameObjects.TileMapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.Animations
{
    public class EntityMoveChain(Entity target) : AnimationChain<Entity>(target)
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_animationIndex == Animations.Count)
            {
                MaxTime = 0;

                Target.IsMove = false;

                Target.PathIndex = -1;

                return;
            }

            var animation = Animations[_animationIndex];

            if (!animation.Initialized)
            {
                Target.PathIndex = _animationIndex;

                Target.LastTilePos = (animation as EntityMoveAnimation).StartPos;
                
                Target.TargetTilePos = (animation as EntityMoveAnimation).EndPos;
            }

            if (!animation.Initialized)
                animation.Initialize();

            animation.Update(gameTime);

            if (animation.MaxTime == 0 && TurnController.PlayerTurn)
            {
                if (ShouldBreak)
                {
                    MaxTime = 0;

                    Target.IsMove = false;

                    Target.PathIndex = -1;

                    return;
                }

                _animationIndex++;
            }

            Target.IsMove = true;
        }
    }
}
