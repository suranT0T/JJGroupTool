using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 静静分组小工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        class record
        {
            public string num { get; set; }
            public float value { get; set; }
            public bool emptySign = false;
        }

        List<record> records = new List<record> { };
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                records = new List<record> { };
                //清空并重新排序展示
                string text = textBox1.Text;
                string[] textarr = text.Split('\n');
                foreach (var item in textarr)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string buf = "";
                        List<string> arBuf = new List<string> { };
                        foreach (var cr in item)
                        {
                            if (cr > 32)
                                buf += cr;
                            else
                            {
                                if (!string.IsNullOrEmpty(buf))
                                {
                                    arBuf.Add(buf);
                                    buf = "";
                                }
                            }
                        }
                        arBuf.Add(buf);
                        if (arBuf.Count > 1)
                        {
                            records.Add(new record { num = arBuf[0], value = float.Parse(arBuf[1]) });
                        }
                           
                    }
                }
                text = "";
                records = records.OrderBy(r => r.value).ToList();
                foreach (var item in records)
                {
                    text += (item.num + "  " + item.value + "\r\n");
                }

                this.textBox1.TextChanged -= new EventHandler( textBox1_TextChanged);
                this.textBox1.Text = text;
                this.textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            }
            catch (Exception ex)
            {

                MessageBox.Show("数据格式错误，检查后重试");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = "";
            int gcount = (int)this.numericUpDown1.Value;
            Dictionary<string, List<record>> result = new Dictionary<string, List<record>> { };
            List<record> buf = new List<record> { };
            for (int i = 0; i < records.Count; i++)
            {
                buf.Add(records[i]);
                if ((i+1) % gcount == 0)
                    buf.Add(new record { emptySign = true });
            }
            //分组
            for (int i = 0; i < buf.Count; i++)
            {
                string gname = "G" + ((i % gcount)+1).ToString();
                if (!result.ContainsKey(gname))
                    result[gname] = new List<record> { };
                if(buf[i].emptySign!=true)
                    result[gname].Add(buf[i]);
            }
            //显示
            //foreach (var item in result)
            //{
            //    text += item.Key + "\r\n";
            //    foreach (var va in item.Value)
            //    {
            //        text += (va.num + '\t' + va.value+"\r\n");
            //    }
            //}
            //textBox2.Text = text;
            int linecount = 0;
            int nodecount = records.Count;
            while (nodecount>0)
            {
                foreach (var item in result)
                {
                    if (linecount == 0)
                    {
                        text += (item.Key + "\t\t");
                    }
                    else
                    {
                        try
                        {
                            text += (item.Value[linecount-1].num + '\t' + item.Value[linecount-1].value + '\t');
                            nodecount--;
                        }
                        catch (Exception)
                        {

                           
                        }
                        
                    }
                }
                linecount++;
                text += "\r\n";
            }
            textBox2.Focus();
            textBox2.Text = text;
            textBox2.SelectAll();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {

        }
    }
}
