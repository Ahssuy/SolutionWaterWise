﻿using WaterFootprint.ViewModels;

namespace WaterFootprint;

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
        }

     
    }

