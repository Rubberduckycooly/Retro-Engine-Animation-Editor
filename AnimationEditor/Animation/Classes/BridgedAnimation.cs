using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationEditor.Animation.Classes
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
    public class BridgedAnimation
    {
        [Serializable]
        public class BridgedAnimationEntry : ICloneable
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
            public List<BridgedFrame> Frames;
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

            public BridgedAnimationEntry(EngineType type)
            {
                engineType = type;
                Frames = new List<BridgedFrame>();
            }

            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                Frames.Clear();
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Import_AnimEntry(this, filepath);
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Import_AnimEntry(this, filepath);
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Import_AnimEntry(this, filepath);
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Import_AnimEntry(this, filepath);
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Import_AnimEntry(this, filepath);
                        break;
                }
            }

            public void ExportTo(EngineType type, string filepath)
            {
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Export_AnimEntry(this, filepath);
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Export_AnimEntry(this, filepath);
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Export_AnimEntry(this, filepath);
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Export_AnimEntry(this, filepath);
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Export_AnimEntry(this, filepath);
                        break;
                }
            }

            public void SaveTo(EngineType type, object animSet)
            {
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Save_AnimEntry(this, (animSet as RSDKv5.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Save_AnimEntry(this, (animSet as RSDKvB.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Save_AnimEntry(this, (animSet as RSDKv2.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Save_AnimEntry(this, (animSet as RSDKv1.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Save_AnimEntry(this, (animSet as RSDKvRS.Animation.AnimationEntry));
                        break;
                }
            }

            public void LoadFrom(EngineType type, object animSet)
            {
                engineType = type;
                Frames.Clear();
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Load_AnimEntry(this, (animSet as RSDKv5.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Load_AnimEntry(this, (animSet as RSDKvB.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Load_AnimEntry(this, (animSet as RSDKv2.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Load_AnimEntry(this, (animSet as RSDKv1.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Load_AnimEntry(this, (animSet as RSDKvRS.Animation.AnimationEntry));
                        break;
                }
            }

        }
        [Serializable]
        public class BridgedFrame : ICloneable
        {
            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public EngineType engineType = EngineType.RSDKv5;

            /// <summary>
            /// the hitbox data for the frame
            /// </summary>
            public List<BridgedHitBox> HitBoxes = new List<BridgedHitBox>();
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

            public BridgedFrame(EngineType type)
            {
                engineType = type;
            }

            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                switch (type)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Import_Frame(this, filepath);
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Import_Frame(this, filepath);
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Import_Frame(this, filepath);
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Import_Frame(this, filepath);
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Import_Frame(this, filepath);
                        break;
                }
            }
            public void ExportTo(EngineType type, string filepath)
            {
                switch (type)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Export_Frame(this, filepath);
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Export_Frame(this, filepath);
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Export_Frame(this, filepath);
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Export_Frame(this, filepath);
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Export_Frame(this, filepath);
                        break;
                }
            }

            public void SaveTo(EngineType type, object frame)
            {
                switch (type)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Save_Frame(this, (frame as RSDKv5.Animation.AnimationEntry.Frame));
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Save_Frame(this, (frame as RSDKvB.Animation.AnimationEntry.Frame));
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Save_Frame(this, (frame as RSDKv2.Animation.AnimationEntry.Frame));
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Save_Frame(this, (frame as RSDKv1.Animation.AnimationEntry.Frame));
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Save_Frame(this, (frame as RSDKvRS.Animation.AnimationEntry.Frame));
                        break;
                }
            }
            public void LoadFrom(EngineType type, object frame)
            {
                engineType = type;
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Load_Frame(this, (frame as RSDKv5.Animation.AnimationEntry.Frame));
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Load_Frame(this, (frame as RSDKvB.Animation.AnimationEntry.Frame));
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Load_Frame(this, (frame as RSDKv2.Animation.AnimationEntry.Frame));
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Load_Frame(this, (frame as RSDKv1.Animation.AnimationEntry.Frame));
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Load_Frame(this, (frame as RSDKvRS.Animation.AnimationEntry.Frame));
                        break;
                }
            }

        }
        [Serializable]
        public class BridgedHitBox
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
                switch (type)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Import_Hitbox(this, filepath);
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Import_Hitbox(this, filepath);
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Import_Hitbox(this, filepath);
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Import_Hitbox(this, filepath);
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Import_Hitbox(this, filepath);
                        break;
                }
            }
            public void ExportTo(EngineType type, string filepath)
            {
                switch (type)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Export_Hitbox(this, filepath);
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Export_Hitbox(this, filepath);
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Export_Hitbox(this, filepath);
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Export_Hitbox(this, filepath);
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Export_Hitbox(this, filepath);
                        break;
                }
            }
        }

        public EngineType engineType = EngineType.RSDKv5;

        public int TotalFrameCount { get; set; } = 0;

        public string PathMod
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
        public List<BridgedHitBox> RetroCollisionBoxes = new List<BridgedHitBox>();

        public List<BridgedAnimationEntry> Animations = new List<BridgedAnimationEntry>();

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

        public BridgedAnimation(EngineType type)
        {
            Animations.Add(new BridgedAnimationEntry(type));
        }

        public EngineType GetFormat(string filepath)
        {
            return EngineType.Invalid;
        }

        public void LoadFrom(EngineType type, string filepath)
        {
            engineType = type;
            Animations.Clear();
            SpriteSheets.Clear();
            CollisionBoxes.Clear();
            RetroCollisionBoxes.Clear();
            switch (engineType)
            {
                case EngineType.RSDKv5:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Load_Animation(this, filepath);
                    break;
                case EngineType.RSDKvB:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Load_Animation(this, filepath);
                    break;
                case EngineType.RSDKv2:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Load_Animation(this, filepath);
                    break;
                case EngineType.RSDKv1:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Load_Animation(this, filepath);
                    break;
                case EngineType.RSDKvRS:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Load_Animation(this, filepath);
                    break;
            }
        }

        public void SaveTo(EngineType type, string filepath)
        {
            switch (engineType)
            {
                case EngineType.RSDKv5:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv5_Save_Animation(this, filepath);
                    break;
                case EngineType.RSDKvB:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvB_Save_Animation(this, filepath);
                    break;
                case EngineType.RSDKv2:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv2_Save_Animation(this, filepath);
                    break;
                case EngineType.RSDKv1:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKv1_Save_Animation(this, filepath);
                    break;
                case EngineType.RSDKvRS:
                    AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvRS_Save_Animation(this, filepath);
                    break;
            }
        }

    }
}
