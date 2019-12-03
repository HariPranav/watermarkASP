//please check the comments for the changes to be made


using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

protected void Page_Load(object sender, EventArgs e)
{
    PdfStampInExistingFile("@aspsnippets.com");
}
 
private void PdfStampInExistingFile(string text)
{
   
   
   //Use a valid pdf in the local path of your computer
   
    string sourceFilePath = @"C:\Users\anand\Desktop\Test.pdf";
    byte[] bytes = File.ReadAllBytes(sourceFilePath);
    Bitmap bitmap = new Bitmap(200, 30, System.Drawing.Imaging.PixelFormat.Format64bppArgb);
    Graphics graphics = Graphics.FromImage(bitmap);
    graphics.Clear(Color.White);
    graphics.DrawString(text, new System.Drawing.Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Red), new PointF(0.4F, 2.4F));
    bitmap.Save(Server.MapPath("~/Image.jpg"), ImageFormat.Jpeg);
    bitmap.Dispose();
    
    // use a valid image in your local computer 
    
    var img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Image.jpg"));
    img.SetAbsolutePosition(200, 400);
    PdfContentByte waterMark;
    using (MemoryStream stream = new MemoryStream())
    {
        PdfReader reader = new PdfReader(bytes);
        using (PdfStamper stamper = new PdfStamper(reader, stream))
        {
            int pages = reader.NumberOfPages;
            for (int i = 1; i <= pages; i++)
            {
                waterMark = stamper.GetUnderContent(i);
                waterMark.AddImage(img);
            }
        }
        bytes = stream.ToArray();
    }
    File.Delete(Server.MapPath("~/Image.jpg"));
    File.WriteAllBytes(sourceFilePath, bytes);
}
