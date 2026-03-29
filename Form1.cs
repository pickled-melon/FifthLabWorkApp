using FifthLabWorkApp.Objects;

namespace FifthLabWorkApp
{
    public partial class Form1 : Form
    {
        MyRectangle myRect;

        public Form1()
        {
            InitializeComponent();
            myRect = new MyRectangle(0, 0, 0);
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            myRect.Render(g);
        }
    }
}
