using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarPrice.Helpers
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new(prop));

        public void Set<T>(ref T field, T value, [CallerMemberName] string prop = "")
        {
            if (Equals(field, value)) return;

            field = value;
            OnPropertyChanged(prop);
            return;
        }
    }
}
