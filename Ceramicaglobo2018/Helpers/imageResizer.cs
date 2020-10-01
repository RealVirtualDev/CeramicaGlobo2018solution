using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WebSite.Helpers
{
    public class ImageResizer
    {
        private string _imgPath;
        private Image _original;


        public ImageResizer(string path)
        {
            _imgPath = path;
            _original = Image.FromFile(path);
        }


        public Image resize(Size size,bool mantainAspectRatio=true)
        {
            
            int offsetw = 0;
            int offseth = 0;
            Size tempsize;

            if (mantainAspectRatio)
            {
                size = CalculateImageSize(size);
            }
            else
            {
                // centra il soggetto

                if (size.Width > size.Height)
                {
                    if (_original.Width > _original.Height)
                    {
                        // formato giusto, il centramento avviene sull'asse orizzontale
                        tempsize = CalculateImageSize( size.Width, fixedSize.Width);
                        offseth = (tempsize.Height - size.Height) / 2;
                    }
                  else
                    {
                        // formato sbagliato, il centramento avviene sull'asse verticale
                        tempsize = CalculateImageSize( size.Width, fixedSize.Width);

                    //' l'immagine sborda in verticale
                        offseth = (tempsize.Height - size.Height) / 2;
                    }
                }
                else
                {
                    // portrait
                    if (_original.Height > _original.Width)
                    {
                        // formato giusto, il centramento avviene sull'asse orizzontale
                        tempsize = CalculateImageSize(size.Width, fixedSize.Height);
                        offsetw = (tempsize.Width - size.Width) / 2;
                    }
                    else
                    {
                        // formato sbagliato, il centramento avviene sull'asse verticale
                        tempsize = CalculateImageSize( size.Width, fixedSize.Height);
                        offsetw = (tempsize.Width - size.Width) / 2;
                    }
                }
            }

            using (Bitmap TargetBitmap = new Bitmap(_original, size))
            {
                using (Graphics TargetGraphic = Graphics.FromImage(TargetBitmap))
                {
                    TargetGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    TargetGraphic.SmoothingMode = SmoothingMode.HighQuality;
                    TargetGraphic.DrawImage(_original, new Rectangle(0 + offsetw, 0 + offseth, size.Width + offsetw, size.Height + offseth), 0, 0, _original.Width, _original.Height, GraphicsUnit.Pixel);
                                       
                }

                return TargetBitmap;
            }
   

        }

        public Image resize(int sideVal, fixedSize fixedSide)
        {
            Size newSize  = CalculateImageSize(sideVal, fixedSide);

            Bitmap TargetBitmap = new Bitmap(_original, newSize);
           
                using (Graphics TargetGraphic = Graphics.FromImage(TargetBitmap))
                {
                    TargetGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    TargetGraphic.SmoothingMode = SmoothingMode.HighQuality;
                    TargetGraphic.DrawImage(_original, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, _original.Width, _original.Height, GraphicsUnit.Pixel);

                }

            return TargetBitmap;
            

        }

        public Image crop(int width,int height)
        {
            // risultato
            Image resized;
            int xpos, ypos = 0;
       

            if (_original.Width > _original.Height)  // landscape
            {
                // landscape
                resized = resize(height,fixedSize.Height);
                if (resized.Width < width)
                {
                    resized = resize(width, fixedSize.Width);
                    xpos = 0;
                    ypos = -((resized.Height - height) / 2);
                }
                else
                {
                    xpos = -((resized.Width - width) / 2);
                    ypos = 0;
                }
            }
            else // portrait
            {
                resized = resize(width, fixedSize.Width);
                if (resized.Height <= height)
                {
                    resized = resize(height, fixedSize.Height);
                    xpos = -((resized.Width - width) / 2);
                    ypos = 0;
                }
                else
                {
                    xpos = 0;
                    ypos = -((resized.Height - height) / 2);
                }


            }

            Bitmap resultImage = new Bitmap(width, height);
      
                using (Graphics g = Graphics.FromImage(resultImage))
                {
                    g.DrawImage(resized, xpos, ypos);
                }
                resized.Dispose();
                return resultImage;
          

      
        }

        public Image fit(int width, int height,string backColor)
        {
            // RGB
            Color bg = ColorTranslator.FromHtml(backColor);
            // ARGB
            //ColorConverter cc = new ColorConverter();
            //Color? bg2 = (Color)cc.ConvertFromString("#FFDFD991");

            Image resultImage = new Bitmap(width, height);
            Image resized;

            using (Graphics g = Graphics.FromImage(resultImage))
            {
                SolidBrush fondo = new SolidBrush(bg);

                // applico il fondo
                g.FillRectangle(new SolidBrush(bg), 0, 0, width, height);


                // test 1
                Size test = CalculateImageSize(height, fixedSize.Height);
                if (test.Height <= height && test.Width <= width)
                {
                    // test ok
                    // il lato che combacia è l'altezza
                    
                    resized = resize(height, fixedSize.Height);
                    int offx = (width - resized.Width) / 2;
                    g.DrawImage(resized, offx, 0, resized.Width,resized.Height);
                }
                else
                {
                    // il lato che combacia è la larghezza
                    resized = resize(width, fixedSize.Width);
                    int offy = (height - resized.Height) / 2;
                    g.DrawImage(resized, 0, offy, resized.Width, resized.Height);
                }

                //g.DrawImage(resultImage, xpos, ypos);
            }

            return resultImage;

        }

        private Size CalculateImageSize(Size newSize)
        {
            Size mySize = new Size();
            
            float ratioNew = (float)newSize.Width / (float)newSize.Height;
            float ratioCur = (float)_original.Width / (float)_original.Height;

            if (ratioCur > ratioNew) {
                mySize.Width = newSize.Width;
                mySize.Height = Convert.ToInt32(mySize.Width / ratioCur);
            } 
          
            if(ratioCur < ratioNew)
            {
                mySize.Height = newSize.Height;
                mySize.Width = Convert.ToInt32(mySize.Height * ratioCur);
            }

            if (ratioCur == ratioNew)
            {
                mySize.Width = newSize.Width;
                mySize.Height = newSize.Height;
            }
            
            return mySize;
        }

        private Size CalculateImageSize(int sideVal,fixedSize fixedSide )
        {
            Size mySize = new Size();
            float ratioCur= ((float)_original.Width / (float)_original.Height);

            switch(fixedSide){
                case fixedSize.Height:
                    mySize.Height = sideVal;
                    if (_original.Height > _original.Width) {
                        mySize.Width =Convert.ToInt32(sideVal * ratioCur);

                    }
                   else
                    {
                        mySize.Width = Convert.ToInt32(sideVal * ratioCur);
                    }

                    break;
                case fixedSize.Width:
                    mySize.Width = sideVal;
                    if (_original.Height > _original.Width)
                    {
                        mySize.Height = Convert.ToInt32(sideVal / ratioCur);

                    }
                    else
                    {
                        mySize.Height = Convert.ToInt32(sideVal / ratioCur);
                    }
                    break;
            }

            return mySize;
        }

        private ImageCodecInfo GetImageCodec(ImageFormat ImageType)
        {
            ImageCodecInfo[] imgCodecs;
            ImageCodecInfo CodecResult = null;

            imgCodecs = ImageCodecInfo.GetImageEncoders();

            string mType = "image/"+ ImageType.ToString().ToLower();

            foreach(ImageCodecInfo codec in imgCodecs)
            {
                if (codec.MimeType == mType)
                {
                    CodecResult = codec;
                    imgCodecs = null;
                    return CodecResult;
                }
            }

            return null;
        }

       public enum fixedSize
        {
            Width,
            Height
        }

        

    }
}

//Public Shared Function SaveToDisk(ByVal currentImage As Image, ByVal FilePath As String, ByVal ImageType As Imaging.ImageFormat, ByVal ImageQuality As Long) As Boolean
//        Dim encQuality As Imaging.Encoder
//        Dim encColor As Imaging.Encoder
//        Dim ratio As Imaging.EncoderParameter
//        Dim cColor As Imaging.EncoderParameter
//        Dim codecParams As Imaging.EncoderParameters
//        Dim imgCodec As Imaging.ImageCodecInfo

//        encQuality = Imaging.Encoder.Quality
//        encColor = Imaging.Encoder.ColorDepth

//        ratio = New Imaging.EncoderParameter(encQuality, ImageQuality)
//        cColor = New Imaging.EncoderParameter(encColor, CType(24L, Int32))

//        codecParams = New Imaging.EncoderParameters(2)
//        codecParams.Param(0) = ratio
//        codecParams.Param(1) = cColor

//        imgCodec = GetImageCodec(ImageType)

//        currentImage.Save(FilePath, imgCodec, codecParams)
//        Return True

//        ratio.Dispose()
//        cColor.Dispose()
//        codecParams.Dispose()
//        encQuality = Nothing
//        encColor = Nothing
//        ratio = Nothing
//        cColor = Nothing
//        codecParams = Nothing
//        imgCodec = Nothing

//    End Function