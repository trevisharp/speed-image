using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

Application.SetHighDpiMode(HighDpiMode.SystemAware);
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

var form = new Form();
form.WindowState = FormWindowState.Maximized;

PictureBox pb = new PictureBox();
pb.SizeMode = PictureBoxSizeMode.Zoom;
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

form.Load += delegate
{
    process();
};

Application.Run(form);

void process()
{
    var back = Image.FromFile("back.jpg") as Bitmap;

    var data = back.LockBits(
        new Rectangle(0, 0, back.Width, back.Height),
        ImageLockMode.ReadWrite, 
        PixelFormat.Format24bppRgb);

    unsafe
    {
        byte* p = (byte*)data.Scan0.ToPointer();

        //get pixel em x y
        //p[x + y * stride]

        //set pixel em x y
        //p[x + y * stride + 0] = B
        //p[x + y * stride + 1] = G
        //p[x + y * stride + 2] = R

        for (int j = 0; j < data.Height; j++)
        {
            byte* l = p + j * data.Stride;
            for (int i = 0; i < 3 * data.Width; i += 3, l += 3)
            {
                l[0] = (byte)(255 - l[0]);
                l[1] = (byte)(255 - l[1]);
                l[2] = (byte)(255 - l[2]);
            }
        }
    }

    back.UnlockBits(data);
    pb.Image = back;
}