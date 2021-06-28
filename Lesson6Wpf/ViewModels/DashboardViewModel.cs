using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Lesson6Wpf.Helpers;
using Lesson6Wpf.Services;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.Windows;
using System.Net;
using DAL.Models.DTO;

namespace Lesson6Wpf.ViewModels
{
	public class DashboardViewModel : INotifyPropertyChanged
	{
		private readonly IDataService dataService;
		private int currentPage = 1;
		private int takeCount = 5;
		public DashboardViewModel(IDataService dataService)
		{
			this.dataService = dataService;
		}


		#region fields
		public int CurrentPage
		{
			get { return this.currentPage; }
			set
			{
				this.currentPage = value;
				this.OnPropertyChanged(nameof(CurrentPage));
			}
		}
		private int SkipCount
		{
			get { return (currentPage - 1) * takeCount; }
		}

		private ObservableCollection<UnitEventDto> unitEvents = new ObservableCollection<UnitEventDto>();

		public ObservableCollection<UnitEventDto> UnitEvents
		{
			get => unitEvents;
			set
			{
				unitEvents = value;
				OnPropertyChanged(nameof(UnitEvents));
			}
		}

		#endregion

		#region commands

		public ICommand ShowUnitEventsCommand => new RelayCommand(async obj => await ShowUnitEventsCommandExecuted(SkipCount, takeCount));

		public ICommand NextPage => new RelayCommand(async o =>
		{
			CurrentPage++;
			var responce = await ShowUnitEventsCommandExecuted(SkipCount, takeCount);
			if (responce == 0)
			{
				CurrentPage--;
			}

		}
		);
		public ICommand PreviousPage => new RelayCommand(async o =>
		{
			if (CurrentPage == 1)
				return;
			CurrentPage--;
			var responce = await ShowUnitEventsCommandExecuted(SkipCount, takeCount);
			if (responce == 0)
			{
				CurrentPage++;
			}
		});
		public ICommand UpdateEvent => new RelayCommand(async item =>
		{
			await UpdateUnitEventValue(item as UnitEventDto);
		});
		public ICommand DeleteEvent => new RelayCommand(async item =>
		{
			await DeleteUnitEventValue(item as UnitEventDto);
			await ShowUnitEventsCommandExecuted(SkipCount, takeCount);
		});

		private async Task<int> ShowUnitEventsCommandExecuted(int skip, int take)
		{
			var collection = await dataService.GetUnitEvents(skip, take);
			UnitEvents = new ObservableCollection<UnitEventDto>(collection);
			return collection.Count;
		}

		public async Task UpdateUnitEventValue(UnitEventDto unitEvent)
		{
			await dataService.UpdateEvent(unitEvent);
		}
		public async Task DeleteUnitEventValue(UnitEventDto unitEvent)
		{
			await dataService.DeleteEvent(unitEvent);
		}

		#endregion

		#region OnPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

	}
}