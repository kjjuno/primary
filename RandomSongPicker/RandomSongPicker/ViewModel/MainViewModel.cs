using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace RandomSongPicker.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
	/// </para>
	/// <para>
	/// You can also use Blend to data bind with the tool's support.
	/// </para>
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		private string _text;
		private bool _isRollButtonVisible;
		private bool _isRandomLetterVisible;
		private bool _isTextVisible;
		private bool _isSongListVisible;
		private string _currentLetter;
		private SongDatabase _songDatabase = new SongDatabase();
		private Song _selectedSong;

		public Dispatcher Dispatcher { get; set; }

		public string Text
		{
			get { return _text; }
			set
			{
				_text = value;
				Letters = Text.Select(c => new LetterViewModel {IsRevealed = false, Letter = c}).ToList();
				RaisePropertyChanged(() => Text);
				RaisePropertyChanged(() => Letters);
			}
		}

		public List<LetterViewModel> Letters { get; private set; }

		public bool IsRollButtonVisible
		{
			get { return _isRollButtonVisible; }
			set
			{
				_isRollButtonVisible = value;
				RaisePropertyChanged(() => IsRollButtonVisible);
			}
		}

		public bool IsRandomLetterVisible
		{
			get { return _isRandomLetterVisible; }
			set
			{
				_isRandomLetterVisible = value;
				RaisePropertyChanged(() => IsRandomLetterVisible);
			}
		}

		public bool IsTextVisible
		{
			get { return _isTextVisible; }
			set
			{
				_isTextVisible = value;
				RaisePropertyChanged(() => IsTextVisible);
			}
		}

		public bool IsSongListVisible
		{
			get { return _isSongListVisible; }
			set
			{
				_isSongListVisible = value;
				RaisePropertyChanged(() => IsSongListVisible);
			}
		}

		public string CurrentLetter
		{
			get { return _currentLetter; }
			set
			{
				_currentLetter = value;
				RaisePropertyChanged(() => CurrentLetter);
			}
		}

		public Song SelectedSong
		{
			get { return _selectedSong; }
			set
			{
				_selectedSong = value;
				RaisePropertyChanged(() => SelectedSong);
				ShowText();
			}
		}

		public RelayCommand<TextBlock> RollCommand { get; }

		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
		{
			if (IsInDesignMode)
			{
				// Code runs in Blend --> create design time data.
				
			}
			else
			{
				// Code runs "for real"
			}

			IsTextVisible = true;
			IsRollButtonVisible = true;
			IsRandomLetterVisible = false;
			IsSongListVisible = false;

			Text = "Thanksgiving";
			CurrentLetter = "";

			


			RollCommand = new RelayCommand<TextBlock>(RollCommandExecute);
		}

		public ObservableCollection<Song> SearchResults { get; set; } = new ObservableCollection<Song>();

		public void RollCommandExecute(TextBlock textBlock)
		{
			SelectedSong = null;
			ShowRandomLetter();

			Task.Factory.StartNew(async () =>
			{
				Random random = new Random();

				var remaining = Letters.Where(l => !l.IsRevealed).ToArray();
				LetterViewModel chosen = null;

				for (int i = 0; i < 50; i++)
				{
					var idx = random.Next(0, remaining.Length);
					chosen = remaining[idx];

					await Dispatcher.BeginInvoke(new Action(() =>
					{
						textBlock.Text = chosen.Letter.ToString();
					}));

					await Task.Delay(5);
				}

				await Task.Delay(3000);

				await Dispatcher.BeginInvoke(new Action(() =>
				{
					chosen.IsRevealed = true;
					ShowSongList(chosen);
				}));
			});
		}

		private void ShowSongList(LetterViewModel chosen)
		{
			IsTextVisible = false;
			IsRollButtonVisible = false;
			IsRandomLetterVisible = false;
			IsSongListVisible = true;

			SearchResults.Clear();
			var results = _songDatabase.Search(chosen.Letter.ToString());
			foreach (var result in results)
			{
				SearchResults.Add(result);
			}
		}

		private void ShowRandomLetter()
		{
			IsTextVisible = false;
			IsRollButtonVisible = false;
			IsRandomLetterVisible = true;
			IsSongListVisible = false;
		}

		private void ShowText()
		{
			IsTextVisible = true;
			IsRollButtonVisible = true;
			IsRandomLetterVisible = false;
			IsSongListVisible = false;
		}
	}
}