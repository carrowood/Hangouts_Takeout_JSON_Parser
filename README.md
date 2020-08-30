# Hangouts_Takeout_JSON_Parser
A command line console app to create HTML chat conversation files from a "takeout" export JSON file.

## Background
I simply wanted a way to back up all of my chat conversations from Google's Hangouts in a format that was easy to store, easy to read and easy to search. There are several websites that claim to do this, but there is no way I am going to upload years of my chat history to a strange server. 

I spent a couple nights one weekend cooking up this little application that serves my needs fine.  There are a lot of additions, features, optimizations and probably bug fixes that could be added to it, and I certainly don't claim it to  be perfect. However, once it met my needs I stopped coding and thought I would share the code for anyone else needing a starting point to accomplish a similar task.

## Input
Google allows you to export data from your account using a service referred to as "Takeout".  If you use this service and choose to export your hangout data, then the resulting files will include a file named something like "Hangouts.json". This file is verbose and contains a lot of detail about every nuance of your chat conversations.  It is  more information than some people will ever want or need, and certainly is not an easy read if you were to open it in a text editor.  This file is the input file for this application.

## Output
A directory named "output" is created in the applications directory.  In it, a subdirectory is created for each "conversation" between people that it finds.  In that subdirectory, an HTML file is created, one per month, containing the formatted and easily readable chat converwsations.  The timestamps will be displayed in the local time of the user running the application and a note at the top of each file will identify the exact timezone/offset of the timestamps.  See example below.

## Usage
Usage: Hangouts-Takeout-JSON-Parser.exe "C:\Temp\My Dir\Hangouts.json"

## Limitations
- I did not spend anytime identifying or including images or files that were sent and received
- Some emojis come through to the html, others don't.  That was not a "must-have" feature for my needs
- Long html links sent in the chat may over flow the div element that contains them.  CSS is not my strongest talent
- Disk writing needs to be optimized to limit file open/closes
- Doesn't cook me breakfast

## Performance
Performance was NOT a goal when I wrote this.  It was gong to be a run-once-and-throw-away app before I decided to share the code.  With that disclaimer in mind, I successfully parsed a json file > 650Mb, representing about 7 years of my chat history, with no issue. Running in Visual Studio 2019 in debug, using a traditional mechanical HD, it read and parsed the json into memory in about 45 seconds, but then took

## Example Output
(Coming)
