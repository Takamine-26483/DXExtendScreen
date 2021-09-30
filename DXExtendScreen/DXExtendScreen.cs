using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DxLibDLL;

namespace TakamineProduction
{
	/// <summary>任意の倍率に拡大表示可能な描画可能グラフィック</summary>
	public class DXExtendScreen
	{
		private double extendRate = 0;

		/// <summary>描画可能グラフィックのハンドル</summary>
		public int Handle { get; private set; }
		/// <summary>描画可能グラフィックのドットバイドットでのサイズ</summary>
		public Size ScreenOriginalSize { get; private set; }
		/// <summary>描画可能グラフィックを実際に描画する際のサイズ</summary>
		public Size ScreenSize { get; private set; }
		/// <summary>拡大倍率（変更するとRemakeが実行される）</summary>
		public double ExtendRate
		{
			get => extendRate;
			set
			{
				extendRate = value;
				Remake();
			}
		}
		/// <summary>コンストラクタ。描画可能グラフィックを作成する</summary>
		/// <param name="extendRate">拡大倍率</param>
		public DXExtendScreen(double extendRate)
		=> ExtendRate = extendRate;
		/// <summary>現在のクライアント領域に合わせて各プロパティを再設定する。ハンドルは再作成される（SetDrawScreenでこのオブジェクトのハンドルを指定していた場合、再設定する）</summary>
		public void Remake()
		{
			var ThisScreenIsTargeting = DX.GetDrawScreen() == Handle;

			DX.GetScreenState(out int w, out int h, out _);
			ScreenSize = new Size(w, h);
			ScreenOriginalSize = new Size((int)(w / ExtendRate), (int)(h / ExtendRate));

			DX.DeleteGraph(Handle);
			Handle = DX.MakeScreen(ScreenOriginalSize.Width, ScreenOriginalSize.Height, 1);

			if (ThisScreenIsTargeting)
				DX.SetDrawScreen(Handle);
		}
		/// <summary>SetDrawScreenを実行する</summary>
		/// <returns>SetDrawScreenの戻り値</returns>
		public int SetAsDrawTarget() => DX.SetDrawScreen(Handle);
		/// <summary>裏画面に描画可能グラフィックを描き込んでScreenFlipを実行する（SetDrawScreenはこのメソッド実行前に戻される）</summary>
		public void DrawToBackAndFlip()
		{
			var buf = DX.GetDrawScreen();
			DX.SetDrawScreen(DX.DX_SCREEN_BACK);

			DX.DrawExtendGraph(0, 0, ScreenSize.Width, ScreenSize.Height, Handle, 0);
			DX.ScreenFlip();

			DX.SetDrawScreen(buf);
		}
	}
}
