using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterFootprint.ViewModels;
namespace WaterFootprint;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();

        BindingContext = new RegisterViewModel(Navigation);
    }
}