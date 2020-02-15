using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimationEditor.Animation.Classes;

namespace AnimationEditor.Animation.Methods
{
    public static class ImportExportHandler
    {
        #region Load (Animation)

        public static void RSDKv5_Load_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKv5.Animation animsetv5 = new RSDKv5.Animation(new RSDKv5.Reader(filepath));

            RSDKv5_Load_AnimHeader(BridgeHost, animsetv5);
            RSDKv5_Load_CollisionBoxes(BridgeHost, animsetv5);

            for (int a = 0; a < animsetv5.Animations.Count; a++)
            {
                var animset = new BridgedAnimation.BridgedAnimationEntry(EngineType.RSDKv5);
                animset.LoadFrom(EngineType.RSDKv5, animsetv5.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }
        public static void RSDKvB_Load_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKvB.Animation animsetvB = new RSDKvB.Animation(new RSDKvB.Reader(filepath));
            RSDKvB_Load_AnimHeader(BridgeHost, animsetvB);
            RSDKvB_Load_CollisionBoxes(BridgeHost, animsetvB);

            for (int a = 0; a < animsetvB.Animations.Count; a++)
            {
                var animset = new BridgedAnimation.BridgedAnimationEntry(EngineType.RSDKvB);
                animset.LoadFrom(EngineType.RSDKvB, animsetvB.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }
        public static void RSDKv2_Load_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKv2.Animation animsetv2 = new RSDKv2.Animation(new RSDKv2.Reader(filepath));
            RSDKv2_Load_AnimHeader(BridgeHost, animsetv2);
            RSDKv2_Load_CollisionBoxes(BridgeHost, animsetv2);

            for (int a = 0; a < animsetv2.Animations.Count; a++)
            {
                var animset = new BridgedAnimation.BridgedAnimationEntry(EngineType.RSDKv2);
                animset.LoadFrom(EngineType.RSDKv2, animsetv2.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }
        public static void RSDKv1_Load_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKv1.Animation animsetv1 = new RSDKv1.Animation(new RSDKv1.Reader(filepath));
            RSDKv1_Load_AnimHeader(BridgeHost, animsetv1);
            RSDKv1_Load_CollisionBoxes(BridgeHost, animsetv1);

            for (int a = 0; a < animsetv1.Animations.Count; a++)
            {
                var animset = new BridgedAnimation.BridgedAnimationEntry(EngineType.RSDKv1);
                animset.LoadFrom(EngineType.RSDKv1, animsetv1.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }
        public static void RSDKvRS_Load_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKvRS.Animation animsetvRS = new RSDKvRS.Animation(new RSDKvRS.Reader(filepath));
            RSDKvRS_Load_AnimHeader(BridgeHost, animsetvRS);
            RSDKvRS_Load_CollisionBoxes(BridgeHost, animsetvRS);

            for (int a = 0; a < animsetvRS.Animations.Count; a++)
            {
                var animset = new BridgedAnimation.BridgedAnimationEntry(EngineType.RSDKvRS);
                animset.LoadFrom(EngineType.RSDKvRS, animsetvRS.Animations[a]);
                BridgeHost.Animations.Add(animset);
            }
        }

        #endregion
            
        #region Save (Animation)
        public static void RSDKv5_Save_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKv5.Animation animsetv5 = new RSDKv5.Animation();
            RSDKv5_Save_AnimHeader(BridgeHost, animsetv5);
            RSDKv5_Save_CollisionBoxes(BridgeHost, animsetv5);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKv5, animsetv5);
                animsetv5.Animations.Add(animv5);
            }
            animsetv5.Write(new RSDKv5.Writer(filepath));
        }
        public static void RSDKvB_Save_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKvB.Animation animsetvB = new RSDKvB.Animation();
            RSDKvB_Save_AnimHeader(BridgeHost, animsetvB);
            RSDKvB_Save_CollisionBoxes(BridgeHost, animsetvB);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKvB.Animation.AnimationEntry animvB = new RSDKvB.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKvB, animsetvB);
                animsetvB.Animations.Add(animvB);
            }
            animsetvB.Write(new RSDKvB.Writer(filepath));
        }
        public static void RSDKv2_Save_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKv2.Animation animsetv2 = new RSDKv2.Animation();
            RSDKv2_Save_AnimHeader(BridgeHost, animsetv2);
            RSDKv2_Save_CollisionBoxes(BridgeHost, animsetv2);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKv2.Animation.AnimationEntry animv2 = new RSDKv2.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKv2, animsetv2);
                animsetv2.Animations.Add(animv2);
            }
            animsetv2.Write(new RSDKv2.Writer(filepath));
        }
        public static void RSDKv1_Save_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKv1.Animation animsetv1 = new RSDKv1.Animation();
            RSDKv1_Save_AnimHeader(BridgeHost, animsetv1);
            RSDKv1_Save_CollisionBoxes(BridgeHost, animsetv1);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKv1.Animation.AnimationEntry animv1 = new RSDKv1.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKv1, animsetv1);
                animsetv1.Animations.Add(animv1);
            }
            animsetv1.Write(new RSDKv1.Writer(filepath));
        }
        public static void RSDKvRS_Save_Animation(BridgedAnimation BridgeHost, string filepath)
        {
            RSDKvRS.Animation animsetvRS = new RSDKvRS.Animation();
            RSDKvRS_Save_AnimHeader(BridgeHost, animsetvRS);
            RSDKvRS_Save_CollisionBoxes(BridgeHost, animsetvRS);

            for (int a = 0; a < BridgeHost.Animations.Count; a++)
            {
                RSDKvRS.Animation.AnimationEntry animvRS = new RSDKvRS.Animation.AnimationEntry();
                BridgeHost.Animations[a].SaveTo(EngineType.RSDKvRS, animsetvRS);
                animsetvRS.Animations.Add(animvRS);
            }
            animsetvRS.Write(new RSDKvRS.Writer(filepath));
        }

        #endregion


        #region Import (Animation Entry)
        public static void RSDKv5_Import_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry(new RSDKv5.Reader(filepath));
            RSDKv5_Load_AnimEntry(animEntry, animv5);
        }
        public static void RSDKvB_Import_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKvB.Animation.AnimationEntry animvB = new RSDKvB.Animation.AnimationEntry(new RSDKvB.Reader(filepath));
            RSDKvB_Load_AnimEntry(animEntry, animvB);
        }
        public static void RSDKv2_Import_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKv2.Animation.AnimationEntry animv2 = new RSDKv2.Animation.AnimationEntry(new RSDKv2.Reader(filepath));
            RSDKv2_Load_AnimEntry(animEntry, animv2);
        }
        public static void RSDKv1_Import_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKv1.Animation.AnimationEntry animv1 = new RSDKv1.Animation.AnimationEntry(new RSDKv1.Reader(filepath));
            RSDKv1_Load_AnimEntry(animEntry, animv1);
        }
        public static void RSDKvRS_Import_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKvRS.Animation.AnimationEntry animvRS = new RSDKvRS.Animation.AnimationEntry(new RSDKvRS.Reader(filepath));
            RSDKvRS_Load_AnimEntry(animEntry, animvRS);
        }

        #endregion

        #region Export (Animation Entry)

        public static void RSDKv5_Export_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry();
            RSDKv5_Save_AnimEntry(animEntry, animv5);
            animv5.Write(new RSDKv5.Writer(filepath));
        }
        public static void RSDKvB_Export_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKvB.Animation.AnimationEntry animvB = new RSDKvB.Animation.AnimationEntry();
            RSDKvB_Save_AnimEntry(animEntry, animvB);
            animvB.Write(new RSDKvB.Writer(filepath));
        }
        public static void RSDKv2_Export_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKv2.Animation.AnimationEntry animv2 = new RSDKv2.Animation.AnimationEntry();
            RSDKv2_Save_AnimEntry(animEntry, animv2);
            animv2.Write(new RSDKv2.Writer(filepath));
        }
        public static void RSDKv1_Export_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKv1.Animation.AnimationEntry animv1 = new RSDKv1.Animation.AnimationEntry();
            RSDKv1_Save_AnimEntry(animEntry, animv1);
            animv1.Write(new RSDKv1.Writer(filepath));
        }
        public static void RSDKvRS_Export_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, string filepath)
        {
            RSDKvRS.Animation.AnimationEntry animvRS = new RSDKvRS.Animation.AnimationEntry();
            RSDKvRS_Save_AnimEntry(animEntry, animvRS);
            animvRS.Write(new RSDKvRS.Writer(filepath));
        }

        #endregion

        #region Load (Animation Entry)
        public static void RSDKv5_Load_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKv5.Animation.AnimationEntry animv5)
        {
            animEntry.AnimName = animv5.AnimName;
            animEntry.LoopIndex = animv5.LoopIndex;
            animEntry.SpeedMultiplyer = animv5.SpeedMultiplyer;
            animEntry.RotationFlags = animv5.RotationFlags;

            for (int i = 0; i < animv5.Frames.Count; i++)
            {
                BridgedAnimation.BridgedFrame frame = new BridgedAnimation.BridgedFrame(EngineType.RSDKv5);
                RSDKv5_Load_Frame(frame, animv5.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }
        public static void RSDKvB_Load_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKvB.Animation.AnimationEntry animvB)
        {
            animEntry.AnimName = animvB.AnimName;
            animEntry.LoopIndex = animvB.LoopIndex;
            animEntry.SpeedMultiplyer = animvB.SpeedMultiplyer;
            animEntry.RotationFlags = animvB.RotationFlags;

            for (int i = 0; i < animvB.Frames.Count; i++)
            {
                BridgedAnimation.BridgedFrame frame = new BridgedAnimation.BridgedFrame(EngineType.RSDKvB);
                RSDKvB_Load_Frame(frame, animvB.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }
        public static void RSDKv2_Load_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKv2.Animation.AnimationEntry animv2)
        {
            animEntry.AnimName = animv2.AnimName;
            animEntry.LoopIndex = animv2.LoopIndex;
            animEntry.SpeedMultiplyer = animv2.SpeedMultiplyer;
            animEntry.RotationFlags = animv2.RotationFlags;

            for (int i = 0; i < animv2.Frames.Count; i++)
            {
                BridgedAnimation.BridgedFrame frame = new BridgedAnimation.BridgedFrame(EngineType.RSDKv2);
                RSDKv2_Load_Frame(frame, animv2.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }
        public static void RSDKv1_Load_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKv1.Animation.AnimationEntry animv1)
        {
            animEntry.AnimName = animv1.AnimationName;
            animEntry.LoopIndex = animv1.LoopIndex;
            animEntry.SpeedMultiplyer = animv1.SpeedMultiplyer;
            animEntry.RotationFlags = 0;

            for (int i = 0; i < animv1.Frames.Count; i++)
            {
                BridgedAnimation.BridgedFrame frame = new BridgedAnimation.BridgedFrame(EngineType.RSDKv1);
                RSDKv1_Load_Frame(frame, animv1.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }
        public static void RSDKvRS_Load_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKvRS.Animation.AnimationEntry animvRS)
        {
            animEntry.AnimName = animEntry.AnimName;
            animEntry.LoopIndex = animvRS.LoopIndex;
            animEntry.SpeedMultiplyer = animvRS.SpeedMultiplyer;
            animEntry.RotationFlags = 0;

            for (int i = 0; i < animvRS.Frames.Count; i++)
            {
                BridgedAnimation.BridgedFrame frame = new BridgedAnimation.BridgedFrame(EngineType.RSDKvRS);
                RSDKvRS_Load_Frame(frame, animvRS.Frames[i]);
                animEntry.Frames.Add(frame);
            }
        }

        #endregion

        #region Save (Animation Entry)
        public static void RSDKv5_Save_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKv5.Animation.AnimationEntry animv5)
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
        public static void RSDKvB_Save_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKvB.Animation.AnimationEntry animvB)
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
        public static void RSDKv2_Save_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKv2.Animation.AnimationEntry animv2)
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
        public static void RSDKv1_Save_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKv1.Animation.AnimationEntry animv1)
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
        public static void RSDKvRS_Save_AnimEntry(BridgedAnimation.BridgedAnimationEntry animEntry, RSDKvRS.Animation.AnimationEntry animvRS)
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


        #region Import (Animation Frame)

        public static void RSDKv5_Import_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKv5.Reader readerv5 = new RSDKv5.Reader(filepath);
            RSDKv5.Animation.AnimationEntry.Frame framev5 = new RSDKv5.Animation.AnimationEntry.Frame(readerv5);
            readerv5.Close();
            RSDKv5_Load_Frame(frame, framev5);
        }
        public static void RSDKvB_Import_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKvB.Reader readervB = new RSDKvB.Reader(filepath);
            RSDKvB.Animation.AnimationEntry.Frame framevB = new RSDKvB.Animation.AnimationEntry.Frame(readervB);
            readervB.Close();
            RSDKvB_Load_Frame(frame, framevB);
        }
        public static void RSDKv2_Import_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKv2.Reader readerv2 = new RSDKv2.Reader(filepath);
            RSDKv2.Animation.AnimationEntry.Frame framev2 = new RSDKv2.Animation.AnimationEntry.Frame(readerv2);
            readerv2.Close();
            RSDKv2_Load_Frame(frame, framev2);
        }
        public static void RSDKv1_Import_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKv1.Reader readerv1 = new RSDKv1.Reader(filepath);
            RSDKv1.Animation.AnimationEntry.Frame framev1 = new RSDKv1.Animation.AnimationEntry.Frame(readerv1);
            readerv1.Close();
            RSDKv1_Load_Frame(frame, framev1);
        }
        public static void RSDKvRS_Import_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKvRS.Reader readervRS = new RSDKvRS.Reader(filepath);
            RSDKvRS.Animation.AnimationEntry.Frame framevRS = new RSDKvRS.Animation.AnimationEntry.Frame(readervRS);
            readervRS.Close();
            RSDKvRS_Load_Frame(frame, framevRS);
        }

        #endregion

        #region Export (Animation Frame)

        public static void RSDKv5_Export_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKv5.Animation.AnimationEntry.Frame framev5 = new RSDKv5.Animation.AnimationEntry.Frame();
            RSDKv5_Save_Frame(frame, framev5);
            RSDKv5.Writer writerv5 = new RSDKv5.Writer(filepath);
            framev5.Write(writerv5);
            writerv5.Close();
        }
        public static void RSDKvB_Export_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKvB.Animation.AnimationEntry.Frame framevB = new RSDKvB.Animation.AnimationEntry.Frame();
            RSDKvB_Save_Frame(frame, framevB);
            RSDKvB.Writer writervB = new RSDKvB.Writer(filepath);
            framevB.Write(writervB);
            writervB.Close();
        }
        public static void RSDKv2_Export_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKv2.Animation.AnimationEntry.Frame framev2 = new RSDKv2.Animation.AnimationEntry.Frame();
            RSDKv2_Save_Frame(frame, framev2);
            RSDKv2.Writer writerv2 = new RSDKv2.Writer(filepath);
            framev2.Write(writerv2);
            writerv2.Close();
        }
        public static void RSDKv1_Export_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKv1.Animation.AnimationEntry.Frame framev1 = new RSDKv1.Animation.AnimationEntry.Frame();
            RSDKv1_Save_Frame(frame, framev1);
            RSDKv1.Writer writerv1 = new RSDKv1.Writer(filepath);
            framev1.Write(writerv1);
            writerv1.Close();
        }
        public static void RSDKvRS_Export_Frame(BridgedAnimation.BridgedFrame frame, string filepath)
        {
            RSDKvRS.Animation.AnimationEntry.Frame framevRS = new RSDKvRS.Animation.AnimationEntry.Frame();
            RSDKvRS_Save_Frame(frame, framevRS);
            RSDKvRS.Writer writervRS = new RSDKvRS.Writer(filepath);
            framevRS.Write(writervRS);
            writervRS.Close();
        }

        #endregion

        #region Load (Animation Frame)

        public static void RSDKv5_Load_Frame(BridgedAnimation.BridgedFrame frame, RSDKv5.Animation.AnimationEntry.Frame framev5)
        {
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
            frame.HitBoxes = new List<BridgedAnimation.BridgedHitBox>();

            for (int i = 0; i < framev5.HitBoxes.Count; i++)
            {
                BridgedAnimation.BridgedHitBox hb = new BridgedAnimation.BridgedHitBox();
                RSDKv5_Load_Hitbox(hb, framev5.HitBoxes[i]);
                frame.HitBoxes.Add(hb);
            }
        }
        public static void RSDKvB_Load_Frame(BridgedAnimation.BridgedFrame frame, RSDKvB.Animation.AnimationEntry.Frame framevB)
        {
            frame.Delay = framevB.Delay;
            frame.CollisionBox = framevB.CollisionBox;
            frame.Height = framevB.Height;
            frame.PivotX = framevB.PivotX;
            frame.PivotY = framevB.PivotY;
            frame.SpriteSheet = framevB.SpriteSheet;
            frame.Width = framevB.Width;
            frame.X = framevB.X;
            frame.Y = framevB.Y;

            for (int i = 0; i < framevB.HitBoxes.Count; i++)
            {
                BridgedAnimation.BridgedHitBox hb = new BridgedAnimation.BridgedHitBox();
                RSDKvB_Load_Hitbox(hb, framevB.HitBoxes[i]);
                frame.HitBoxes.Add(hb);
            }
        }
        public static void RSDKv2_Load_Frame(BridgedAnimation.BridgedFrame frame, RSDKv2.Animation.AnimationEntry.Frame framev2)
        {
            frame.Delay = framev2.Delay;
            frame.CollisionBox = framev2.CollisionBox;
            frame.Height = framev2.Height;
            frame.PivotX = framev2.PivotX;
            frame.PivotY = framev2.PivotY;
            frame.SpriteSheet = framev2.SpriteSheet;
            frame.Width = framev2.Width;
            frame.X = framev2.X;
            frame.Y = framev2.Y;

            for (int i = 0; i < framev2.HitBoxes.Count; i++)
            {
                BridgedAnimation.BridgedHitBox hb = new BridgedAnimation.BridgedHitBox();
                RSDKv2_Load_Hitbox(hb, framev2.HitBoxes[i]);
                frame.HitBoxes.Add(hb);
            }
        }
        public static void RSDKv1_Load_Frame(BridgedAnimation.BridgedFrame frame, RSDKv1.Animation.AnimationEntry.Frame framev1)
        {
            frame.Delay = framev1.Delay;
            frame.CollisionBox = framev1.CollisionBox;
            frame.Height = framev1.Height;
            frame.PivotX = framev1.PivotX;
            frame.PivotY = framev1.PivotY;
            frame.SpriteSheet = framev1.SpriteSheet;
            frame.Width = framev1.Width;
            frame.X = framev1.X;
            frame.Y = framev1.Y;

            for (int i = 0; i < framev1.HitBoxes.Count; i++)
            {
                BridgedAnimation.BridgedHitBox hb = new BridgedAnimation.BridgedHitBox();
                RSDKv1_Load_Hitbox(hb, framev1.HitBoxes[i]);
                frame.HitBoxes.Add(hb);
            }
        }
        public static void RSDKvRS_Load_Frame(BridgedAnimation.BridgedFrame frame, RSDKvRS.Animation.AnimationEntry.Frame framevRS)
        {
            //frame.CollisionBox = framevRS.CollisionBox;
            frame.Delay = framevRS.Delay;
            frame.Height = framevRS.Height;
            frame.PivotX = framevRS.PivotX;
            frame.PivotY = framevRS.PivotY;
            frame.SpriteSheet = framevRS.SpriteSheet;
            frame.Width = framevRS.Width;
            frame.X = framevRS.X;
            frame.Y = framevRS.Y;
            BridgedAnimation.BridgedHitBox hb = new BridgedAnimation.BridgedHitBox();
            RSDKvRS_Load_Hitbox(hb, framevRS.CollisionBox);
            frame.HitBoxes.Add(hb);
        }

        #endregion

        #region Save (Animation Frame)

        public static void RSDKv5_Save_Frame(BridgedAnimation.BridgedFrame frame, RSDKv5.Animation.AnimationEntry.Frame framev5)
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
        public static void RSDKvB_Save_Frame(BridgedAnimation.BridgedFrame frame, RSDKvB.Animation.AnimationEntry.Frame framevB)
        {
            framevB.CollisionBox = frame.CollisionBox;
            framevB.Height = (byte)frame.Height;
            framevB.PivotX = (sbyte)frame.PivotX;
            framevB.PivotY = (sbyte)frame.PivotY;
            framevB.SpriteSheet = frame.SpriteSheet;
            framevB.Width = (byte)frame.Width;
            framevB.X = (byte)frame.X;
            framevB.Y = (byte)frame.Y;

            for (int i = 0; i < frame.HitBoxes.Count; i++)
            {
                RSDKvB.Animation.AnimationEntry.Frame.HitBox hb = new RSDKvB.Animation.AnimationEntry.Frame.HitBox();
                RSDKvB_Save_Hitbox(frame.HitBoxes[i], hb);
                framevB.HitBoxes.Add(hb);
            }
        }
        public static void RSDKv2_Save_Frame(BridgedAnimation.BridgedFrame frame, RSDKv2.Animation.AnimationEntry.Frame framev2)
        {
            framev2.CollisionBox = frame.CollisionBox;
            framev2.Height = (byte)frame.Height;
            framev2.PivotX = (sbyte)frame.PivotX;
            framev2.PivotY = (sbyte)frame.PivotY;
            framev2.SpriteSheet = frame.SpriteSheet;
            framev2.Width = (byte)frame.Width;
            framev2.X = (byte)frame.X;
            framev2.Y = (byte)frame.Y;

            for (int i = 0; i < frame.HitBoxes.Count; i++)
            {
                RSDKv2.Animation.AnimationEntry.Frame.HitBox hb = new RSDKv2.Animation.AnimationEntry.Frame.HitBox();
                RSDKv2_Save_Hitbox(frame.HitBoxes[i], hb);
                framev2.HitBoxes.Add(hb);
            }
        }
        public static void RSDKv1_Save_Frame(BridgedAnimation.BridgedFrame frame, RSDKv1.Animation.AnimationEntry.Frame framev1)
        {
            framev1.CollisionBox = frame.CollisionBox;
            framev1.Height = (byte)frame.Height;
            framev1.PivotX = (sbyte)frame.PivotX;
            framev1.PivotY = (sbyte)frame.PivotY;
            framev1.SpriteSheet = frame.SpriteSheet;
            framev1.Width = (byte)frame.Width;
            framev1.X = (byte)frame.X;
            framev1.Y = (byte)frame.Y;

            for (int i = 0; i < frame.HitBoxes.Count; i++)
            {
                RSDKv1.Animation.AnimationEntry.Frame.HitBox hb = new RSDKv1.Animation.AnimationEntry.Frame.HitBox();
                RSDKv1_Save_Hitbox(frame.HitBoxes[i], hb);
                framev1.HitBoxes.Add(hb);
            }
        }
        public static void RSDKvRS_Save_Frame(BridgedAnimation.BridgedFrame frame, RSDKvRS.Animation.AnimationEntry.Frame framevRS)
        {
            framevRS.Height = (byte)frame.Height;
            framevRS.PivotX = (sbyte)frame.PivotX;
            framevRS.PivotY = (sbyte)frame.PivotY;
            framevRS.SpriteSheet = frame.SpriteSheet;
            framevRS.Width = (byte)frame.Width;
            framevRS.X = (byte)frame.X;
            framevRS.Y = (byte)frame.Y;

            for (int i = 0; i < frame.HitBoxes.Count; i++)
            {
                RSDKvRS.Animation.AnimationEntry.Frame.HitBox hb = new RSDKvRS.Animation.AnimationEntry.Frame.HitBox();
                RSDKvRS_Save_Hitbox(frame.HitBoxes[i], hb);
                framevRS.CollisionBox = hb;
            }
        }

        #endregion


        #region Load (Animation Hitbox)

        public static void RSDKv5_Load_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKv5.Animation.AnimationEntry.Frame.HitBox hbV5)
        {
            hb.Bottom = hbV5.Bottom;
            hb.Right = hbV5.Right;
            hb.Top = hbV5.Top;
            hb.Left = hbV5.Left;
        }
        public static void RSDKvB_Load_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKvB.Animation.AnimationEntry.Frame.HitBox hbvB)
        {
            //TODO: Add Hitbox Load to RSDKvB Format
        }
        public static void RSDKv2_Load_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKv2.Animation.AnimationEntry.Frame.HitBox hbv2)
        {
            //TODO: Add Hitbox Load to RSDKv2 Format
        }
        public static void RSDKv1_Load_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKv1.Animation.AnimationEntry.Frame.HitBox hbv1)
        {
            //TODO: Add Hitbox Load to RSDKv1 Format
        }
        public static void RSDKvRS_Load_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKvRS.Animation.AnimationEntry.Frame.HitBox hbvRS)
        {
            hb.Bottom = hbvRS.Bottom;
            hb.Right = hbvRS.Right;
            hb.Left = hbvRS.Left;
            hb.Top = hbvRS.Top;
        }

        #endregion

        #region Save (Animation Hitbox)

        public static void RSDKv5_Save_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKv5.Animation.AnimationEntry.Frame.HitBox hbv5)
        {
            hb.Bottom = hbv5.Bottom;
            hb.Right = hbv5.Right;
            hb.Top = hbv5.Top;
            hb.Left = hbv5.Left;
        }
        public static void RSDKvB_Save_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKvB.Animation.AnimationEntry.Frame.HitBox hbvB)
        {
            //TODO: Add Hitbox Load to RSDKvB Format
        }
        public static void RSDKv2_Save_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKv2.Animation.AnimationEntry.Frame.HitBox hbv2)
        {
            //TODO: Add Hitbox Load to RSDKv2 Format
        }
        public static void RSDKv1_Save_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKv1.Animation.AnimationEntry.Frame.HitBox hbv1)
        {
            //TODO: Add Hitbox Load to RSDKv1 Format
        }
        public static void RSDKvRS_Save_Hitbox(BridgedAnimation.BridgedHitBox hb, RSDKvRS.Animation.AnimationEntry.Frame.HitBox hbvRS)
        {
            //TODO: Fix Hitbox Load to RSDKvRS Format
            /*
            hb.Bottom = hbvRS.Bottom;
            hb.Right = hbvRS.Right;
            hb.Top = hbvRS.Top;
            hb.Left = hbvRS.Left;
            */
        }

        #endregion

        #region Import (Animation Hitbox)

        public static void RSDKv5_Import_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Import to RSDKv5 Format
            /*
            RSDKv5.Reader readerv5 = new RSDKv5.Reader(filepath);
            RSDKv5.Animation.AnimationEntry.Frame.HitBox hbv5 = new RSDKv5.Animation.AnimationEntry.Frame.HitBox(readerv5);
            readerv5.Close();
            RSDKv5_Load_Hitbox(hb, hbv5);
            */
        }
        public static void RSDKvB_Import_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Import to RSDKvB Format
        }
        public static void RSDKv2_Import_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Import to RSDKv2 Format
        }
        public static void RSDKv1_Import_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Import to RSDKv1 Format
        }
        public static void RSDKvRS_Import_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Import to RSDKvRS Format
        }

        #endregion

        #region Export (Animation Hitbox)

        public static void RSDKv5_Export_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
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
        public static void RSDKvB_Export_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Export to RSDKvB Format
        }
        public static void RSDKv2_Export_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Export to RSDKv2 Format
        }
        public static void RSDKv1_Export_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Export to RSDKv1 Format
        }
        public static void RSDKvRS_Export_Hitbox(BridgedAnimation.BridgedHitBox hb, string filepath)
        {
            //TODO: Add Hitbox Export to RSDKvRS Format
        }

        #endregion


        #region Load (Collision Boxes)

        public static void RSDKv5_Load_CollisionBoxes(BridgedAnimation BridgeHost, RSDKv5.Animation animsetv5)
        {
            BridgeHost.CollisionBoxes = animsetv5.CollisionBoxes;
        }
        public static void RSDKvB_Load_CollisionBoxes(BridgedAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            //TODO: Add Hitbox Import to RSDKvB Format
        }
        public static void RSDKv2_Load_CollisionBoxes(BridgedAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            //TODO: Add Hitbox Import to RSDKv2 Format
        }
        public static void RSDKv1_Load_CollisionBoxes(BridgedAnimation BridgeHost, RSDKv1.Animation animsetv1)
        {
            //TODO: Add Hitbox Import to RSDKv1 Format
        }
        public static void RSDKvRS_Load_CollisionBoxes(BridgedAnimation BridgeHost, RSDKvRS.Animation animsetvRS)
        {
            BridgeHost.CollisionBoxes.Add("Hitbox");
        }

        #endregion

        #region Save (Collision Boxes)

        public static void RSDKv5_Save_CollisionBoxes(BridgedAnimation BridgeHost, RSDKv5.Animation animsetv5)
        {
            animsetv5.CollisionBoxes = BridgeHost.CollisionBoxes;
        }
        public static void RSDKvB_Save_CollisionBoxes(BridgedAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            // TODO - Fix Hitbox Export for RSDKvB
            /*
            for (int i = 0; i < BridgeHost.RetroCollisionBoxes.Count; i++)
            {
                animsetvB.CollisionBoxes.Add(new RSDKvB.Animation.sprHitbox());
                animsetvB.CollisionBoxes[i].Bottom = (sbyte)BridgeHost.RetroCollisionBoxes[i].Bottom;
                animsetvB.CollisionBoxes[i].Right = (sbyte)BridgeHost.RetroCollisionBoxes[i].Right;
                animsetvB.CollisionBoxes[i].Top = (sbyte)BridgeHost.RetroCollisionBoxes[i].Top;
                animsetvB.CollisionBoxes[i].Left = (sbyte)BridgeHost.RetroCollisionBoxes[i].Left;
            }*/
        }
        public static void RSDKv2_Save_CollisionBoxes(BridgedAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            //TODO - Fix Hitbox Export for RSDKv2
            /* 
            for (int i = 0; i < BridgeHost.RetroCollisionBoxes.Count; i++)
            {
                animsetv2.CollisionBoxes.Add(new RSDKv2.Animation.sprHitbox());
                animsetv2.CollisionBoxes[i].Bottom = (sbyte)BridgeHost.RetroCollisionBoxes[i].Bottom;
                animsetv2.CollisionBoxes[i].Right = (sbyte)BridgeHost.RetroCollisionBoxes[i].Right;
                animsetv2.CollisionBoxes[i].Top = (sbyte)BridgeHost.RetroCollisionBoxes[i].Top;
                animsetv2.CollisionBoxes[i].Left = (sbyte)BridgeHost.RetroCollisionBoxes[i].Left;
            }*/
        }
        public static void RSDKv1_Save_CollisionBoxes(BridgedAnimation BridgeHost, RSDKv1.Animation animsetv1)
        {
            // TODO - Fix Hitbox Export for RSDKv1
            /*
            for (int i = 0; i < BridgeHost.RetroCollisionBoxes.Count; i++)
            {
                animsetv1.CollisionBoxes.Add(new RSDKv1.Animation.sprHitbox());
                animsetv1.CollisionBoxes[i].Bottom = (sbyte)BridgeHost.RetroCollisionBoxes[i].Bottom;
                animsetv1.CollisionBoxes[i].Right = (sbyte)BridgeHost.RetroCollisionBoxes[i].Right;
                animsetv1.CollisionBoxes[i].Top = (sbyte)BridgeHost.RetroCollisionBoxes[i].Top;
                animsetv1.CollisionBoxes[i].Left = (sbyte)BridgeHost.RetroCollisionBoxes[i].Left;
            }*/
        }
        public static void RSDKvRS_Save_CollisionBoxes(BridgedAnimation BridgeHost, RSDKvRS.Animation animsetvRS)
        {
            //TODO: Add Hitbox Export to RSDKvRS Format
        }

        #endregion


        #region Load (Animation Header Properties)

        public static void RSDKv5_Load_AnimHeader(BridgedAnimation BridgeHost, RSDKv5.Animation animsetv5)
        {
            BridgeHost.SpriteSheets = animsetv5.SpriteSheets;
            BridgeHost.TotalFrameCount = animsetv5.TotalFrameCount;
        }
        public static void RSDKvB_Load_AnimHeader(BridgedAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            BridgeHost.SpriteSheets = animsetvB.SpriteSheets;
        }
        public static void RSDKv2_Load_AnimHeader(BridgedAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            BridgeHost.SpriteSheets = animsetv2.SpriteSheets;
        }
        public static void RSDKv1_Load_AnimHeader(BridgedAnimation BridgeHost, RSDKv1.Animation animsetv1)
        {
            for (int i = 0; i < animsetv1.SpriteSheets.Length; i++)
            {
                BridgeHost.SpriteSheets.Add(animsetv1.SpriteSheets[i]);
            }
        }
        public static void RSDKvRS_Load_AnimHeader(BridgedAnimation BridgeHost, RSDKvRS.Animation animsetvRS)
        {
            BridgeHost.PlayerType = animsetvRS.PlayerType;
            BridgeHost.Unknown = animsetvRS.Unknown;

            for (int i = 0; i < animsetvRS.SpriteSheets.Length; i++)
            {
                BridgeHost.SpriteSheets.Add(animsetvRS.SpriteSheets[i]);
            }
        }

        #endregion

        #region Save (Animation Header Properties)

        public static void RSDKv5_Save_AnimHeader(BridgedAnimation BridgeHost, RSDKv5.Animation animsetv5)
        {
            animsetv5.SpriteSheets = BridgeHost.SpriteSheets;
        }
        public static void RSDKvB_Save_AnimHeader(BridgedAnimation BridgeHost, RSDKvB.Animation animsetvB)
        {
            animsetvB.SpriteSheets = BridgeHost.SpriteSheets;
        }
        public static void RSDKv2_Save_AnimHeader(BridgedAnimation BridgeHost, RSDKv2.Animation animsetv2)
        {
            animsetv2.SpriteSheets = BridgeHost.SpriteSheets;
        }
        public static void RSDKv1_Save_AnimHeader(BridgedAnimation BridgeHost, RSDKv1.Animation animsetv1)
        {
            for (int i = 0; i < BridgeHost.SpriteSheets.Count; i++)
            {
                if (i >= 3) break;
                animsetv1.SpriteSheets[i] = BridgeHost.SpriteSheets[i];
            }
        }
        public static void RSDKvRS_Save_AnimHeader(BridgedAnimation BridgeHost, RSDKvRS.Animation animsetvRS)
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

    }
}
