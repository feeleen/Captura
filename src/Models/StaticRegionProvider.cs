﻿using Screna;
using System.Drawing;
using System.Windows;

namespace Captura
{
    class StaticRegionProvider : IImageProvider
    {
        readonly RegionSelector _regSel;

        public StaticRegionProvider(RegionSelector RegSel)
        {
            _regSel = RegSel;

            Height = (int) RegSel.Height;
            Width = (int) RegSel.Width;

            RegSel.ResizeMode = ResizeMode.NoResize;
        }
        
        public Bitmap Capture()
        {
            return ScreenShot.Capture(_regSel.Rectangle);
        }

        public int Height { get; }

        public int Width { get; }

        public void Dispose() => _regSel.ResizeMode = ResizeMode.CanResize;
    }
}