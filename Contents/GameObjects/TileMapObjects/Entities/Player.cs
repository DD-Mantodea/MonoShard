using Mantodea.Contents;
using Mantodea.Contents.Animations;
using Mantodea.Contents.Extensions;
using Mantodea.Contents.UI.Components;
using Mantodea.Contents.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoShard.Assets;
using MonoShard.Contents.Animations;
using MonoShard.Contents.Extensions;
using MonoShard.Contents.Graphics;
using MonoShard.Contents.Logic;
using MonoShard.Contents.Rooms;
using System;

namespace MonoShard.Contents.GameObjects.TileMapObjects.Entities
{
    public abstract class Player : Entity
    {
        public Player(Room room, string head) : base(room)
        {
            Timer = new();

            CurrentPath = [];

            Direction = -1;

            _offsets = [new(-1, -1), new(-1, 0), new(0, 1), new(1, 0), new(1, -1), new(1, 0), new(0, 1), new(-1, 0)];

            SetHead(head);

            Shadow = TextureManager.GetSprite("s_shadow_small");

            Target = TextureManager.GetSprite("s_highlight").RemoveOffset();

            WayPoint = TextureManager.GetSprite("s_waypoint");
        }

        public bool DrawSelectBox = true;

        public bool DrawPath = true;

        public static Player LocalPlayer;

        public Timer Timer;

        private readonly Vector2[] _offsets;

        private Vector2 HeadOffset;

        private Vector2 BodyOffset;

        private StoneShardSprite HeadNormal;

        private StoneShardSprite HelmetHeadNormal;

        private StoneShardSprite HeadBlood;

        private StoneShardSprite HelmetHeadBlood;

        private StoneShardSprite Body;

        private StoneShardSprite Shadow;

        private StoneShardSprite Target;

        private StoneShardSprite WayPoint;

        public override int Width => 40;

        public override int Height => 80 / 3;

        public override Vector2 Position => base.Position + new Vector2(StoneShard.TileSize) / 2;

        public override Rectangle Rectangle => RectangleUtils.FromVector2(Position - new Vector2(StoneShard.TileSize) / 2, new(Width, Height));

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Shadow, Position.Add(-2, 8), Color.Black * 0.4f, layerDepth: 1 - TilePosition.Y / 1000 + 0.0003f);

            spriteBatch.DrawRectangle(Rectangle, Color.Red, layerDepth: 1 - TilePosition.Y / 1000 + 0.0003f);

            spriteBatch.Draw(Body, Position.Sub(6 + 2 * Direction, 2) + DrawOffset, Color.White,
                Rotation, Vector2.Zero, Vector2.One, Direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1 - TilePosition.Y / 1000 + 0.0002f);

            spriteBatch.Draw(HeadNormal, Position.Sub(3 - Direction, 2) + DrawOffset, Color.White,
                Rotation, Vector2.Zero, Vector2.One, Direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1 - TilePosition.Y / 1000 + 0.0001f);
        
            if (DrawSelectBox)
            {
                if (Room.Reachable(Room.MouseTile))
                    Target.Frame = 0;
                else
                    Target.Frame = 1;

                spriteBatch.Draw(Target, (Room.MouseTile * 2 / 3 + Room.PreviousMouseTile / 3 + Room.TilePosition) * StoneShard.TileSize - new Vector2(2, 0), Color.White, layerDepth: 0);
            }

            if (DrawPath)
            {
                foreach (var item in CurrentPath)
                {
                    var scale = new Vector2((float)(0.7 + Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 200 + CurrentPath.IndexOf(item)) / 6));

                    var offset = new Vector2(StoneShard.TileSize - 10 * scale.X) / 2;

                    var waypointPos = item + Room.TilePosition;

                    if (CurrentPath.IndexOf(item) == 0)
                    {
                        if (Vector2.Distance(item, TargetTilePos) == 1)
                        {
                            spriteBatch.Draw(WayPoint, waypointPos * StoneShard.TileSize + offset + new Vector2(2, 2), Color.Black, scale: scale, layerDepth: 0.001f);
                            spriteBatch.Draw(WayPoint, waypointPos * StoneShard.TileSize + offset, Color.White, 0, Vector2.Zero, scale: scale, layerDepth: 0);
                        }
                        else
                        {
                            spriteBatch.Draw(WayPoint, waypointPos * StoneShard.TileSize + offset + new Vector2(2, 2), Color.Black, scale: scale, layerDepth: 0.001f);
                            spriteBatch.Draw(WayPoint, (waypointPos + (TargetTilePos - item) / 2) * StoneShard.TileSize + offset + new Vector2(2, 2), Color.Black, scale: scale, layerDepth: 0.001f);
                            spriteBatch.Draw(WayPoint, waypointPos * StoneShard.TileSize + offset, Color.White, scale: scale, layerDepth: 0);
                            spriteBatch.Draw(WayPoint, (waypointPos + (TargetTilePos - item) / 2) * StoneShard.TileSize + offset, Color.White, scale: scale, layerDepth: 0);
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(item, CurrentPath[CurrentPath.IndexOf(item) - 1]) == 1)
                        {
                            spriteBatch.Draw(WayPoint, waypointPos * StoneShard.TileSize + offset + new Vector2(2, 2), Color.Black, scale: scale, layerDepth: 0.001f);
                            spriteBatch.Draw(WayPoint, waypointPos * StoneShard.TileSize + offset, Color.White, scale: scale, layerDepth: 0);
                        }
                        else
                        {
                            spriteBatch.Draw(WayPoint, waypointPos * StoneShard.TileSize + offset + new Vector2(2, 2), Color.Black, scale: scale, layerDepth: 0.001f);
                            spriteBatch.Draw(WayPoint, (waypointPos + (CurrentPath[CurrentPath.IndexOf(item) - 1] - item) / 2) * StoneShard.TileSize + offset + new Vector2(2, 2), Color.Black, scale: scale, layerDepth: 0.001f);
                            spriteBatch.Draw(WayPoint, waypointPos * StoneShard.TileSize + offset, Color.White, 0, Vector2.Zero, scale: scale, layerDepth: 0);
                            spriteBatch.Draw(WayPoint, (waypointPos + (CurrentPath[CurrentPath.IndexOf(item) - 1] - item) / 2) * StoneShard.TileSize + offset, Color.White, scale: scale, layerDepth: 0);
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateSprite(gameTime);

            var wayfinder = new WayFinder(Room.Reachable, TilePosition, Room.MouseTile, Room);

            if (!IsMove)
            {
                if (Room.Reachable(Room.MouseTile))
                    CurrentPath = wayfinder.FindPath();
                else 
                    CurrentPath?.Clear();
            }

            CurrentAnimation?.Update(gameTime);
        }

        public void UpdateSprite(GameTime gameTime)
        {
            Timer[0]++; //Timer0 Timer1控制眨眼 Timer2 控制身体状态 Timer3 Timer4控制待机移动

            if (!IsMove) Timer[3]++;

            if (Timer[0] == 20 && Timer[1] == 1)
            {
                Timer[1] = 0;
                Timer[0] = 0;
            }
            else if (Timer[0] == 210)
            {
                Timer[1] = 1;
                Timer[0] = 0;
            }

            if (Timer[3] == 10)
            {
                if (Timer[4] < 7) Timer[4]++;
                if (Timer[4] == 7) Timer[4] = 0;
                DrawOffset = _offsets[Timer[4]];
                Timer[3] = 0;
            }
        }

        public void SetHead(string head)
        {
            HeadNormal = TextureManager.GetSprite($"{head}_normal");

            HelmetHeadNormal = TextureManager.GetSprite($"{head}_helmet_normal");

            HeadBlood = TextureManager.GetSprite($"{head}_blood");

            HelmetHeadBlood = TextureManager.GetSprite($"{head}_helmet_blood");
        }

        public void SetBody(string body)
        {
            Body = TextureManager.GetSprite(body);
        }

        public void SetHeadOffset(Vector2 offset)
        {
            HeadOffset = offset;
        }

        public void SetBodyOffset(Vector2 offset)
        {
            BodyOffset = offset;
        }

        public bool PlayAnimation(Animation<Entity> animation)
        {
            if (CurrentAnimation != null && CurrentAnimation.Time != 0)
                return false;

            CurrentAnimation = animation;

            return true;
        }

        public void GoToRoom(Room room, Vector2 pos)
        {
            Room = room;

            SetPos(pos);
        }

        #region Attributes

        /// <summary>
        /// 主手伤害
        /// </summary>
        public GameValue<int> MainHandDamage;

        /// <summary>
        /// 副手伤害
        /// </summary>
        public GameValue<int> OffHandDamage;

        public GameValue<int> BonusRange;

        public GameValue<int> Health;

        public GameValue<int> MaxHealth;

        public GameValue<int> Energy;

        public GameValue<int> MaxEnergy;

        public GameValue<int> Protection;

        public GameValue<int> BlockPower;

        public GameValue<int> MaxBlockPower;

        public GameValue<int> Vision;

        public GameValue<float> WeaponDamage;

        public GameValue<float> MainHandEfficiency;

        public GameValue<float> OffHandEfficiency;

        public GameValue<float> BodypartDamage;

        public GameValue<float> ArmorDamage;

        public GameValue<float> ArmorPenetration;

        public GameValue<float> Accuracy;

        public GameValue<float> CritChance;

        public GameValue<float> CritEfficiency;

        public GameValue<float> CounterChance;

        public GameValue<float> FumbleChance;

        public GameValue<float> SkillsEnergyCost;

        public GameValue<float> SpellsEnergyCost;

        public GameValue<float> CooldownsDuration;

        public GameValue<float> BleedChance;

        public GameValue<float> DazeChance;

        public GameValue<float> StunChance;

        public GameValue<float> KnockbackChance;

        public GameValue<float> ImmobilizationChance;

        public GameValue<float> StaggerChance;

        public GameValue<float> LifeDrain;

        public GameValue<float> EnergyDrain;

        public GameValue<float> ExperienceGain;

        public GameValue<float> HealthRestoration;

        public GameValue<float> HealingEfficiency;

        public GameValue<float> EnergyRestoration;

        public GameValue<float> BlockChance;

        public GameValue<float> BlockPowerRecovery;

        public GameValue<float> DodgeChance;

        public GameValue<float> Stealth;

        public GameValue<float> NoiseProduced;

        public GameValue<float> Lockpicking_Disarming;

        public GameValue<float> Fortitude;

        public GameValue<float> DamageReflection;

        public GameValue<float> BleedResistance;

        public GameValue<float> ControlResistance;

        public GameValue<float> MoveResistance;

        public GameValue<float> HungerResistance;

        public GameValue<float> IntoxicationResistance;

        public GameValue<float> PainResistance;

        public GameValue<float> FatigueResistance;

        public GameValue<float> DamageTaken;

        public GameValue<float> PhysicalResistance;

        public GameValue<float> NatureResistance;

        public GameValue<float> MagicResistance;

        public GameValue<float> SlashingResistance;

        public GameValue<float> PiercingResistance;

        public GameValue<float> CrushingResistance;

        public GameValue<float> RendingResistance;

        public GameValue<float> FireResistance;

        public GameValue<float> PoisonResistance;

        public GameValue<float> FrostResistance;

        public GameValue<float> ShockResistance;

        public GameValue<float> CausticResistace;

        public GameValue<float> ArcaneResistace;

        public GameValue<float> SacredResistance;

        public GameValue<float> UnholyResistance;

        public GameValue<float> PsionicResistance;

        public GameValue<float> MagicPower;

        public GameValue<float> MiracleChance;

        public GameValue<float> MiraclePotency;

        public GameValue<float> BackfireChance;

        public GameValue<float> BackfireDamage;

        public GameValue<float> PyromanticPower;

        public GameValue<float> GeomanticPower;

        public GameValue<float> VenomanticPower;

        public GameValue<float> CryomanticPower;

        public GameValue<float> ElectromanticPower;

        public GameValue<float> ArcanisticPower;

        public GameValue<float> AstromanticPower;

        public GameValue<float> PsionicPower;

        public GameValue<float> ChronomanticPower;

        public GameValue<bool> HurtHeavily;

        #endregion
    }
}
