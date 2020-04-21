using AnimationEditor.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationEditor.ViewModels
{
    public class HitboxV3EditorViewModel : Xe.Tools.Wpf.BaseNotifyPropertyChanged
    {
        private EditorAnimation _animationData;
        private int _selectedHitboxEntryIndex;
        private int _selectedHitboxIndex;

        public ObservableCollection<EditorAnimation.EditorRetroHitBox> HitboxEntries { get; }

        public ObservableCollection<string> HitboxItems { get; }

        public IEnumerable<string> AnimationsUsed { get; set; }

        public bool CanHitboxBeingRemoved => (AnimationsUsed?.Count() ?? 0) == 0 && IsHitboxEntrySelected;

        #region Hitbox entries

        public int SelectedHitboxEntryIndex
        {
            get => _selectedHitboxEntryIndex;
            set
            {
                _selectedHitboxEntryIndex = value;
                AnimationsUsed = _animationData.Animations
                    .SelectMany(x => x.Frames, (a, f) => new
                    {
                        Name = a.AnimName,
                        Hitbox = f.CollisionBox
                    })
                    .Where(x => x.Hitbox == value)
                    .Select(x => x.Name)
                    .Distinct();

                OnPropertyChanged();
                OnPropertyChanged(nameof(CanHitboxBeingRemoved));
                OnPropertyChanged(nameof(IsHitboxEntrySelected));
                OnPropertyChanged(nameof(SelectedHitboxEntryValue));
                OnPropertyChanged(nameof(AnimationsUsed));
                var entry = SelectedHitboxEntryValue;
            }
        }

        public bool IsHitboxEntrySelected => SelectedHitboxEntryIndex >= 0;

        public EditorAnimation.EditorRetroHitBox SelectedHitboxEntryValue
        {
            get
            {
                EditorAnimation.EditorRetroHitBox entry;
                var index = SelectedHitboxEntryIndex;
                if (index < 0 || HitboxEntries == null || (entry = HitboxEntries[index]) == null)
                    entry = new EditorAnimation.EditorRetroHitBox();
                return entry;
            }
        }

        #endregion

        #region Hitbox

        public int SelectedHitboxIndex
        {
            get => _selectedHitboxIndex;
            set
            {
                _selectedHitboxIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsHitboxEntrySelected));
                OnPropertyChanged(nameof(SelectedHitboxValue));
                var entry = SelectedHitboxEntryValue;
            }
        }

        public bool IsHitboxSelected => SelectedHitboxIndex >= 0;

        public EditorAnimation.EditorRetroHitBox.HitboxInfo SelectedHitboxValue => SelectedHitboxEntryValue.Hitboxes[SelectedHitboxIndex];

        #endregion
        
        public HitboxV3EditorViewModel(MainViewModel vm)
        {
            _animationData = vm.AnimationData;
            HitboxEntries = vm.RetroHitboxes;
            HitboxItems = vm.RetroHitboxStrings;
        }

        public void AddHitboxEntry()
        {
            var entry = new EditorAnimation.EditorRetroHitBox();
            HitboxEntries.Add(entry);
            HitboxItems.Add(GetHitboxEntryString(entry));
            SelectedHitboxEntryIndex = HitboxEntries.Count - 1;
        }

        public void RemoveHitboxEntry()
        {
            HitboxEntries.RemoveAt(SelectedHitboxEntryIndex);
            HitboxItems.RemoveAt(SelectedHitboxEntryIndex);
        }

        private static string GetHitboxEntryString(EditorAnimation.EditorRetroHitBox entry)
        {
            return MainViewModel.GetHitboxEntryString(entry);
        }
        private static string GetHitboxString(EditorAnimation.EditorRetroHitBox.HitboxInfo hb)
        {
            return MainViewModel.GetHitboxString(hb);
        }
    }
}
