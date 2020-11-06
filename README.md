# SimpleWPFProjectsToBeginWith
Wpf projects for beginners to learn its magic. Or... at least I find them very useful while learning wpf in 2020.
Most of them are adapted from [MacDonald's book](https://github.com/Apress/pro-wpf-4.5-in-csharp). 
But I tried to write it with newer C# features (with .Net Core 3.0) and seperate view from the rest (which the book didn't do often).

Here are lists of wpf techniques covered in each sub project:

1. BombDropper
	*UserControl
	*Animation system and StoryBoard
		*EventTriggers inside Animation
	*Hit test inside canvas control
	
2. ControlTemplateBrowser
	*How reflection can be used to check source code of controltemplate inside each control
	*Treeview

3. Custom Controls
	*Two ways to define "custom control" in WPF: 
		*UserControl
		*Using themes subfile (seperate .cs file with view)
			*How to define Dependency Property inside custom control class
			*How to define callbacks of when Dependency Property changes
			
4. Data Validation
	*Two validation techniques:
		*Validation rules: quick check in view, easy to factorize
		*INotifyDataErrorInfo: easy to check multiple properties at the same time inside Viewmodel

5. Very Basic MVVM
	*A very basic MVVM implementation to unrealistic show students' info in grid view.
	
6. RichTextEditor
	*A naive text editor that can create and update and save .rtf .xaml .txt format files using the RichTextBox control in XCeed Toolkit.
	*Two panels showing raw text and formatted text allowing to make changes at the same time.
	*More importantly, it is a more sophisticated MVVM implementation. Easy to scale up.
		*Since MVVM tends to have large viewmodel as the project grows, we can:
			*Create viewmodel base class and seperate viewmodels by their functions / business logic with a RootViewModel connecting to all
			*Create commands class 
			*Create resource dictionaries in App.xaml (for instance: converters, color theme)
	*Using something as MVVM Light could really save time. Lesson learned.
	
To continue Wpf journey, I suggest to implement some useful and functioning (hopefully) applications (and I will do so!).
Here are some projects that I recommend and will start from:
* [Dragabtz](https://github.com/ButchersBoy/Dragablz) So coooool...
* [MaterialDesign](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) Really cool controls!
* [Wpf Toolkit from Xceed](https://github.com/xceedsoftware/wpftoolkit) Including very handy upgrades to default controls and frequently used functions not implemented by any built-in controls. Clear documentation.
* [MotionList] (https://github.com/MaterialDesignInXAML/MotionList) How cool things can be done part by part to form a mind-blowing custom control!
* [HandyControl] (https://github.com/HandyOrg/HandyControl) So many beautiful controls...  Screenshot, PropertyGrid, various built-in & modern button styles.
* [MVVM Light] (https://github.com/lbugnion/mvvmlight) An almost must-have if you're tired to write down same code for MVVM again and again. But on the other side, every person has his/her favorite way to implement MVVM.

