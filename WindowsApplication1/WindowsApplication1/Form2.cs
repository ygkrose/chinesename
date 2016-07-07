using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace WindowsApplication1
{
    public partial class Form2 : Form
    {
        Dictionary<string, ArrayList> al = new Dictionary<string, ArrayList>();
        const string firstname = "謝";
        const int firstnamecount = 17; //陳
        Form form1;

        public Form2(Form frm1)
        {
            InitializeComponent();
            checkedListBox1.Items.Clear(); checkedListBox2.Items.Clear();
            form1 = frm1;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string linestr="";
            string recentcnt="0";
            al.Clear();
            string a = WindowsApplication1.Properties.Resources.word;
            Stream s = new MemoryStream(System.Text.Encoding.Default.GetBytes(a));

            StreamReader rd = new StreamReader(s, System.Text.Encoding.Default);
            //StreamReader rd=new StreamReader("d:word.txt",System.Text.Encoding.Default);
            
            //string a = rd.CurrentEncoding.EncodingName ;
            try
            {
                while (!rd.EndOfStream)
                {
                    linestr = rd.ReadLine();
                    if (linestr.IndexOf(":") > -1)
                    {
                        recentcnt = linestr.Substring(0, linestr.IndexOf(":"));
                        checkedListBox1.Items.Add(recentcnt);
                        checkedListBox2.Items.Add(recentcnt);
                    }
                    else
                    {
                        ArrayList _word = new ArrayList();

                        for (int i = 0; i < linestr.Length; i++)
                        {
                            _word.Add(linestr.Substring(i, 1));
                        }
                        
                        if (al.ContainsKey(recentcnt))
                            al[recentcnt].Add(_word);
                        else
                            al.Add(recentcnt, _word);
                    }
                }
                rd.Close(); 
            }
            catch
            {
                rd.Close(); 
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true; this.Cursor = Cursors.WaitCursor;  
            int k = 0;
            ArrayList allname = new ArrayList(); checkedListBox4.Items.Clear();   
            foreach (string cnt1 in checkedListBox1.CheckedItems)
            {
                foreach (string cnt2 in checkedListBox2.CheckedItems)
                {
                    int sum = Convert.ToInt32(cnt1) + Convert.ToInt32(cnt2) + firstnamecount;
                    string tsum = sum < 10 ? " " + sum.ToString() : sum.ToString();
                    if (checkedListBox3.CheckedItems.Contains(tsum.ToString()))
                    {
                        ArrayList la = al[cnt1.Trim()]; ArrayList lb = al[cnt2.Trim()];
                        foreach (string first in la)
                            foreach (string second in lb)
                            {
                                if (checkedListBox5.CheckedItems.Contains(first) && checkedListBox6.CheckedItems.Contains(second))
                                {
                                    allname.Add(first + second);
                                    checkedListBox4.Items.Add(firstname + first + second + "(" + sum + "劃)");
                                    k++;
                                }
                            }
                    }
                }
            }
            label6.Text = "結   果 " + k.ToString() + " 筆符合";
            this.Cursor = Cursors.Default; 
            Application.UseWaitCursor = false; 
        }

        private void doProcess()
        {
            //foreach (string cnt1 in checkedListBox5.CheckedItems)
            //{
            //    foreach (string cnt2 in checkedListBox6.CheckedItems)
            //    {
                     
            //        int sum = Convert.ToInt32(cnt1) + Convert.ToInt32(cnt2) + 13;
            //        string tsum = sum < 10 ? " " + sum.ToString() : sum.ToString();
            //        if (checkedListBox3.CheckedItems.Contains(tsum.ToString()))
            //        {
            //            ArrayList la = al[cnt1.Trim()]; ArrayList lb = al[cnt2.Trim()];
            //            foreach (string first in la)
            //                foreach (string second in lb)
            //                {
            //                    if (checkedListBox5.CheckedItems.Contains(first) && checkedListBox6.CheckedItems.Contains(second))
            //                    {
            //                        allname.Add(first + second);
            //                        checkedListBox4.Items.Add("楊" + first + second + "(" + sum + "劃)");
            //                        k++;
            //                    }
            //                }
            //        }
            //    }
            //}
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox3.Items.Clear();
            checkedListBox5.Items.Clear();
            checkedListBox6.Items.Clear(); 
            foreach (string cnt1 in checkedListBox1.CheckedItems)
            {
                string[] ary1=new string[al[cnt1].Count];
                al[cnt1].CopyTo(ary1);
                 
                checkedListBox5.Items.AddRange(ary1);   
                foreach (string cnt2 in checkedListBox2.CheckedItems)
                {
                    ary1 = new string[al[cnt2].Count];
                    al[cnt2].CopyTo(ary1);
                    
                    checkedListBox6.Items.AddRange(ary1);
                    int sum = Convert.ToInt32(cnt1) + Convert.ToInt32(cnt2) + firstnamecount;
                    string tsum = sum < 10 ? " " + sum.ToString() : sum.ToString();
                    if (!checkedListBox3.Items.Contains(tsum.ToString()))
                    {
                        checkedListBox3.Items.Add(sum < 10 ? " " + sum.ToString() : sum.ToString());
                    }
                }
            }
            checkedListBox3.Sorted=true;
            checkedListBox5.Sorted = true;
            for (int j = 0; j < checkedListBox3.Items.Count; j++)
                checkedListBox3.SetItemChecked(j, true);
            //for (int j = 0; j < checkedListBox5.Items.Count; j++)
            //    checkedListBox5.SetItemChecked(j, true);
            //for (int j = 0; j < checkedListBox6.Items.Count; j++)
            //    checkedListBox6.SetItemChecked(j, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkedListBox4.CheckedItems.Count == 0) { return; }
            int k = 0;
            //saveFileDialog1.InitialDirectory ="c:\\";
            saveFileDialog1.FileName = "姓名存檔";
            saveFileDialog1.DefaultExt = "txt";
            //RichTextBox rtb = new RichTextBox();
            foreach (string _name in checkedListBox4.CheckedItems)
            {
                if (k > 0 && (k % 10) == 0)
                    richTextBox1.Text += _name + ", \r\n";
                else
                    richTextBox1.Text += _name + ",";
                k++;
            }
            richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.Text.LastIndexOf(","));
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                //richTextBox1.SaveFile(saveFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkedListBox5.CheckedItems.Count > 0)
            {
                for (int j = 0; j < checkedListBox5.Items.Count; j++)
                    checkedListBox5.SetItemChecked(j, false);
            }
            else
            {
                for (int j = 0; j < checkedListBox5.Items.Count; j++)
                    checkedListBox5.SetItemChecked(j, true);
            }
            button1_Click(sender, e);  
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkedListBox6.CheckedItems.Count > 0)
            {
                for (int j = 0; j < checkedListBox6.Items.Count; j++)
                    checkedListBox6.SetItemChecked(j, false);
            }
            else
            {
                for (int j = 0; j < checkedListBox6.Items.Count; j++)
                    checkedListBox6.SetItemChecked(j, true);
            }
            button1_Click(sender, e);
        }

        private void checkedListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
             
             
        }

        private void checkedListBox4_DoubleClick(object sender, EventArgs e)
        {
            string name = (sender as CheckedListBox).SelectedItem.ToString();
            form1.Controls[8].Text = name.Substring(0, name.IndexOf("("));
            form1.BringToFront();
        }
    }
}