# BFME2-MacroMapper
A tool for auto renaming and distributing macros for easier modding

Instructions:

Extract INI.big from BFME2 / BFME RotWK.

Open this program and select the directory where you extracted the 
files. The program won't run unless it can find the gamedata.ini.
The program will log every step.


What it does:

Renames all macros with a prefix of M_, and places the newly editable macros inside _gamedata.inc
Moves the gamedata block outside of gamedata.ini and inside /object/gamedata.ini
Takes care of /default/water.ini to include _gamedata.inc.

This program does not change the original files, rather outputs to a 
new directory of your choice (default is the location of the .exe)

Inspired By: https://forums.revora.net/topic/45037-tool-to-fix-mod-command-v103/

