# DataValidation
In my opinion, Data validation is a topic little bit confusing in wpf.
You have two ways to do it:
* Validation rules
* INotifyDataErrorInfo

They achieve the same thing but if you use MVVM model, they fit into different parts:

`<Validation rules>` is for "View". It can be factorized easily and reused.\
It is best for simple validation logics checked in the front end and has to be placed in Xaml.
```xaml
	<TextBlock Margin="7" Grid.Row="2">Unit Cost:</TextBlock>
	<TextBox Margin="5" Grid.Row="2" Grid.Column="1">
		<TextBox.Text>
			<Binding Path="UnitCost" NotifyOnValidationError="true" StringFormat="{}{0:C}">
				<Binding.ValidationRules>
					<local:PositivePriceRule Max="999.99"></local:PositivePriceRule>
					<ExceptionValidationRule></ExceptionValidationRule>
				</Binding.ValidationRules>
			</Binding>
		</TextBox.Text>
	</TextBox>
```
``` c#
	internal class PositivePriceRule: ValidationRule
	{
        private decimal min = 0;
        private decimal max = Decimal.MaxValue;

        public decimal Min
        {
            get { return min; }
            set { min = value; }
        }

        public decimal Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Value needed to be validated</param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal price = 0;
            if(value is String valString && valString.Length > 0)
            {
                if(Decimal.TryParse(valString, out price))
                {
                    if((price < Min) || (price > Max))
                    {
                        return new ValidationResult(false,
                          "Not in the range " + Min + " to " + Max + ".");
                    }
                    else
                    {
                        return new ValidationResult(true, null);
                    }
                }
            }
            return new ValidationResult(false, "Illegal characters.");
        }
    }
```
`<INotifyDataErrorInfo>` fits into the ViewModel part naturally. 
In my opinion, if several rules needs to be checked at the same time or the validation is part of business logic, the rules implementation should go here.
Another benefit of doing this is that a list of errors is available in ViewModel, and we can check the error info and count them anytime. Otherwise, if we use validation rules, `<Validation.HasError>` needs to be called everytime if we want to check error status.
It works similar to INotifyPropertyChanged
```xaml
	<TextBlock Margin="7" Grid.Row="1">Model Name:</TextBlock>
	<TextBox Margin="5" Grid.Row="1" Grid.Column="1" Text="{Binding Path=ModelName, 
	ValidatesOnNotifyDataErrors=True,NotifyOnValidationError=True}"></TextBox>
```
In view model
``` c#
	public string ModelNumber { get => ProductChosen.ModelNumber; set
		{
			// rule: only digit and alpha characters
			// implemented via INotifyDataErrorInfo
			bool valid = true;
			foreach(char c in value)
			{
				if(Char.IsLetterOrDigit(c) == false)
				{
					valid = false;
					break;
				}
			}
			if(!valid)
			{
				this.SetErrors(new List<string>() { "The modelname can only contain digit and alpha numbers" });
			}
			else
			{
				this.ClearErrors();
			}
			//!!! data will still be changed into invalid value
			ProductChosen.ModelNumber = value;
			NotifyPropertyChanged();
		}
	}
	
	#region INotifyDataErrorInfo implementation
		private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
		private void SetErrors(List<string> propertyErrors,[CallerMemberName]string propertyName="")
		{
			if(propertyName == "") return;
			// remove errors existing in the name of this property
			errors.Remove(propertyName);
			// add new errors to this property 
			errors.Add(propertyName, propertyErrors);
			//invoke error notification event for view
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}
		private void ClearErrors([CallerMemberName]string propertyName = "")
		{
			if(propertyName == "") return;
			errors.Remove(propertyName);
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}
		public IEnumerable GetErrors(string propertyName)
		{
			if(string.IsNullOrEmpty(propertyName))
			{
				return errors.Values;
			}
			else
			{
				if(errors.ContainsKey(propertyName)) return errors[propertyName];
				else return null;
			}
		}
		public bool HasErrors
		{
			get => errors.Count > 0;
		}
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
	#endregion
```
To apply another controltemplate whenever there's a validation error or calling something, we can set the parent control say `<grid>` as:
Such setting works in both ways of validation.
```
	<Grid Name="gridProductDetails"
		  Validation.Error="validationError">
		<Grid.Resources>

			<Style TargetType="{x:Type TextBox}">
				<Setter Property="Validation.ErrorTemplate">
					<Setter.Value>
						<ControlTemplate>
							<DockPanel LastChildFill="True">
								<TextBlock DockPanel.Dock="Right"
			  Foreground="Red" FontSize="14" FontWeight="Bold"
				ToolTip="{Binding ElementName=adornerPlaceholder,
				Path=AdornedElement.(Validation.Errors)[0].ErrorContent}"
					   >*</TextBlock>
								<Border BorderBrush="Green" BorderThickness="1">
									<AdornedElementPlaceholder Name="adornerPlaceholder"></AdornedElementPlaceholder>
								</Border>
							</DockPanel>
						</ControlTemplate>
					</Setter.Value>
				</Setter>
				<Style.Triggers>
					<Trigger Property="Validation.HasError" Value="true">
						<Setter Property="ToolTip"
		  Value="{Binding RelativeSource={RelativeSource Self},
				Path=(Validation.Errors)[0].ErrorContent}"/>
					</Trigger>
				</Style.Triggers>
			</Style>
		</Grid.Resources>
		...
	</Grid>
```
And code behind:
```c#
	private void validationError(object sender, ValidationErrorEventArgs e)
	{
		if(e.Action==ValidationErrorEventAction.Added)
		{
			MessageBox.Show(e.Error.ErrorContent.ToString());
		}
	}
```