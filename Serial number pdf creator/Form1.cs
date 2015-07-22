using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.Configuration;

using System.Data.SqlClient;

using System.Reflection;


using it = iTextSharp.text;


namespace SeminarPaper
{
    public partial class Form1 : Form
    {

        MemoryStream stream = new MemoryStream();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf檔(*.pdf)|*.pdf";
        }
        string pa;
        private void make_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            //this.txtPath.Text = path.SelectedPath;

            pa = path.SelectedPath;
            
            
            Make_pdf();


            try
            {
                /*
               if (saveFileDialog1.ShowDialog() == DialogResult.OK)
               {
                   
                   StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);

                   sw.Write(stream);
                   sw.Flush();
                   sw.Close();
                    * 
               }
                 * */

                MessageBox.Show("生成成功!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("存檔錯誤!");
            }

            

        }
       
        private void Make_pdf()
        {
            //第一頁
            int ti = Convert.ToInt32(times.Text);
            int he = Convert.ToInt32(head.Text);
            string nu;
            if (he / 10 == 0) nu = "0000000" + he.ToString();
            else if (he / 100 == 0) nu = "000000" + he.ToString();
            else if (he / 1000 == 0) nu = "00000" + he.ToString();
            else if (he / 10000 == 0) nu = "0000" + he.ToString();
            else if (he / 10000 == 0) nu = "000" + he.ToString();
            else if (he / 100000 == 0) nu = "00" + he.ToString();
            else if (he / 1000000 == 0) nu = "0" + he.ToString();
            else nu = he.ToString();


            PdfReader reader = new PdfReader(@"data\Doc1.pdf");


            //

            //將範本檔按丟入stream並給PdfStamper
            //PdfStamper stamper = new PdfStamper(reader, stream);


            PdfStamper stamper = new PdfStamper(reader, new FileStream(pa+@"\"+times.Text+@".pdf", FileMode.Create));
            stamper.Writer.CloseStream = false;
            AcroFields acroFields = stamper.AcroFields;
            //BaseFont bfkaiu = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //BaseFont bftimes = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //重設pdf樣式
            BaseFont bf = BaseFont.CreateFont("C:/WINDOWS/Fonts/kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            acroFields.SetFieldProperty("Text1", "textfont", bf, null);


            //將資料放進pdf內

            acroFields.SetField("Text1", nu);

            //infile.Flush();

            int i=1;
            int insPage = 2;

            //第n頁
            while (i < ti)// 若 page-i，i比page小 代表還有頁數還沒完成
            {
                i++; he++;
                string nu2;
                if (he / 10 == 0) nu2 = "0000000" + he.ToString();
                else if (he / 100 == 0) nu2 = "000000" + he.ToString();
                else if (he / 1000 == 0) nu2 = "00000" + he.ToString();
                else if (he / 10000 == 0) nu2 = "0000" + he.ToString();
                else if (he / 10000 == 0) nu2 = "000" + he.ToString();
                else if (he / 100000 == 0) nu2 = "00" + he.ToString();
                else if (he / 1000000 == 0) nu2 = "0" + he.ToString();
                else nu2 = he.ToString();

                //將Sample PDF檔丟入PdfReader物件
                MemoryStream stream2 = new MemoryStream();
                PdfReader reader2 = new PdfReader(@"data\Doc1.pdf");
                
                //將範本檔按丟入stream並給PdfStamper
                PdfStamper stamper2 = new PdfStamper(reader2, stream2);
                stamper2.Writer.CloseStream = false;
                AcroFields acroFields2 = stamper2.AcroFields;
                //BaseFont bfkaiu2 = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                //BaseFont bftimes2 = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                //重設pdf樣式
                //BaseFont bf2 = BaseFont.CreateFont("C:/WINDOWS/Fonts/kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                it.Font textFont2 = new it.Font(bf, 12);

                acroFields2.SetFieldProperty("Text1", "textfont", bf, null);
                //將資料放進pdf內
                acroFields2.SetField("Text1", nu2);



                
                
                stamper2.FormFlattening = true;
                stamper2.Close();
                reader2.Close();

                //將第n頁裝進第一頁後面
                MemoryStream rm = new MemoryStream(stream2.GetBuffer(), 0, stream2.GetBuffer().Length);
                PdfReader tesrreader = new PdfReader(rm);
                stamper.InsertPage(insPage, reader.GetPageSize(1));
                stamper.GetUnderContent(insPage).AddTemplate(stamper.GetImportedPage(tesrreader, 1), 0, 0);

                insPage += 1;//插入變數++

            }
            //第n頁END
            stamper.FormFlattening = true;
            stamper.Close();
            
            reader.Close();
            //PDF結束
            //stamper.Flush();
            Dispose();
        
        }



    }
}
