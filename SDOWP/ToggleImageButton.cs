using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace SDOWP
{
	public partial class ToggleImageButton : PictureBox
	{
        private Image fUnmodifiedImage;      
        public void SetImage(Image pImage)
        {
            base.Image = pImage;
            fUnmodifiedImage = pImage;
        }

        private bool _Selected = true;
        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (value != _Selected)
                {
                    _Selected = value;
                    UpdateImage();
                };
            }
        }

		[Category("Custom")]
		[Description("Used for ordering")]
		[Browsable(true)]
		public int Order { get; set; }

		public ToggleImageButton()
		{
			InitializeComponent();
		}

		private void ToggleImageButton_Click(object sender, EventArgs e)
		{
            _Selected = !_Selected;
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (!_Selected)
            {
                this.Image = MakeGrayscale3(new Bitmap(this.Image));
            }
            else
            {
                this.Image = fUnmodifiedImage;
            }
        }


		/// <summary>
		/// http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		private static Bitmap MakeGrayscale3(Bitmap original)
		{
			//create a blank bitmap the same size as original
			Bitmap newBitmap = new Bitmap(original.Width, original.Height);

			//get a graphics object from the new image
			Graphics g = Graphics.FromImage(newBitmap);

			//create the grayscale ColorMatrix
			//      ColorMatrix colorMatrix = new ColorMatrix(
			//         new float[][] 
			//{
			//   new float[] {.3f, .3f, .3f, 0, 0},
			//   new float[] {.59f, .59f, .59f, 0, 0},
			//   new float[] {.11f, .11f, .11f, 0, 0},
			//   new float[] {0, 0, 0, 1, 0},
			//   new float[] {0, 0, 0, 0, 1}
			//});
			ColorMatrix colorMatrix = new ColorMatrix(
			   new float[][]
                       {
                        new float[] {0.4f, 0, 0, 0, 0},
                        new float[] {0, 0.4f, 0, 0, 0},
                        new float[] {0, 0, 0.4f, 0, 0},
                        new float[] {0, 0, 0, 0.7f, 0},
                        new float[] {0, 0, 0, 0, 1}
                       });

			//create some image attributes
			ImageAttributes attributes = new ImageAttributes();

			//set the color matrix attribute
			attributes.SetColorMatrix(colorMatrix);

			//draw the original image on the new image
			//using the grayscale color matrix
			g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
			   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

			//dispose the Graphics object
			g.Dispose();
			return newBitmap;
		}
	}
}
