using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using TakamineProduction;
using DxLibDLL;

namespace Sample
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			DX.ChangeWindowMode(1);
			DX.SetGraphMode(800, 600, 32);
			DX.DxLib_Init();
			
			var i = 0;
			int pic = DX.LoadGraph("pic.jpg");
			
			//★　インスタンスを作成（１倍で作成）
			var exScr = new DXExtendScreen(1);

			//★　SetDrawScreenを実行
			exScr.SetAsDrawTarget();


			while(DX.ProcessMessage() != -1)
			{
				DX.ClearDrawScreen();


				if (++i % 60 == 0)//★　１秒ごとに倍率を１倍増やす
					exScr.ExtendRate += 1;
				
				DX.DrawGraph(0, 0, pic, 0);
				DX.DrawPixel(100, 20, DX.GetColor(255, 0, 0));
				DX.DrawLine(80, 30, 120, 30, DX.GetColor(255, 0, 0));
				DX.DrawString(0, 0, exScr.ExtendRate.ToString("F2") + "倍　で表示。", DX.GetColor(255, 0, 255));
				
				DX.DrawString(0, 40, "正常ｱｽ比:" + exScr.IsNormalAspect.ToString(), DX.GetColor(255, 0, 255));

				//★　裏画面に描画可能グラフィックを描き込んでScreenFlipを実行する
				exScr.DrawToBackAndFlip();
			}


			DX.DxLib_End();
		}
	}
}
