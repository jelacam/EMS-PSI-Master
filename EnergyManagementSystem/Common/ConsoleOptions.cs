using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace EMS.Common
{
	public static class ConsoleOptions
	{
		#region WindowsOption
		public static void SetWindowOptions(ConsoleColor consoleForegroundColor, int column, int row, int numberOfColumns = 2, int numerOfRows = 3)
		{
			Console.WindowWidth = 50;
			Console.WindowHeight = 1;
			Console.BufferWidth = 50;
			Console.BufferHeight = 1;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = consoleForegroundColor;

			int consoleWidth = (int)(SystemParameters.FullPrimaryScreenWidth / numberOfColumns) + 5;
			int consoleHeight = (int)(SystemParameters.FullPrimaryScreenHeight / numerOfRows) + 5;
			int consoleX = (int)(column * SystemParameters.FullPrimaryScreenWidth / numberOfColumns) ;
			int consoleY = (int)(row * SystemParameters.FullPrimaryScreenHeight / numerOfRows);

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
		#endregion
	}
}
