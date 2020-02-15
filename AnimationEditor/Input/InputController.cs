using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using AnimationEditor.Pages;

namespace AnimationEditor.Services
{
    public class InputController
    {
        private MainWindow Instance;
        public InputController(MainWindow instance)
        {
            Instance = instance;
        }

        Point AnchorPoint = new Point(0, 0);
        bool HitboxTopLeft = false;
        bool HitboxTopRight = false;
        bool HitboxBottomLeft = false;
        bool HitboxBottomRight = false;

        bool FrameTopLeft = false;
        bool FrameTopRight = false;
        bool FrameBottomLeft = false;
        bool FrameBottomRight = false;

        double dirX;
        double dirY;

        public void MouseMove(object sender, MouseEventArgs e)
        {
            //TODO: Handle Zooming
            double rate = Instance.ViewModel.Zoom;
            double x1 = AnchorPoint.X;
            double y1 = AnchorPoint.Y;
            double x2 = e.GetPosition(Instance).X;
            double y2 = e.GetPosition(Instance).Y;

            UpdateRegions();
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Instance.ButtonShowFieldHitbox.IsChecked == true)
                {
                    dirX = (x2 - x1) / rate;
                    dirY = (y2 - y1) / rate;

                    if (dirX >= 1 || dirX <= -1)
                    {
                        AdjustHitboxSize((int)dirX, (int)dirY, e, true, false);
                    }
                    if (dirY >= 1 || dirY <= -1)
                    {
                        AdjustHitboxSize((int)dirX, (int)dirY, e, false, true);
                    }
                }
                else
                {
                    dirX = (x2 - x1) / rate;
                    dirY = (y2 - y1) / rate;

                    if (IsAltPressed())
                    {
                        AdjustFrameOffset(dirX, dirY, e);
                    }
                    else
                    {
                        AdjustFrameSize(dirX, dirY, e);
                    }
                }
            }
        }


        public void MouseDown(object sender, MouseEventArgs e)
        {
            if (Instance.HitboxSection1.IsMouseOver) HitboxTopLeft = true;
            else if (Instance.HitboxSection2.IsMouseOver) HitboxTopRight = true;
            else if (Instance.HitboxSection3.IsMouseOver) HitboxBottomLeft = true;
            else if (Instance.HitboxSection4.IsMouseOver) HitboxBottomRight = true;

            if (Instance.ImageSection1.IsMouseOver) FrameTopLeft = true;
            else if (Instance.ImageSection2.IsMouseOver) FrameTopRight = true;
            else if (Instance.ImageSection3.IsMouseOver) FrameBottomLeft = true;
            else if (Instance.ImageSection4.IsMouseOver) FrameBottomRight = true;

            Instance.CanvasView.Focus();
            AnchorPoint = e.GetPosition(Instance);
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            AnchorPoint = e.GetPosition(Instance);
            HitboxTopLeft = false;
            HitboxTopRight = false;
            HitboxBottomLeft = false;
            HitboxBottomRight = false;
            FrameTopLeft = false;
            FrameTopRight = false;
            FrameBottomLeft = false;
            FrameBottomRight = false;
        }

        private void AdjustHitboxSize(int distanceX, int distanceY, MouseEventArgs e, bool resetX = false, bool resetY = false)
        {
            if (HitboxBottomRight)
            {
                if (Instance.HitboxBottomNUD.Value + distanceY >= 1)
                {
                    Instance.HitboxBottomNUD.Value += (int)(distanceY);
                    AnchorPoint = e.GetPosition(Instance);
                    dirY = 0;
                }
                if (Instance.HitboxRightNUD.Value + distanceX >= 1)
                {
                    Instance.HitboxRightNUD.Value += (int)(distanceX);
                    AnchorPoint = e.GetPosition(Instance);
                    dirX = 0;
                }
            }
            if (HitboxBottomLeft)
            {
                if (Instance.HitboxBottomNUD.Value + distanceY >= 1)
                {
                    Instance.HitboxBottomNUD.Value += (int)(distanceY);
                    AnchorPoint = e.GetPosition(Instance);
                    dirY = 0;
                }
                if (Instance.HitboxLeftNUD.Value + distanceX <= -1)
                {
                    Instance.HitboxLeftNUD.Value += (int)(distanceX);
                    AnchorPoint = e.GetPosition(Instance);
                    dirX = 0;
                }
            }
            if (HitboxTopRight)
            {
                if (Instance.HitboxTopNUD.Value + distanceY <= -1)
                {
                    Instance.HitboxTopNUD.Value += (int)(distanceY);
                    AnchorPoint = e.GetPosition(Instance);
                    dirY = 0;
                }
                if (Instance.HitboxRightNUD.Value + distanceX >= 1)
                {
                    Instance.HitboxRightNUD.Value += (int)(distanceX);
                    AnchorPoint = e.GetPosition(Instance);
                    dirX = 0;
                }
            }
            if (HitboxTopLeft)
            {
                if (Instance.HitboxTopNUD.Value + distanceY <= -1)
                {
                    Instance.HitboxTopNUD.Value += (int)(distanceY);
                    AnchorPoint = e.GetPosition(Instance);
                    dirY = 0;
                }
                if (Instance.HitboxLeftNUD.Value + distanceX <= -1)
                {
                    Instance.HitboxLeftNUD.Value += (int)(distanceX);
                    AnchorPoint = e.GetPosition(Instance);
                    dirX = 0;
                }
            }
        }

        private void AdjustFrameOffset(double distanceX, double distanceY, MouseEventArgs e)
        {
            double rate = Instance.ViewModel.Zoom;

            if (dirY >= 1 || dirY <= -1)
            {
                Instance.PivotY_NUD.Value += (int)(distanceY);
                AnchorPoint = e.GetPosition(Instance);
                dirY = 0;
            }

            if (dirX >= 1 || dirX <= -1)
            {
                Instance.PivotX_NUD.Value += (int)(distanceX);
                AnchorPoint = e.GetPosition(Instance);
                dirX = 0;
            }
        }

        public bool IsCtrlPressed()
        {
            return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        }

        public bool IsAltPressed()
        {
            return Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
        }

        public bool IsShiftPressed()
        {
            return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        }

        private void AdjustFrameSize(double distanceX, double distanceY, MouseEventArgs e)
        {
            if (IsCtrlPressed()) AdjustNUDs(Instance.FrameHeightNUD, Instance.FrameWidthNUD, true, true, IsCtrlPressed(), IsCtrlPressed());
            if (IsShiftPressed()) AdjustNUDs(Instance.FrameTopNUD, Instance.FrameLeftNUD, false, false, IsShiftPressed(), IsShiftPressed());

            void AdjustNUDs(Xceed.Wpf.Toolkit.IntegerUpDown Y, Xceed.Wpf.Toolkit.IntegerUpDown X, bool NegX = false, bool NegY = false, bool XAllowed = false, bool YAllowed = false)
            {
                Instance.Interfacer.UpdateFrameNUDMaxMin();
                if (dirY >= 1 || dirY <= -1)
                {
                    int YResult = Y.Value.Value - (NegY ? -(int)distanceY : (int)distanceY);
                    if (YAllowed)
                    {
                        if (YResult <= Y.Maximum && YResult >= Y.Minimum) Y.Value = YResult;
                        else if (YResult > Y.Maximum) Y.Value = Y.Maximum;
                        else if (YResult < Y.Minimum) Y.Value = Y.Minimum;
                    }


                    AnchorPoint = e.GetPosition(Instance);
                    dirY = 0;
                }
                Instance.Interfacer.UpdateFrameNUDMaxMin();
                if (dirX >= 1 || dirX <= -1)
                {
                    int XResult = X.Value.Value - (NegX ? -(int)distanceX : (int)distanceX);
                    if (XAllowed)
                    {
                        if (XResult <= X.Maximum && XResult >= X.Minimum) X.Value = XResult;
                        else if (XResult > X.Maximum) X.Value = X.Maximum;
                        else if (XResult < X.Minimum) X.Value = X.Minimum;
                    }


                    AnchorPoint = e.GetPosition(Instance);
                    dirX = 0;
                }
            }

        }

        private void UpdateRegions()
        {
            UpdateGrid(Instance.HitboxSection1);
            UpdateGrid(Instance.HitboxSection2);
            UpdateGrid(Instance.HitboxSection3);
            UpdateGrid(Instance.HitboxSection4);

            void UpdateGrid(System.Windows.Controls.Grid g, bool CtrlOnly = false)
            {

                if (g.IsMouseOver && (CtrlOnly ? IsCtrlPressed() : true))
                {
                    g.Background = new SolidColorBrush(Colors.White);
                    g.Opacity = 0.5;
                }
                else
                {
                    g.Background = new SolidColorBrush(Colors.Transparent);
                    g.Opacity = 0.1;
                }
            }




        }


        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (IsCtrlPressed())
                {
                    Instance.FrameHeightNUD.Value += 1;
                }
                else if (IsAltPressed())
                {
                    Instance.PivotY_NUD.Value += 1;
                }
                else if (IsShiftPressed())
                {
                    Instance.FrameTopNUD.Value += 1;
                }
            }
            if (e.Key == Key.Up)
            {
                if (IsCtrlPressed())
                {
                    Instance.FrameHeightNUD.Value -= 1;
                }
                else if (IsAltPressed())
                {
                    Instance.PivotY_NUD.Value -= 1;
                }
                else if (IsShiftPressed())
                {
                    Instance.FrameTopNUD.Value -= 1;
                }
            }
            if (e.Key == Key.Left)
            {
                if (IsCtrlPressed())
                {
                    Instance.FrameWidthNUD.Value -= 1;
                }
                else if (IsAltPressed())
                {
                    Instance.PivotX_NUD.Value -= 1;
                }
                else if (IsShiftPressed())
                {
                    Instance.FrameLeftNUD.Value -= 1;
                }
            }
            if (e.Key == Key.Right)
            {
                if (IsCtrlPressed())
                {
                    Instance.FrameWidthNUD.Value += 1;
                }
                else if (IsAltPressed())
                {
                    Instance.PivotX_NUD.Value += 1;
                }
                else if (IsShiftPressed())
                {
                    Instance.FrameLeftNUD.Value += 1;
                }
            }
        }
    }
}
