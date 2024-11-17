using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using SavePDFApp.Models;
using System.Windows.Media;
using System.Windows.Input;
using SavePDFApp.Commands;

namespace SavePDFApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _textBoxContent;
        private string _labelContent;

        public string TextBoxContent
        {
            get => _textBoxContent;
            set
            {
                _textBoxContent = value;
                LabelContent = value; // Update label content
                OnPropertyChanged();
            }
        }

        public string LabelContent
        {
            get => _labelContent;
            set
            {
                _labelContent = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Person> Persons { get; }

        public ICommand PrintCommand { get; }

        public MainViewModel()
        {
            // Initialize table data
            Persons = new ObservableCollection<Person>
            {
                new Person { Number = 1, LastName = "Иванов", FirstName = "Иван" },
                new Person { Number = 2, LastName = "Сидоров", FirstName = "Сидор" },
                new Person { Number = 3, LastName = "Петров", FirstName = "Петр" }
            };

            PrintCommand = new RelayCommand(PrintToPdf);
        }

        public void PrintToPdf()
        {
            // Open the save file dialog
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = TextBoxContent,
                DefaultExt = ".pdf",
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            // If the user selects a file path
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                using (PdfDocument document = new PdfDocument())
                {
                    document.Info.Title = TextBoxContent;

                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Define fonts
                    XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
                    XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
                    XFont contentFont = new XFont("Arial", 12);

                    // Center the title
                    string title = TextBoxContent;
                    double titleWidth = gfx.MeasureString(title, titleFont).Width;
                    gfx.DrawString(title, titleFont, XBrushes.Black,
                        new XPoint((page.Width - titleWidth) / 2, 60));

                    // Calculate table dimensions
                    int startY = 100;
                    int cellHeight = 20;
                    int numberOfColumns = 3;

                    double startX = 40;
                    double tableWidth = page.Width - 2 * startX;
                    double cellWidth = tableWidth / numberOfColumns;

                    // Draw header row with borders
                    gfx.DrawRectangle(XPens.Black, startX, startY, cellWidth, cellHeight);
                    gfx.DrawRectangle(XPens.Black, startX + cellWidth, startY, cellWidth, cellHeight);
                    gfx.DrawRectangle(XPens.Black, startX + 2 * cellWidth, startY, cellWidth, cellHeight);

                    gfx.DrawString("Номер", headerFont, XBrushes.Black, new XPoint(startX + 5, startY + 15));
                    gfx.DrawString("Фамилия", headerFont, XBrushes.Black, new XPoint(startX + cellWidth + 5, startY + 15));
                    gfx.DrawString("Имя", headerFont, XBrushes.Black, new XPoint(startX + 2 * cellWidth + 5, startY + 15));

                    // Draw table content with borders
                    double currentY = startY + cellHeight;
                    foreach (var person in Persons)
                    {
                        // Draw borders for each cell
                        gfx.DrawRectangle(XPens.Black, startX, currentY, cellWidth, cellHeight);
                        gfx.DrawRectangle(XPens.Black, startX + cellWidth, currentY, cellWidth, cellHeight);
                        gfx.DrawRectangle(XPens.Black, startX + 2 * cellWidth, currentY, cellWidth, cellHeight);

                        // Add content inside the cells
                        gfx.DrawString(person.Number.ToString(), contentFont, XBrushes.Black,
                            new XPoint(startX + 5, currentY + 15));
                        gfx.DrawString(person.LastName, contentFont, XBrushes.Black,
                            new XPoint(startX + cellWidth + 5, currentY + 15));
                        gfx.DrawString(person.FirstName, contentFont, XBrushes.Black,
                            new XPoint(startX + 2 * cellWidth + 5, currentY + 15));

                        currentY += cellHeight; // Move to the next row
                    }

                    // Save the PDF
                    document.Save(filePath);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
