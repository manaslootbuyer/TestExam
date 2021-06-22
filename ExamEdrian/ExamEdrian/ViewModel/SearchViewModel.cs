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

        DateTime _minDate;
        public DateTime MinDate
        {
            get => _minDate;
            set => SetProperty(ref _minDate, value);
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
            CustomersList = new ObservableCollection<CustomerDTO>();
            MinDate = DateTime.Now;
            SelectedDate = DateTime.Now;

            CustomersList.Add(new CustomerDTO
            {
                GuestName = "Test",
                Arrived = "test",
                Depart = "test",
                ReservationId = "1"
            });
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
            var result = await _searchService.GetCustomers(ParkCode, SelectedDate.ToString("YYYY-MM-DD"));
            ShowMainLoader = false;

            if(result != null && result.Any())
            {
                CustomersList.AddRange(result);
            }
            else
            {
                await Page.DisplayAlert("", Strings.NoResultsFound, Strings.Ok);
            }
            SearchCommand.CanRun = true;
        }
    }
}
