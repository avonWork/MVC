using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace 縱火0709.Models
{
    public class Utility
    {
        #region "密碼加密"

        public const int DefaultSaltSize = 5;

        /// <summary>
        /// 產生Salt
        /// </summary>
        /// <returns>Salt</returns>
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[DefaultSaltSize];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 密碼加密
        /// </summary>
        /// <param name="password">密碼明碼</param>
        /// <returns>Hash後密碼</returns>
        public static string CreateHash(string password)
        {
            string salt = CreateSalt();
            string saltAndPassword = String.Concat(password, salt);
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, "SHA1");
            hashedPassword = string.Concat(hashedPassword, salt);
            return hashedPassword;
        }

        /// <summary>
        /// Computes a salted hash of the password and salt provided and returns as a base64 encoded string.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use in the hash.</param>
        public static string GenerateHashWithSalt(string password, string salt)
        {
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            HashAlgorithm algorithm = new SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // return the has as a base 64 encoded string
            return Convert.ToBase64String(hash);
        }

        #endregion "密碼加密"

        #region "將使用者資料寫入cookie,產生AuthenTicket"

        /// <summary>
        /// 將使用者資料寫入cookie,產生AuthenTicket
        /// </summary>
        /// <param name="userData">使用者資料</param>
        /// <param name="userId">UserAccount</param>
        static public void SetAuthenTicket(string userData, string userId, string TicketCookie)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立Cookie
            //HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //自定義cookie
            HttpCookie authenticationcookie = new HttpCookie(TicketCookie, encryptedTicket);
            //將Cookie寫入回應
            HttpContext.Current.Response.Cookies.Add(authenticationcookie);
        }

        #endregion "將使用者資料寫入cookie,產生AuthenTicket"

        #region "舉世無敵縮圖程式"

        /// <summary>
        /// 舉世無敵縮圖程式(多載)
        /// 1.會自動判斷是比較高還是比較寬，以比較大的那一方決定要縮的尺寸
        /// 2.指定寬度，等比例縮小
        /// 3.指定高度，等比例縮小
        /// </summary>
        /// <param name="name">原檔檔名</param>
        /// <param name="source">來源路徑</param>
        /// <param name="target">目的路徑</param>
        /// <param name="suffix">縮圖辯識符號</param>
        /// <param name="maxWidth">指定要縮的寬度</param>
        /// <param name="maxHeight">指定要縮的高度</param>
        /// <remarks></remarks>
        public static void GenerateThumbnailImage(string name, string source, string target, string suffix, int maxWidth, int maxHeight)
        {
            System.Drawing.Image baseImage = System.Drawing.Image.FromFile(source + "\\" + name);
            var ratio = 0.0F; //存放縮圖比例
            float h = baseImage.Height;//圖像原尺寸高度
            float w = baseImage.Width;//圖像原尺寸寬度
            int ht;//圖像縮圖後高度
            int wt; //圖像縮圖後寬度
            if (w > h)
            {//圖像比較寬
                ratio = maxWidth / w;//計算寬度縮圖比例
                if (maxWidth < w)
                {
                    ht = Convert.ToInt32(ratio * h);
                    wt = maxWidth;
                }
                else
                {
                    ht = Convert.ToInt32(baseImage.Height);
                    wt = Convert.ToInt32(baseImage.Width);
                }
            }
            else
            {//比較高
                ratio = maxHeight / h;//計算寬度縮圖比例
                if (maxHeight < h)
                {
                    ht = maxHeight;
                    wt = Convert.ToInt32(ratio * w);
                }
                else
                {
                    ht = Convert.ToInt32(baseImage.Height);
                    wt = Convert.ToInt32(baseImage.Width);
                }
            }
            var filename = target + "\\" + suffix + name;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(wt, ht);
            System.Drawing.Graphics graphic = Graphics.FromImage(img);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphic.DrawImage(baseImage, 0, 0, wt, ht);
            img.Save(filename);
            img.Dispose();
            graphic.Dispose();
            baseImage.Dispose();
        }

        /// <summary>
        /// 舉世無敵縮圖程式(多載)
        /// 1.會自動判斷是比較高還是比較寬，以比較大的那一方決定要縮的尺寸
        /// 2.指定寬度，等比例縮小
        /// 3.指定高度，等比例縮小
        /// </summary>
        /// <param name="name">原檔檔名</param>
        /// <param name="source">來源檔案的Stream,可接受上傳檔案</param>
        /// <param name="target">目的路徑</param>
        /// <param name="suffix">縮圖辯識符號</param>
        /// <param name="maxWidth">指定要縮的寬度</param>
        /// <param name="maxHeight">指定要縮的高度</param>
        /// <remarks></remarks>
        public static void GenerateThumbnailImage(string name, System.IO.Stream source, string target, string suffix, int maxWidth, int maxHeight)
        {
            System.Drawing.Image baseImage = System.Drawing.Image.FromStream(source);
            var ratio = 0.0F; //存放縮圖比例
            float h = baseImage.Height; //圖像原尺寸高度
            float w = baseImage.Width;  //圖像原尺寸寬度
            int ht; //圖像縮圖後高度
            int wt;//圖像縮圖後寬度
            if (w > h)
            {
                ratio = maxWidth / w; //計算寬度縮圖比例
                if (maxWidth < w)
                {
                    ht = Convert.ToInt32(ratio * h);
                    wt = maxWidth;
                }
                else
                {
                    ht = Convert.ToInt32(baseImage.Height);
                    wt = Convert.ToInt32(baseImage.Width);
                }
            }
            else
            {
                ratio = maxHeight / h; //計算寬度縮圖比例
                if (maxHeight < h)
                {
                    ht = maxHeight;
                    wt = Convert.ToInt32(ratio * w);
                }
                else
                {
                    ht = Convert.ToInt32(baseImage.Height);
                    wt = Convert.ToInt32(baseImage.Width);
                }
            }
            var filename = target + "\\" + suffix + name;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(wt, ht);
            System.Drawing.Graphics graphic = Graphics.FromImage(img);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphic.DrawImage(baseImage, 0, 0, wt, ht);
            img.Save(filename);
            img.Dispose();
            graphic.Dispose();
            baseImage.Dispose();
        }

        /// <summary>
        /// 舉世無敵縮圖程式(指定寬度，等比例縮小)
        /// </summary>
        /// <param name="name">原檔檔名</param>
        /// <param name="source">來源路徑</param>
        /// <param name="target">目的路徑</param>
        /// <param name="suffix">縮圖辯識符號</param>
        /// <param name="maxWidth">指定要縮的寬度</param>
        /// <remarks></remarks>
        public static void GenerateThumbnailImage(int maxWidth, string name, string source, string target, string suffix)
        {
            System.Drawing.Image baseImage = System.Drawing.Image.FromFile(source + "\\" + name);
            var ratio = 0.0F; //存放縮圖比例
            float h = baseImage.Height; //圖像原尺寸高度
            float w = baseImage.Width; //圖像原尺寸寬度
            int ht; //圖像縮圖後高度
            int wt; //圖像縮圖後寬度
            ratio = maxWidth / w;//計算寬度縮圖比例
            if (maxWidth < w)
            {
                ht = Convert.ToInt32(ratio * h);
                wt = maxWidth;
            }
            else
            {
                ht = Convert.ToInt32(baseImage.Height);
                wt = Convert.ToInt32(baseImage.Width);
            }
            var filename = target + "\\" + suffix + name;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(wt, ht);
            System.Drawing.Graphics graphic = Graphics.FromImage(img);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphic.DrawImage(baseImage, 0, 0, wt, ht);
            img.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
            img.Dispose();
            graphic.Dispose();
            baseImage.Dispose();
        }

        /// <summary>
        /// 舉世無敵縮圖程式(指定高度，等比例縮小)
        /// </summary>
        /// <param name="name">原檔檔名</param>
        /// <param name="source">來源路徑</param>
        /// <param name="target">目的路徑</param>
        /// <param name="suffix">縮圖辯識符號</param>
        /// <param name="maxHeight">指定要縮的高度</param>
        /// <remarks></remarks>
        public static void GenerateThumbnailImage(string name, string source, string target, string suffix, int maxHeight)
        {
            System.Drawing.Image baseImage = System.Drawing.Image.FromFile(source + "\\" + name);
            var ratio = 0.0F;//存放縮圖比例
            float h = baseImage.Height; //圖像原尺寸高度
            float w = baseImage.Width;  //圖像原尺寸寬度
            int ht; //圖像縮圖後高度
            int wt; //圖像縮圖後寬度
            ratio = maxHeight / h; //計算寬度縮圖比例
            if (maxHeight < h)
            {
                ht = maxHeight;
                wt = Convert.ToInt32(ratio * w);
            }
            else
            {
                ht = Convert.ToInt32(baseImage.Height);
                wt = Convert.ToInt32(baseImage.Width);
            }
            var filename = target + "\\" + suffix + name;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(wt, ht);
            System.Drawing.Graphics graphic = Graphics.FromImage(img);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphic.DrawImage(baseImage, 0, 0, wt, ht);
            img.Save(filename);
            img.Dispose();
            graphic.Dispose();
            baseImage.Dispose();
        }

        #endregion "舉世無敵縮圖程式"

        #region 產生圖片驗證碼

        /// <summary>
        /// 隨機生成指定長度的驗證碼字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomCode(int length)
        {
            string s = "0123456789zxcvbnmasdfghjklqwertyuiop";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int index;
            for (int i = 0; i < length; i++)
            {
                index = rand.Next(0, s.Length);
                sb.Append(s[index]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 產生刪除線 num 代表幾條
        /// </summary>
        /// <param name="g"></param>
        /// <param name="num"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void PaintInterLine(Graphics g, int num, int width, int height)
        {
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }

        #endregion 產生圖片驗證碼

        #region 寄信

        /// <summary>
        /// 寄信通用
        /// </summary>
        /// <param name="receiveEmail">收信方email </param>
        /// <param name="subjectTitle">收信方email主旨標題 </param>
        /// <param name="bodycontent">信件內容</param>
        public static void SendEmail(string receiveEmail, string subjectTitle, string bodycontent)
        {
            //設定smtp主機
            string smtpAddress = "smtp.gmail.com";
            //設定Port
            int portNumber = 587;
            bool enableSSL = true;
            //填入寄送方email和密碼
            string emailFrom = ConfigurationSettings.AppSettings["email"];
            string password = ConfigurationSettings.AppSettings["password"];
            //收信方email 可以用逗號區分多個收件人
            string emailTo = receiveEmail; //參數1
            //主旨
            string filetime = DateTime.Now.ToString("yyyy.MM.dd");
            string subject = filetime + subjectTitle;
            //內容
            string body = bodycontent;
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                // 若你的內容是HTML格式，則為True
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail); //如果有錯記得打開低安全
                }
            }
        }

        #endregion 寄信


    }
}