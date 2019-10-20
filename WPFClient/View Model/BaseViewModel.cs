using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.View_Model
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    { 
            //The interface only includes this evennt
            public event PropertyChangedEventHandler PropertyChanged;

            //Common implementations of SetProperty
            protected bool SetProperty<T>(ref T field, T value, [CallerMemberName]string name = null)
            {
                bool propertyChanged = false;

                //If we have a different value, do stuff
                if (!EqualityComparer<T>.Default.Equals(field, value))
                {
                    field = value;
                    OnPropertyChanged(name);
                    propertyChanged = true;
                }

                return propertyChanged;
            }

            protected void OnPropertyChanged([CallerMemberName]string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }