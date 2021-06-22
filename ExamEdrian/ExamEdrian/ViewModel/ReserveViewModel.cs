using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ExamEdrian.Helpers;
using ExamEdrian.Models;
using ExamEdrian.Resources;
using ExamEdrian.Services;
using MvvmAspire;

namespace ExamEdrian.ViewModel
{
    public class ReserveViewModel : AppViewModel
    {
        #region Properties
      
        string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        #endregion

        #region Commands

        public RelayCommand SaveCommand { get; private set; }

        #endregion

        readonly ISearchService _searchService;
        readonly string _resId;
        public ReserveViewModel(ISearchService searchService, string resId)
        {
            _resId = resId;
            _searchService = searchService;
            SaveCommand = new RelayCommand(async () => await Save());
        }

        async Task Save()
        {
            SaveCommand.CanRun = false;
            if (string.IsNullOrEmpty(Email) || !AppHelper.IsValidEmail(Email))
            {
                await Page.DisplayAlert("", Strings.EmailErrorMessage, Strings.Ok);
                SaveCommand.CanRun = true;
                return;
            }
            ShowMainLoader = true;
            var result = await _searchService.SaveEmailAsync(new Commands.ReserveCommand
            {
                 ResId = _resId,
                 UserEmail = Email
            });
            ShowMainLoader = false;

            if (result)
            {
                await Page.DisplayAlert("", Strings.SaveSuccessfully, Strings.Ok);
                await Navigation.PopAsync();
            }
            else
            {
                await Page.DisplayAlert("", Strings.SavingFailed, Strings.Ok);
            }
            SaveCommand.CanRun = true;
        }
    }
}
