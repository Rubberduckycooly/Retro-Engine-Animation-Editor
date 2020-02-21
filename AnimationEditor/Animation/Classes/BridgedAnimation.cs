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
        #region Classes
        [Serializable]
        public class BridgedAnimationEntry : ICloneable
        {
            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public EngineType engineType = EngineType.RSDKv5;
            public BridgedAnimation Parent { get; private set; }

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

            public BridgedAnimationEntry(EngineType type, BridgedAnimation parent)
            {
                engineType = type;
                Frames = new List<BridgedFrame>();
                Parent = parent;
            }

            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                Frames.Clear();
                AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvU_Import_AnimEntry(this, filepath);
            }

            public void ExportTo(EngineType type, string filepath)
            {
                AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvU_Export_AnimEntry(this, filepath);
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
            public BridgedAnimationEntry Parent { get; private set; }

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

            #region Hitboxes
            /// <summary>
            /// the hitbox data for the frame
            /// </summary>
            public List<BridgedHitBox> HitBoxes { get => GetHitBoxes(); set => SetHitboxes(value); }

            public List<BridgedHitBox> GetHitBoxes()
            {
                if (engineType == EngineType.RSDKv5) return RSDKv5_HitBoxes;
                else return GetRetroCollisionBoxes();
            }

            public List<BridgedHitBox> GetRetroCollisionBoxes()
            {
                if (Parent != null)
                {
                    if (Parent.Parent != null)
                    {
                        if (Parent.Parent.RetroCollisionBoxes != null)
                        {
                            if (Parent.Parent.RetroCollisionBoxes.ElementAtOrDefault(CollisionBox) != null)
                            {
                                return Parent.Parent.RetroCollisionBoxes[CollisionBox].ToBridgedHitBox();
                            }
                        }
                    }
                }
                return new List<BridgedHitBox>();
            }

            public void SetRetroCollisionBoxes(List<BridgedHitBox> value)
            {
                if (Parent != null)
                {
                    if (Parent.Parent != null)
                    {
                        if (Parent.Parent.RetroCollisionBoxes != null)
                        {
                            if (Parent.Parent.RetroCollisionBoxes.ElementAtOrDefault(CollisionBox) != null)
                            {
                                Parent.Parent.RetroCollisionBoxes[CollisionBox].FromBridgedHitBox(value);
                            }
                        }
                    }
                }
            }

            public void SetHitboxes(List<BridgedHitBox> value)
            {
                if (engineType == EngineType.RSDKv5) RSDKv5_HitBoxes = value;
                else SetRetroCollisionBoxes(value);
            }

            private List<BridgedHitBox> RSDKv5_HitBoxes = new List<BridgedHitBox>();

            #endregion

            public BridgedFrame()
            {
                engineType = EngineType.Invalid;
                Parent = null;
            }

            public BridgedFrame(EngineType type, BridgedAnimationEntry parent)
            {
                engineType = type;
                Parent = parent;
            }

            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvU_Import_Frame(this, filepath);
            }
            public void ExportTo(EngineType type, string filepath)
            {
                AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvU_Export_Frame(this, filepath);
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
                AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvU_Import_Hitbox(this, filepath);
            }
            public void ExportTo(EngineType type, string filepath)
            {
                AnimationEditor.Animation.Methods.ImportExportHandler.RSDKvU_Export_Hitbox(this, filepath);
            }
        }
        [Serializable]
        public class BridgedRetroHitBox
        {
            public struct HitboxInfo
            {
                public sbyte Left;
                public sbyte Top;
                public sbyte Right;
                public sbyte Bottom;
            }

            public HitboxInfo[] Hitboxes = new HitboxInfo[8];

            public BridgedRetroHitBox()
            {

            }

            public void FromBridgedHitBox(List<BridgedHitBox> value)
            {
                for (int i = 0; i < 8; i++)
                {
                    Hitboxes[i].Bottom = (sbyte)value[i].Bottom;
                    Hitboxes[i].Top = (sbyte)value[i].Top;
                    Hitboxes[i].Left = (sbyte)value[i].Left;
                    Hitboxes[i].Right = (sbyte)value[i].Right;
                }
            }

            public List<BridgedHitBox> ToBridgedHitBox()
            {
                List<BridgedHitBox> bridgedHitBoxes = new List<BridgedHitBox>();
                for (int i = 0; i < 8; i++)
                {
                    var box = new BridgedHitBox();
                    box.Bottom = Hitboxes[i].Bottom;
                    box.Top = Hitboxes[i].Top;
                    box.Left = Hitboxes[i].Left;
                    box.Right = Hitboxes[i].Right;
                    bridgedHitBoxes.Add(box);
                }
                return bridgedHitBoxes;
            }
        }
        #endregion
        public List<BridgedRetroHitBox> RetroCollisionBoxes { get; set; } = new List<BridgedRetroHitBox>();
        public EngineType EngineType { get; set; } = EngineType.RSDKv5;
        public int TotalFrameCount { get; set; } = 0;
        public string PathMod
        {
            get
            {
                switch (EngineType)
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
        public List<string> SpriteSheetsPaths { get; set; } = new List<string>();
        public List<string> CollisionBoxes { get => CollisionBoxesList; set => CollisionBoxesList = value; }
        private List<string> CollisionBoxesList { get; set; } = new List<string>();
        public List<BridgedAnimationEntry> Animations { get; set; } = new List<BridgedAnimationEntry>();

        #region Stuff for RSDKvRS
        public bool DreamcastVer { get; set; } = false;
        /// <summary>
        /// Unknown Value (RSDKvRS Only)
        /// </summary>
        public byte Unknown { get; set; } = 0;
        /// <summary>
        /// What Moves to give the player (RSDKvRS Only)
        /// </summary>
        public byte PlayerType { get; set; } = 0;
        #endregion

        public BridgedAnimation(EngineType type)
        {
            Animations.Add(new BridgedAnimationEntry(type, this));
        }

        public EngineType GetFormat(string filepath)
        {
            return EngineType.Invalid;
        }

        public void LoadFrom(EngineType type, string filepath)
        {
            EngineType = type;
            Animations.Clear();
            SpriteSheets.Clear();
            CollisionBoxes.Clear();
            switch (EngineType)
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
            switch (EngineType)
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
