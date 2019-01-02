using System;
using System.Windows;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using Outlook = Microsoft.Office.Interop.Outlook;
using Regex = System.Text.RegularExpressions.Regex; 

namespace NewAccount
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //----Retrieve User Input----

        //Function for "Submit" button
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //perform error checking on user input

            Regex regex = new Regex(@"\s+"); // matches at least 2 whitespaces
            //if (regex.IsMatch(inputString))

            //check name for correct length
            if (nameBox.Text.Trim().Split().Length == 1)
            {
                MessageBox.Show("You must enter the employees first and last name.");
                return; 
            }

            //check that name doesn't include middle name
            if (nameBox.Text.Trim().Split().Length > 2)
            {
                MessageBox.Show("Only enter the employees first and last name.");
                return;
            }

            //check that username doesnt contain whitespace
            if (usernameBox.Text.Trim().Contains(" "))
            {
                MessageBox.Show("Username cannot contain whitespace.");
                return;
            }

            //check that username has been entered
            if (usernameBox.Text.Trim().Length==0)
            {
                MessageBox.Show("You must enter the employees username.");
                return;
            }

            //check that email has been entered
            if (emailBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("You must enter the employees email address.");
                return;
            }

            //check that email doesnt contain whitespace
            if (emailBox.Text.Trim().Contains(" "))
            {
                MessageBox.Show("Email address cannot contain whitespace.");
                return;
            }

            //check to see if telehphone number has been entered
            if (telephoneBox.Text.Trim().Length == 0)
            {
                telephoneBox.Text = "N/A"; 
            }

            //check that department has been entered
            if (departmentBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Error: You must enter the employees department.");
                return; 
            }

            //assign user input for processing
            string name = nameBox.Text;
            string username = usernameBox.Text;
            string email = emailBox.Text;
            string department = departmentBox.Text;
            string phone = telephoneBox.Text;
            string remarks = "";
            if (remarksBox.Text != "N/A")
                remarks = remarksBox.Text;

            SubmitCredentials(name, username, email, department, phone, remarks);
            resetButton.Content = "Enter Another";

        }

        //Function for "default email" button
        private void DefaultEmail_Click(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text.Trim().Length==0)
            {
                MessageBox.Show("You must first enter the employees name before selecting \"Default Email\" option.");
            }

           else
            {
                string[] fullName = nameBox.Text.Trim().Split();

                //check if name has been entered incorrectly
                if (fullName.Length != 2)
                {
                    //check that both first and last are included
                    if (fullName.Length == 1)
                        MessageBox.Show("You must enter both first and last name before selecting the \"Default Email\" option.");
                    
                    //if extra whitespace is in between first and last name, remove whitespace and set default email
                    else if (fullName[1].Trim() == "")
                    {
                        string firstName = fullName[0];
                        string lastName = fullName.GetValue(fullName.Length - 1).ToString();
                        emailBox.Text = firstName + "." + lastName + "@talgov.com";
                        nameBox.Text = firstName + " " + lastName;
                    }

                    //check that name doesn't include middle name
                    else
                        MessageBox.Show("Only enter the employees first and last name before selecting the " +
                            "\"Default Email\" option.");
                }

                //else if name has been entered correctly, set default email
                else
                {
                    string firstName = fullName[0];
                    string lastName = fullName[1];
                    emailBox.Text = firstName + "." + lastName + "@talgov.com";
                    nameBox.Text = firstName + " " + lastName;
                }
            }
        }

        //Function for "reset" button
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            resetButton.Content = "Reset"; 
            nameBox.Text = "";
            usernameBox.Text = "";
            emailBox.Text = "";
            departmentBox.Text = "";
            telephoneBox.Text = "";
            remarksBox.Text = ""; 
        }

        //----Process User Input----

        //Function for submitting credentials into Outlook and Excel
        private void SubmitCredentials(string name, string user, string email, string department, string phone, string remarks)
        {
            //----Construct Message----
            string msg = "Good";
            //Determine if it is morning, afternoon, or evening
            int currTime = DateTime.Now.Hour;
            if (currTime < 12)
                msg += " morning,\n\n";
            else if (currTime < 17)
                msg += " afternoon,\n\n";
            else
                msg += " evening,\n\n";
            //split name to seperate first and last name
            string[] fullName = name.Split();
            string firstName = fullName[0];
            string lastName = fullName[1];
            //message is written in HTML for formatting purposes
            msg += "<br /><br />The network account for " + "<b>" + name + "</b>" + " has been created:<br /><br />Username: " + "<b>" +
            user + "</b>" + "<br />Email: " + "<a href = \"mailto: " + email + "\">" + email + "</a>" + "<br /><br />Please call or have the user call HelpDesk for the password.<br /><br />" +
            "It may take up to 24 hours for changes to reflect in Outlooks address book.";

            //----Construct Email----
            Outlook.Application ol_app = new Outlook.Application();
            Outlook._MailItem ol_mailItem = (Outlook._MailItem)ol_app.CreateItem(Outlook.OlItemType.olMailItem);
            ol_mailItem.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
            ol_mailItem.HTMLBody = msg;
            ol_mailItem.Subject = "New User Account for " + name;
            ol_mailItem.CC = "sos@talgov.com";
            ol_mailItem.Display(true);

            //----Construct Excel Entry----
            string path = "\\\\city\\fileserver\\iss\\HelpDesk\\_CountyE-mailAdd-DeleteList.xlsx";

            //open Excel application
            Excel.Application xcl_app = new Excel.Application
            {
                Visible = true
            };
            //open excel workbook
            Excel.Workbook xcl_wb = xcl_app.Workbooks.Open(path);
            //open excel worksheet
            Excel.Worksheet xcl_ws = (Excel.Worksheet)xcl_wb.Worksheets.get_Item("--- Added ---");
            xcl_ws.Activate();
            //find first empty row
            int xcl_firstEmpty = 0;
            double xcl_notEmpty = 1.0;
            while (xcl_notEmpty > 0)
            {
                string xcl_cellAddr = "A" + (++xcl_firstEmpty).ToString();
                Excel.Range row = xcl_app.get_Range(xcl_cellAddr, xcl_cellAddr).EntireRow;
                xcl_notEmpty = xcl_app.WorksheetFunction.CountA(row);
            }
            //insert record into first empty row
            xcl_ws.Cells[xcl_firstEmpty, 1] = email;
            xcl_ws.Cells[xcl_firstEmpty, 2] = lastName;
            xcl_ws.Cells[xcl_firstEmpty, 3] = firstName;
            xcl_ws.Cells[xcl_firstEmpty, 4] = department;
            xcl_ws.Cells[xcl_firstEmpty, 5] = phone;
            xcl_ws.Cells[xcl_firstEmpty, 6] = remarks;

            //xcl_wb.Close(true, Type.Missing, Type.Missing);
            xcl_app.Quit();
        }
    }
}
