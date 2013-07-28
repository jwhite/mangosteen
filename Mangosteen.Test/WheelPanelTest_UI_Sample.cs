//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Mangosteen.Test
//{
//    [TestFixture]
//    public class SettingsUserControlTests
//    {
//        private const string _XamlFilePath = @"C:\Projects\Fohjin.XamlTest\WpfControlLibrary\SettingsUserControl1.xaml";
//        //private const string _XamlFilePath = @"C:\Projects\Fohjin.XamlTest\WpfControlLibrary\SettingsUserControl2.xaml";

//        private const string _SoundsCheckBox = "soundsCheckBox";
//        private const string _SoundsCheckBoxNotFound = "Could not find CheckBox control '" + _SoundsCheckBox + "'";
//        private const string _SoundsCheckBoxIssue = "Issue with a value of '" + _SoundsCheckBox + "'";

//        private const string _BrokenCheckBox = "brokenCheckBox";
//        private const string _BrokenCheckBoxNotFound = "Could not find CheckBox control '" + _BrokenCheckBox + "'";
//        private const string _BrokenCheckBoxIssue = "Issue with a value of '" + _BrokenCheckBox + "'";

//        private const string _BtnOnSuccessFileDialog = "btnOnSuccessFileDialog";
//        private const string _BtnOnSuccessFileDialogNotFound = "Could not find Button control '" + _BtnOnSuccessFileDialog + "'";
//        private const string _BtnOnSuccessFileDialogIssue = "Issue with a value of '" + _BtnOnSuccessFileDialog + "'";
        
//        private const string _BtnOnBrokenFileDialog = "btnOnBokenFileDialog";
//        private const string _BtnOnBrokenFileDialogNotFound = "Could not find Button control '" + _BtnOnBrokenFileDialog + "'";
//        private const string _BtnOnBrokenFileDialogIssue = "Issue with a value of '" + _BtnOnBrokenFileDialog + "'";

//        private const string _TxtWebDashboardUrl = "txtWebDashboardUrl";
//        private const string _TxtWebDashboardUrlNotFound = "Could not find TextBox control '" + _TxtWebDashboardUrl + "'";
//        //private const string _TxtWebDashboardUrlIssue = "Issue with a value of '" + _TxtWebDashboardUrl + "'";

//        private const string _TxtPollInterval = "txtPollInterval";
//        private const string _TxtPollIntervalNotFound = "Could not find TextBox control '" + _TxtPollInterval + "'";
//        //private const string _TxtPollIntervalIssue = "Issue with a value of '" + _TxtPollInterval + "'";

//        private const string _BtnSave = "btnSave";
//        private const string _BtnSaveNotFound = "Could not find Button control '" + _BtnSave + "'";
//        private const string _BtnSaveIssue = "Issue with a value of '" + _BtnSave + "'";

//        [Test]
//        public void VerifyThatbtnOnSuccessFileDialogIsDisabled()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnOnSuccessFileDialog);
//            Assert.IsNotNull(button, _BtnOnSuccessFileDialogNotFound);
//            Assert.IsFalse(button.IsEnabled, _BtnOnSuccessFileDialogIssue);
//        }
//        [Test]
//        public void VerifyThatbtnOnSuccessFileDialogIsEnabled()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            CheckBox checkBox = XamlUnitTestHelper.GetObject<CheckBox>(_SoundsCheckBox);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnOnSuccessFileDialog);

//            Assert.IsNotNull(button, _BtnOnSuccessFileDialogNotFound);
//            Assert.IsNotNull(checkBox, _SoundsCheckBoxNotFound);

//            checkBox.IsChecked = true;
//            Assert.IsTrue(button.IsEnabled, _BtnOnSuccessFileDialogIssue);
//        }
//        [Test]
//        public void VerifyThatbtnOnSuccessFileDialogIsDisabledAgain()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            CheckBox checkBox = XamlUnitTestHelper.GetObject<CheckBox>(_SoundsCheckBox);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnOnSuccessFileDialog);

//            Assert.IsNotNull(button, _BtnOnSuccessFileDialogNotFound);
//            Assert.IsNotNull(checkBox, _SoundsCheckBoxNotFound);

//            Assert.IsFalse(checkBox.IsChecked.Value, _SoundsCheckBoxIssue);
//            Assert.IsFalse(button.IsEnabled, _BtnOnSuccessFileDialogIssue);

//            checkBox.IsChecked = true;
//            Assert.IsTrue(checkBox.IsChecked.Value, _SoundsCheckBoxIssue);
//            Assert.IsTrue(button.IsEnabled, _BtnOnSuccessFileDialogIssue);

//            checkBox.IsChecked = false;
//            Assert.IsFalse(checkBox.IsChecked.Value, _SoundsCheckBoxIssue);
//            Assert.IsFalse(button.IsEnabled, _BtnOnSuccessFileDialogIssue);
//        }

//        [Test]
//        public void VerifyThatbtnOnBrokenFileDialogIsDisabled()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnOnBrokenFileDialog);
//            Assert.IsNotNull(button, _BtnOnBrokenFileDialogNotFound);
//            Assert.IsFalse(button.IsEnabled, _BtnOnBrokenFileDialogIssue);
//        }
//        [Test]
//        public void VerifyThatbtnOnBrokenFileDialogIsEnabled()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            CheckBox checkBox = XamlUnitTestHelper.GetObject<CheckBox>(_BrokenCheckBox);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnOnBrokenFileDialog);

//            Assert.IsNotNull(button, _BtnOnBrokenFileDialogNotFound);
//            Assert.IsNotNull(checkBox, _BrokenCheckBoxNotFound);

//            checkBox.IsChecked = true;
//            Assert.IsTrue(button.IsEnabled, _BtnOnBrokenFileDialogIssue);
//        }
//        [Test]
//        public void VerifyThatbtnOnBrokenFileDialogIsDisabledAgain()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            CheckBox checkBox = XamlUnitTestHelper.GetObject<CheckBox>(_BrokenCheckBox);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnOnBrokenFileDialog);

//            Assert.IsNotNull(button, _BtnOnBrokenFileDialogNotFound);
//            Assert.IsNotNull(checkBox, _BrokenCheckBoxNotFound);

//            Assert.IsFalse(checkBox.IsChecked.Value, _BrokenCheckBoxIssue);
//            Assert.IsFalse(button.IsEnabled, _BtnOnBrokenFileDialogIssue);

//            checkBox.IsChecked = true;
//            Assert.IsTrue(checkBox.IsChecked.Value, _BrokenCheckBoxIssue);
//            Assert.IsTrue(button.IsEnabled, _BtnOnBrokenFileDialogIssue);

//            checkBox.IsChecked = false;
//            Assert.IsFalse(checkBox.IsChecked.Value, _BrokenCheckBoxIssue);
//            Assert.IsFalse(button.IsEnabled, _BtnOnBrokenFileDialogIssue);
//        }

//        [Test]
//        public void VerifyThatbtnSaveIsDisabled()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnSave);
//            Assert.IsNotNull(button, _BtnSaveNotFound);
//            Assert.IsFalse(button.IsEnabled, _BtnSaveIssue);
//        }
//        [Test]
//        public void VerifyThatbtnSaveIsDisabled1()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnSave);
//            TextBox txtWebDashboardUrl = XamlUnitTestHelper.GetObject<TextBox>(_TxtWebDashboardUrl);
//            TextBox txtPollInterval = XamlUnitTestHelper.GetObject<TextBox>(_TxtPollInterval);
//            Assert.IsNotNull(button, _BtnSaveNotFound);
//            Assert.IsNotNull(txtWebDashboardUrl, _TxtWebDashboardUrlNotFound);
//            Assert.IsNotNull(txtPollInterval, _TxtPollIntervalNotFound);

//            txtWebDashboardUrl.Text = "test text";
//            txtPollInterval.Text = "";
//            Assert.IsFalse(button.IsEnabled, _BtnSaveIssue);
//        }
//        [Test]
//        public void VerifyThatbtnSaveIsDisabled2()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnSave);
//            TextBox txtWebDashboardUrl = XamlUnitTestHelper.GetObject<TextBox>(_TxtWebDashboardUrl);
//            TextBox txtPollInterval = XamlUnitTestHelper.GetObject<TextBox>(_TxtPollInterval);
//            Assert.IsNotNull(button, _BtnSaveNotFound);
//            Assert.IsNotNull(txtWebDashboardUrl, _TxtWebDashboardUrlNotFound);
//            Assert.IsNotNull(txtPollInterval, _TxtPollIntervalNotFound);

//            txtWebDashboardUrl.Text = "";
//            txtPollInterval.Text = "10";
//            Assert.IsFalse(button.IsEnabled, _BtnSaveIssue);
//        }
//        [Test]
//        public void VerifyThatbtnSaveIsEnabled()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnSave);
//            TextBox txtWebDashboardUrl = XamlUnitTestHelper.GetObject<TextBox>(_TxtWebDashboardUrl);
//            TextBox txtPollInterval = XamlUnitTestHelper.GetObject<TextBox>(_TxtPollInterval);
//            Assert.IsNotNull(button, _BtnSaveNotFound);
//            Assert.IsNotNull(txtWebDashboardUrl, _TxtWebDashboardUrlNotFound);
//            Assert.IsNotNull(txtPollInterval, _TxtPollIntervalNotFound);

//            txtWebDashboardUrl.Text = "test text";
//            txtPollInterval.Text = "10";
//            Assert.IsTrue(button.IsEnabled, _BtnSaveIssue);
//        }
//        [Test]
//        public void VerifyThatbtnOnBrokenFileDialogIsDisabledAgain1()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnSave);
//            TextBox txtWebDashboardUrl = XamlUnitTestHelper.GetObject<TextBox>(_TxtWebDashboardUrl);
//            TextBox txtPollInterval = XamlUnitTestHelper.GetObject<TextBox>(_TxtPollInterval);
//            Assert.IsNotNull(button, _BtnSaveNotFound);
//            Assert.IsNotNull(txtWebDashboardUrl, _TxtWebDashboardUrlNotFound);
//            Assert.IsNotNull(txtPollInterval, _TxtPollIntervalNotFound);

//            txtWebDashboardUrl.Text = "test text";
//            txtPollInterval.Text = "10";
//            Assert.IsTrue(button.IsEnabled, _BtnSaveIssue);

//            txtWebDashboardUrl.Text = "";
//            Assert.IsFalse(button.IsEnabled, _BtnSaveIssue);
//        }
//        [Test]
//        public void VerifyThatbtnOnBrokenFileDialogIsDisabledAgain2()
//        {
//            XamlUnitTestHelper.LoadXaml(_XamlFilePath);
//            Button button = XamlUnitTestHelper.GetObject<Button>(_BtnSave);
//            TextBox txtWebDashboardUrl = XamlUnitTestHelper.GetObject<TextBox>(_TxtWebDashboardUrl);
//            TextBox txtPollInterval = XamlUnitTestHelper.GetObject<TextBox>(_TxtPollInterval);
//            Assert.IsNotNull(button, _BtnSaveNotFound);
//            Assert.IsNotNull(txtWebDashboardUrl, _TxtWebDashboardUrlNotFound);
//            Assert.IsNotNull(txtPollInterval, _TxtPollIntervalNotFound);

//            txtWebDashboardUrl.Text = "test text";
//            txtPollInterval.Text = "10";
//            Assert.IsTrue(button.IsEnabled, _BtnSaveIssue);

//            txtPollInterval.Text = "";
//            Assert.IsFalse(button.IsEnabled, _BtnSaveIssue);
//        }
//    }
//}
