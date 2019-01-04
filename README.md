# New User Utility

## Description
This is a tool that was built specifically for use by the Help Desk for the City of Tallahassee to automate the process of documenting and reporting user credentials. Whenever a new user account is created, this tool will simplify the process of documenting and reporting their credentials. Instead of having to manually report the credentials in Outlook and document the credentials in Excel, this program will perform both tasks.  

# How To Install
All commands should be run from the project root directory.
- Find the "NUU Installer.msi" file in the Help Desk common drive and run it. 
- This will install the program, it's name is New User Utility.exe. 
- To uninstall the program, locate "AcctUtilitySetup" in the Control Panel.  

# How to use
1. Fill out the form.
![alt text](https://i.imgur.com/knrAcKw.png)
- The name, username, email, and department fields are required. The phone number and the remarks fields are optional. 
- Both first and last name must be included. 
- Only include the first and last name. Don't include the middle name. 
- The username cannot contain any whitespace 
- The email cannot contain any whitespace. 
- Selecting the "Default Email" option will auto-populate the email field with a default email in the form of Firstname.Lastname@talgov.com. In order to use this option, the first and last name need to be already filled out. 
2. Once you hit "Submit", the Outlook email will be auto-generated. 
![alt text](https://i.imgur.com/knrAcKw.png)
- The only thing you need to do is specify the person to send the credentials to, and hit "Send". 
- The program will automatically determine if it is morning, afternoon, or evening based on the current time. This will be used in the email greeting (e.g. Good morning, Good evening). 
- A generic Help Desk signature will be automatically appended to the email.
3. Once you send the Outlook email, the Excel sheet will automatically open.
![alt text](https://i.imgur.com/knrAcKw.png)
- The program will automatically fill in the relevant information, and bring up the "Save/Dont Save" window. If the changes look correct, hit "Save". The Excel sheet will automatically close.
- If you want report another user's credentials, hit "Enter Another" button in New User Utility window.
