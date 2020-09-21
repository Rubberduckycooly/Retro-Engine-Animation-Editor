using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationEditor.Classes
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
    public class EditorAnimation
    {
        #region Classes
        [Serializable]
        public class EditorAnimationInfo : ICloneable
        {
            public object Clone()
            {
                List<EditorFrame> frames = new List<EditorFrame>();
                foreach (var entry in this.Frames)
                {
                    frames.Add(entry.Clone() as EditorFrame);
                }
                EditorAnimationInfo item = this.MemberwiseClone() as EditorAnimationInfo;
                item.Frames = frames;
                return this.MemberwiseClone();
            }

            public EngineType engineType = EngineType.RSDKv5;
            public EditorAnimation Parent { get; private set; }

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
            public List<EditorFrame> Frames;
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

            public EditorAnimationInfo(EngineType type, EditorAnimation parent)
            {
                engineType = type;
                Frames = new List<EditorFrame>();
                Parent = parent;
            }

            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                Frames.Clear();
                AnimationEditor.Methods.AnimationReader.RSDKvU_Import_AnimEntry(this, filepath);
            }

            public void ExportTo(EngineType type, string filepath)
            {
                AnimationEditor.Methods.AnimationWriter.RSDKvU_Export_AnimEntry(this, filepath);
            }

            public void SaveTo(EngineType type, object animSet)
            {
                switch (engineType)
                {
                    case EngineType.RSDKv5:
                        AnimationEditor.Methods.AnimationWriter.RSDKv5_Save_AnimEntry(this, (animSet as RSDKv5.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Methods.AnimationWriter.RSDKvB_Save_AnimEntry(this, (animSet as RSDKvB.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Methods.AnimationWriter.RSDKv2_Save_AnimEntry(this, (animSet as RSDKv2.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Methods.AnimationWriter.RSDKv1_Save_AnimEntry(this, (animSet as RSDKv1.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Methods.AnimationWriter.RSDKvRS_Save_AnimEntry(this, (animSet as RSDKvRS.Animation.AnimationEntry));
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
                        AnimationEditor.Methods.AnimationReader.RSDKv5_Load_AnimEntry(this, (animSet as RSDKv5.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKvB:
                        AnimationEditor.Methods.AnimationReader.RSDKvB_Load_AnimEntry(this, (animSet as RSDKvB.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKv2:
                        AnimationEditor.Methods.AnimationReader.RSDKv2_Load_AnimEntry(this, (animSet as RSDKv2.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKv1:
                        AnimationEditor.Methods.AnimationReader.RSDKv1_Load_AnimEntry(this, (animSet as RSDKv1.Animation.AnimationEntry));
                        break;
                    case EngineType.RSDKvRS:
                        AnimationEditor.Methods.AnimationReader.RSDKvRS_Load_AnimEntry(this, (animSet as RSDKvRS.Animation.AnimationEntry));
                        break;
                }
            }

        }
        [Serializable]
        public class EditorFrame : ICloneable
        {
            public object Clone()
            {
                return this.MemberwiseClone();
            }

            public EngineType engineType = EngineType.RSDKv5;
            public EditorAnimationInfo Parent { get; private set; }

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
            public List<EditorHitbox> HitBoxes { get; set; }

            #region Intended For Future Use in Exportation to Other Formats
            public List<EditorHitbox> GetRetroCollisionBoxes()
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
                return new List<EditorHitbox>();
            }
            public void SetRetroCollisionBoxes(List<EditorHitbox> value)
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
            #endregion

            #endregion

            public EditorFrame()
            {
                engineType = EngineType.Invalid;
                Parent = null;
                HitboxInitialization();
            }

            public EditorFrame(EngineType type, EditorAnimationInfo parent)
            {
                engineType = type;
                Parent = parent;
                HitboxInitialization();
            }


            private void HitboxInitialization()
            {
                if (engineType == EngineType.RSDKv5)
                {
                    HitBoxes = new List<EditorHitbox>();
                    for (int i = 0; i < Parent.Parent.CollisionBoxes.Count; i++)
                    {
                        HitBoxes.Add(new EditorHitbox());
                    }
                }
                else if (engineType != EngineType.Invalid)
                {
                    //TODO : Validate this Works as the Default Hitbox Initalization for Older Versions
                    HitBoxes = new List<EditorHitbox>();
                    for (int i = 0; i < Parent.Parent.RetroCollisionBoxes.Count; i++)
                    {
                        HitBoxes.Add(new EditorHitbox());
                    }
                }

            }

            public void ImportFrom(EngineType type, string filepath)
            {
                engineType = type;
                AnimationEditor.Methods.AnimationReader.RSDKvU_Import_Frame(this, filepath);
            }
            public void ExportTo(EngineType type, string filepath)
            {
                AnimationEditor.Methods.AnimationWriter.RSDKvU_Export_Frame(this, filepath);
            }

            #region Frames

            public System.Windows.Media.Imaging.BitmapSource Sprite
            {
                get
                {
                    return Services.GlobalService.SpriteService.GetCroppedFrame(this.SpriteSheet, this);
                }
            }

            #endregion

        }
        [Serializable]
        public class EditorHitbox
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
                AnimationEditor.Methods.AnimationReader.RSDKvU_Import_Hitbox(this, filepath);
            }
            public void ExportTo(EngineType type, string filepath)
            {
                AnimationEditor.Methods.AnimationWriter.RSDKvU_Export_Hitbox(this, filepath);
            }
        }
        [Serializable]
        public class EditorRetroHitBox
        {
            public struct HitboxInfo
            {
                public sbyte Left;
                public sbyte Top;
                public sbyte Right;
                public sbyte Bottom;
            }

            public HitboxInfo[] Hitboxes = new HitboxInfo[8];

            public EditorRetroHitBox()
            {

            }

            public void FromBridgedHitBox(List<EditorHitbox> value)
            {
                for (int i = 0; i < 8; i++)
                {
                    Hitboxes[i].Bottom = (sbyte)value[i].Bottom;
                    Hitboxes[i].Top = (sbyte)value[i].Top;
                    Hitboxes[i].Left = (sbyte)value[i].Left;
                    Hitboxes[i].Right = (sbyte)value[i].Right;
                }
            }

            public List<EditorHitbox> ToBridgedHitBox()
            {
                List<EditorHitbox> bridgedHitBoxes = new List<EditorHitbox>();
                for (int i = 0; i < 8; i++)
                {
                    var box = new EditorHitbox();
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
        public List<EditorRetroHitBox> RetroCollisionBoxes { get; set; } = new List<EditorRetroHitBox>();
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
        public ObservableCollection<string> SpriteSheets { get; set; } = new ObservableCollection<string>();
        public List<string> CollisionBoxes { get => CollisionBoxesList; set => CollisionBoxesList = value; }
        private List<string> CollisionBoxesList { get; set; } = new List<string>();
        public List<EditorAnimationInfo> Animations { get; set; } = new List<EditorAnimationInfo>();

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

        public EditorAnimation(EngineType type)
        {
            Animations.Add(new EditorAnimationInfo(type, this));
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
                    AnimationEditor.Methods.AnimationReader.RSDKv5_Load_Animation(this, filepath);
                    break;
                case EngineType.RSDKvB:
                    AnimationEditor.Methods.AnimationReader.RSDKvB_Load_Animation(this, filepath);
                    break;
                case EngineType.RSDKv2:
                    AnimationEditor.Methods.AnimationReader.RSDKv2_Load_Animation(this, filepath);
                    break;
                case EngineType.RSDKv1:
                    AnimationEditor.Methods.AnimationReader.RSDKv1_Load_Animation(this, filepath);
                    break;
                case EngineType.RSDKvRS:
                    AnimationEditor.Methods.AnimationReader.RSDKvRS_Load_Animation(this, filepath);
                    break;
            }
        }

        public void SaveTo(EngineType type, string filepath)
        {
            switch (EngineType)
            {
                case EngineType.RSDKv5:
                    AnimationEditor.Methods.AnimationWriter.RSDKv5_Save_Animation(this, filepath);
                    break;
                case EngineType.RSDKvB:
                    AnimationEditor.Methods.AnimationWriter.RSDKvB_Save_Animation(this, filepath);
                    break;
                case EngineType.RSDKv2:
                    AnimationEditor.Methods.AnimationWriter.RSDKv2_Save_Animation(this, filepath);
                    break;
                case EngineType.RSDKv1:
                    AnimationEditor.Methods.AnimationWriter.RSDKv1_Save_Animation(this, filepath);
                    break;
                case EngineType.RSDKvRS:
                    AnimationEditor.Methods.AnimationWriter.RSDKvRS_Save_Animation(this, filepath);
                    break;
            }
        }

    }
}
