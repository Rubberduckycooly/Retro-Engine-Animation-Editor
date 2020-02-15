using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationEditor
{

    public enum EngineType
    {
        RSDKv5,
        RSDKvB,
        RSDKv2,
        RSDKv1,
        RSDKvRS,
        Invalid
    }

    [Serializable]
    public class Animation
    {
        [Serializable]
        public class AnimationEntry : ICloneable
        {
            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public EngineType engineType = EngineType.RSDKv5;

            /// <summary>
            /// the name of the animtion
            /// </summary>
            public string AnimName
            {
                get;
                set;
            }
            /// <summary>
            /// the list of frames in this animation
            /// </summary>
            public List<Frame> Frames;
            /// <summary>
            /// the frame to loop back from
            /// </summary>
            public byte LoopIndex;
            /// <summary>
            /// the amount to multiply each frame's "Delay" value
            /// </summary>
            public short SpeedMultiplyer;
            /// <summary>
            /// the rotation style of the animation
            /// </summary>
            public byte RotationFlags;

            public AnimationEntry(EngineType type)
            {
                engineType = type;
                Frames = new List<Frame>();
            }

            #region Import and Export

            #region Import
            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                Frames.Clear();
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        RSDKv5_Import(filepath);
                        break;
                    case EngineType.RSDKvB:
                        RSDKvB_Import(filepath);
                        break;
                    case EngineType.RSDKv2:
                        RSDKv2_Import(filepath);
                        break;
                    case EngineType.RSDKv1:
                        RSDKv1_Import(filepath);
                        break;
                    case EngineType.RSDKvRS:
                        RSDKvRS_Import(filepath);
                        break;
                }
            }

            private void RSDKv5_Import(string filepath)
            {
                RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry(new RSDKv5.Reader(filepath));
                AnimName = animv5.AnimName;
                LoopIndex = animv5.LoopIndex;
                SpeedMultiplyer = animv5.SpeedMultiplyer;
                RotationFlags = animv5.RotationFlags;

                for (int i = 0; i < animv5.Frames.Count; i++)
                {
                    Frame frame = new Frame(EngineType.RSDKv5);
                    frame.CollisionBox = animv5.Frames[i].CollisionBox;
                    frame.Delay = animv5.Frames[i].Delay;
                    frame.Height = animv5.Frames[i].Height;
                    frame.ID = (ushort)animv5.Frames[i].ID;
                    frame.PivotX = animv5.Frames[i].PivotX;
                    frame.PivotY = animv5.Frames[i].PivotY;
                    frame.Width = animv5.Frames[i].Width;
                    frame.X = animv5.Frames[i].X;
                    frame.Y = animv5.Frames[i].Y;
                    Frames.Add(frame);
                }
            }
            private void RSDKvB_Import(string filepath)
            {
                RSDKvB.Animation.AnimationEntry animvB = new RSDKvB.Animation.AnimationEntry(new RSDKvB.Reader(filepath));
                AnimName = animvB.AnimName;
                LoopIndex = animvB.LoopIndex;
                SpeedMultiplyer = animvB.SpeedMultiplyer;
                RotationFlags = animvB.RotationFlags;

                for (int i = 0; i < animvB.Frames.Count; i++)
                {
                    Frame frame = new Frame(EngineType.RSDKvB);
                    frame.CollisionBox = animvB.Frames[i].CollisionBox;
                    frame.Delay = animvB.Frames[i].Delay;
                    frame.Height = animvB.Frames[i].Height;
                    frame.PivotX = animvB.Frames[i].PivotX;
                    frame.PivotY = animvB.Frames[i].PivotY;
                    frame.Width = animvB.Frames[i].Width;
                    frame.X = animvB.Frames[i].X;
                    frame.Y = animvB.Frames[i].Y;
                    Frames.Add(frame);
                }
            }
            private void RSDKv2_Import(string filepath)
            {
                RSDKv2.Animation.AnimationEntry animv2 = new RSDKv2.Animation.AnimationEntry(new RSDKv2.Reader(filepath));
                AnimName = animv2.AnimName;
                LoopIndex = animv2.LoopIndex;
                SpeedMultiplyer = animv2.SpeedMultiplyer;
                RotationFlags = animv2.RotationFlags;

                for (int i = 0; i < animv2.Frames.Count; i++)
                {
                    Frame frame = new Frame(EngineType.RSDKv2);
                    frame.CollisionBox = animv2.Frames[i].CollisionBox;
                    frame.Delay = animv2.Frames[i].Delay;
                    frame.Height = animv2.Frames[i].Height;
                    frame.PivotX = animv2.Frames[i].PivotX;
                    frame.PivotY = animv2.Frames[i].PivotY;
                    frame.Width = animv2.Frames[i].Width;
                    frame.X = animv2.Frames[i].X;
                    frame.Y = animv2.Frames[i].Y;
                    Frames.Add(frame);
                }
            }
            private void RSDKv1_Import(string filepath)
            {
                RSDKv1.Animation.AnimationEntry animv1 = new RSDKv1.Animation.AnimationEntry(new RSDKv1.Reader(filepath));
                AnimName = "Retro Engine v1 Animation";
                LoopIndex = animv1.LoopIndex;
                SpeedMultiplyer = animv1.SpeedMultiplyer;
                RotationFlags = 0;

                for (int i = 0; i < animv1.Frames.Count; i++)
                {
                    Frame frame = new Frame(EngineType.RSDKv1);
                    frame.CollisionBox = animv1.Frames[i].CollisionBox;
                    frame.Delay = animv1.Frames[i].Delay;
                    frame.Height = animv1.Frames[i].Height;
                    frame.PivotX = animv1.Frames[i].PivotX;
                    frame.PivotY = animv1.Frames[i].PivotY;
                    frame.Width = animv1.Frames[i].Width;
                    frame.X = animv1.Frames[i].X;
                    frame.Y = animv1.Frames[i].Y;
                    Frames.Add(frame);
                }
            }
            private void RSDKvRS_Import(string filepath)
            {
                RSDKvRS.Animation.AnimationEntry animvRS = new RSDKvRS.Animation.AnimationEntry(new RSDKvRS.Reader(filepath));
                AnimName = "Retro-Sonic Animation";
                LoopIndex = animvRS.LoopIndex;
                SpeedMultiplyer = animvRS.SpeedMultiplyer;
                RotationFlags = 0;

                for (int i = 0; i < animvRS.Frames.Count; i++)
                {
                    Frame frame = new Frame(EngineType.RSDKvRS);
                    frame.Delay = animvRS.Frames[i].Delay;
                    frame.Height = animvRS.Frames[i].Height;
                    frame.PivotX = animvRS.Frames[i].PivotX;
                    frame.PivotY = animvRS.Frames[i].PivotY;
                    frame.Width = animvRS.Frames[i].Width;
                    frame.X = animvRS.Frames[i].X;
                    frame.Y = animvRS.Frames[i].Y;
                    Frames.Add(frame);
                }
            }

            #endregion

            #region Export
            public void ExportTo(EngineType type, string filepath)
            {
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        RSDKv5_Export(filepath);
                        break;
                    case EngineType.RSDKvB:
                        RSDKvB_Export(filepath);
                        break;
                    case EngineType.RSDKv2:
                        RSDKv2_Export(filepath);
                        break;
                    case EngineType.RSDKv1:
                        RSDKv1_Export(filepath);
                        break;
                    case EngineType.RSDKvRS:
                        RSDKvRS_Export(filepath);
                        break;
                }
            }
            private void RSDKv5_Export(string filepath)
            {
                RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry();
                animv5.AnimName = AnimName;
                animv5.LoopIndex = LoopIndex;
                animv5.SpeedMultiplyer = SpeedMultiplyer;
                animv5.RotationFlags = RotationFlags;

                for (int i = 0; i < Frames.Count; i++)
                {
                    RSDKv5.Animation.AnimationEntry.Frame frame = new RSDKv5.Animation.AnimationEntry.Frame();
                    frame.CollisionBox = Frames[i].CollisionBox;
                    frame.Delay = Frames[i].Delay;
                    frame.Height = Frames[i].Height;
                    frame.ID = (short)Frames[i].ID;
                    frame.PivotX = Frames[i].PivotX;
                    frame.PivotY = Frames[i].PivotY;
                    frame.Width = Frames[i].Width;
                    frame.X = Frames[i].X;
                    frame.Y = Frames[i].Y;
                    animv5.Frames.Add(frame);
                }
                animv5.Write(new RSDKv5.Writer(filepath));
            }
            private void RSDKvB_Export(string filepath)
            {
                RSDKvB.Animation.AnimationEntry animvB = new RSDKvB.Animation.AnimationEntry();
                animvB.AnimName = AnimName;
                animvB.LoopIndex = LoopIndex;
                animvB.SpeedMultiplyer = (byte)SpeedMultiplyer;
                animvB.RotationFlags = RotationFlags;

                for (int i = 0; i < Frames.Count; i++)
                {
                    RSDKvB.Animation.AnimationEntry.Frame frame = new RSDKvB.Animation.AnimationEntry.Frame();
                    frame.CollisionBox = Frames[i].CollisionBox;
                    frame.Height = (byte)Frames[i].Height;
                    frame.PivotX = (sbyte)Frames[i].PivotX;
                    frame.PivotY = (sbyte)Frames[i].PivotY;
                    frame.Width = (byte)Frames[i].Width;
                    frame.X = (byte)Frames[i].X;
                    frame.Y = (byte)Frames[i].Y;
                    animvB.Frames.Add(frame);
                }
                animvB.Write(new RSDKvB.Writer(filepath));
            }
            private void RSDKv2_Export(string filepath)
            {
                RSDKv2.Animation.AnimationEntry animv2 = new RSDKv2.Animation.AnimationEntry();
                animv2.AnimName = AnimName;
                animv2.LoopIndex = LoopIndex;
                animv2.SpeedMultiplyer = (byte)SpeedMultiplyer;
                animv2.RotationFlags = RotationFlags;

                for (int i = 0; i < Frames.Count; i++)
                {
                    RSDKv2.Animation.AnimationEntry.Frame frame = new RSDKv2.Animation.AnimationEntry.Frame();
                    frame.CollisionBox = Frames[i].CollisionBox;
                    frame.Height = (byte)Frames[i].Height;
                    frame.PivotX = (sbyte)Frames[i].PivotX;
                    frame.PivotY = (sbyte)Frames[i].PivotY;
                    frame.Width = (byte)Frames[i].Width;
                    frame.X = (byte)Frames[i].X;
                    frame.Y = (byte)Frames[i].Y;
                    animv2.Frames.Add(frame);
                }
                animv2.Write(new RSDKv2.Writer(filepath));
            }
            private void RSDKv1_Export(string filepath)
            {
                RSDKv1.Animation.AnimationEntry animv1 = new RSDKv1.Animation.AnimationEntry();
                animv1.LoopIndex = LoopIndex;
                animv1.SpeedMultiplyer = (byte)SpeedMultiplyer;

                for (int i = 0; i < Frames.Count; i++)
                {
                    RSDKv1.Animation.AnimationEntry.Frame frame = new RSDKv1.Animation.AnimationEntry.Frame();
                    frame.CollisionBox = Frames[i].CollisionBox;
                    frame.Height = (byte)Frames[i].Height;
                    frame.PivotX = (sbyte)Frames[i].PivotX;
                    frame.PivotY = (sbyte)Frames[i].PivotY;
                    frame.Width = (byte)Frames[i].Width;
                    frame.X = (byte)Frames[i].X;
                    frame.Y = (byte)Frames[i].Y;
                    animv1.Frames.Add(frame);
                }
                animv1.Write(new RSDKv1.Writer(filepath));
            }
            private void RSDKvRS_Export(string filepath)
            {
                RSDKvRS.Animation.AnimationEntry animvRS = new RSDKvRS.Animation.AnimationEntry();
                animvRS.LoopIndex = LoopIndex;
                animvRS.SpeedMultiplyer = (byte)SpeedMultiplyer;

                for (int i = 0; i < Frames.Count; i++)
                {
                    RSDKvRS.Animation.AnimationEntry.Frame frame = new RSDKvRS.Animation.AnimationEntry.Frame();
                    //frame.CollisionBox = Frames[i].CollisionBox;
                    frame.Height = (byte)Frames[i].Height;
                    frame.PivotX = (sbyte)Frames[i].PivotX;
                    frame.PivotY = (sbyte)Frames[i].PivotY;
                    frame.Width = (byte)Frames[i].Width;
                    frame.X = (byte)Frames[i].X;
                    frame.Y = (byte)Frames[i].Y;
                    animvRS.Frames.Add(frame);
                }
                animvRS.Write(new RSDKvRS.Writer(filepath));
            }

            #endregion

            #endregion

        }
        [Serializable]
        public class Frame : ICloneable
        {
            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public EngineType engineType = EngineType.RSDKv5;

            /// <summary>
            /// the hitbox data for the frame
            /// </summary>
            public List<HitBox> HitBoxes = new List<HitBox>();
            /// <summary>
            /// the spritesheet ID
            /// </summary>
            public byte SpriteSheet = 0;
            /// <summary>
            /// the collisionBox ID
            /// </summary>
            public byte CollisionBox = 0;
            /// <summary>
            /// how many frames to wait before the next frame is shown
            /// </summary>
            public short Delay = 0;
            /// <summary>
            /// special value, used for things like the title card letters (and strangely, mighty's victory anim)
            /// </summary>
            public ushort ID = 0;
            /// <summary>
            /// the Xpos on the sheet
            /// </summary>
            public short X = 0;
            /// <summary>
            /// the Ypos on the sheet
            /// </summary>
            public short Y = 0;
            /// <summary>
            /// the width of the frame
            /// </summary>
            public short Width = 0;
            /// <summary>
            /// the height of the frame
            /// </summary>
            public short Height = 0;
            /// <summary>
            /// the X Offset for the frame
            /// </summary>
            public short PivotX = 0;
            /// <summary>
            /// the Y Offset for the frame
            /// </summary>
            public short PivotY = 0;

            public Frame(EngineType type)
            {
                engineType = type;
            }

            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                switch (type)
                {
                    //case EngineType.RSDKv5:
                    default:
                        RSDKv5.Reader readerv5 = new RSDKv5.Reader(filepath);
                        RSDKv5.Animation.AnimationEntry.Frame framev5 = new RSDKv5.Animation.AnimationEntry.Frame(readerv5);
                        readerv5.Close();
                        CollisionBox = framev5.CollisionBox;
                        Delay = framev5.Delay;
                        Height = framev5.Height;
                        ID = (ushort)framev5.ID;
                        PivotX = framev5.PivotX;
                        PivotY = framev5.PivotY;
                        SpriteSheet = framev5.SpriteSheet;
                        Width = framev5.Width;
                        X = framev5.X;
                        Y = framev5.Y;
                        for (int i = 0; i < HitBoxes.Count; i++)
                        {
                            HitBox hb = new HitBox();
                            hb.Bottom = framev5.HitBoxes[i].Bottom;
                            hb.Right = framev5.HitBoxes[i].Right;
                            hb.Top = framev5.HitBoxes[i].Top;
                            hb.Left = framev5.HitBoxes[i].Left;
                            HitBoxes.Add(hb);
                        }
                        break;
                    case EngineType.RSDKvB:
                        RSDKvB.Reader readervB = new RSDKvB.Reader(filepath);
                        RSDKvB.Animation.AnimationEntry.Frame framevB = new RSDKvB.Animation.AnimationEntry.Frame(readervB);
                        readervB.Close();
                        CollisionBox = framevB.CollisionBox;
                        Height = framevB.Height;
                        PivotX = framevB.PivotX;
                        PivotY = framevB.PivotY;
                        SpriteSheet = framevB.SpriteSheet;
                        Width = framevB.Width;
                        X = framevB.X;
                        Y = framevB.Y;
                        break;
                    case EngineType.RSDKv2:
                        RSDKv2.Reader readerv2 = new RSDKv2.Reader(filepath);
                        RSDKv2.Animation.AnimationEntry.Frame framev2 = new RSDKv2.Animation.AnimationEntry.Frame(readerv2);
                        readerv2.Close();
                        CollisionBox = framev2.CollisionBox;
                        Height = framev2.Height;
                        PivotX = framev2.PivotX;
                        PivotY = framev2.PivotY;
                        SpriteSheet = framev2.SpriteSheet;
                        Width = framev2.Width;
                        X = framev2.X;
                        Y = framev2.Y;
                        break;
                    case EngineType.RSDKv1:
                        RSDKv1.Reader readerv1 = new RSDKv1.Reader(filepath);
                        RSDKv1.Animation.AnimationEntry.Frame framev1 = new RSDKv1.Animation.AnimationEntry.Frame(readerv1);
                        readerv1.Close();
                        CollisionBox = framev1.CollisionBox;
                        Height = framev1.Height;
                        PivotX = framev1.PivotX;
                        PivotY = framev1.PivotY;
                        SpriteSheet = framev1.SpriteSheet;
                        Width = framev1.Width;
                        X = framev1.X;
                        Y = framev1.Y;
                        break;
                    case EngineType.RSDKvRS:
                        RSDKvRS.Reader readervRS = new RSDKvRS.Reader(filepath);
                        RSDKvRS.Animation.AnimationEntry.Frame framevRS = new RSDKvRS.Animation.AnimationEntry.Frame(readervRS);
                        readervRS.Close();
                        //CollisionBox = framevRS.CollisionBox;
                        Height = framevRS.Height;
                        PivotX = framevRS.PivotX;
                        PivotY = framevRS.PivotY;
                        SpriteSheet = framevRS.SpriteSheet;
                        Width = framevRS.Width;
                        X = framevRS.X;
                        Y = framevRS.Y;
                        break;
                }
            }

            public void ExportTo(EngineType type, string filepath)
            {
                switch (type)
                {
                    case EngineType.RSDKv5:
                        RSDKv5.Animation.AnimationEntry.Frame framev5 = new RSDKv5.Animation.AnimationEntry.Frame();
                        framev5.CollisionBox = CollisionBox;
                        framev5.Delay = Delay;
                        framev5.Height = Height;
                        framev5.ID = (short)ID;
                        framev5.PivotX = PivotX;
                        framev5.PivotY = PivotY;
                        framev5.SpriteSheet = SpriteSheet;
                        framev5.Width = Width;
                        framev5.X = X;
                        framev5.Y = Y;
                        for (int i = 0; i < HitBoxes.Count; i++)
                        {
                            RSDKv5.Animation.AnimationEntry.Frame.HitBox hb = new RSDKv5.Animation.AnimationEntry.Frame.HitBox();
                            hb.Bottom = HitBoxes[i].Bottom;
                            hb.Right = HitBoxes[i].Right;
                            hb.Top = HitBoxes[i].Top;
                            hb.Left = HitBoxes[i].Left;
                            framev5.HitBoxes.Add(hb);
                        }
                        RSDKv5.Writer writerv5 = new RSDKv5.Writer(filepath);
                        framev5.Write(writerv5);
                        writerv5.Close();
                        break;
                    case EngineType.RSDKvB:
                        RSDKvB.Animation.AnimationEntry.Frame framevB = new RSDKvB.Animation.AnimationEntry.Frame();
                        framevB.CollisionBox = CollisionBox;
                        framevB.Height = (byte)Height;
                        framevB.PivotX = (sbyte)PivotX;
                        framevB.PivotY = (sbyte)PivotY;
                        framevB.SpriteSheet = SpriteSheet;
                        framevB.Width = (byte)Width;
                        framevB.X = (byte)X;
                        framevB.Y = (byte)Y;
                        RSDKvB.Writer writervB = new RSDKvB.Writer(filepath);
                        framevB.Write(writervB);
                        writervB.Close();
                        break;
                    case EngineType.RSDKv2:
                        RSDKv2.Animation.AnimationEntry.Frame framev2 = new RSDKv2.Animation.AnimationEntry.Frame();
                        framev2.CollisionBox = CollisionBox;
                        framev2.Height = (byte)Height;
                        framev2.PivotX = (sbyte)PivotX;
                        framev2.PivotY = (sbyte)PivotY;
                        framev2.SpriteSheet = SpriteSheet;
                        framev2.Width = (byte)Width;
                        framev2.X = (byte)X;
                        framev2.Y = (byte)Y;
                        RSDKv2.Writer writerv2 = new RSDKv2.Writer(filepath);
                        framev2.Write(writerv2);
                        writerv2.Close();
                        break;
                    case EngineType.RSDKv1:
                        RSDKv1.Animation.AnimationEntry.Frame framev1 = new RSDKv1.Animation.AnimationEntry.Frame();
                        framev1.CollisionBox = CollisionBox;
                        framev1.Height = (byte)Height;
                        framev1.PivotX = (sbyte)PivotX;
                        framev1.PivotY = (sbyte)PivotY;
                        framev1.SpriteSheet = SpriteSheet;
                        framev1.Width = (byte)Width;
                        framev1.X = (byte)X;
                        framev1.Y = (byte)Y;
                        RSDKv1.Writer writerv1 = new RSDKv1.Writer(filepath);
                        framev1.Write(writerv1);
                        writerv1.Close();
                        break;
                    case EngineType.RSDKvRS:
                        RSDKvRS.Animation.AnimationEntry.Frame framevRS = new RSDKvRS.Animation.AnimationEntry.Frame();
                        //framevRS.CollisionBox = CollisionBox; //Don't understand that yet
                        framevRS.Height = (byte)Height;
                        framevRS.PivotX = (sbyte)PivotX;
                        framevRS.PivotY = (sbyte)PivotY;
                        framevRS.SpriteSheet = SpriteSheet;
                        framevRS.Width = (byte)Width;
                        framevRS.X = (byte)X;
                        framevRS.Y = (byte)Y;
                        RSDKvRS.Writer writervRS = new RSDKvRS.Writer(filepath);
                        framevRS.Write(writervRS);
                        writervRS.Close();
                        break;
                }
            }

        }
        [Serializable]
        public class HitBox
        {
            public EngineType engineType = EngineType.RSDKv5;
            /// <summary>
            /// the Xpos of the hitbox
            /// </summary>
            public short Left;
            /// <summary>
            /// the Width of the hitbox
            /// </summary>
            public short Right;
            /// <summary>
            /// the Ypos of the hitbox
            /// </summary>
            public short Top;
            /// <summary>
            /// the height of the hitbox
            /// </summary>
            public short Bottom;

            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        break;
                    case EngineType.RSDKvB:
                        break;
                    case EngineType.RSDKv2:
                        break;
                    case EngineType.RSDKv1:
                        break;
                    case EngineType.RSDKvRS:
                        break;
                }
            }

            public void ExportTo(EngineType type, string filepath)
            {
                switch (type)
                {
                    case EngineType.RSDKv5:
                        break;
                    case EngineType.RSDKvB:
                        break;
                    case EngineType.RSDKv2:
                        break;
                    case EngineType.RSDKv1:
                        break;
                    case EngineType.RSDKvRS:
                        break;
                }
            }
        }

        public EngineType engineType = EngineType.RSDKv5;

        public int TotalFrameCount = 0;

        public string pathmod
        {
            get
            {
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        return "";
                    case EngineType.RSDKvB:
                        return "Sprites\\";
                    case EngineType.RSDKv2:
                        return "Sprites\\";
                    case EngineType.RSDKv1:
                        return "Sprites\\";
                    case EngineType.RSDKvRS:
                        return "Characters\\";
                    default:
                        return "";
                }
            }
        }

        public List<string> SpriteSheets { get => SpriteSheetsPaths; set => SpriteSheetsPaths = value; }
        public List<string> SpriteSheetsPaths = new List<string>();
        public List<string> CollisionBoxes { get => CollisionBoxesList; set => CollisionBoxesList = value; }
        public List<string> CollisionBoxesList = new List<string>();
        /// <summary>
        /// a list of the hitboxes that the animations can use (For Pre-v5 Formats)
        /// </summary>
        public List<HitBox> RetroCollisionBoxes = new List<HitBox>();

        public List<AnimationEntry> Animations = new List<AnimationEntry>();

        //Stuff for RSDKvRS
        public bool DreamcastVer = false;

        /// <summary>
        /// Unknown Value (RSDKvRS Only)
        /// </summary>
        public byte Unknown = 0;
        /// <summary>
        /// What Moves to give the player (RSDKvRS Only)
        /// </summary>
        public byte PlayerType = 0;

        public Animation(EngineType type)
        {
            Animations.Add(new AnimationEntry(type));
        }

        public EngineType GetFormat(string filepath)
        {
            return EngineType.Invalid;
        }

        #region Import and Export

        #region Import
        public void ImportFrom(EngineType type, string filepath)
        {
            engineType = type;
            Animations.Clear();
            SpriteSheets.Clear();
            CollisionBoxes.Clear();
            RetroCollisionBoxes.Clear();
            switch (engineType)
            {
                case EngineType.RSDKv5:
                    RSDKv5_Import(filepath);
                    break;
                case EngineType.RSDKvB:
                    RSDKvB_Import(filepath);
                    break;
                case EngineType.RSDKv2:
                    RSDKv2_Import(filepath);
                    break;
                case EngineType.RSDKv1:
                    RSDKv1_Import(filepath);
                    break;
                case EngineType.RSDKvRS:
                    RSDKvRS_Import(filepath);
                    break;
            }
        }
        private void RSDKv5_Import(string filepath)
        {
            RSDKv5.Animation animsetv5 = new RSDKv5.Animation(new RSDKv5.Reader(filepath));

            SpriteSheets = animsetv5.SpriteSheets;
            CollisionBoxes = animsetv5.CollisionBoxes;
            TotalFrameCount = animsetv5.TotalFrameCount;

            for (int a = 0; a < animsetv5.Animations.Count; a++)
            {
                Animations.Add(new AnimationEntry(EngineType.RSDKv5));
                Animations[a].Frames.Clear();
                Animations[a].AnimName = animsetv5.Animations[a].AnimName;
                Animations[a].LoopIndex = animsetv5.Animations[a].LoopIndex;
                Animations[a].SpeedMultiplyer = animsetv5.Animations[a].SpeedMultiplyer;
                Animations[a].RotationFlags = animsetv5.Animations[a].RotationFlags;

                for (int i = 0; i < animsetv5.Animations[a].Frames.Count; i++)
                {
                    Animations[a].Frames.Add(new Frame(EngineType.RSDKv5));
                    Animations[a].Frames[i].HitBoxes.Clear();
                    Animations[a].Frames[i].CollisionBox = animsetv5.Animations[a].Frames[i].CollisionBox;
                    Animations[a].Frames[i].Delay = animsetv5.Animations[a].Frames[i].Delay;
                    Animations[a].Frames[i].Height = animsetv5.Animations[a].Frames[i].Height;
                    Animations[a].Frames[i].ID = (ushort)animsetv5.Animations[a].Frames[i].ID;
                    Animations[a].Frames[i].PivotX = animsetv5.Animations[a].Frames[i].PivotX;
                    Animations[a].Frames[i].PivotY = animsetv5.Animations[a].Frames[i].PivotY;
                    Animations[a].Frames[i].Width = animsetv5.Animations[a].Frames[i].Width;
                    Animations[a].Frames[i].X = animsetv5.Animations[a].Frames[i].X;
                    Animations[a].Frames[i].Y = animsetv5.Animations[a].Frames[i].Y;
                    Animations[a].Frames[i].SpriteSheet = animsetv5.Animations[a].Frames[i].SpriteSheet;

                    for (int h = 0; h < animsetv5.CollisionBoxes.Count; h++)
                    {
                        Animations[a].Frames[i].HitBoxes.Add(new HitBox());
                        Animations[a].Frames[i].HitBoxes[h].Bottom = animsetv5.Animations[a].Frames[i].HitBoxes[h].Bottom;
                        Animations[a].Frames[i].HitBoxes[h].Right = animsetv5.Animations[a].Frames[i].HitBoxes[h].Right;
                        Animations[a].Frames[i].HitBoxes[h].Left = animsetv5.Animations[a].Frames[i].HitBoxes[h].Left;
                        Animations[a].Frames[i].HitBoxes[h].Top = animsetv5.Animations[a].Frames[i].HitBoxes[h].Top;
                    }
                }
            }
        }
        private void RSDKvB_Import(string filepath)
        {
            RSDKvB.Animation animsetvB = new RSDKvB.Animation(new RSDKvB.Reader(filepath));

            SpriteSheets = animsetvB.SpriteSheets;

            for (int a = 0; a < animsetvB.Animations.Count; a++)
            {
                Animations.Add(new AnimationEntry(EngineType.RSDKvB));
                Animations[a].AnimName = animsetvB.Animations[a].AnimName;
                Animations[a].LoopIndex = animsetvB.Animations[a].LoopIndex;
                Animations[a].SpeedMultiplyer = animsetvB.Animations[a].SpeedMultiplyer;
                Animations[a].RotationFlags = animsetvB.Animations[a].RotationFlags;

                for (int i = 0; i < animsetvB.Animations[a].Frames.Count; i++)
                {
                    Animations[a].Frames.Add(new Frame(EngineType.RSDKvB));
                    Animations[a].Frames[i].CollisionBox = animsetvB.Animations[a].Frames[i].CollisionBox;
                    Animations[a].Frames[i].Delay = animsetvB.Animations[a].Frames[i].Delay;
                    Animations[a].Frames[i].Height = animsetvB.Animations[a].Frames[i].Height;
                    Animations[a].Frames[i].PivotX = animsetvB.Animations[a].Frames[i].PivotX;
                    Animations[a].Frames[i].PivotY = animsetvB.Animations[a].Frames[i].PivotY;
                    Animations[a].Frames[i].Width = animsetvB.Animations[a].Frames[i].Width;
                    Animations[a].Frames[i].X = animsetvB.Animations[a].Frames[i].X;
                    Animations[a].Frames[i].Y = animsetvB.Animations[a].Frames[i].Y;
                    Animations[a].Frames[i].SpriteSheet = animsetvB.Animations[a].Frames[i].SpriteSheet;
                }
            }
        }
        private void RSDKv2_Import(string filepath)
        {
            RSDKv2.Animation animsetv2 = new RSDKv2.Animation(new RSDKv2.Reader(filepath));
            SpriteSheets = animsetv2.SpriteSheets;

            for (int a = 0; a < animsetv2.Animations.Count; a++)
            {
                Animations.Add(new AnimationEntry(EngineType.RSDKv2));
                Animations[a].AnimName = animsetv2.Animations[a].AnimName;
                Animations[a].LoopIndex = animsetv2.Animations[a].LoopIndex;
                Animations[a].SpeedMultiplyer = animsetv2.Animations[a].SpeedMultiplyer;
                Animations[a].RotationFlags = animsetv2.Animations[a].RotationFlags;

                for (int i = 0; i < animsetv2.Animations[a].Frames.Count; i++)
                {
                    Animations[a].Frames.Add(new Frame(EngineType.RSDKv2));
                    Animations[a].Frames[i].CollisionBox = animsetv2.Animations[a].Frames[i].CollisionBox;
                    Animations[a].Frames[i].Delay = animsetv2.Animations[a].Frames[i].Delay;
                    Animations[a].Frames[i].Height = animsetv2.Animations[a].Frames[i].Height;
                    Animations[a].Frames[i].PivotX = animsetv2.Animations[a].Frames[i].PivotX;
                    Animations[a].Frames[i].PivotY = animsetv2.Animations[a].Frames[i].PivotY;
                    Animations[a].Frames[i].Width = animsetv2.Animations[a].Frames[i].Width;
                    Animations[a].Frames[i].X = animsetv2.Animations[a].Frames[i].X;
                    Animations[a].Frames[i].Y = animsetv2.Animations[a].Frames[i].Y;
                    Animations[a].Frames[i].SpriteSheet = animsetv2.Animations[a].Frames[i].SpriteSheet;

                }
            }
        }
        private void RSDKv1_Import(string filepath)
        {
            RSDKv1.Animation animsetv1 = new RSDKv1.Animation(new RSDKv1.Reader(filepath));

            for (int i = 0; i < animsetv1.SpriteSheets.Length; i++)
            {
                SpriteSheets.Add(animsetv1.SpriteSheets[i]);
            }

            for (int a = 0; a < animsetv1.Animations.Count; a++)
            {
                Animations.Add(new AnimationEntry(EngineType.RSDKv1));
                Animations[a].AnimName = animsetv1.AnimNames[a];
                Animations[a].LoopIndex = animsetv1.Animations[a].LoopIndex;
                Animations[a].SpeedMultiplyer = animsetv1.Animations[a].SpeedMultiplyer;

                for (int i = 0; i < animsetv1.Animations[a].Frames.Count; i++)
                {
                    Animations[a].Frames.Add(new Frame(EngineType.RSDKv2));
                    Animations[a].Frames[i].CollisionBox = animsetv1.Animations[a].Frames[i].CollisionBox;
                    Animations[a].Frames[i].Delay = animsetv1.Animations[a].Frames[i].Delay;
                    Animations[a].Frames[i].Height = animsetv1.Animations[a].Frames[i].Height;
                    Animations[a].Frames[i].PivotX = animsetv1.Animations[a].Frames[i].PivotX;
                    Animations[a].Frames[i].PivotY = animsetv1.Animations[a].Frames[i].PivotY;
                    Animations[a].Frames[i].Width = animsetv1.Animations[a].Frames[i].Width;
                    Animations[a].Frames[i].X = animsetv1.Animations[a].Frames[i].X;
                    Animations[a].Frames[i].Y = animsetv1.Animations[a].Frames[i].Y;
                    Animations[a].Frames[i].SpriteSheet = animsetv1.Animations[a].Frames[i].SpriteSheet;

                }
            }
        }
        private void RSDKvRS_Import(string filepath)
        {
            RSDKvRS.Animation animsetvRS = new RSDKvRS.Animation(new RSDKvRS.Reader(filepath));

            PlayerType = animsetvRS.PlayerType;
            Unknown = animsetvRS.Unknown;

            CollisionBoxes.Add("Hitbox");

            for (int i = 0; i < animsetvRS.SpriteSheets.Length; i++)
            {
                SpriteSheets.Add(animsetvRS.SpriteSheets[i]);
            }

            for (int a = 0; a < animsetvRS.Animations.Count; a++)
            {
                Animations.Add(new AnimationEntry(EngineType.RSDKvRS));
                Animations[a].AnimName = animsetvRS.AnimNames[a];
                Animations[a].LoopIndex = animsetvRS.Animations[a].LoopIndex;
                Animations[a].SpeedMultiplyer = animsetvRS.Animations[a].SpeedMultiplyer;

                for (int i = 0; i < animsetvRS.Animations[a].Frames.Count; i++)
                {
                    Animations[a].Frames.Add(new Frame(EngineType.RSDKvRS));
                    Animations[a].Frames[i].Delay = animsetvRS.Animations[a].Frames[i].Delay;
                    Animations[a].Frames[i].Height = animsetvRS.Animations[a].Frames[i].Height;
                    Animations[a].Frames[i].PivotX = animsetvRS.Animations[a].Frames[i].PivotX;
                    Animations[a].Frames[i].PivotY = animsetvRS.Animations[a].Frames[i].PivotY;
                    Animations[a].Frames[i].Width = animsetvRS.Animations[a].Frames[i].Width;
                    Animations[a].Frames[i].X = animsetvRS.Animations[a].Frames[i].X;
                    Animations[a].Frames[i].Y = animsetvRS.Animations[a].Frames[i].Y;
                    Animations[a].Frames[i].SpriteSheet = animsetvRS.Animations[a].Frames[i].SpriteSheet;

                    Animations[a].Frames[i].HitBoxes.Add(new HitBox());
                    Animations[a].Frames[i].HitBoxes[0].Bottom = animsetvRS.Animations[a].Frames[i].CollisionBox.Bottom;
                    Animations[a].Frames[i].HitBoxes[0].Right = animsetvRS.Animations[a].Frames[i].CollisionBox.Right;
                    Animations[a].Frames[i].HitBoxes[0].Left = animsetvRS.Animations[a].Frames[i].CollisionBox.Left;
                    Animations[a].Frames[i].HitBoxes[0].Top = animsetvRS.Animations[a].Frames[i].CollisionBox.Top;
                }
            }
        }
        #endregion

        #region Export
        public void ExportTo(EngineType type, string filepath)
        {
            switch (engineType)
            {
                case EngineType.RSDKv5:
                    RSDKv5_Export(filepath);
                    break;
                case EngineType.RSDKvB:
                    RSDKvB_Export(filepath);
                    break;
                case EngineType.RSDKv2:
                    RSDKv2_Export(filepath);
                    break;
                case EngineType.RSDKv1:
                    RSDKv1_Export(filepath);
                    break;
                case EngineType.RSDKvRS:
                    RSDKvRS_Export(filepath);
                    break;
            }
        }

        private void RSDKv5_Export(string filepath)
        {
            RSDKv5.Animation animsetv5 = new RSDKv5.Animation();
            animsetv5.SpriteSheets = SpriteSheets;
            animsetv5.CollisionBoxes = CollisionBoxes;

            for (int a = 0; a < Animations.Count; a++)
            {
                RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry();
                animv5.AnimName = Animations[a].AnimName;
                animv5.LoopIndex = Animations[a].LoopIndex;
                animv5.SpeedMultiplyer = Animations[a].SpeedMultiplyer;
                animv5.RotationFlags = Animations[a].RotationFlags;

                for (int i = 0; i < Animations[a].Frames.Count; i++)
                {
                    RSDKv5.Animation.AnimationEntry.Frame frame = new RSDKv5.Animation.AnimationEntry.Frame();
                    frame.CollisionBox = Animations[a].Frames[i].CollisionBox;
                    frame.SpriteSheet = Animations[a].Frames[i].SpriteSheet;
                    frame.Delay = Animations[a].Frames[i].Delay;
                    frame.Height = Animations[a].Frames[i].Height;
                    frame.ID = (short)Animations[a].Frames[i].ID;
                    frame.PivotX = Animations[a].Frames[i].PivotX;
                    frame.PivotY = Animations[a].Frames[i].PivotY;
                    frame.Width = Animations[a].Frames[i].Width;
                    frame.X = Animations[a].Frames[i].X;
                    frame.Y = Animations[a].Frames[i].Y;

                    for (int h = 0; h < Animations[a].Frames[i].HitBoxes.Count; h++)
                    {
                        var HitBoxes = new RSDKv5.Animation.AnimationEntry.Frame.HitBox();
                        HitBoxes.Bottom = Animations[a].Frames[i].HitBoxes[h].Bottom;
                        HitBoxes.Right = Animations[a].Frames[i].HitBoxes[h].Right;
                        HitBoxes.Left = Animations[a].Frames[i].HitBoxes[h].Left;
                        HitBoxes.Top = Animations[a].Frames[i].HitBoxes[h].Top;
                        frame.HitBoxes.Add(HitBoxes);
                    }

                    animv5.Frames.Add(frame);
                }
                animsetv5.Animations.Add(animv5);
            }
            animsetv5.Write(new RSDKv5.Writer(filepath));
        }
        private void RSDKvB_Export(string filepath)
        {
            RSDKvB.Animation animsetvB = new RSDKvB.Animation();
            animsetvB.SpriteSheets = SpriteSheets;

            /* TODO - Fix Hitbox Export for RSDKvB
            for (int i = 0; i < RetroCollisionBoxes.Count; i++)
            {
                animsetvB.CollisionBoxes.Add(new RSDKvB.Animation.sprHitbox());
                animsetvB.CollisionBoxes[i].Bottom = (sbyte)RetroCollisionBoxes[i].Bottom;
                animsetvB.CollisionBoxes[i].Right = (sbyte)RetroCollisionBoxes[i].Right;
                animsetvB.CollisionBoxes[i].Top = (sbyte)RetroCollisionBoxes[i].Top;
                animsetvB.CollisionBoxes[i].Left = (sbyte)RetroCollisionBoxes[i].Left;
            }*/
            for (int a = 0; a < Animations.Count; a++)
            {

                RSDKvB.Animation.AnimationEntry animvB = new RSDKvB.Animation.AnimationEntry();
                animvB.AnimName = Animations[a].AnimName;
                animvB.LoopIndex = Animations[a].LoopIndex;
                animvB.SpeedMultiplyer = (byte)Animations[a].SpeedMultiplyer;
                animvB.RotationFlags = Animations[a].RotationFlags;

                for (int i = 0; i < Animations[a].Frames.Count; i++)
                {
                    RSDKvB.Animation.AnimationEntry.Frame frame = new RSDKvB.Animation.AnimationEntry.Frame();
                    frame.SpriteSheet = Animations[a].Frames[i].SpriteSheet;
                    frame.CollisionBox = Animations[a].Frames[i].CollisionBox;
                    frame.Height = (byte)Animations[a].Frames[i].Height;
                    frame.PivotX = (sbyte)Animations[a].Frames[i].PivotX;
                    frame.PivotY = (sbyte)Animations[a].Frames[i].PivotY;
                    frame.Width = (byte)Animations[a].Frames[i].Width;
                    frame.X = (byte)Animations[a].Frames[i].X;
                    frame.Y = (byte)Animations[a].Frames[i].Y;
                    animvB.Frames.Add(frame);
                }
                animsetvB.Animations.Add(animvB);
            }
            animsetvB.Write(new RSDKvB.Writer(filepath));
        }
        private void RSDKv2_Export(string filepath)
        {
            RSDKv2.Animation animsetv2 = new RSDKv2.Animation();
            animsetv2.SpriteSheets = SpriteSheets;

            /* TODO - Fix Hitbox Export for RSDKv2
            for (int i = 0; i < RetroCollisionBoxes.Count; i++)
            {
                animsetv2.CollisionBoxes.Add(new RSDKv2.Animation.sprHitbox());
                animsetv2.CollisionBoxes[i].Bottom = (sbyte)RetroCollisionBoxes[i].Bottom;
                animsetv2.CollisionBoxes[i].Right = (sbyte)RetroCollisionBoxes[i].Right;
                animsetv2.CollisionBoxes[i].Top = (sbyte)RetroCollisionBoxes[i].Top;
                animsetv2.CollisionBoxes[i].Left = (sbyte)RetroCollisionBoxes[i].Left;
            }*/
            for (int a = 0; a < Animations.Count; a++)
            {
                RSDKv2.Animation.AnimationEntry animv2 = new RSDKv2.Animation.AnimationEntry();
                animv2.AnimName = Animations[a].AnimName;
                animv2.LoopIndex = Animations[a].LoopIndex;
                animv2.SpeedMultiplyer = (byte)Animations[a].SpeedMultiplyer;
                animv2.RotationFlags = Animations[a].RotationFlags;

                for (int i = 0; i < Animations[a].Frames.Count; i++)
                {
                    RSDKv2.Animation.AnimationEntry.Frame frame = new RSDKv2.Animation.AnimationEntry.Frame();
                    frame.SpriteSheet = Animations[a].Frames[i].SpriteSheet;
                    frame.CollisionBox = Animations[a].Frames[i].CollisionBox;
                    frame.Height = (byte)Animations[a].Frames[i].Height;
                    frame.PivotX = (sbyte)Animations[a].Frames[i].PivotX;
                    frame.PivotY = (sbyte)Animations[a].Frames[i].PivotY;
                    frame.Width = (byte)Animations[a].Frames[i].Width;
                    frame.X = (byte)Animations[a].Frames[i].X;
                    frame.Y = (byte)Animations[a].Frames[i].Y;
                    animv2.Frames.Add(frame);
                }
                animsetv2.Animations.Add(animv2);
            }
            animsetv2.Write(new RSDKv2.Writer(filepath));
        }
        private void RSDKv1_Export(string filepath)
        {
            RSDKv1.Animation animsetv1 = new RSDKv1.Animation();

            for (int i = 0; i < SpriteSheets.Count; i++)
            {
                if (i >= 3) break;
                animsetv1.SpriteSheets[i] = SpriteSheets[i];
            }
            /* TODO - Fix Hitbox Export for RSDKv1
            for (int i = 0; i < RetroCollisionBoxes.Count; i++)
            {
                animsetv1.CollisionBoxes.Add(new RSDKv1.Animation.sprHitbox());
                animsetv1.CollisionBoxes[i].Bottom = (sbyte)RetroCollisionBoxes[i].Bottom;
                animsetv1.CollisionBoxes[i].Right = (sbyte)RetroCollisionBoxes[i].Right;
                animsetv1.CollisionBoxes[i].Top = (sbyte)RetroCollisionBoxes[i].Top;
                animsetv1.CollisionBoxes[i].Left = (sbyte)RetroCollisionBoxes[i].Left;
            }*/
            for (int a = 0; a < Animations.Count; a++)
            {

                RSDKv1.Animation.AnimationEntry animv1 = new RSDKv1.Animation.AnimationEntry();
                animv1.LoopIndex = Animations[a].LoopIndex;
                animv1.SpeedMultiplyer = (byte)Animations[a].SpeedMultiplyer;

                for (int i = 0; i < Animations[a].Frames.Count; i++)
                {
                    RSDKv1.Animation.AnimationEntry.Frame frame = new RSDKv1.Animation.AnimationEntry.Frame();
                    frame.SpriteSheet = Animations[a].Frames[i].SpriteSheet;
                    frame.CollisionBox = Animations[a].Frames[i].CollisionBox;
                    frame.Height = (byte)Animations[a].Frames[i].Height;
                    frame.PivotX = (sbyte)Animations[a].Frames[i].PivotX;
                    frame.PivotY = (sbyte)Animations[a].Frames[i].PivotY;
                    frame.Width = (byte)Animations[a].Frames[i].Width;
                    frame.X = (byte)Animations[a].Frames[i].X;
                    frame.Y = (byte)Animations[a].Frames[i].Y;
                    animv1.Frames.Add(frame);
                }
                animsetv1.Animations.Add(animv1);
            }
            animsetv1.Write(new RSDKv1.Writer(filepath));
        }
        private void RSDKvRS_Export(string filepath)
        {
            RSDKvRS.Animation animsetvRS = new RSDKvRS.Animation();

            animsetvRS.PlayerType = PlayerType;
            animsetvRS.Unknown = Unknown;

            for (int i = 0; i < SpriteSheets.Count; i++)
            {
                if (i >= 3) break;
                animsetvRS.SpriteSheets[i] = SpriteSheets[i];
            }

            for (int a = 0; a < Animations.Count; a++)
            {

                RSDKvRS.Animation.AnimationEntry animvRS = new RSDKvRS.Animation.AnimationEntry();
                animvRS.LoopIndex = Animations[a].LoopIndex;
                animvRS.SpeedMultiplyer = (byte)Animations[a].SpeedMultiplyer;

                for (int i = 0; i < Animations[a].Frames.Count; i++)
                {
                    RSDKvRS.Animation.AnimationEntry.Frame frame = new RSDKvRS.Animation.AnimationEntry.Frame();
                    frame.SpriteSheet = Animations[a].Frames[i].SpriteSheet;
                    frame.Height = (byte)Animations[a].Frames[i].Height;
                    frame.PivotX = (sbyte)Animations[a].Frames[i].PivotX;
                    frame.PivotY = (sbyte)Animations[a].Frames[i].PivotY;
                    frame.Width = (byte)Animations[a].Frames[i].Width;
                    frame.X = (byte)Animations[a].Frames[i].X;
                    frame.Y = (byte)Animations[a].Frames[i].Y;
                    animvRS.Frames.Add(frame);
                }
                animsetvRS.Animations.Add(animvRS);
            }
            animsetvRS.Write(new RSDKvRS.Writer(filepath));
        }

        #endregion

        #endregion

    }
}
