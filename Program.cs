using System;
using System.IO;
using System.Windows.Forms;
using RDotNet;
using RDotNet.Graphics;

namespace rnet_and_r_portable
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

            string rHome = Path.Combine(Directory.GetCurrentDirectory(), @"R-portable");
            Environment.SetEnvironmentVariable("PATH", rHome);
            REngine engine = REngine.GetInstance();
            Application.ThreadExit += (sender, e) => engine.Dispose();
            engine.Initialize();

            var ImageLocation = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".png");
            Application.ThreadExit += (sender, e) => {
                try
                {
                    File.Delete(ImageLocation);
                }
                finally { }
            };
            var outGraphForm = new Form1();
            outGraphForm.pictureBox1.ImageLocation = ImageLocation;
            engine.Evaluate(string.Format("png('{0}', {1}, {2})", ImageLocation.Replace('\\', '/'), 
                outGraphForm.pictureBox1.Width, outGraphForm.pictureBox1.Height));
            engine.Evaluate(@"plot(1:10, pch=1:10, col=1:10, cex=seq(1, 2, length=10))
                  text(c(1), c(1), c('Text here'), col=c('red'))");            
            engine.Evaluate("dev.off()");
            Application.Run(outGraphForm);
        }
    }
}
