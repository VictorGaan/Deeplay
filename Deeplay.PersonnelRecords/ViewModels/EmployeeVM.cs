using Deeplay.PersonnelRecords.Models;
using Deeplay.PersonnelRecords.Persistence.Contexts;
using Deeplay.PersonnelRecords.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Deeplay.PersonnelRecords.ViewModels
{
    public class EmployeeVM : ObservableObject
    {
        private ObservableCollection<Position> _positions;
        private ObservableCollection<Subdivision> _subdivisions;
        private ObservableCollection<Gender> _genders;
        private Employee _employee;

        public ICommand SubmitCommand { get; }
        public Employee Employee
        {
            get => _employee;
            set
            {
                if (_employee != value)
                    _employee = value;
                OnPropertyChanged();
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
        public ObservableCollection<Gender> Genders
        {
            get => _genders;
            set
            {
                if (_genders != value)
                    _genders = value;
                OnPropertyChanged();
            }
        }

        private DataContext _context;
        public EmployeeVM(DataContext context, Employee employee = null)
        {
            _employee = employee ?? new Employee() { BirthDate = DateTime.Now };
            _context = context;
            _genders = new ObservableCollection<Gender>(_context.Genders.ToList());
            _positions = new ObservableCollection<Position>(_context.Positions.ToList());
            _subdivisions = new ObservableCollection<Subdivision>(_context.Subdivisions.ToList());
            SubmitCommand = new RelayCommand(AddEmployee);
        }

        private void AddEmployee()
        {
            var departmentHeadAny = _context.Employees.Any(x => x.Subdivision == _employee.Subdivision && x.Position == _employee.Position);
            if (departmentHeadAny)
            {
                MessageBox.Show("В подразделение может быть только один руководитель.");
                return;
            }
            if (_employee.Id == 0)
            {
                _context.Add(_employee);
            }
            _context.SaveChanges();
            App.Current.Services.GetRequiredService<EmployeeWindow>().Hide();
        }
    }
}
