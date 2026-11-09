using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AttendanceManager.ViewModels
{
    public class AttendanceItemViewModel : INotifyPropertyChanged
    {
        private int _index;
        private bool _isPresent;
        private string? _note;
        private bool _isOriginalPresent;
        private string? _originalNote;

        public event Action? OnAttendanceChanged;

        public int Index
        {
            get => _index;
            set { _index = value; OnPropertyChanged(); }
        }

        public int AttendanceId { get; set; }
        public int ScheduleId { get; set; }
        public int StudentId { get; set; }
        public string RollNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public bool IsPresent
        {
            get => _isPresent;
            set
            {
                if (_isPresent != value)
                {
                    _isPresent = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsAbsent));
                    OnPropertyChanged(nameof(StatusText));
                    OnPropertyChanged(nameof(IsModified));
                    OnAttendanceChanged?.Invoke();
                }
            }
        }

        public bool IsAbsent
        {
            get => !_isPresent;
            set
            {
                if (value)
                {
                    IsPresent = false;
                }
            }
        }

        public string StatusText => IsPresent ? "Có mặt" : "Vắng";

        public string? Note
        {
            get => _note;
            set
            {
                if (_note != value)
                {
                    _note = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsModified));
                    OnAttendanceChanged?.Invoke();
                }
            }
        }

        public bool OriginalIsPresent
        {
            get => _isOriginalPresent;
            set
            {
                _isOriginalPresent = value;
                OnPropertyChanged(nameof(IsModified));
            }
        }

        public string? OriginalNote
        {
            get => _originalNote;
            set
            {
                _originalNote = value;
                OnPropertyChanged(nameof(IsModified));
            }
        }

        public bool IsModified => (IsPresent != OriginalIsPresent) || ((Note ?? string.Empty).Trim() != (OriginalNote ?? string.Empty).Trim());

        public void AcceptChanges()
        {
            OriginalIsPresent = IsPresent;
            OriginalNote = Note;
            OnPropertyChanged(nameof(IsModified));
        }

        public void ResetChanges()
        {
            IsPresent = OriginalIsPresent;
            Note = OriginalNote;
            OnPropertyChanged(nameof(IsModified));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
