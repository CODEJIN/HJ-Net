using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionistModel
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.FormClosed += AboutForm_FormClosed;

            StringBuilder mainLicenseDisplayStringBuilder = new StringBuilder();
            mainLicenseDisplayStringBuilder.AppendLine("HJ-Net for Connectionist modeling");
            mainLicenseDisplayStringBuilder.AppendLine();
            mainLicenseDisplayStringBuilder.AppendLine("Copyright(c) 2015 - 2016 Heejo You");
            mainLicenseDisplayStringBuilder.AppendLine();
            mainLicenseDisplayStringBuilder.AppendLine("All rights reserved.");
            mainLicenseDisplayStringBuilder.AppendLine();
            mainLicenseDisplayStringBuilder.AppendLine("MIT License");
            mainLicenseDisplayStringBuilder.AppendLine();
            mainLicenseDisplayStringBuilder.AppendLine("Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:");
            mainLicenseDisplayStringBuilder.AppendLine();
            mainLicenseDisplayStringBuilder.AppendLine("The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.");
            mainLicenseDisplayStringBuilder.AppendLine();
            mainLicenseDisplayStringBuilder.AppendLine("THE SOFTWARE IS PROVIDED* AS IS *, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.");
            mainLicenseDisplayTextBox.Text = mainLicenseDisplayStringBuilder.ToString();


            StringBuilder msAGLLicenseDisplayStringBuilder = new StringBuilder();
            msAGLLicenseDisplayStringBuilder.AppendLine("Microsoft Automatic Graph Layout, MSAGL");
            msAGLLicenseDisplayStringBuilder.AppendLine();
            msAGLLicenseDisplayStringBuilder.AppendLine("Copyright(c) Microsoft Corporation");
            msAGLLicenseDisplayStringBuilder.AppendLine();
            msAGLLicenseDisplayStringBuilder.AppendLine("All rights reserved.");
            msAGLLicenseDisplayStringBuilder.AppendLine();
            msAGLLicenseDisplayStringBuilder.AppendLine("MIT License");
            msAGLLicenseDisplayStringBuilder.AppendLine();
            msAGLLicenseDisplayStringBuilder.AppendLine("Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:");
            msAGLLicenseDisplayStringBuilder.AppendLine();
            msAGLLicenseDisplayStringBuilder.AppendLine("The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.");
            msAGLLicenseDisplayStringBuilder.AppendLine();
            msAGLLicenseDisplayStringBuilder.AppendLine("THE SOFTWARE IS PROVIDED* AS IS *, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.");
            msAGLLicenseDisplayTextBox.Text = msAGLLicenseDisplayStringBuilder.ToString();


            StringBuilder mathNetLicenseDisplayStringBuilder = new StringBuilder();

            mathNetLicenseDisplayStringBuilder.AppendLine("Math.NET Numerics License(MIT / X11)");
            mathNetLicenseDisplayStringBuilder.AppendLine();
            mathNetLicenseDisplayStringBuilder.AppendLine("Copyright(c) 2002 - 2015 Math.NET");
            mathNetLicenseDisplayStringBuilder.AppendLine();
            mathNetLicenseDisplayStringBuilder.AppendLine("Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/ or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:");
            mathNetLicenseDisplayStringBuilder.AppendLine();
            mathNetLicenseDisplayStringBuilder.AppendLine("The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.");
            mathNetLicenseDisplayStringBuilder.AppendLine();
            mathNetLicenseDisplayStringBuilder.AppendLine("THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.");
            mathDotNetLicenseTextBox.Text = mathNetLicenseDisplayStringBuilder.ToString();
        }

        private void AboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Visible = true;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
