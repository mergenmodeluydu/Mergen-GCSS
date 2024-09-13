using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

public class DataGridViewHandler
{
    private DataGridView dataGridView;

    public DataGridViewHandler(DataGridView dataGridView)
    {
        this.dataGridView = dataGridView;
    }

    public void AddData(int LastInt1, int LastInt2, string LastString1, string LastString2, float LastFloat1, float LastFloat2, float LastFloat3, float LastFloat4, float LastFloat5, float LastFloat6, float LastFloat7, float LastFloat8, float LastFloat9, float LastFloat10, float LastFloat11, float LastFloat12, float LastFloat13, float LastFloat14, string LastString3, float LastFloat15, int LastInt3)
    {
        dataGridView.Invoke((MethodInvoker)delegate
        {
            dataGridView.Rows.Add(LastInt1, LastInt2, LastString1, LastString2, LastFloat1, LastFloat2, LastFloat3, LastFloat4, LastFloat5, LastFloat6, LastFloat7, LastFloat8, LastFloat9, LastFloat10, LastFloat11, LastFloat12, LastFloat13, LastFloat14, LastString3, LastFloat15, LastInt3);
            // 1. Paket numarasına göre sıralama yap
            dataGridView.Sort(dataGridView.Columns[0], System.ComponentModel.ListSortDirection.Descending);

            // 2. En üst satırı seç ve vurgula
            dataGridView.Rows[0].Selected = false;
        });
    }
    public void SaveDataGridViewToCSV(string filePath)
    {
        StringBuilder csvContent = new StringBuilder();

        // Başlıkları ekleyin
        string[] columnNames = dataGridView.Columns
            .Cast<DataGridViewColumn>()
            .Select(column => column.HeaderText)
            .ToArray();
        csvContent.AppendLine(string.Join(";", columnNames));

        // Satırları ekleyin
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            if (!row.IsNewRow)
            {
                string[] cells = row.Cells
                    .Cast<DataGridViewCell>()
                    .Select(cell => cell.Value?.ToString() ?? string.Empty)
                    .ToArray();
                csvContent.AppendLine(string.Join(";", cells));
            }
        }

        // CSV dosyasına yaz
        File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);
    }
}
