using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magento_Price_Updater
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            startUp();
        }

        public static void startUp()
        {
            try
            {
                string seed = "C:\\Users\\Danny\\source\\repos\\Magento Price Updater\\Magento Price Updater\\databases\\seed.sql"; //TODO remove this hardcode
                DatabaseUtil.setupDatabase(seed);
            }
            catch (Exception ex)
            {
                FileUtil.writeExeptionToFile(ex.ToString());
            }
        }
    }
}
