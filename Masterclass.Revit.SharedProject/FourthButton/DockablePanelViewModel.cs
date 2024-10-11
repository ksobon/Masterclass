using Masterclass.Revit.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Masterclass.Revit.FourthButton
{
    public class DockablePanelViewModel : INotifyPropertyChanged
    {
        public RelayCommand<object> LoadRequirements { get; set; }
        public RelayCommand<object> ClearRequirements { get; set; }

        private ObservableCollection<RequirementWrapper> _requirements = new ObservableCollection<RequirementWrapper>();
        public ObservableCollection<RequirementWrapper> Requirements
        {
            get { return _requirements;}
            set { _requirements = value; RaisePropertyChanged(nameof(Requirements)); }
        }

        public DockablePanelViewModel()
        {
            LoadRequirements = new RelayCommand<object>(OnLoadRequirements);
            ClearRequirements = new RelayCommand<object>(OnClearRequirements);

            AppCommand.Messages.ElementsAdded += OnElementsAdded;
        }

        public void Clean()
        {
            AppCommand.Messages.ElementsAdded -= OnElementsAdded;
        }

        private void OnElementsAdded(object sender, EventArgs e)
        {
            var obj = (Messages.ElementsAddedEventArgs)e;
            foreach (var element in obj.AddedElements)
            {
                var found = Requirements.FirstOrDefault(x =>
                    x.FamilyName == element.FamilyName && x.FamilyType == element.FamilyType);
                if (found == null)
                    continue;

                found.PlacedCount++;
            }
        }

        private void OnClearRequirements(object obj)
        {
            foreach (var rw in Requirements)
            {
                rw.PlacedCount = 0;
            }
        }

        private void OnLoadRequirements(object obj)
        {
            var filePath = DialogUtils.SelectFile();
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var requirements = new List<RequirementWrapper>();
            var existingFile = new FileInfo(filePath);
            using (var package = new ExcelPackage(existingFile))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.End.Row;

                for (var row = 2; row <= rowCount; row++)
                {
                    var fn = worksheet.Cells[row, 1].Value.ToString();
                    var ft = worksheet.Cells[row, 2].Value.ToString();
                    var count = int.Parse(worksheet.Cells[row, 3].Value.ToString());
                    var req = new RequirementWrapper(fn, ft, count);
                    requirements.Add(req);
                }
            }

            Requirements.Clear();
            requirements.ForEach(x => Requirements.Add(x));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
