using AnimationEditor.Classes;
using System.Collections.Generic;
using System.Linq;

namespace AnimationEditor.Methods
{
    public static class ImportExportHandler
    {
        #region Load (Animation)

        public static void RSDKv5_Load_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKv5.Animation animsetv5 = new RSDKv5.Animation(new RSDKv5.Reader(filepath));
            RSDKv5_Load_AnimHeader(BridgeHost, animsetv5);

            for (int a = 0; a < animsetv5.Animations.Count; a++)
            {
                var animset = new EditorAnimation.EditorAnimationInfo(EngineType.RSDKv5, BridgeHost);
                animset.LoadFrom(EngineType.RSDKv5, animsetv5.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }
        public static void RSDKvB_Load_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKvB.Animation animsetvB = new RSDKvB.Animation(new RSDKvB.Reader(filepath));
            RSDKvB_Load_AnimHeader(BridgeHost, animsetvB);
            RSDKvB_Load_CollisionBoxes(BridgeHost, animsetvB);

            for (int a = 0; a < animsetvB.Animations.Count; a++)
            {
                var animset = new EditorAnimation.EditorAnimationInfo(EngineType.RSDKvB, BridgeHost);
                animset.LoadFrom(EngineType.RSDKvB, animsetvB.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }
        public static void RSDKv2_Load_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKv2.Animation animsetv2 = new RSDKv2.Animation(new RSDKv2.Reader(filepath));
            RSDKv2_Load_AnimHeader(BridgeHost, animsetv2);
            RSDKv2_Load_CollisionBoxes(BridgeHost, animsetv2);

            for (int a = 0; a < animsetv2.Animations.Count; a++)
            {
                var animset = new EditorAnimation.EditorAnimationInfo(EngineType.RSDKv2, BridgeHost);
                animset.LoadFrom(EngineType.RSDKv2, animsetv2.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }
        public static void RSDKv1_Load_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKv1.Animation animsetv1 = new RSDKv1.Animation(new RSDKv1.Reader(filepath));
            RSDKv1_Load_AnimHeader(BridgeHost, animsetv1);
            RSDKv1_Load_CollisionBoxes(BridgeHost, animsetv1);

            for (int a = 0; a < animsetv1.Animations.Count; a++)
            {
                var animset = new EditorAnimation.EditorAnimationInfo(EngineType.RSDKv1, BridgeHost);
                animset.LoadFrom(EngineType.RSDKv1, animsetv1.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }
        public static void RSDKvRS_Load_Animation(EditorAnimation BridgeHost, string filepath)
        {
            RSDKvRS.Animation animsetvRS = new RSDKvRS.Animation(new RSDKvRS.Reader(filepath));
            RSDKvRS_Load_AnimHeader(BridgeHost, animsetvRS);
            RSDKvRS_Load_CollisionBoxes(BridgeHost, animsetvRS);

            for (int a = 0; a < animsetvRS.Animations.Count; a++)
            {
                var animset = new EditorAnimation.EditorAnimationInfo(EngineType.RSDKvRS, BridgeHost);
                animset.LoadFrom(EngineType.RSDKvRS, animsetvRS.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }

        #endregion
            
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


        #region Load (Animation Entry)
        public static void RSDKv5_Load_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKv5.Animation.AnimationEntry animv5)
        {
            animEntry.AnimName = animv5.AnimName;
            animEntry.LoopIndex = animv5.LoopIndex;
            animEntry.SpeedMultiplyer = animv5.SpeedMultiplyer;
            animEntry.RotationFlags = animv5.RotationFlags;

            for (int i = 0; i < animv5.Frames.Count; i++)
            {
                EditorAnimation.EditorFrame frame = new EditorAnimation.EditorFrame(EngineType.RSDKv5, animEntry);
                RSDKv5_Load_Frame(frame, animv5.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }
        public static void RSDKvB_Load_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKvB.Animation.AnimationEntry animvB)
        {
            animEntry.AnimName = animvB.AnimName;
            animEntry.LoopIndex = animvB.LoopIndex;
            animEntry.SpeedMultiplyer = animvB.SpeedMultiplyer;
            animEntry.RotationFlags = animvB.RotationFlags;

            for (int i = 0; i < animvB.Frames.Count; i++)
            {
                EditorAnimation.EditorFrame frame = new EditorAnimation.EditorFrame(EngineType.RSDKvB, animEntry);
                RSDKvB_Load_Frame(frame, animvB.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }
        public static void RSDKv2_Load_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKv2.Animation.AnimationEntry animv2)
        {
            animEntry.AnimName = animv2.AnimName;
            animEntry.LoopIndex = animv2.LoopIndex;
            animEntry.SpeedMultiplyer = animv2.SpeedMultiplyer;
            animEntry.RotationFlags = animv2.RotationFlags;

            for (int i = 0; i < animv2.Frames.Count; i++)
            {
                EditorAnimation.EditorFrame frame = new EditorAnimation.EditorFrame(EngineType.RSDKv2, animEntry);
                RSDKv2_Load_Frame(frame, animv2.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }
        public static void RSDKv1_Load_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKv1.Animation.AnimationEntry animv1)
        {
            animEntry.AnimName = animv1.AnimationName;
            animEntry.LoopIndex = animv1.LoopIndex;
            animEntry.SpeedMultiplyer = animv1.SpeedMultiplyer;
            animEntry.RotationFlags = 0;

            for (int i = 0; i < animv1.Frames.Count; i++)
            {
                EditorAnimation.EditorFrame frame = new EditorAnimation.EditorFrame(EngineType.RSDKv1, animEntry);
                RSDKv1_Load_Frame(frame, animv1.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }
        public static void RSDKvRS_Load_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, RSDKvRS.Animation.AnimationEntry animvRS)
        {
            animEntry.AnimName = animEntry.AnimName;
            animEntry.LoopIndex = animvRS.LoopIndex;
            animEntry.SpeedMultiplyer = animvRS.SpeedMultiplyer;
            animEntry.RotationFlags = 0;

            for (int i = 0; i < animvRS.Frames.Count; i++)
            {
                EditorAnimation.EditorFrame frame = new EditorAnimation.EditorFrame(EngineType.RSDKvRS, animEntry);
                RSDKvRS_Load_Frame(frame, animvRS.Frames[i]);
                animEntry.Frames.Add(frame);
            }
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


        #region Load (Animation Frame)

        public static void RSDKv5_Load_Frame(EditorAnimation.EditorFrame frame, RSDKv5.Animation.AnimationEntry.Frame framev5)
        {
            frame.engineType = EngineType.RSDKv5;
            frame.CollisionBox = framev5.CollisionBox;
            frame.Delay = framev5.Delay;
            frame.Height = framev5.Height;
            frame.ID = (ushort)framev5.ID;
            frame.PivotX = framev5.PivotX;
            frame.PivotY = framev5.PivotY;
            frame.SpriteSheet = framev5.SpriteSheet;
            frame.Width = framev5.Width;
            frame.X = framev5.X;
            frame.Y = framev5.Y;
            frame.HitBoxes = new List<EditorAnimation.EditorHitbox>();

            for (int i = 0; i < framev5.HitBoxes.Count; i++)
            {
                EditorAnimation.EditorHitbox hb = new EditorAnimation.EditorHitbox();
                RSDKv5_Load_Hitbox(hb, framev5.HitBoxes[i]);
                frame.HitBoxes.Add(hb);
            }
        }
        public static void RSDKvB_Load_Frame(EditorAnimation.EditorFrame frame, RSDKvB.Animation.AnimationEntry.Frame framevB)
        {
            frame.engineType = EngineType.RSDKvB;
            frame.Delay = framevB.Delay;
            frame.CollisionBox = framevB.CollisionBox;
            frame.Height = framevB.Height;
            frame.PivotX = framevB.PivotX;
            frame.PivotY = framevB.PivotY;
            frame.SpriteSheet = framevB.SpriteSheet;
            frame.Width = framevB.Width;
            frame.X = framevB.X;
            frame.Y = framevB.Y;
        }
        public static void RSDKv2_Load_Frame(EditorAnimation.EditorFrame frame, RSDKv2.Animation.AnimationEntry.Frame framev2)
        {
            frame.engineType = EngineType.RSDKv2;
            frame.Delay = framev2.Delay;
            frame.CollisionBox = framev2.CollisionBox;
            frame.Height = framev2.Height;
            frame.PivotX = framev2.PivotX;
            frame.PivotY = framev2.PivotY;
            frame.SpriteSheet = framev2.SpriteSheet;
            frame.Width = framev2.Width;
            frame.X = framev2.X;
            frame.Y = framev2.Y;
        }
        public static void RSDKv1_Load_Frame(EditorAnimation.EditorFrame frame, RSDKv1.Animation.AnimationEntry.Frame framev1)
        {
            frame.engineType = EngineType.RSDKv1;
            frame.Delay = framev1.Delay;
            frame.CollisionBox = framev1.CollisionBox;
            frame.Height = framev1.Height;
            frame.PivotX = framev1.PivotX;
            frame.PivotY = framev1.PivotY;
            frame.SpriteSheet = framev1.SpriteSheet;
            frame.Width = framev1.Width;
            frame.X = framev1.X;
            frame.Y = framev1.Y;
        }
        public static void RSDKvRS_Load_Frame(EditorAnimation.EditorFrame frame, RSDKvRS.Animation.AnimationEntry.Frame framevRS)
        {
            frame.engineType = EngineType.RSDKvRS;
            frame.Delay = framevRS.Delay;
            frame.Height = framevRS.Height;
            frame.PivotX = framevRS.PivotX;
            frame.PivotY = framevRS.PivotY;
            frame.SpriteSheet = framevRS.SpriteSheet;
            frame.Width = framevRS.Width;
            frame.X = framevRS.X;
            frame.Y = framevRS.Y;

            EditorAnimation.EditorHitbox hb = new EditorAnimation.EditorHitbox();
            RSDKvRS_Load_Hitbox(hb, framevRS.CollisionBox);
            frame.HitBoxes.Add(hb);
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


        #region Load (Animation Hitbox) (RSDKv5 and RSDKvRS)

        public static void RSDKv5_Load_Hitbox(EditorAnimation.EditorHitbox hb, RSDKv5.Animation.AnimationEntry.Frame.HitBox hbV5)
        {
            hb.Bottom = hbV5.Bottom;
            hb.Right = hbV5.Right;
            hb.Top = hbV5.Top;
            hb.Left = hbV5.Left;
        }

        public static void RSDKvRS_Load_Hitbox(EditorAnimation.EditorHitbox hb, RSDKvRS.Animation.AnimationEntry.Frame.HitBox hbvRS)
        {
            hb.Bottom = hbvRS.Bottom;
            hb.Right = hbvRS.Right;
            hb.Top = hbvRS.Top;
            hb.Left = hbvRS.Left;
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


        #region Load (Collision Boxes) (RSDKvB and Below)

        public static void RSDKvB_Load_CollisionBoxes(EditorAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            var collisionBoxes = new List<EditorAnimation.EditorRetroHitBox>();
            for (int i = 0; i < animsetvB.CollisionBoxes.Count; i++)
            {
                var hitboxEntry = new EditorAnimation.EditorRetroHitBox();
                for (int j = 0; j < animsetvB.CollisionBoxes[i].Hitboxes.Length; j++)
                {
                    hitboxEntry.Hitboxes[j].Bottom = animsetvB.CollisionBoxes[i].Hitboxes[j].Bottom;
                    hitboxEntry.Hitboxes[j].Top = animsetvB.CollisionBoxes[i].Hitboxes[j].Top;
                    hitboxEntry.Hitboxes[j].Left = animsetvB.CollisionBoxes[i].Hitboxes[j].Left;
                    hitboxEntry.Hitboxes[j].Right = animsetvB.CollisionBoxes[i].Hitboxes[j].Right;
                }
                collisionBoxes.Add(hitboxEntry);
            }
            BridgeHost.RetroCollisionBoxes = collisionBoxes;
        }
        public static void RSDKv2_Load_CollisionBoxes(EditorAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            var collisionBoxes = new List<EditorAnimation.EditorRetroHitBox>();
            for (int i = 0; i < animsetv2.CollisionBoxes.Count; i++)
            {
                var hitboxEntry = new EditorAnimation.EditorRetroHitBox();
                for (int j = 0; j < animsetv2.CollisionBoxes[i].Hitboxes.Length; j++)
                {
                    hitboxEntry.Hitboxes[j].Bottom = animsetv2.CollisionBoxes[i].Hitboxes[j].Bottom;
                    hitboxEntry.Hitboxes[j].Top = animsetv2.CollisionBoxes[i].Hitboxes[j].Top;
                    hitboxEntry.Hitboxes[j].Left = animsetv2.CollisionBoxes[i].Hitboxes[j].Left;
                    hitboxEntry.Hitboxes[j].Right = animsetv2.CollisionBoxes[i].Hitboxes[j].Right;
                }
                collisionBoxes.Add(hitboxEntry);
            }
            BridgeHost.RetroCollisionBoxes = collisionBoxes;
        }
        public static void RSDKv1_Load_CollisionBoxes(EditorAnimation BridgeHost, RSDKv1.Animation animsetv1)
        {
            var collisionBoxes = new List<EditorAnimation.EditorRetroHitBox>();
            for (int i = 0; i < animsetv1.CollisionBoxes.Count; i++)
            {
                var hitboxEntry = new EditorAnimation.EditorRetroHitBox();
                for (int j = 0; j < animsetv1.CollisionBoxes[i].Hitboxes.Length; j++)
                {
                    hitboxEntry.Hitboxes[j].Bottom = animsetv1.CollisionBoxes[i].Hitboxes[j].Bottom;
                    hitboxEntry.Hitboxes[j].Top = animsetv1.CollisionBoxes[i].Hitboxes[j].Top;
                    hitboxEntry.Hitboxes[j].Left = animsetv1.CollisionBoxes[i].Hitboxes[j].Left;
                    hitboxEntry.Hitboxes[j].Right = animsetv1.CollisionBoxes[i].Hitboxes[j].Right;
                }
                collisionBoxes.Add(hitboxEntry);
            }
            BridgeHost.RetroCollisionBoxes = collisionBoxes;
        }
        public static void RSDKvRS_Load_CollisionBoxes(EditorAnimation BridgeHost, RSDKvRS.Animation animsetvRS)
        {

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


        #region Load (Animation Header Properties)

        private static List<string> GetV3HitboxList()
        {
            return new List<string>() 
            {
                "North - N",
                "Northeast - NE",
                "East - E",
                "Southeast - SE",
                "South - S",
                "Southwest - SW",
                "West - W",
                "Northwest - NW"
            };
        }

        private static List<string> GetOldV3HitboxList()
        {
            var output = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                output.Add(string.Format("Hitbox #{0}", i));
            }
            return output;
        }

        public static void RSDKv5_Load_AnimHeader(EditorAnimation BridgeHost, RSDKv5.Animation animsetv5)
        {
            BridgeHost.SpriteSheets = animsetv5.SpriteSheets;
            BridgeHost.TotalFrameCount = animsetv5.TotalFrameCount;
            BridgeHost.CollisionBoxes = animsetv5.CollisionBoxes;
        }
        public static void RSDKvB_Load_AnimHeader(EditorAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            BridgeHost.SpriteSheets = animsetvB.SpriteSheets;
            BridgeHost.CollisionBoxes = GetV3HitboxList();
        }
        public static void RSDKv2_Load_AnimHeader(EditorAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            BridgeHost.SpriteSheets = animsetv2.SpriteSheets;
            BridgeHost.CollisionBoxes = GetV3HitboxList();
        }
        public static void RSDKv1_Load_AnimHeader(EditorAnimation BridgeHost, RSDKv1.Animation animsetv1)
        {
            for (int i = 0; i < animsetv1.SpriteSheets.Length; i++)
            {
                BridgeHost.SpriteSheets.Add(animsetv1.SpriteSheets[i]);
            }
            BridgeHost.CollisionBoxes = GetV3HitboxList();
        }
        public static void RSDKvRS_Load_AnimHeader(EditorAnimation BridgeHost, RSDKvRS.Animation animsetvRS)
        {
            BridgeHost.PlayerType = animsetvRS.PlayerType;
            BridgeHost.Unknown = animsetvRS.Unknown;

            for (int i = 0; i < animsetvRS.SpriteSheets.Length; i++)
            {
                BridgeHost.SpriteSheets.Add(animsetvRS.SpriteSheets[i]);
            }

            BridgeHost.CollisionBoxes.Add("Hitbox");
        }

        #endregion

        #region Save (Animation Header Properties)

        public static void RSDKv5_Save_AnimHeader(EditorAnimation BridgeHost, RSDKv5.Animation animsetv5)
        {
            animsetv5.SpriteSheets = BridgeHost.SpriteSheets;
            animsetv5.CollisionBoxes = BridgeHost.CollisionBoxes;
        }
        public static void RSDKvB_Save_AnimHeader(EditorAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            animsetvB.SpriteSheets = BridgeHost.SpriteSheets;
        }
        public static void RSDKv2_Save_AnimHeader(EditorAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            animsetv2.SpriteSheets = BridgeHost.SpriteSheets;
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


        #region Import/Export
        public static void RSDKvU_Import_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, string filepath)
        {
            RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry(new RSDKv5.Reader(filepath));
            RSDKv5_Load_AnimEntry(animEntry, animv5);
        }
        public static void RSDKvU_Export_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, string filepath)
        {
            RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry();
            RSDKv5_Save_AnimEntry(animEntry, animv5);
            animv5.Write(new RSDKv5.Writer(filepath));
        }
        public static void RSDKvU_Import_Hitbox(EditorAnimation.EditorHitbox hb, string filepath)
        {
            //TODO: Add Hitbox Import to RSDKv5 Format
            /*
            RSDKv5.Reader readerv5 = new RSDKv5.Reader(filepath);
            RSDKv5.Animation.AnimationEntry.Frame.HitBox hbv5 = new RSDKv5.Animation.AnimationEntry.Frame.HitBox(readerv5);
            readerv5.Close();
            RSDKv5_Load_Hitbox(hb, hbv5);
            */
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
        public static void RSDKvU_Import_Frame(EditorAnimation.EditorFrame frame, string filepath)
        {
            RSDKv5.Reader readerv5 = new RSDKv5.Reader(filepath);
            RSDKv5.Animation.AnimationEntry.Frame framev5 = new RSDKv5.Animation.AnimationEntry.Frame(readerv5);
            readerv5.Close();
            RSDKv5_Load_Frame(frame, framev5);
        }

        #endregion

    }
}
