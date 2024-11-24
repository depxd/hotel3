using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel3
{
    public partial class HistoryForm : Form
    {
        private DataTable historyDataTable;

        public HistoryForm(DataTable historyData)
        {
            InitializeComponent();
            historyDataTable = historyData;
            dataGridViewHistory.DataSource = historyDataTable;
        }
    }
}
