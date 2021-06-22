using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ExamEdrian.Models;
using ExamEdrian.Resources;
using ExamEdrian.Services;
using MvvmAspire;

namespace ExamEdrian.ViewModel
{
    public class SearchViewModel : AppViewModel
    {
        #region Properties
        private ObservableCollection<CustomerDTO> _customerList;
        public ObservableCollection<CustomerDTO> CustomersList
        {
            get => _customerList;
            set => SetProperty(ref _customerList, value);
        }

        CustomerDTO _selectedCustomer;
        public CustomerDTO SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                if (_selectedCustomer != null)
                {
                    if (GoToReserveCommand.CanRun)
                    {
                        GoToReserveCommand.Execute(_selectedCustomer.ReservationId);
                        _selectedCustomer = null;
                    }
                }
            }
        }

        string _parkCode;
        public string ParkCode
        {
            get => _parkCode;
            set => SetProperty(ref _parkCode, value);
        }

        DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        #endregion

        #region Commands

        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand<string> GoToReserveCommand { get; private set; }

        #endregion

        readonly ISearchService _searchService;
        public SearchViewModel(ISearchService searchService)
        {
            _searchService = searchService;
            SearchCommand = new RelayCommand(async () => await Search());
            GoToReserveCommand = new RelayCommand<string>(async resId => await GoToReserve(resId));
            SelectedDate = DateTime.Now;
            CustomersList = new ObservableCollection<CustomerDTO>();
        }

        private async Task GoToReserve(string resId)
        {
            GoToReserveCommand.CanRun = false;
            await Navigation.PushAsync<ReserveViewModel>(new { resId = resId });
            GoToReserveCommand.CanRun = false;
        }

        async Task Search()
        {
            SearchCommand.CanRun = false;
            if (string.IsNullOrEmpty(ParkCode))
            {
                await Page.DisplayAlert("", Strings.ParkCodeErrorMessage, Strings.Ok);
                SearchCommand.CanRun = true;
                return;
            }
            ShowMainLoader = true;
            var result = await _searchService.GetCustomers(ParkCode, SelectedDate.ToString("yyyy-MM-dd"));
            ShowMainLoader = false;

            if(result != null && result.Any())
            {
                CustomersList = new ObservableCollection<CustomerDTO>(result);
            }
            else
            {
                await Page.DisplayAlert("", Strings.NoResultsFound, Strings.Ok);
                CustomersList.Clear();
            }
            SearchCommand.CanRun = true;
        }
    }
}
