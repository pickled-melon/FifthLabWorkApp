using FifthLabWorkApp.Objects;

namespace FifthLabWorkApp
{
    public partial class Form1 : Form
    {
        const int CIRCLES_COUNT = 10;

        RedCircle redCircle;
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        Random rnd;

        int score = 0;

        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            player.OnOverlap += (p, obj) =>
            {
                var objectNames = obj.ToString().Split(".");

                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {objectNames[objectNames.Length-1]}\n" + txtLog.Text;
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            rnd = new Random();

            objects.Add(player);
            objects.Add(marker);

            for (int i = 0; i < CIRCLES_COUNT; i++)
            {
                objects.Add(new GreenCircle(rnd.Next(0, pbMain.Width), rnd.Next(0, pbMain.Height), 0));
            }

            player.OnOverlap += (p, obj) =>
            {
                if (obj is GreenCircle grC)
                {
                    grC.X = rnd.Next(0, pbMain.Width);
                    grC.Y = rnd.Next(0, pbMain.Height);

                    score++;
                }
            };

            redCircle = new RedCircle(rnd.Next(0, pbMain.Width), rnd.Next(0, pbMain.Height), 0, pbMain.Width);
            objects.Add(redCircle);

            redCircle.OnOverlap += (rc, obj) =>
            {
                if (obj is Player)
                {
                    score--;
                    redCircle.Reset();
                    redCircle.X = rnd.Next(0, pbMain.Width);
                    redCircle.Y = rnd.Next(0, pbMain.Height);
                }
            };
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            UpdatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }

            g.ResetTransform();

            var scoreString = $"Очки: {score}";
            var font = new Font("Arial", 12, FontStyle.Bold);

            g.DrawString(scoreString,
                font,
                new SolidBrush(Color.Black), 
                pbMain.Width - g.MeasureString(scoreString, font).Width - 10,
                10
            );
        }

        private void pbMain_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // UpdatePlayer();

            if (redCircle != null)
            {
                redCircle.Update();
            }

            pbMain.Invalidate();
        }

        private void UpdatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
