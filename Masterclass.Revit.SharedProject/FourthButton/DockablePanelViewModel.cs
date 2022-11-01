using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Masterclass.Revit.Utilities;
using OfficeOpenXml;

namespace Masterclass.Revit.FourthButton
{
    public class DockablePanelViewModel : ViewModelBase
    {
        public RelayCommand LoadRequirements { get; set; }
        public RelayCommand ClearRequirements { get; set; }

        private ObservableCollection<RequirementWrapper> _requirements = new ObservableCollection<RequirementWrapper>();
        public ObservableCollection<RequirementWrapper> Requirements
        {
            get { return _requirements;}
            set { _requirements = value; RaisePropertyChanged(() => Requirements); }
        }

        public DockablePanelViewModel()
        {
            LoadRequirements = new RelayCommand(OnLoadRequirements);
            ClearRequirements = new RelayCommand(OnClearRequirements);

            Messenger.Default.Register<AddedElementsMessage>(this, OnAddedElementsMessage);
        }

        private void OnClearRequirements()
        {
            foreach (var rw in Requirements)
            {
                rw.PlacedCount = 0;
            }
        }

        private void OnAddedElementsMessage(AddedElementsMessage obj)
        {
            foreach (var element in obj.AddedElements)
            {
                var found = Requirements.FirstOrDefault(x =>
                    x.FamilyName == element.FamilyName && x.FamilyType == element.FamilyType);
                if (found == null)
                    continue;

                found.PlacedCount++;
            }
        }

        private void OnLoadRequirements()
        {
            var filePath = DialogUtils.SelectFile();
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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
    }
}
