using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.Commands;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// Контрол для редактирования даты и времени, поддерживает свойства выборки даты и времени.
    /// </summary>
    public partial class DateTimeControl : IActiveAware
    {
        // ReSharper disable MemberCanBePrivate.Global

        /// <summary>
        /// The <see cref="SelectedDate" /> dependency property's name.
        /// </summary>
        private const string SelectedDatePropertyName = "SelectedDate";

        /// <summary>
        /// The <see cref="DisplayDate" /> dependency property's name.
        /// </summary>
        private const string DisplayDatePropertyName = "DisplayDate";

        /// <summary>
        /// The <see cref="Time" /> dependency property's name.
        /// </summary>
        private const string TimePropertyName = "Time";

        /// <summary>
        /// Identifies the <see cref="SelectedDate" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
            SelectedDatePropertyName,
            typeof (DateTime?),
            typeof (DateTimeControl),
            new PropertyMetadata(SelectedDateChangedCallback));

        /// <summary>
        /// Identifies the <see cref="DisplayDate" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayDateProperty = DependencyProperty.Register(
            DisplayDatePropertyName,
            typeof (DateTime),
            typeof (DateTimeControl),
            null);

        /// <summary>
        /// Identifies the <see cref="Time" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            TimePropertyName,
            typeof (DateTime),
            typeof (DateTimeControl),
            new PropertyMetadata(TimeChangedCallback));

        public event EventHandler IsActiveChanged;

        public void InvokeIsActiveChanged(EventArgs e)
        {
            EventHandler handler = IsActiveChanged;
            if (handler != null) handler(this, e);
        }

        public DateTimeControl()
        {
            InitializeComponent();
            datePicker.SelectedDateChanged -= DatePickerSelectedDateChanged;
            datePicker.SelectedDateChanged += DatePickerSelectedDateChanged;
            FormatLabel.Text = "(" + Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern + ")";
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

        /// <summary>
        /// Gets or sets the value of the <see cref="Time" />
        /// property. This is a dependency property.
        /// </summary>
        public DateTime Time
        {
            get { return (DateTime) GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="DisplayDate" />
        /// property. This is a dependency property.
        /// </summary>
        public DateTime DisplayDate
        {
            get { return (DateTime) GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="SelectedDate" />
        /// property. This is a dependency property.
        /// </summary>
        public DateTime? SelectedDate
        {
            get { return (DateTime?) GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        private static void TimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = (DateTimeControl)d;
            if (!thisObject.IsActive) return;

            var time = e.NewValue.Cast<DateTime>();

            if (thisObject.SelectedDate.HasValue)
            {
                thisObject.SelectedDate = thisObject.SelectedDate.Value.Date.Add(time.TimeOfDay);
            }
        }

        private static void SelectedDateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = d.Cast<DateTimeControl>();
            var newValue = e.NewValue == null ? DateTime.Now : e.NewValue.Cast<DateTime>();

            if (!thisObject.IsActive) return;

            // Чтобы не срабатывало при изменении Time.
            if (e.OldValue != null)
            {
                var oldValue = e.OldValue.Cast<DateTime>();

                if (newValue.Date == oldValue.Date) return;

                thisObject.datePicker.SelectedDate = newValue;
            }

            // Обновить SelectedDate при изменении TimePicker.
            if (!thisObject.SelectedDate.HasValue) return;

            thisObject.SelectedDate = newValue.Date.Add(thisObject.Time.TimeOfDay);
            thisObject.datePicker.SelectedDate = newValue;
        }

        private void DatePickerSelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!IsActive) return;

            SelectedDate = datePicker.SelectedDate;

            Debug.WriteLine(">> DateTimeControl SelectedDateChanged");
        }
    }
}