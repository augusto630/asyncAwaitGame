using System;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;

namespace asyncAwaitGame
{
    public partial class MainWindow : Window
    {
        private Canvas mCvs;
        private Point mPos;

        public MainWindow()
        {
            InitializeComponent();

            mCvs = new Canvas()
            {
                Background = Brushes.Black,
            };

            this.Content = mCvs;

            Run();
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            mPos = e.GetPointerPoint(this).Position;
        }

        private async void Run()
        {
            while (Bounds.Width == 0 || Bounds.Height == 0)
                await Task.Delay(100);


            Size rSize = new Size(10, 100);
            Size bSize = new Size(10, 10);

            var leftPaddle = new Panel()
            {
                Background = Brushes.White,
                Width = rSize.Width,
                Height = rSize.Height,
            };

            var rightPaddle = new Panel()
            {
                Background = Brushes.White,
                Width = rSize.Width,
                Height = rSize.Height,
            };

            var ball = new Panel()
            {
                Background = Brushes.White,
                Width = bSize.Width,
                Height = bSize.Height,
            };

            mCvs.Children.Add(leftPaddle);
            mCvs.Children.Add(rightPaddle);
            mCvs.Children.Add(ball);

            Point lPos = new Point(5, Bounds.Height / 2);
            Point rPos = new Point(Bounds.Width - 15, Bounds.Height / 2);
            Point bPos = new Point(Bounds.Width / 2, Bounds.Height / 2);

            var step = Bounds.Width / 500;
            var xDir = -1;
            var yDir = 0;
            var rd = new Random();

            while (true)
            {
                if (bPos.X + bSize.Width > Bounds.Width ||
                    bPos.X < 0)
                    bPos = new Point(Bounds.Width / 2, Bounds.Height / 2);

                if (bPos.Y > Bounds.Height || bPos.Y < 0)
                    yDir *= -1;

                if (xDir > 0 && CollidedRight(bPos, bSize, rPos, rSize))
                {
                    xDir *= -1;
                    yDir = CollisionHeight(bPos, bSize, rPos, rSize);
                }

                if (xDir < 0 && CollidedLeft(bPos, bSize, lPos, rSize))
                {
                    xDir *= -1;
                    yDir = CollisionHeight(bPos, bSize, lPos, rSize);
                }

                bPos += new Point(step * xDir, step * yDir);
                lPos = new Point(lPos.X, mPos.Y);
                rPos = new Point(rPos.X, Bounds.Height - mPos.Y);

                Canvas.SetLeft(leftPaddle, lPos.X);
                Canvas.SetTop(leftPaddle, lPos.Y);

                Canvas.SetLeft(rightPaddle, rPos.X);
                Canvas.SetTop(rightPaddle, rPos.Y);

                Canvas.SetLeft(ball, bPos.X);
                Canvas.SetTop(ball, bPos.Y);

                await Task.Delay(10);
            }

            bool CollidedRight(Point bPos, Size bSize, Point rectPos, Size rSize)
            {
                if (bPos.X + bSize.Width >= rectPos.X &&
                    bPos.Y >= rectPos.Y && bPos.Y + bSize.Height <= rectPos.Y + rSize.Height)
                    return true;

                return false;
            }

            bool CollidedLeft(Point bPos, Size bSize, Point rectPos, Size rSize)
            {
                if (bPos.X - step <= rectPos.X + rSize.Width &&
                    bPos.Y >= rectPos.Y && bPos.Y + bSize.Height <= rectPos.Y + rSize.Height)
                    return true;

                return false;
            }

            int CollisionHeight(Point bPos, Size bSize, Point rectPos, Size rSize)
            {
                if (bPos.X < rectPos.X + rSize.Height / 2)
                    return 1;

                if (bPos.X > rectPos.X + rSize.Height / 2)
                    return -1;

                return 0;
            }
        }
    }
}