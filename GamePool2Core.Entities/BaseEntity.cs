using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GamePool2Core.Entities
{
        [Serializable]   

    public class BaseEntity : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        #region RaisePropertyChanged([CallerMemberName] string)
        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="caller">The caller.</param>
        /// <author>Jason Estes</author>
        /// <datetime>9/17/2014</datetime>
        internal void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
        #endregion RaisePropertyChanged
    }
}
