using AnimationEditor.Classes;
using AnimationEditor.Pages;
using AnimationEditor.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NUD = Xceed.Wpf.Toolkit.IntegerUpDown;
using GenerationsLib.WPF;

namespace AnimationEditor.Services
{
    public class PropertyService
    {
        #region On Property Changed Extension

        private void OnPropertyChanged(string entry)
        {
            Instance.ViewModel.CallPropertyChanged(entry);
        }

        #endregion

        #region Definitions

        #region UI
        public bool PreventIndexUpdate { get; set; } = false;
        private MainWindow Instance;
        #endregion

        #region Bitmaps
        public SkiaSharp.SKBitmap CurrentSpriteSheet;
        public SkiaSharp.SKBitmap CurrentSpriteSheetFrame;
        public string CurrentSpriteSheetName;

        #endregion

        #region Modes
        public bool ShowFrameBorder { get; set; } = false;
        public bool ShowSolidImageBackground { get; set; } = false;
        public bool SetBackgroundColorToMatchSpriteSheet { get; set; } = false;
        public bool ForceCenterFrame { get; set; } = false;
        public bool ShowHitBox { get; set; } = false;
        public bool ShowAlignmentLines { get; set; } = false;
        public bool ShowFullFrame { get; set; } = false;
        public bool isPlaybackEnabled { get; set; } = false;

        #endregion

        #region Playback Settings
        public bool isForcePlaybackOn { get; set; } = false;
        public int ForcePlaybackDuration { get; set; } = 256;
        public int ForcePlaybackSpeed { get; set; } = 128;
        #endregion

        #region Colors
        public Color AlignmentLinesColor { get; set; } = (Color)ColorConverter.ConvertFromString("#FFFF0000");
        public Color CanvasBackground { get; set; } = (Color)ColorConverter.ConvertFromString("#303030");
        public Color HitboxBackground { get; set; } = (Color)ColorConverter.ConvertFromString("#FFE700FF");
        public Color FrameBorder { get; set; } = Colors.Black;
        public Color FrameBackground { get; set; } = Colors.Transparent;
        #endregion

        #region Opacity
        private double _RefrenceOpacity = 100;
        public double RefrenceOpacity { get => _RefrenceOpacity; set => _RefrenceOpacity = value * 0.01; }
        #endregion

        #region Status

        bool isAnimationLoaded
        {
            get => Instance.ViewModel != null && Instance.ViewModel.LoadedAnimationFile != null;
        }
        bool isHitboxesValid
        {
            get => Instance.ViewModel.Hitboxes != null && Instance.ViewModel.SelectedHitbox != null && Instance.ViewModel.Hitboxes.Count != 0;
        }
        bool isCurrentSpriteSheetsValid
        {
            get => Instance.ViewModel.CurrentFrame_SpriteSheet != null && Instance.ViewModel.SpriteSheets != null;
        }
        bool isCurrentSpriteSheetOutOfRange
        {
            get => Instance.ViewModel.CurrentFrame_SpriteSheet.Value - 1 > Instance.ViewModel.SpriteSheets.Count;
        }
        bool isSpriteSheetCountNotZero
        {
            get => Instance.ViewModel.SpriteSheets.Count > 0;
        }

        #endregion



        #endregion

        #region Init
        public PropertyService(MainWindow window)
        {
            Instance = window;
        }
        #endregion

        #region Update Properties

        public void InvalidateSelectionProperties()
        {
            UpdateSelectedAnimationProperties();
            UpdateSelectedItemProperties();

            UpdateSelectedFrameProperties();
            UpdateSpriteSheetProperties();

            if (isHitboxesValid) UpdateSelectedHitboxProperties();
        }
        public void InvalidateSprite()
        {
            Instance.ViewModel.InvalidateCroppedFrame(Instance.ViewModel.SelectedFrameIndex);
        }

        public void UpdateSelectedItemProperties()
        {
            int LastSelectedAnimationIndex = Instance.ViewModel.SelectedAnimationIndex;

            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationEntries));

            if (isAnimationLoaded)
            {
                Instance.ViewModel.SelectedAnimationIndex = Instance.ViewModel.GetIndexWithinRange(Instance.ViewModel.SelectedAnimationEntries.Count, LastSelectedAnimationIndex);
            }
            else
            {
                Instance.ViewModel.SelectedAnimationIndex = -1;
            }

            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimation));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedFrameIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationEntries));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationFrameSet));
        }
        public void UpdateSpriteSheetProperties()
        {
            OnPropertyChanged(nameof(Instance.ViewModel.SpriteSheetPaths));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_SpriteSheet));
        }
        public void UpdateSelectedHitboxProperties()
        {
            OnPropertyChanged(nameof(Instance.ViewModel.Hitboxes));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedFrameHitboxIndex));

            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxLeft));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxRight));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxTop));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxBottom));
        }
        public void UpdateSelectedFrameProperties()
        {
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Width));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Height));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_X));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Y));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_PivotX));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_PivotY));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_FrameID));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_FrameDuration));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_CollisionBox));
        }
        public void UpdateSelectedAnimationProperties()
        {
            OnPropertyChanged(nameof(Instance.ViewModel.Speed));
            OnPropertyChanged(nameof(Instance.ViewModel.Loop));
            OnPropertyChanged(nameof(Instance.ViewModel.PlayerType));
            OnPropertyChanged(nameof(Instance.ViewModel.Flags));
        }

        #endregion

        #region Methods
        public void UpdateControls()
        {
            InvalidateSelectionProperties();

            GlobalService.UIService.UpdateIndexStatusVisual();
            GlobalService.UIService.UpdateEntrySections();

            GlobalService.UIService.UpdateFrameSections();
            GlobalService.UIService.UpdateGeneralSections();
            GlobalService.UIService.UpdateTypeLimitationsSections();

            UpdateCanvasVisual();
            Instance.CanvasView.InvalidateVisual();
        }
        public void UnloadControls()
        {
            //GlobalService.UIService.ToggleFrameSectionEvents(false);
            //GlobalService.UIService.ToggleHitboxSectionEvents(false);
            //GlobalService.UIService.ToggleEntrySectionEvents(false);
            //GlobalService.UIService.ToggleListSelectionEvents(false);

            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Width));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Height));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_X));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_Y));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_PivotX));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_PivotY));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_FrameID));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_FrameDuration));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_CollisionBox));
            OnPropertyChanged(nameof(Instance.ViewModel.CurrentFrame_SpriteSheet));
            OnPropertyChanged(nameof(Instance.ViewModel.Hitboxes));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedFrameHitboxIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxLeft));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxRight));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxTop));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedHitboxBottom));

            OnPropertyChanged(nameof(Instance.ViewModel.Speed));
            OnPropertyChanged(nameof(Instance.ViewModel.Loop));
            OnPropertyChanged(nameof(Instance.ViewModel.PlayerType));
            OnPropertyChanged(nameof(Instance.ViewModel.Flags));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationEntries));

            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedFrameIndex));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationFrameSet));
            OnPropertyChanged(nameof(Instance.ViewModel.SelectedAnimationEntries));

            OnPropertyChanged(nameof(Instance.ViewModel.SpriteSheetPaths));
        }
        public void MoveToAdjacentFrameIndex(bool subtract = false, bool updateUI = true)
        {
            if (Instance.ViewModel.SelectedAnimationFrameSet != null && Instance.ViewModel.SelectedAnimationFrameSet.Count > 0)
            {
                if (subtract)
                {
                    if (Instance.ViewModel.SelectedFrameIndex - 1 > -1) Instance.ViewModel.SelectedFrameIndex--;
                }
                else
                {
                    if (Instance.ViewModel.SelectedFrameIndex + 1 < Instance.FramesList.Items.Count) Instance.ViewModel.SelectedFrameIndex++;
                    else Instance.ViewModel.SelectedFrameIndex = (Instance.ViewModel.Loop != null ? Instance.ViewModel.Loop.Value : 0);
                }
                Instance.FramesList.ScrollIntoView(Instance.FramesList.SelectedItem);
                if (updateUI)
                {
                    InvalidateSelectionProperties();
                    UpdateControls();
                }
            }
        }
        public void RefreshUIThemes()
        {
            /*
            Instance.ImportAnimationContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.ExportAnimationContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.ImportFrameContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.ExportFrameContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.HelpContext.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;
            Instance.PlaybackOptionsContextMenu.Style = Application.Current.FindResource("DefaultContextMenuStyle") as Style;

            Instance.ImportAnimationContext.Refresh(); 
            Instance.ExportAnimationContext.Items.Refresh();
            Instance.ImportFrameContext.Items.Refresh();
            Instance.ExportFrameContext.Items.Refresh();
            Instance.HelpContext.Items.Refresh();
            Instance.PlaybackOptionsContextMenu.Items.Refresh();
            */
        }
        public void FixAnimationProperties()
        {
            if (isCurrentSpriteSheetsValid && isCurrentSpriteSheetOutOfRange) Instance.ViewModel.CurrentFrame_SpriteSheet = 0;
        }

        #endregion

        #region Drawing/Rendering

        #region Update Bitmaps
        public void UpdateSheetImage()
        {
            if (Instance.ViewModel.SpriteSheets == null || Instance.ViewModel.SpriteSheets.Count == 0)
            {
                CurrentSpriteSheet = null;
                return;
            }


            if (Instance.MenuViewTransparentSpriteSheets.IsChecked)
            {
                var image = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentFrame_SpriteSheet.Value];
                if (image.isReady) CurrentSpriteSheet = SkiaSharp.Views.WPF.WPFExtensions.ToSKBitmap(image.TransparentImage);
                else CurrentSpriteSheet = null;
            }
            else
            {
                var image = Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentFrame_SpriteSheet.Value];
                if (image.isReady) CurrentSpriteSheet = SkiaSharp.Views.WPF.WPFExtensions.ToSKBitmap(image.Image);
                else CurrentSpriteSheet = null;
            }
        }
        public void UpdateFrameImage()
        {
            if (Instance.ViewModel.SpriteSheets == null) return;
            if (CurrentSpriteSheet == null) return;

            double val_x = Instance.ViewModel.CurrentFrame_X.Value;
            double val_y = Instance.ViewModel.CurrentFrame_Y.Value;
            double val_width = Instance.ViewModel.CurrentFrame_Width.Value;
            double val_height = Instance.ViewModel.CurrentFrame_Height.Value;

            if (val_width != 0 && val_height != 0)
            {
                try
                {
                    System.Drawing.Bitmap sourceImage = SkiaSharp.Views.Desktop.Extensions.ToBitmap(CurrentSpriteSheet);
                    System.Drawing.Bitmap croppedImg = (System.Drawing.Bitmap)Extensions.BitmapExtensions.CropImage(sourceImage, new System.Drawing.Rectangle((int)val_x, (int)val_y, (int)val_width, (int)val_height));
                    BitmapImage croppedBitmapImage = (BitmapImage)Extensions.BitmapExtensions.ToWpfBitmap(croppedImg);
                    CurrentSpriteSheetFrame = SkiaSharp.Views.WPF.WPFExtensions.ToSKBitmap(croppedBitmapImage);

                    sourceImage.Dispose();
                    sourceImage = null;

                    croppedImg.Dispose();
                    croppedImg = null;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

            }
        }

        #endregion
        public void UpdateCanvasVisual()
        {
            FixAnimationProperties();

            UpdateSheetImage();
            UpdateFrameImage();

            UpdateCanvasBackgroundColor();
            Instance.CanvasView.InvalidateVisual();
        }
        public void PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var canvas = e.Surface.Canvas;
            float Zoom = (float)Instance.ViewModel.Zoom;

            canvas.Scale(Zoom);

            float width = info.Width / Zoom;
            float height = info.Height / Zoom;

            float width_half = width / 2;
            float height_half = height / 2;

            canvas.Clear(SkiaSharp.SKColors.Transparent);

            if (CurrentSpriteSheet != null || CurrentSpriteSheetFrame != null)
            {
                DrawSprite(canvas, width_half, height_half, width, height);
                if (ShowAlignmentLines) DrawAlignmentLines(canvas, width_half, height_half, width, height);
            }
        }
        public void DrawAlignmentLines(SkiaSharp.SKCanvas canvas, float width_half, float height_half, float width, float height)
        {
            SkiaSharp.SKPoint x1 = new SkiaSharp.SKPoint(0, height_half);
            SkiaSharp.SKPoint y1 = new SkiaSharp.SKPoint(width, height_half);
            SkiaSharp.SKPoint x2 = new SkiaSharp.SKPoint(width_half, 0);
            SkiaSharp.SKPoint y2 = new SkiaSharp.SKPoint(width_half, height);

            canvas.DrawLine(x1, y1, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
            canvas.DrawLine(x2, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(AlignmentLinesColor) });
        }
        public void DrawFrameBorder(SkiaSharp.SKCanvas canvas, float bx, float by, float w, float h)
        {
            SkiaSharp.SKPoint x1 = new SkiaSharp.SKPoint(bx, by);
            SkiaSharp.SKPoint x2 = new SkiaSharp.SKPoint(bx + w, by);
            SkiaSharp.SKPoint y1 = new SkiaSharp.SKPoint(bx, by + h);
            SkiaSharp.SKPoint y2 = new SkiaSharp.SKPoint(bx + w, by + h);

            canvas.DrawLine(x1, x2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBorder) });
            canvas.DrawLine(y1, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBorder) });
            canvas.DrawLine(x1, y1, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBorder) });
            canvas.DrawLine(x2, y2, new SkiaSharp.SKPaint() { Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBorder) });

            var paint = new SkiaSharp.SKPaint();
            var transparency = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(FrameBackground);
            paint.Color = transparency;

            canvas.DrawRect(new SkiaSharp.SKRect() { Top = by, Left = bx, Size = new SkiaSharp.SKSize(w, h) }, paint);
        }
        public void DrawHitbox(SkiaSharp.SKCanvas canvas, float center_x, float center_y)
        {
            float l = center_x - Instance.ViewModel.SelectedHitboxLeft.Value;
            float r = center_x - Instance.ViewModel.SelectedHitboxRight.Value;

            float b = center_y + Instance.ViewModel.SelectedHitboxBottom.Value;
            float t = center_y + Instance.ViewModel.SelectedHitboxTop.Value;

            var paint = new SkiaSharp.SKPaint();
            paint.Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(HitboxBackground);

            var SemiOpacity = GenerationsLib.WPF.ColorExt.ToSWMColor(System.Drawing.Color.FromArgb(128, GenerationsLib.WPF.ColorExt.ToSDColor(HitboxBackground)));
            var paint2 = new SkiaSharp.SKPaint();
            paint2.Color = SkiaSharp.Views.WPF.WPFExtensions.ToSKColor(SemiOpacity);


            SkiaSharp.SKPoint x1 = new SkiaSharp.SKPoint(l, t);
            SkiaSharp.SKPoint y1 = new SkiaSharp.SKPoint(r, t);
            SkiaSharp.SKPoint x2 = new SkiaSharp.SKPoint(l, b);
            SkiaSharp.SKPoint y2 = new SkiaSharp.SKPoint(r, b);

            canvas.DrawLine(x1, y1, paint);
            canvas.DrawLine(x2, y2, paint);
            canvas.DrawLine(x1, x2, paint);
            canvas.DrawLine(y1, y2, paint);

            canvas.DrawRect(new SkiaSharp.SKRect() { Top = t, Left = l, Bottom = b, Right = r }, paint2);
        }
        private void DrawSprite(SkiaSharp.SKCanvas canvas, float width_half, float height_half, float width, float height)
        {
            if (CurrentSpriteSheetFrame == null || CurrentSpriteSheet == null) return;

            int frame_x = (int)Instance.ViewModel.CurrentFrame_X;
            int frame_y = (int)Instance.ViewModel.CurrentFrame_Y;

            int frame_width = (int)Instance.ViewModel.CurrentFrame_Width;
            int frame_height = (int)Instance.ViewModel.CurrentFrame_Height;

            if (frame_width == 0 || frame_height == 0)
            {
                return;
            }

            int frame_center_x = (ForceCenterFrame ? frame_width / 2 : -(int)Instance.ViewModel.CurrentFrame_PivotX.Value);
            int frame_center_y = (ForceCenterFrame ? frame_height / 2 : -(int)Instance.ViewModel.CurrentFrame_PivotY.Value);

            float img_center_x = width_half - frame_center_x;
            float img_center_y = height_half - frame_center_y;

            float hitbox_center_x = width_half;
            float hitbox_center_y = height_half;

            float img_full_center_x = width_half - frame_x - frame_center_x;
            float img_full_center_y = height_half - frame_y - frame_center_y;

            float img_full_border_center_x = width_half - frame_center_x;
            float img_full_border_center_y = height_half - frame_center_y;

            float x;
            float y;
            float w;
            float h;


            float bx;
            float by;



            if (ShowFullFrame)
            {
                x = img_full_center_x;
                y = img_full_center_y;
                w = frame_width;
                h = frame_height;

                bx = img_center_x;
                by = img_center_y;
            }
            else
            {
                x = img_center_x;
                y = img_center_y;
                w = frame_width;
                h = frame_height;

                bx = x;
                by = y;
            }

            canvas.DrawBitmap((ShowFullFrame ? CurrentSpriteSheet : CurrentSpriteSheetFrame), new SkiaSharp.SKPoint(x, y));

            if (ShowFrameBorder) DrawFrameBorder(canvas, bx, by, w, h);

            if (ShowHitBox) DrawHitbox(canvas, hitbox_center_x, hitbox_center_y);


        }
        public void UpdateCanvasBackgroundColor()
        {
            if (SetBackgroundColorToMatchSpriteSheet && isCurrentSpriteSheetsValid && isSpriteSheetCountNotZero) Instance.CanvasBackground.Background = new SolidColorBrush(Instance.ViewModel.SpriteSheets[Instance.ViewModel.CurrentFrame_SpriteSheet.Value].TransparentColor);
            else if (Instance.BGColorPicker.SelectedColor != null) Instance.CanvasBackground.Background = new SolidColorBrush(CanvasBackground);
        }

        #endregion
    }
}
