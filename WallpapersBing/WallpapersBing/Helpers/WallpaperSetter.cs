using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WallpapersBing.Helpers
{
    class WallpaperSetter
    {
        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, string pvParam, UInt32 fWinIni);
        const uint SPI_SETDESKWALLPAPER = 0x14;
        const uint SPIF_UPDATEINIFILE = 0x01;
        const uint SPIF_SENDWININICHANGE = 0x02;

        public static bool SetWallpaper(string path) =>
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
    }
}
