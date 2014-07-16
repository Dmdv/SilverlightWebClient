﻿using System;
using WebClient.ICS.Client.Commands;

namespace WebClient.ICS.Client.Editors
{
    public partial class BooleanEditor : IActiveAware
    {
        public BooleanEditor()
        {
            InitializeComponent();
            BooleanEditorControl.ItemsSource = new[] { "True", "False" };
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                InvokeIsActiveChanged(null);
            }
        }

        public event EventHandler IsActiveChanged;

        public void InvokeIsActiveChanged(EventArgs e)
        {
            var handler = IsActiveChanged;
            if (handler != null) handler(this, e);
        }
    }
}
