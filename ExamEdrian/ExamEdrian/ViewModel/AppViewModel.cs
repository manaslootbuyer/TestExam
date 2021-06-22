using Acr.UserDialogs;
using MvvmAspire;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExamEdrian
{
    public abstract class AppViewModel : BaseViewModel
    {
        #region Properties
        public IUserDialogs Dialogs;
        private bool _showMainLoader;

        public bool ShowMainLoader
        {
            get => _showMainLoader;
            set
            {
                SetProperty(ref _showMainLoader, value);

                if (_showMainLoader)
                {
                    this.Dialogs.ShowLoading("");
                }
                else
                {
                    this.Dialogs.HideLoading();
                }
            }
        }
        #endregion

        public AppViewModel()
        {
            Dialogs = UserDialogs.Instance;
        }
  
        /// <summary>
        /// Exposes navigation functionality to the view model.
        /// </summary>
        [Unity.Attributes.Dependency]
        public MvvmAspire.Services.INavigation Navigation { get; set; }

        /// <summary>
        /// Exposes page functionality to the view model.
        /// </summary>
        [Unity.Attributes.Dependency]
        public IPage Page { get; set; }

        public override void OnPush(bool modal)
        {
            base.OnPush(modal);
        }

        public override void OnRevisit(bool modal, bool fromModal)
        {
            base.OnRevisit(modal, fromModal);
        }

        public override void OnPop(bool modal)
        {
            base.OnPop(modal);
        }

        public override void OnRemove(bool modal)
        {
            base.OnRemove(modal);
        }
    }
}
