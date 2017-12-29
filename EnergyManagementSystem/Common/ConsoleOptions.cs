using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;

namespace EMS.Common
{
	public static class ConsoleOptions
	{
		#region WindowsOption
		public static void SetWindowOptions(ConsoleColor consoleForegroundColor, int column, int row, int numberOfColumns = 3, int numerOfRows = 3)
		{
			Console.WindowWidth = 50;
			Console.WindowHeight = 1;
			Console.BufferWidth = 50;
			Console.BufferHeight = 1;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = consoleForegroundColor;
			double scalingFactorX = GetScalingFactorX();
			double scalingFactorY = scalingFactorX;

			int consoleWidth = (int)(SystemParameters.FullPrimaryScreenWidth * scalingFactorX / numberOfColumns) + 5;
			int consoleHeight = (int)(SystemParameters.FullPrimaryScreenHeight * scalingFactorY / numerOfRows) + 5;
			int consoleX = (int)(column * SystemParameters.FullPrimaryScreenWidth * scalingFactorX / numberOfColumns) ;
			int consoleY = (int)(row * SystemParameters.FullPrimaryScreenHeight * scalingFactorY / numerOfRows);

			SetWindowPosition(consoleX, consoleY, consoleWidth, consoleHeight);
		}

		const int SWP_NOZORDER = 0x4;
		const int SWP_NOACTIVATE = 0x10;

		[DllImport("kernel32")]
		static extern IntPtr GetConsoleWindow();


		[DllImport("user32")]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
			int x, int y, int cx, int cy, int flags);

		/// <summary>
		/// Sets the console window location and size in pixels
		/// </summary>
		public static void SetWindowPosition(int x, int y, int width, int height)
		{
			SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
		}

		public static IntPtr Handle
		{
			get
			{
				return GetConsoleWindow();
			}
		}

		[DllImport("gdi32.dll")]
		static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
		public enum DeviceCap
		{
			DESKTOPVERTRES = 117,
			DESKTOPHORZRES = 118
			// http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
		}


		public static float GetScalingFactorY()
		{
			Graphics g = Graphics.FromHwnd(IntPtr.Zero);
			IntPtr desktop = g.GetHdc();
			int LogicalScreenHeight = (int)SystemParameters.FullPrimaryScreenHeight;
			int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

			float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

			return ScreenScalingFactor; // 1.25 = 125%
		}

		public static float GetScalingFactorX()
		{
			Graphics g = Graphics.FromHwnd(IntPtr.Zero);
			IntPtr desktop = g.GetHdc();
			int LogicalScreenWidth = (int)SystemParameters.FullPrimaryScreenWidth;
			int PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPHORZRES);

			float ScreenScalingFactor = (float)PhysicalScreenWidth / (float)LogicalScreenWidth;

			return ScreenScalingFactor; // 1.25 = 125%
		}
		#endregion
	}
}
