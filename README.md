TwitterImageArchiver
=========

Downloads all yfrog images given a Twitter backup archive

TwitterImageArchiver was a quickly built application I made to archive my yfrog images since Twitter's default backup does not have this feature. I tried an online service which did not appear to work, and I was concerned about privacy, hence I made this.

Yfrog requires a developer key to retrieve images which I did not have, hence this app automatically scrapes the required images (synchronously).


How to use
-----------

Download the binary (TwitterImageArchiver.exe) and put it within your Twitter backup archive folder, where ``tweets.csv`` resides.

Run the app - the images will be downloaded where possible, missing images are indicated with a text file with the Tweet's ID. 
The resultant files are stored in a new folder called Images.

License
-

MIT
