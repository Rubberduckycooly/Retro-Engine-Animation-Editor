using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimationEditor.Methods;

namespace AnimationEditor.Services
{
    public static class GlobalService
    {
        public static PropertyService PropertyHandler { get; set; }
        public static InputController InputControl { get; set; }
        public static FileService FileHandler { get; set; }
        public static SpriteService SpriteService { get; set; }
        public static UIService UIService { get; set; }
        public static PlaybackService PlaybackService { get; set; }
    }
}
