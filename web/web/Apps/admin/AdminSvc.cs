using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Common.Extensions;
using ServiceStack;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using ServiceStack.Common.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ServiceStack.Service;
using System.Drawing.Drawing2D;

namespace web.Apps.admin
{
    [Route("/login", "GET")]
    public class LoginDto
    { }
    public class LoginDtoResponse
    { }

    [Route("/logout")]
    public class LogoutDto
    { }

    public class LogoutDtoResponse
    {
    }

    [Route("/auth/session", "GET")]
    public class SessionDto
    {
        //public string Username { get; set; }
    }

    [Route("/authorized/users", "GET")]
    public class UsersDto
    {
    }

    [Route("/abc", "GET")]
    public class GroupsDto
    {
        //public string Role { get; set; }
    }
    [Route("/html", "GET")]
    public class GetHtml
    {
    }
    //[Authenticate]
    //[DefaultView("apps/admin/default")]
    public class AdminAppSvc : Service
    {
        static int _counter = 0;
        //public AdminAppSvcDtoResponse Get(AdminAppSvcDto req)
        //{
        //    return new AdminAppSvcDtoResponse { };
        //}

        //public object Any(LogoutDto req)
        //{
        //    this.RemoveSession();

        //    return this.Redirect("/");
        //}

        public object Get(SessionDto req)
        {
            var ses = base.Request.GetSession();

            return ses;
        }

        //public string Get(GetHtml req)
        //{
        //    return "<table><tr><td>hahaha</td></tr></table>";
        //}
        //[Authenticate]
        //public object Get(UsersDto req)
        //{
        //    return "tangina nio, users";
        //}

        //Dictionary<string, string> icons = new Dictionary<string, string>();
        //Dictionary<string, string> icon_counter = new Dictionary<string, string>();
        //public AdminAppSvc()
        //{
        //    icons.Add("one", "http://getfavicon.appspot.com/http://www.google.com");
        //    icons.Add("two", "http://getfavicon.appspot.com/http://www.yahoo.com");
        //    icons.Add("three", "http://getfavicon.appspot.com/http://www.facebook.com");
        //    icons.Add("four", "http://getfavicon.appspot.com/http://www.twitter.com");
        //    icons.Add("five", "http://getfavicon.appspot.com/http://www.reddit.com");
        //    //icons.Add("six", "http://getfavicon.appspot.com/http://www.youtube.com");
        //    //icons.Add("six1", "http://getfavicon.appspot.com/https://github.com");


        //}
        //[Authenticate]
        //[AddHeader(ContentType = "image/png")]
        public ImageResult Get(GroupsDto req)
        {
            _counter++;

            var iconImgs = new Dictionary<Image, int>();
            var iconHeight = 0;
            var iconsWidth = 0;
            var client = new System.Net.WebClient();

            foreach (var icon in icons)
            {
                if (!File.Exists(icon.Key))
                {
                    File.Delete(icon.Key);
                    client.DownloadFile(
                        icon.Value, icon.Key);
                }
                var iconImg = Image.FromFile(icon.Key);

                //var bytes = client.down( Request.RawUrl + "/html");
                //var mem = new MemoryStream();
                //mem.Write(bytes, 0, bytes.Length);
                //var iconImg2 = Image.FromStream(mem);

                //iconHeight += 16 + 2;
                //iconsWidth += 32 + 5;
                //iconImgs.Add(iconImg, iconsWidth);

                var iconImg2 = Image.FromFile(icon.Key);
                iconHeight += iconImg2.Height + 2;
                iconsWidth += iconImg2.Width + 1;
                iconImgs.Add(iconImg2, iconsWidth);

                //iconImg.Dispose();
            }

            return DrawTextHoriz(_counter + ". caydev: " + Request.GetUrlHostName(), iconsWidth, 100, iconImgs, SystemFonts.SmallCaptionFont, Brushes.Black, Color.LightSkyBlue);
        }

        public static ImageCodecInfo GetImageCodeInfo(string mimeType)
        {
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in info)
                if (ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                    return ici;
            return null;
        }

        private ImageResult DrawTextVert(String text, int w, int h, Image[] images, Font font, Color textColor, Color backColor)
        {

            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width + w, (int)textSize.Height + h);

            drawing = Graphics.FromImage(img);



            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            var hh = 15;
            foreach (var iconImg in images)
            {
                //drawing.DrawImage(iconImg, new Point(4, hh));
                //hh += iconImg.Height + 3;
                drawing.DrawImage(iconImg, new Rectangle(4, hh, 16, 16));
                drawing.DrawString(hh.ToString(), SystemFonts.DefaultFont, Brushes.Black, new PointF(iconImg.Width + 6, hh));
                hh += 16 + 3;

            }
            drawing.DrawString("powered by: caydev2010@gmail.com", font, Brushes.DarkBlue, new PointF(3, hh));
            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return new ImageResult(img);

        }

        private ImageResult DrawTextHoriz(String text, int w, int h, Dictionary<Image, int> images, Font font, Brush brush, Color backColor)
        {
            StringFormat sf = new StringFormat { };

            var pow = "Title: the quick brown foxs the lazy dog"
                       + "\nViews: " + _counter.ToString()
                       + "\nCreated by http://caydev.heroku.com";
            var powLen = 0;
            var powTall = 0;

            var width = 0;
            SizeF textSize;

            using (var img1 = new Bitmap(1, 1))
            using (var drawing1 = Graphics.FromImage(img1))
            {
                var foo = drawing1.MeasureString(pow, font, new PointF(0, 0), sf);
                powLen = (int)foo.Width;
                powTall = (int)foo.Height;

                foreach (var item in images)
                {
                    textSize = drawing1.MeasureString(item.Value.ToString(), font);
                    width += 16 + (int)textSize.Width + 5;
                }
                width += 4; // margin both sides               
            }



            //create a new image of the right size
            //img = new Bitmap(powLen > width ? powLen : width, 16 + 2 + powTall + 2);
            var img = new Bitmap(powLen > width ? powLen : width, 16 + 3 + powTall + 2);
            using (var drawing = Graphics.FromImage(img))
            {
                //drawing.SmoothingMode = SmoothingMode.AntiAlias;
                drawing.InterpolationMode = InterpolationMode.HighQualityBicubic;
                drawing.SmoothingMode = SmoothingMode.HighQuality;
                drawing.CompositingQuality = CompositingQuality.HighQuality;
                drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //paint the background
                drawing.Clear(backColor);

                var ww = 2f;
                foreach (var iconImg in images)
                {
                    drawing.DrawImage(iconImg.Key, new RectangleF(ww, 2f, 16f, 16f));
                    ww += 16f + 2f;

                    var cnt = (ww).ToString();
                    textSize = drawing.MeasureString((iconImg.Value + cnt), font);//iconImg.Value.ToString(), font);

                    drawing.DrawString(cnt, font, Brushes.Black, new PointF(ww, 2f));
                    ww += (int)textSize.Width + 3f;

                }
                drawing.DrawString(pow, font, brush, new PointF(2, img.Height - 2 - powTall), sf);
            }
            return new ImageResult(img);
        }

        public class ImageResult : IDisposable, IStreamWriter, IHasOptions
        {
            private readonly Image image;
            private readonly ImageFormat imgFormat;

            public ImageResult(Image image, ImageFormat imgFormat = null)
            {
                this.image = image;
                this.imgFormat = imgFormat ?? ImageFormat.Png;
                this.Options = new Dictionary<string, string> {
                { HttpHeaders.ContentType, "image/" + this.imgFormat.ToString().ToLower() }
            };
            }

            public void WriteTo(Stream responseStream)
            {
                image.Save(responseStream, imgFormat);
            }

            public void Dispose()
            {
                this.image.Dispose();
            }

            public IDictionary<string, string> Options { get; set; }
        }
    }

}