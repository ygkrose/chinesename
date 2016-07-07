using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;


namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        const string _gental = "女";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //http://sunfate.nownews.com/FREE/name/freename.asp

             
            //HttpWebRequest request = HttpWebRequest.Create("http://sunfate.nownews.com/FREE/name/freename.asp") as HttpWebRequest;

            //StreamReader stIn = new StreamReader(request.GetResponse().GetResponseStream());
            //string strResponse = stIn.ReadToEnd();
            //stIn.Close();
            //string tmp = strResponse.Substring(strResponse.LastIndexOf("<form"), strResponse.LastIndexOf("</form>") + 6 - strResponse.LastIndexOf("<form") + 1);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { return; }  
            label2.Visible = false; label3.Visible = false; label4.Visible = false; textBox2.Text = ""; textBox3.Text = "";
            this.Cursor = Cursors.WaitCursor;  
            UTF8Encoding enc = new UTF8Encoding();
            string postData = "_Name=" + textBox1.Text.Trim()  ;
            byte[] data = enc.GetBytes(postData);
            HttpWebRequest request = HttpWebRequest.Create("http://mychat.to/s.php?word=nccsoft&ID=ncc&p=ba_bm") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream newStream = request.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            StreamReader stIn = new StreamReader(request.GetResponse().GetResponseStream());
            string strResponse = stIn.ReadToEnd();
            stIn.Close();


            string tmp = strResponse.Substring(strResponse.LastIndexOf("<table"), strResponse.Substring(strResponse.LastIndexOf("<table")).IndexOf("</table>") + 8);
            webBrowser1.DocumentText = tmp;

            //=================================================================================================================================

            postData = "StrLName=" + textBox1.Text.Substring(0, 1) + "&StrFName=" + textBox1.Text.Substring(1) + "&StrSex=" + _gental + "&StrYear1=2008&StrMonth=09&StrDay=13";
            //data = System.Text.Encoding.GetEncoding("BIG5").GetBytes(postData);
            data = System.Text.UnicodeEncoding.GetEncoding("big5").GetBytes(postData);
            request = HttpWebRequest.Create("http://sunfate.nownews.com/FREE/name/freename_show.asp") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            stIn = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.UnicodeEncoding.GetEncoding("big5"));
            strResponse = stIn.ReadToEnd();
            stIn.Close();
            if (strResponse.IndexOf("無法找到此") > -1)
            {
                strResponse = strResponse.Substring(strResponse.IndexOf("無法找到此"), strResponse.LastIndexOf("')") - strResponse.IndexOf("無法找到此"));
                MessageBox.Show(strResponse.Replace("\\n\\n", ""));
                tmp = WindowsApplication1.Properties.Resources.cc;
            }
            else
               tmp = strResponse.Substring(strResponse.IndexOf("<table width=\"500\""), strResponse.LastIndexOf("<table width=\"520\"") - strResponse.IndexOf("<table width=\"500\""));
            webBrowser2.DocumentText = tmp;
            this.Cursor = Cursors.Default;
        }

        private void form2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2(this);
            fm2.Show(); 
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElement he = webBrowser1.Document.GetElementsByTagName("table")[0];
            int sum1 = Convert.ToInt32(he.Children[0].Children[1].Children[1].InnerText.Trim()) + Convert.ToInt32(he.Children[0].Children[2].Children[1].InnerText.Trim()) + Convert.ToInt32(he.Children[0].Children[3].Children[1].InnerText.Trim());
            int sum2 = Convert.ToInt32(he.Children[0].Children[1].Children[2].InnerText.Trim()) + Convert.ToInt32(he.Children[0].Children[2].Children[2].InnerText.Trim()) + Convert.ToInt32(he.Children[0].Children[3].Children[2].InnerText.Trim());
            label2.Text = sum1.ToString();
            label4.Text = sum2.ToString();
            label2.Visible = true; label3.Visible = true; label4.Visible = true;

            string a = WindowsApplication1.Properties.Resources._ref;
            Stream s = new MemoryStream(System.Text.Encoding.Default.GetBytes(a));
            StreamReader rd = new StreamReader(s, System.Text.Encoding.Default);
            while (!rd.EndOfStream)
            {
                string linestr = rd.ReadLine();
                if (linestr.IndexOf(":") > -1)
                {
                    string recentcnt = linestr.Substring(0, linestr.IndexOf(":"));
                    if (recentcnt == label2.Text)
                    {
                        textBox2.Text = linestr; textBox2.Visible = true;
                    }
                    if (recentcnt == label4.Text)
                    {
                        textBox3.Text = linestr; textBox3.Visible = true;
                    }
                }
            }
            rd.Close(); 

        }
    }
}