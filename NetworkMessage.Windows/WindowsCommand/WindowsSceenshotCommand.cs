using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsSceenshotCommand : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult screenshotResult;
            try
            {
                int width = Screen.AllScreens.Sum(s => s.Bounds.Width);
                int height = Screen.AllScreens.Max(s => s.Bounds.Height);

                using Bitmap bitmap = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, new Size(width, height));
                }
                ImageConverter converter = new ImageConverter();
                screenshotResult = new ScreenshotResult((byte[])converter.ConvertTo(bitmap, typeof(byte[])));

            }
            catch (NullReferenceException nullEx)
            { 
                screenshotResult = new ScreenshotResult(nullEx.Message,nullEx);
            }
            catch (Exception ex)
            {
                screenshotResult = new ScreenshotResult(ex.Message,ex);
            }
            return Task.FromResult(screenshotResult);
        }
    }
}
