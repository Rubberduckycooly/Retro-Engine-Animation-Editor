using AnimationEditor.Classes;
using System.Collections.Generic;
using System.Linq;

namespace AnimationEditor.Methods
{
    public static class AnimationReader
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

        #region Import (Any) (RSDKvU Format)
        public static void RSDKvU_Import_AnimEntry(EditorAnimation.EditorAnimationInfo animEntry, string filepath)
        {
            RSDKv5.Animation.AnimationEntry animv5 = new RSDKv5.Animation.AnimationEntry(new RSDKv5.Reader(filepath));
            RSDKv5_Load_AnimEntry(animEntry, animv5);
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
