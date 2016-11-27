using GalaSoft.MvvmLight;

namespace RandomSongPicker.ViewModel
{
	public class LetterViewModel : ViewModelBase
	{
		private bool _isRevealed;
		private char _letter;

		public bool IsRevealed
		{
			get { return _isRevealed; }
			set
			{
				_isRevealed = value;
				RaisePropertyChanged(() => IsRevealed);
			}
		}

		public char Letter
		{
			get { return _letter; }
			set
			{
				_letter = value;
				RaisePropertyChanged(() => Letter);
			}
		}
	}
}