using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace FifthLabWorkApp.Objects
{
    class RedCircle : BaseObject
    {
        private float radius;
        private float minRadius = 10f;
        private float maxRadius = 200f;
        private float velocity = 0.5f;

        public RedCircle(float x,  float y,  float angle, float maxRadius = 200f) : base(x, y, angle)
        {
            this.maxRadius = maxRadius;
            radius = minRadius;
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(128, 255, 0, 0)), -radius, -radius, radius * 2, radius * 2);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-radius, -radius, radius * 2, radius * 2);
            return path;
        }

        public void Update()
        {
            radius += velocity;

            if (radius > maxRadius)
            {
                radius = maxRadius;
            }
                
        }

        public void Reset()
        {
            radius = minRadius;
        }
    }
}
