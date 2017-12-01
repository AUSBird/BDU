# BDU
Bulk Download Utility (BDU)

Is a piece of software that allows you to download files from the internet in bulk.

### Instructions
1: Launch Application  
2: Enter all the values that it asks for (Description for each value is explained below)  
3: Wait for download  
4: If files failed to download, the app will tell you to re-execute the script to get the rest  

### Values
- (int) Thread Count -> The number of files to download at the same time (Don’t over kill this value, use 1 - 10)  
	- Example: 5  
- (string) Address -> The web URL where the files are that you want to download (Replace the ID of the file with \[])  
	- Original: https://assets.pokemon.com/assets/cms2/img/pokedex/full/001.png  
	- Example: https://assets.pokemon.com/assets/cms2/img/pokedex/full/[].png  
- (int) ID Padding -> The number of zeros to add to the front of the ID  
	- Example: Padding 4 on ID 1 is 0001 & Padding 4 on ID 256 is 0256  
	- Example: 4  
- (int) First ID -> The ID of the first image to download  
	- Example 1  
- (int) Last ID -> The ID of the last file to download  
	- Example: 675  
