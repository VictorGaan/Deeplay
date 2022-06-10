using Deeplay.PersonnelRecords.Models;
using Deeplay.PersonnelRecords.Persistence.Contexts;
using Deeplay.PersonnelRecords.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Deeplay.PersonnelRecords.ViewModels
{
    public class MainVM : ObservableObject
    {
        private ICollectionView _employees;
        private Employee _employee;
        private ObservableCollection<Position> _positions;
        private ObservableCollection<Subdivision> _subdivisions;
        private Position _position;
        private Subdivision _subdivision;
        public ICommand InsertCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public Position SelectedPosition
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    LoadData();
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(Employees));
            }
        }

        public Subdivision SelectedSubdivision
        {
            get => _subdivision;
            set
            {
                if (_subdivision != value)
                {
                    _subdivision = value;
                    LoadData();
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(Employees));
            }
        }

        public ObservableCollection<Position> Positions
        {
            get => _positions;
            set
            {
                if (_positions != value)
                    _positions = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Subdivision> Subdivisions
        {
            get => _subdivisions;
            set
            {
                if (_subdivisions != value)
                    _subdivisions = value;
                OnPropertyChanged();
            }
        }
        public Employee SelectedEmployee
        {
            get => _employee;
            set
            {
                if (_employee != value)
                    _employee = value;
                OnPropertyChanged();
            }
        }
        public ICollectionView Employees
        {
            get => _employees;
            set
            {
                if (value != _employees)
                    _employees = value;
                OnPropertyChanged();
            }
        }

        private DataContext _context;
        public MainVM(DataContext context)
        {
            _context = context;
            var employees = new ObservableCollection<Employee>(_context.Employees);
            _positions = new ObservableCollection<Position>(_context.Positions);
            _subdivisions = new ObservableCollection<Subdivision>(_context.Subdivisions);
            _positions.Insert(0, new Position { Title = "All Positions" });
            _subdivisions.Insert(0, new Subdivision { Title = "All Subdivisions" });


            _employees = CollectionViewSource.GetDefaultView(employees);
            _employees.GroupDescriptions.Add(new PropertyGroupDescription("Subdivision.Title"));
            DeleteCommand = new RelayCommand(DeleteEmployee);
            InsertCommand = new RelayCommand(() =>
            {
                var window = App.Current.Services.GetRequiredService<EmployeeWindow>();
                window.DataContext = new EmployeeVM(_context);
                window.ShowDialog();
                LoadData();
            });
            UpdateCommand = new RelayCommand(() =>
            {
                if (SelectedEmployee == null)
                    return;
                var window = App.Current.Services.GetRequiredService<EmployeeWindow>();
                window.DataContext = new EmployeeVM(_context, _employee);
                window.ShowDialog();
                LoadData();
            });
        }

        private void LoadData()
        {
            var employees = _context.Employees.ToList();
            if (_position != null)
                employees = employees.Where(x => x.Position == _position || _position.Title == "All Positions").ToList();
            if (_subdivision != null)
                employees = employees.Where(x => x.Subdivision == _subdivision || _subdivision.Title == "All Subdivisions").ToList();
            _employees = CollectionViewSource.GetDefaultView(employees);
            _employees.GroupDescriptions.Add(new PropertyGroupDescription("Subdivision.Title"));
            OnPropertyChanged(nameof(Employees));
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee == null)
                return;

            var message = MessageBox.Show("Действительно удалить выбранного пользователя?"
                , "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (message == MessageBoxResult.No)
                return;

            _context.Remove(SelectedEmployee);
            _context.SaveChanges();
            LoadData();
        }
    }
}
