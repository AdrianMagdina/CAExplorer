// programmed by Adrian Magdina in 2013
// in this file is the implementation of validation rules for input fields.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace CAExplorerNamespace
{
    //validation of input value for columns field
    public class CAPropertiesDialogColumnsValidationRule : ValidationRule
    {
        public CAPropertiesDialogColumnsValidationRule()
        {
        }

        public ViewModelBase ViewModel { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int aColumns = 0;

            try
            {
                string aColumnsString = (string)value;
                if (aColumnsString.Length > 0)
                {
                    aColumns = Int32.Parse(aColumnsString, CultureInfo.CurrentCulture);
                }
            }
            catch (FormatException e)
            {
                string aMessage = "Columns must be a number! :\n " + e.Message;

                return new ValidationResult(false, aMessage);
            }

            if ((aColumns < Constants.MinCAColumns) || (aColumns > Constants.MaxCAColumns))
            {
                string aMessage = "Value of Columns must be between: " + Constants.MinCAColumns + " - " + Constants.MaxCAColumns + "!";

                return new ValidationResult(false, aMessage);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }

    //validation of input value for rows field
    public class CAPropertiesDialogRowsValidationRule : ValidationRule
    {
        public CAPropertiesDialogRowsValidationRule()
        {
        }

        public ViewModelBase ViewModel { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int aRows = 0;

            try
            {
                string aRowsString = (string)value;
                if (aRowsString.Length > 0)
                {
                    aRows = Int32.Parse(aRowsString, CultureInfo.CurrentCulture);
                }
            }
            catch (FormatException e)
            {
                string aMessage = "Rows must be a number! :\n " + e.Message;

                return new ValidationResult(false, aMessage);
            }

            if ((aRows < Constants.MinCARows) || (aRows > Constants.MaxCARows))
            {
                string aMessage = "Value of Rows must be between: " + Constants.MinCARows + " - " + Constants.MaxCARows + "!";

                return new ValidationResult(false, aMessage);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }

    //validation of input value for cell size x field
    public class CAPropertiesDialogCellSizeXValidationRule : ValidationRule
    {
        public CAPropertiesDialogCellSizeXValidationRule()
        {
        }

        public ViewModelBase ViewModel { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int aCellSizeX = 0;

            try
            {
                string aCellSizeXString = (string)value;
                if (aCellSizeXString.Length > 0)
                {
                    aCellSizeX = Int32.Parse(aCellSizeXString, CultureInfo.CurrentCulture);
                }
            }
            catch (FormatException e)
            {
                string aMessage = "Cell Size X must be a number! :\n " + e.Message;

                //DialogMediator.ShowMessageBox(ViewModel, "Cell Size X", aMessage, MessageBoxButton.OK, MessageBoxImage.Error);

                return new ValidationResult(false, aMessage);
            }

            if ((aCellSizeX < Constants.MinCellSizeX) || (aCellSizeX > Constants.MaxCellSizeX))
            {
                string aMessage = "Value of Cell Size X must be between: " + Constants.MinCellSizeX + " - " + Constants.MaxCellSizeX + "!";

                return new ValidationResult(false, aMessage);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }

    //validation of input value for cell size y field
    public class CAPropertiesDialogCellSizeYValidationRule : ValidationRule
    {
        public CAPropertiesDialogCellSizeYValidationRule()
        {
        }

        public ViewModelBase ViewModel { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int aCellSizeY = 0;

            try
            {
                string aCellSizeYString = (string)value;
                if (aCellSizeYString.Length > 0)
                {
                    aCellSizeY = Int32.Parse(aCellSizeYString, CultureInfo.CurrentCulture);
                }
            }
            catch (FormatException e)
            {
                string aMessage = "Cell Size Y must be a number! :\n " + e.Message;

                return new ValidationResult(false, aMessage);
            }

            if ((aCellSizeY < Constants.MinCellSizeY) || (aCellSizeY > Constants.MaxCellSizeY))
            {
                string aMessage = "Value of Cell Size Y must be between: " + Constants.MinCellSizeY + " - " + Constants.MaxCellSizeY + "!";

                return new ValidationResult(false, aMessage);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
