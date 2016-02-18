using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ConnectionistModel
{
    static class Program
    {
        /// <summary>        
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());            
        }
    }

    static class SimulatorAccessor
    {
        public static Random random = new Random();
        public static Simulator simulator = new Simulator(random);
        public static List<BatchData> batchDataList = new List<BatchData>();
    }
    static class ArchitectureGraphAccessor
    {
        public static Form architectureGraphForm = new Form();
        public static Microsoft.Msagl.GraphViewerGdi.GViewer architectureGraphViewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        public static Microsoft.Msagl.Drawing.Graph architectureGraph = new Microsoft.Msagl.Drawing.Graph("ArchitectureGraph");
        public static System.Collections.Generic.List<string> nodeList = new System.Collections.Generic.List<string>();
    }
}
