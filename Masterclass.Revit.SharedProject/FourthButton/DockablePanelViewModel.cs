using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Masterclass.Revit.Utilities;

namespace Masterclass.Revit.FourthButton
{
    public class DockablePanelViewModel : ViewModelBase
    {
        public RelayCommand LoadRequirements { get; set; }

        public DockablePanelViewModel()
        {
            LoadRequirements = new RelayCommand(OnLoadRequirements);
        }

        private void OnLoadRequirements()
        {
            var filePath = DialogUtils.SelectFile();
            if (string.IsNullOrWhiteSpace(filePath))
                return;


        }
    }
}
