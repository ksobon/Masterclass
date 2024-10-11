using System.ComponentModel;
using MessagePack;
using MessagePack.Resolvers;

namespace Masterclass.Revit
{
    public class StoredSettings : INotifyPropertyChanged
    {
        private string _requirementsFilePath;
        public string RequirementsFilePath
        {
            get { return _requirementsFilePath; }
            set { _requirementsFilePath = value; RaisePropertyChanged(nameof(RequirementsFilePath)); }
        }

        public StoredSettings()
        {
        }

        public StoredSettings FromByteArray(byte[] arrBytes)
        {
            return MessagePackSerializer.Deserialize<StoredSettings>(arrBytes,
                ContractlessStandardResolver.Options);
        }

        public byte[] ToByteArray()
        {
            return MessagePackSerializer.Serialize(this, ContractlessStandardResolver.Options);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
