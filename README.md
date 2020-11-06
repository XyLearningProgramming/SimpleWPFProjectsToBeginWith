# SimpleWPFProjectsToBeginWith
Wpf projects for beginners to learn its magic. Or... at least I find them very useful while learning wpf in 2020.
Most of them are adapted from [MacDonald's book](https://github.com/Apress/pro-wpf-4.5-in-csharp). 
But I tried to write it with newer C# features (with .Net Core 3.0) and seperate view from the rest (which the book didn't do often).

Here are lists of wpf techniques covered in each sub project:

1. BombDropper <br />
	1.1 UserControl <br />
	1.2 Animation system and StoryBoard <br />
		*EventTriggers inside Animation <br />
	1.3 Hit test inside canvas control <br />
	
2. ControlTemplateBrowser <br />
	2.1 How reflection can be used to check source code of controltemplate inside each control <br />
	2.2 Treeview <br />

3. Custom Controls <br />
	3.1. Two ways to define "custom control" in WPF:  <br />
		3.1.2 UserControl <br />
		3.1.3 Using themes subfile (seperate .cs file with view) <br />
			How to define Dependency Property inside custom control class <br />
			How to define callbacks of when Dependency Property changes <br />
			
4. Data Validation <br />
	4.1 Two validation techniques: <br />
		4.2 Validation rules: quick check in view, easy to factorize <br />
		4.3 INotifyDataErrorInfo: easy to check multiple properties at the same time inside Viewmodel <br />

5. Very Basic MVVM <br />
	5.1 A very basic MVVM implementation to unrealistic show students' info in grid view. <br />
	
6. RichTextEditor <br />
	6.1 A naive text editor that can create and update and save .rtf .xaml .txt format files using the RichTextBox control in XCeed Toolkit. <br />
	6.2 Two panels showing raw text and formatted text allowing to make changes at the same time.<br />
	6.3 More importantly, it is a more sophisticated MVVM implementation. Easy to scale up.<br />
		6.4 Since MVVM tends to have large viewmodel as the project grows, we can:<br />
			6.4.1 Create viewmodel base class and seperate viewmodels by their functions / business logic with a RootViewModel connecting to all<br />
			6.4.2 Create commands class <br />
			6.4.3 Create resource dictionaries in App.xaml (for instance: converters, color theme)<br />
	6.5 Using something as MVVM Light could really save time. Lesson learned.<br />
	
To continue Wpf journey, I suggest to implement small applications (and I will do so!).<br />
Here are some projects that I recommend and will start from:<br />
* [Dragabtz](https://github.com/ButchersBoy/Dragablz) So coooool... <br />
* [MaterialDesign](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) Really cool controls! <br />
* [Wpf Toolkit from Xceed](https://github.com/xceedsoftware/wpftoolkit) Including very handy upgrades to default controls and frequently used functions not implemented by any built-in controls. Clear documentation. <br />
* [MotionList] (https://github.com/MaterialDesignInXAML/MotionList) How cool things can be done part by part to form a mind-blowing custom control! <br />
* [HandyControl] (https://github.com/HandyOrg/HandyControl) So many beautiful controls...  Screenshot, PropertyGrid, various built-in & modern button styles. <br />
* [MVVM Light] (https://github.com/lbugnion/mvvmlight) An almost must-have if you're tired to write down same code for MVVM again and again. But on the other side, every person has his/her favorite way to implement MVVM.

