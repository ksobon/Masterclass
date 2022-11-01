using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Masterclass.Revit.FourthButton
{
    public class RequirementWrapper : INotifyPropertyChanged
    {
        public string FamilyName { get; set; }
        public string FamilyType { get; set; }
        public int RequiredCount { get; set; }

        private int _placedCount;
        public int PlacedCount
        {
            get { return _placedCount;}
            set { _placedCount = value; RaisePropertyChanged(nameof(PlacedCount)); }
        }

        public string FamilyNameAndType
        {
            get { return $"{FamilyName} - {FamilyType}"; }
        }

        public RequirementWrapper(string fn, string ft, int count)
        {
            FamilyName = fn;
            FamilyType = ft;
            RequiredCount = count;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
