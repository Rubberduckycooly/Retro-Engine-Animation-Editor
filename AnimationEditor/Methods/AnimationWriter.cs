using AnimationEditor.Classes;
using System.Collections.Generic;
using System.Linq;

namespace AnimationEditor.Methods
{
    public static class AnimationWriter
    {
        #region Save (Animation)
        public static void RSDKv5_Save_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKv5.Animation animsetv5 = new RSDKv5.Animation();
            RSDKv5_Save_AnimHeader(BridgeHost, animsetv5);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKv5, animv5);
                animsetv5.Animations.Add(animv5);
            }
            animsetv5.Write(new RSDKv5.Writer(filepath));
        }
        public static void RSDKvB_Save_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKvB.Animation animsetvB = new RSDKvB.Animation();
            RSDKvB_Save_AnimHeader(BridgeHost, animsetvB);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKvB.Animation.AnimationEntry animvB = new RSDKvB.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKvB, animvB);
                animsetvB.Animations.Add(animvB);
            }

            RSDKvB_Save_CollisionBoxes(BridgeHost, animsetvB);

            animsetvB.Write(new RSDKvB.Writer(filepath));
        }
        public static void RSDKv2_Save_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKv2.Animation animsetv2 = new RSDKv2.Animation();
            RSDKv2_Save_AnimHeader(BridgeHost, animsetv2);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKv2.Animation.AnimationEntry animv2 = new RSDKv2.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKv2, animv2);
                animsetv2.Animations.Add(animv2);
            }

            RSDKv2_Save_CollisionBoxes(BridgeHost, animsetv2);

            animsetv2.Write(new RSDKv2.Writer(filepath));
        }
        public static void RSDKv1_Save_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKv1.Animation animsetv1 = new RSDKv1.Animation();
            RSDKv1_Save_AnimHeader(BridgeHost, animsetv1);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKv1.Animation.AnimationEntry animv1 = new RSDKv1.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKv1, animv1);
                animsetv1.Animations.Add(animv1);
            }

            RSDKv1_Save_CollisionBoxes(BridgeHost, animsetv1);

            animsetv1.Write(new RSDKv1.Writer(filepath));
        }
        public static void RSDKvRS_Save_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKvRS.Animation animsetvRS = new RSDKvRS.Animation();
            RSDKvRS_Save_AnimHeader(BridgeHost, animsetvRS);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKvRS.Animation.AnimationEntry animvRS = new RSDKvRS.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKvRS, animvRS);
                animsetvRS.Animations.Add(animvRS);
            }

            animsetvRS.Write(new RSDKvRS.Writer(filepath));
        }

        #endregion

        #region Save (Animation Entry)
        public static void RSDKv5_Save_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKv5.Animation.AnimationEntry animv5)
        {
            animv5.AnimName = animEntry.AnimName;
            animv5.LoopIndex = animEntry.LoopIndex;
            animv5.SpeedMultiplyer = animEntry.SpeedMultiplyer;
            animv5.RotationFlags = animEntry.RotationFlags;

            for (int i = 0; i < animEntry.Frames.Count; i++)
            {

                RSDKv5.Animation.AnimationEntry.Frame frame = new RSDKv5.Animation.AnimationEntry.Frame();
                RSDKv5_Save_Frame(animEntry.Frames[i], frame);
                animv5.Frames.Add(frame);
            }
        }
        public static void RSDKvB_Save_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKvB.Animation.AnimationEntry animvB)
        {
            animvB.AnimName = animEntry.AnimName;
            animvB.LoopIndex = animEntry.LoopIndex;
            animvB.SpeedMultiplyer = (byte)animEntry.SpeedMultiplyer;
            animvB.RotationFlags = animEntry.RotationFlags;

            for (int i = 0; i < animEntry.Frames.Count; i++)
            {

                RSDKvB.Animation.AnimationEntry.Frame frame = new RSDKvB.Animation.AnimationEntry.Frame();
                RSDKvB_Save_Frame(animEntry.Frames[i], frame);
                animvB.Frames.Add(frame);
            }
        }
        public static void RSDKv2_Save_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKv2.Animation.AnimationEntry animv2)
        {
            animv2.AnimName = animEntry.AnimName;
            animv2.LoopIndex = animEntry.LoopIndex;
            animv2.SpeedMultiplyer = (byte)animEntry.SpeedMultiplyer;
            animv2.RotationFlags = animEntry.RotationFlags;

            for (int i = 0; i < animEntry.Frames.Count; i++)
            {
                RSDKv2.Animation.AnimationEntry.Frame frame = new RSDKv2.Animation.AnimationEntry.Frame();
                RSDKv2_Save_Frame(animEntry.Frames[i], frame);
                animv2.Frames.Add(frame);
            }
        }
        public static void RSDKv1_Save_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKv1.Animation.AnimationEntry animv1)
        {
            animv1.LoopIndex = animEntry.LoopIndex;
            animv1.SpeedMultiplyer = (byte)animEntry.SpeedMultiplyer;

            for (int i = 0; i < animEntry.Frames.Count; i++)
            {

                RSDKv1.Animation.AnimationEntry.Frame frame = new RSDKv1.Animation.AnimationEntry.Frame();
                RSDKv1_Save_Frame(animEntry.Frames[i], frame);
                animv1.Frames.Add(frame);
            }
        }
        public static void RSDKvRS_Save_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKvRS.Animation.AnimationEntry animvRS)
        {
            animvRS.LoopIndex = animEntry.LoopIndex;
            animvRS.SpeedMultiplyer = (byte)animEntry.SpeedMultiplyer;

            for (int i = 0; i < animEntry.Frames.Count; i++)
            {

                RSDKvRS.Animation.AnimationEntry.Frame frame = new RSDKvRS.Animation.AnimationEntry.Frame();
                RSDKvRS_Save_Frame(animEntry.Frames[i], frame);
                animvRS.Frames.Add(frame);
            }
        }

        #endregion

        #region Save (Animation Frame)

        public static void RSDKv5_Save_Frame(EditorAnimation.EditorFrame frame, RSDKv5.Animation.AnimationEntry.Frame framev5)
        {
            framev5.CollisionBox = frame.CollisionBox;
            framev5.Delay = frame.Delay;
            framev5.Height = frame.Height;
            framev5.ID = (short)frame.ID;
            framev5.PivotX = frame.PivotX;
            framev5.PivotY = frame.PivotY;
            framev5.SpriteSheet = frame.SpriteSheet;
            framev5.Width = frame.Width;
            framev5.X = frame.X;
            framev5.Y = frame.Y;

            for (int i = 0; i < frame.HitBoxes.Count; i++)
            {
                RSDKv5.Animation.AnimationEntry.Frame.HitBox hb = new RSDKv5.Animation.AnimationEntry.Frame.HitBox();
                RSDKv5_Save_Hitbox(frame.HitBoxes[i], hb);
                framev5.HitBoxes.Add(hb);
            }
        }
        public static void RSDKvB_Save_Frame(EditorAnimation.EditorFrame frame, RSDKvB.Animation.AnimationEntry.Frame framevB)
        {
            framevB.CollisionBox = frame.CollisionBox;
            framevB.Height = (byte)frame.Height;
            framevB.PivotX = (sbyte)frame.PivotX;
            framevB.PivotY = (sbyte)frame.PivotY;
            framevB.SpriteSheet = frame.SpriteSheet;
            framevB.Width = (byte)frame.Width;
            framevB.X = (byte)frame.X;
            framevB.Y = (byte)frame.Y;

        }
        public static void RSDKv2_Save_Frame(EditorAnimation.EditorFrame frame, RSDKv2.Animation.AnimationEntry.Frame framev2)
        {
            framev2.CollisionBox = frame.CollisionBox;
            framev2.Height = (byte)frame.Height;
            framev2.PivotX = (sbyte)frame.PivotX;
            framev2.PivotY = (sbyte)frame.PivotY;
            framev2.SpriteSheet = frame.SpriteSheet;
            framev2.Width = (byte)frame.Width;
            framev2.X = (byte)frame.X;
            framev2.Y = (byte)frame.Y;

        }
        public static void RSDKv1_Save_Frame(EditorAnimation.EditorFrame frame, RSDKv1.Animation.AnimationEntry.Frame framev1)
        {
            framev1.CollisionBox = frame.CollisionBox;
            framev1.Height = (byte)frame.Height;
            framev1.PivotX = (sbyte)frame.PivotX;
            framev1.PivotY = (sbyte)frame.PivotY;
            framev1.SpriteSheet = frame.SpriteSheet;
            framev1.Width = (byte)frame.Width;
            framev1.X = (byte)frame.X;
            framev1.Y = (byte)frame.Y;

        }
        public static void RSDKvRS_Save_Frame(EditorAnimation.EditorFrame frame, RSDKvRS.Animation.AnimationEntry.Frame framevRS)
        {
            framevRS.Height = (byte)frame.Height;
            framevRS.PivotX = (sbyte)frame.PivotX;
            framevRS.PivotY = (sbyte)frame.PivotY;
            framevRS.SpriteSheet = frame.SpriteSheet;
            framevRS.Width = (byte)frame.Width;
            framevRS.X = (byte)frame.X;
            framevRS.Y = (byte)frame.Y;

            RSDKvRS.Animation.AnimationEntry.Frame.HitBox hb = new RSDKvRS.Animation.AnimationEntry.Frame.HitBox();
            RSDKvRS_Save_Hitbox(frame.HitBoxes[0], hb);
            framevRS.CollisionBox = hb;
        }

        #endregion

        #region Save (Animation Hitbox)  (RSDKv5 and RSDKvRS)
        public static void RSDKv5_Save_Hitbox(EditorAnimation.EditorHitbox hb, RSDKv5.Animation.AnimationEntry.Frame.HitBox hbv5)
        {
            hbv5.Bottom = hb.Bottom;
            hbv5.Right = hb.Right;
            hbv5.Top = hb.Top;
            hbv5.Left = hb.Left;
        }
        public static void RSDKvRS_Save_Hitbox(EditorAnimation.EditorHitbox hb, RSDKvRS.Animation.AnimationEntry.Frame.HitBox hbvRS)
        {
            hbvRS.Bottom = (sbyte)hb.Bottom;
            hbvRS.Right = (sbyte)hb.Right;
            hbvRS.Top = (sbyte)hb.Top;
            hbvRS.Left = (sbyte)hb.Left;
        }
        #endregion

        #region Save (Collision Boxes) (RSDKvB and Below)

        public static void RSDKvB_Save_CollisionBoxes(EditorAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            animsetvB.CollisionBoxes = new List<RSDKvB.Animation.sprHitbox>(8);
            for (int a = 0; a < BridgeHost.RetroCollisionBoxes.Count; a++)
            {
                animsetvB.CollisionBoxes.Insert(a, new RSDKvB.Animation.sprHitbox());
                for (int f = 0; f < 8; f++)
                {
                    if (animsetvB.CollisionBoxes[a].Hitboxes == null) animsetvB.CollisionBoxes[a].Hitboxes = new RSDKvB.Animation.sprHitbox.HitboxInfo[8];

                    animsetvB.CollisionBoxes[a].Hitboxes[f].Bottom = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Bottom;
                    animsetvB.CollisionBoxes[a].Hitboxes[f].Top = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Top;
                    animsetvB.CollisionBoxes[a].Hitboxes[f].Left = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Left;
                    animsetvB.CollisionBoxes[a].Hitboxes[f].Right = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Right;
                }
            }
        }
        public static void RSDKv2_Save_CollisionBoxes(EditorAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            animsetv2.CollisionBoxes = new List<RSDKv2.Animation.sprHitbox>(8);
            for (int a = 0; a < BridgeHost.RetroCollisionBoxes.Count; a++)
            {
                animsetv2.CollisionBoxes.Insert(a, new RSDKv2.Animation.sprHitbox());
                for (int f = 0; f < 8; f++)
                {
                    if (animsetv2.CollisionBoxes[a].Hitboxes == null) animsetv2.CollisionBoxes[a].Hitboxes = new RSDKv2.Animation.sprHitbox.HitboxInfo[8];

                    animsetv2.CollisionBoxes[a].Hitboxes[f].Bottom = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Bottom;
                    animsetv2.CollisionBoxes[a].Hitboxes[f].Top = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Top;
                    animsetv2.CollisionBoxes[a].Hitboxes[f].Left = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Left;
                    animsetv2.CollisionBoxes[a].Hitboxes[f].Right = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Right;
                }
            }
        }
        public static void RSDKv1_Save_CollisionBoxes(EditorAnimation BridgeHost, RSDKv1.Animation animsetv1)
        {
            animsetv1.CollisionBoxes = new List<RSDKv1.Animation.sprHitbox>(8);
            for (int a = 0; a < BridgeHost.RetroCollisionBoxes.Count; a++)
            {
                animsetv1.CollisionBoxes.Insert(a, new RSDKv1.Animation.sprHitbox());
                for (int f = 0; f < 8; f++)
                {
                    if (animsetv1.CollisionBoxes[a].Hitboxes == null) animsetv1.CollisionBoxes[a].Hitboxes = new RSDKv1.Animation.sprHitbox.HitboxInfo[8];

                    animsetv1.CollisionBoxes[a].Hitboxes[f].Bottom = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Bottom;
                    animsetv1.CollisionBoxes[a].Hitboxes[f].Top = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Top;
                    animsetv1.CollisionBoxes[a].Hitboxes[f].Left = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Left;
                    animsetv1.CollisionBoxes[a].Hitboxes[f].Right = BridgeHost.RetroCollisionBoxes[a].Hitboxes[f].Right;
                }
            }
        }


        #endregion

        #region Save (Animation Header Properties)

        public static void RSDKv5_Save_AnimHeader(EditorAnimation BridgeHost, RSDKv5.Animation animsetv5)
        {
            animsetv5.SpriteSheets = BridgeHost.SpriteSheets.ToList();
            animsetv5.CollisionBoxes = BridgeHost.CollisionBoxes;
        }
        public static void RSDKvB_Save_AnimHeader(EditorAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            animsetvB.SpriteSheets = BridgeHost.SpriteSheets.ToList();
        }
        public static void RSDKv2_Save_AnimHeader(EditorAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            animsetv2.SpriteSheets = BridgeHost.SpriteSheets.ToList();
        }
        public static void RSDKv1_Save_AnimHeader(EditorAnimation BridgeHost, RSDKv1.Animation animsetv1)
        {
            for (int i = 0; i < BridgeHost.SpriteSheets.Count; i++)
            {
                if (i >= 3) break;
                animsetv1.SpriteSheets[i] = BridgeHost.SpriteSheets[i];
            }
        }
        public static void RSDKvRS_Save_AnimHeader(EditorAnimation BridgeHost, RSDKvRS.Animation animsetvRS)
        {
            animsetvRS.PlayerType = BridgeHost.PlayerType;
            animsetvRS.Unknown = BridgeHost.Unknown;

            for (int i = 0; i < BridgeHost.SpriteSheets.Count; i++)
            {
                if (i >= 3) break;
                animsetvRS.SpriteSheets[i] = BridgeHost.SpriteSheets[i];
            }
        }

        #endregion

        #region Export (Any) (RSDKvU Format)
        public static void RSDKvU_Export_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, string filepath)
        {
            RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry();
            RSDKv5_Save_AnimEntry(animEntry, animv5);
            animv5.Write(new RSDKv5.Writer(filepath));
        }
        public static void RSDKvU_Export_Hitbox(EditorAnimation.EditorHitbox hb, string filepath)
        {
            //TODO: Add Hitbox Import to RSDKv5 Format
            /*
            RSDKv5.Animation.AnimationEntry.Frame.HitBox hbv5 = new RSDKv5.Animation.AnimationEntry.Frame.HitBox();
            RSDKv5_Save_Hitbox(hb, hbv5);
            RSDKv5.Writer writerv5 = new RSDKv5.Writer(filepath);
            hbv5.Write(writerv5);
            writerv5.Close();
            */
        }
        public static void RSDKvU_Export_Frame(EditorAnimation.EditorFrame frame, string filepath)
        {
            RSDKv5.Animation.AnimationEntry.Frame framev5 = new RSDKv5.Animation.AnimationEntry.Frame();
            RSDKv5_Save_Frame(frame, framev5);
            RSDKv5.Writer writerv5 = new RSDKv5.Writer(filepath);
            framev5.Write(writerv5);
            writerv5.Close();
        }

        #endregion

    }
}
