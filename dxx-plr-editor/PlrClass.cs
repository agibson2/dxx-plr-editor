/*
 *  Program to import and export Descent 1 and Descent 2 PLR files
 *    This program will can change details of the PLR file
 *    from the command line for instance changing descent macros and allowing
 *    color in them easily using command line switches.  You can have a batch
 *    file that sets F9 through F12 macros depending on what you plan to do.
 *    You can have competition macros or just fun macros when you launch your
 *    descent games.
 * 
 *    StatiC
 * 
 *   agibson2
 *      @
 *    gmail
 *      .
 *     com
 */


using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace dxxplreditor
{
	public class PlrClass
	{
		const int PLR_MAX_JOYSTICK_NAME_BUFFER = 13;

		public bool import_valid;
		const Int32 FILE_SIGNATURE = ((((((Int32)('D') << 8) | 'P') << 8) | 'L') << 8) | 'R';
		//public struct mission_protocol_entry
		//{
		//	public string mis_protocol_name;
		//	public byte mis_protocol_maxlevel;
		//}

		// non PLR file info
		public bool overwriteExistingFile = false;
		public bool debugEnabled = false;
		public bool displayInfoMessages = true;
		public int descentversion = -1;
		bool isbigendianfile = false;
		int imported_filesize = 0;
		// order of weapons below matters!
		string[] valid_d1_primary_weapons = { "omega", "superlaser", "spreadfire", "plasma", "laser", "fusion", "vulcan" };
		string[] valid_d1_secondary_weapons = { "mega", "smart", "homing", "concussion", "proximity" };
		string[] valid_d2_primary_weapons = { "laser", "vulcan", "spreadfire", "plasma", "fusion", "superlaser", "gauss", "helix", "phoenix", "omega" };
		string[] valid_d2_secondary_weapons = { "concussion", "homing", "proximity", "smartmissile", "mega", "flash", "guided", "smartmine", "mercury", "earthshaker" };
		// end non PLR file info

		public Int32 signature;

		// d1
		public UInt16 savegame_version;  // saved_game_version in d1 and player_file_version in d2
		public UInt16 struct_version;  // d1 only

		public Int16 resolution;
		//public byte[] unknown1 = {0, 1, 2};
		public UInt16 game_window_w;
		public UInt16 game_window_h;
		public Int32 default_difficulty;  // byte for d2 and 32 bit for d1
		public Int32 auto_leveling;  //1=on. byte for d2 and 32 bit for d1
		public byte show_reticle; //1=on
		public byte cockpit_mode;
		public byte video_resolution;  //0-4
		public byte missile_view;  //1=on
		public byte headlight_on_when_picked_up; //1=on
		public byte show_guided_missile_in_main_display;  //1=on
		//public byte unknown3
		public byte automap_always_hires;

		public Int32 num_mission_protocol_entries;  //  16 bit for d2 and 32 bit for d1 ... see note below

		PlrMission[] mission_arr;
		// string9 mis_protocol_name[i]
		// Byte mis_protocol_maxlevel[i]


		const int PLR_SAVED_GAMED_SW_SIZE = 1740;
		byte[] saved_game_data_shareware = new byte[PLR_SAVED_GAMED_SW_SIZE];

		public string multiplayer_macro_f9;
		public string multiplayer_macro_f10;
		public string multiplayer_macro_f11;
		public string multiplayer_macro_f12;


		const int PLR_CONFIGURATION_LENGTH = 480;
		public byte[] configuration;  //480 but smaller for d1

		// d1
		public byte controller_type;  //type of controller (0-7)

		// d2 savegame_version >= 20
		public byte obsolete_winjoy_map;  // unknown4; //2

		// d2
		public byte control_type_dos;
		public byte control_type_win;

		public byte obsolete_joy_sens;

		//public byte[] weapons_order_list;  //30  //primary/secondary weapons order list
		int PLR_MAX_WEAPON_ORDER_ENTRIES = 11;
		PlrWeaponOrder[] weapons_order_arr;

		UInt32 cockpit_3dview1;
		UInt32 cockpit_3dview2;

		public UInt32 lifetimerankings_num_kills;
		public UInt32 lifetimerankings_num_deaths;
		public UInt32 lifetimerankings_checksum;

		public string guidebot_callsign;  //10

		public string joystick_name;  //13 bytes with null

		public string name;

		public PlrClass ()
		{
			name = "unknown";
		}

		public PlrClass (string nm)
		{
			name = nm;
		}

		public void SetSavegame_version( UInt16 newSavegame_version )
		{
			savegame_version = newSavegame_version;
		}

		public void SetStruct_version( UInt16 newStruct_version )
		{
			struct_version = newStruct_version;
		}

		public void SetName(string newName)
		{
			name = newName;
		}

		public void SetSignature( Int32 newSignature )
		{
			signature = newSignature;
		}

		//public int SetResolution( Int16 newResolution )
		//{
		//	if((newResolution >= 0) || (newResolution <= 4)) {
		//		resolution = newResolution;
		//		return 1;
		//	} else {
		//		return 0;
		//	}
		//}

		public void SetGame_window_w( UInt16 newGame_window_w )
		{
			game_window_w = newGame_window_w;
		}

		public void SetGame_window_h( UInt16 newGame_window_h )
		{
			game_window_h = newGame_window_h;
		}

		public void SetDefault_difficulty( byte newDefault_difficulty )
		{
			default_difficulty = newDefault_difficulty;
		}

		public void SetDefault_difficulty( Int32 newDefault_difficulty )
		{
			default_difficulty = newDefault_difficulty;
		}

		//public int SetUnknown1( byte[] newUnknown1 )
		//{
		//	int retVal = 3;
		//	if (newUnknown1.Length > 0) {
		//		if (newUnknown1.Length > 3) {
		//			Console.WriteLine ("PlrClass.cs: Trying to copy more than 3 bytes to Unknown1.  Truncating to first 3 bytes");
		//		} else {
		//		}
		//
		//		Array.Copy(newUnknown1, unknown1, 3 );
		//		retVal = 0;
		//	} else if (newUnknown1.Length == 0) {
		//		Console.WriteLine ("PlrClass.cs: Trying to set newUnknown1 with a 0 byte string");
		//		retVal = 1;
		//	} else {
		//		Console.WriteLine ("PlrClass.cs: Trying to set newUnknown1 with a negative length string?");
		//		retVal = 1;
		//	}
		//
		//	return (retVal);
		//}

		public void SetAuto_leveling( byte newAuto_leveling )
		{
			auto_leveling = newAuto_leveling;
		}

		public void SetAuto_leveling( Int32 newAuto_leveling )
		{
			auto_leveling = newAuto_leveling;
		}

		public void SetShow_reticle( byte newShow_reticle )
		{
			show_reticle = newShow_reticle;
		}

		public void SetCockpit_mode( byte newCockpit_mode )
		{
			cockpit_mode = newCockpit_mode;
		}

		public void SetVideo_resolution( byte newVideo_resolution )
		{
			video_resolution = newVideo_resolution;
		}

		public void SetMissile_view( byte newMissile_view )
		{
			missile_view = newMissile_view;
		}

		public void SetHeadlight_on_when_picked_up( byte newHeadlight_on_when_picked_up )
		{
			headlight_on_when_picked_up = newHeadlight_on_when_picked_up;
		}

		public void SetShow_guided_missile_in_main_display( byte newShow_guided_missile_in_main_display )
		{
			show_guided_missile_in_main_display = newShow_guided_missile_in_main_display;
		}

		public void SetAutomap_always_hires( byte newAutomap_always_hires )
		{
			automap_always_hires = newAutomap_always_hires;
		}

		public void SetNum_mission_protocol_entries( Int16 newNum_mission_protocol_entries )
		{
			num_mission_protocol_entries = newNum_mission_protocol_entries;
		}

		public void SetNum_mission_protocol_entries( Int32 newNum_mission_protocol_entries )
		{
			num_mission_protocol_entries = newNum_mission_protocol_entries;
		}

		public bool isDescentVersionValid ()
		{
			if ((descentversion == 1) || (descentversion == 2)) {
				return(true);
			} else {
				return(false);
			}
		}
		public string EncodeColors( string toEncode ) {
			if (isDescentVersionValid () == false) {
				Console.WriteLine ("ERROR: Descent version not known yet so can't EncodeColors as D1 and D2 have different color palette");
				return(null);
			}

			if (debugEnabled == true) { Console.WriteLine ("EncodeColors Descent {0}: {0}", descentversion, toEncode); };
			Encoding f9Binary = Encoding.GetEncoding (28591);
			byte[] newString = new byte[35];
			bool escape = false;
			bool escapehex = false;
			byte escapehexposition = 0;
			byte[] hexchars = new byte[2];
			//byte colordefault = 100;
			//byte color = 0;
			byte currposition = 0;
			foreach (char c in toEncode) {
				//Console.WriteLine ("EncodeColors: c={0}", c);
				if(currposition >= newString.Length - 1) {
					Console.WriteLine("WARNING: '{0}' too long. Truncated to string below", toEncode);
					Console.WriteLine("         '{0}'", f9Binary.GetString(newString));
					break;
				}
				if (c == '/') {
					if (escape == true) {
						// remove the previos 0x01 with the /
						newString [--currposition] = (byte) c;
						escape = false;
						currposition++;
					} else {
						newString [currposition] = 0x01;
						escape = true;
						currposition++;
					}
				} else {
					if(escape == true) {
						if(c == 'x') {
							escapehex = true;
							escapehexposition = 0;
						} else if (escapehex == true) {
							if ((c >= '0' && c <= '9')
								|| (c >= 'a' && c <= 'f')
								|| (c >= 'A' && c <= 'F')) {
								if (escapehexposition == 0) {
									hexchars [0] = (byte)c;
									escapehexposition = 1;
								} else {
									hexchars [1] = (byte)c;
									newString [currposition] = (byte)Convert.ToUInt16 (System.Text.Encoding.UTF8.GetString (hexchars), 16);
									escapehexposition = 0;
									escape = false;
									escapehex = false;
									currposition++;
								}
							} else {
								Console.WriteLine ("ERROR: Macro color /x## invalid.  /x must be followed by a 0 through 9 or a through f");
								escapehex = false;
								escape = false;
								escapehexposition = 0;
								return(null);
							}
						} else {
							byte thiscolor = 0;
							escape=false;
							switch(c) {
							case 'r':  // red
								thiscolor = descentversion == 2 ? (byte)0xc3 : (byte)0xc9;
								break;

							case 'w':  // white
								thiscolor = descentversion == 2 ? (byte)0x01 : (byte)0x31;
								break;
							
							case 'b':  // blue
								thiscolor = descentversion == 2 ? (byte)0x0a : (byte)0x53;
								break;

							case 'Y':  // bright yellow
								thiscolor = descentversion == 2 ? (byte)0x4d : (byte)0xb0;
								break;

							case 'R':  // bright red
								thiscolor = descentversion == 2 ? (byte)0x69 : (byte)0xc2;
								break;

							case 'g':  // green
								thiscolor = descentversion == 2 ? (byte)0x40 : (byte)0x94;
								break;
							
							case 'G':  // bright green
								thiscolor = descentversion == 2 ? (byte)0x8d : (byte)0x99;
								break;

							case 'o':  // orange
								thiscolor = descentversion == 2 ? (byte)0x4b : (byte)0xd1;
								break;

							case 'O':  // bright orange
								thiscolor = descentversion == 2 ? (byte)0x78 : (byte)0xbe;
								break;
							
							case 'p':  // purple
								thiscolor = descentversion == 2 ? (byte)0xf0 : (byte)0xe9;
								break;

							case 'P':  // bright purple
								thiscolor = descentversion == 2 ? (byte)0x0f : (byte)0xe6;
								break;
							
							default:
								Console.WriteLine("ERROR: Invalid color '{0}' trying to parse color", c);
								return(null);
								//break;
							}

							newString[currposition] = thiscolor;
							currposition++;
						}
					} else {
						newString[currposition] = (byte) c;
						currposition++;
					}
				}
				
				//currposition++;
			}

			newString [currposition] = 0x00;

			string convertedString;
			convertedString = f9Binary.GetString (newString);
			if (debugEnabled == true) { Console.WriteLine ("EncodeColors returned '{0}'", convertedString); }
			return(convertedString);
		}

		public int SetMultiplayer_macro_f9( string newMultiplayer_macro_f9 )
		{
			multiplayer_macro_f9 = EncodeColors (newMultiplayer_macro_f9);
			if (multiplayer_macro_f9 == null) {
				return(-1);
			} else {
				return(1);
			}
			//multiplayer_macro_f9 = newMultiplayer_macro_f9;
		}

		public int SetMultiplayer_macro_f10( string newMultiplayer_macro_f10 )
		{
			multiplayer_macro_f10 = EncodeColors (newMultiplayer_macro_f10);
			if (multiplayer_macro_f10 == null) {
				return(-1);
			} else {
				return(1);
			}
			//multiplayer_macro_f9 = newMultiplayer_macro_f9;multiplayer_macro_f10 = newMultiplayer_macro_f10;
		}

		public int SetMultiplayer_macro_f11( string newMultiplayer_macro_f11 )
		{
			multiplayer_macro_f11 = EncodeColors (newMultiplayer_macro_f11);
			if (multiplayer_macro_f11 == null) {
				return(-1);
			} else {
				return(1);
			}
			//multiplayer_macro_f9 = newMultiplayer_macro_f9;multiplayer_macro_f11 = newMultiplayer_macro_f11;
		}

		public int SetMultiplayer_macro_f12( string newMultiplayer_macro_f12 )
		{
			multiplayer_macro_f12 = EncodeColors (newMultiplayer_macro_f12);
			if (multiplayer_macro_f12 == null) {
				return(-1);
			} else {
				return(1);
			}
			//multiplayer_macro_f9 = newMultiplayer_macro_f9;multiplayer_macro_f12 = newMultiplayer_macro_f12;
		}

		public void SetObsolete_winjoy_map( byte newObsolete_winjoy_map )
		{
			obsolete_winjoy_map = newObsolete_winjoy_map;
		}

		public void SetController_type( byte newController_type )
		{
			controller_type = newController_type;
		}

		public void SetControl_type_dos( byte newControl_type_dos )
		{
			control_type_dos = newControl_type_dos;
		}

		public void SetControl_type_win( byte newControl_type_win )
		{
			control_type_win = newControl_type_win;
		}

		public void SetObsolete_joy_sens( byte newObsolete_joy_sens )
		{
			obsolete_joy_sens = newObsolete_joy_sens;
		}

		public void SetJoystick_name( string newJoystick_name )
		{
			joystick_name = newJoystick_name;
		}

		public void SetGuidebot_callsign( string newGuidebot_callsign )
		{
			guidebot_callsign = newGuidebot_callsign;
		}

		public int Dump()
		{
			if (import_valid == false) {
				Console.WriteLine ("WARNING: Can't dump PLR.  Structure in memory is invalid");
				return (-1);
			}

			Console.WriteLine ("name: {0}  (Descent {1})", name, descentversion);
			Console.WriteLine (" signature(1,2): {0:X}", signature);
			Console.WriteLine (" savegame_version(1,2): " + savegame_version);
			if (descentversion == 1) {
				Console.WriteLine (" struct_version(1): " + struct_version);
			}
			//if (descentversion == 2) {
			//	Console.WriteLine (" resolution(2): " + resolution);
			//}
			if (descentversion == 2) {
				Console.WriteLine (" game_window_h(2): " + game_window_h);   //+ BitConverter.ToString (unknown1));
				Console.WriteLine (" game_window_w(2): " + game_window_w);
			}
			Console.WriteLine (" default_difficulty(1,2): " + default_difficulty);
			Console.WriteLine (" auto_leveling(1,2): " + auto_leveling);
			if (descentversion == 2) {
				Console.WriteLine (" show_reticle(2): " + show_reticle);
				Console.WriteLine (" cockpit_mode(2): " + cockpit_mode);
				Console.WriteLine (" video_resolution(2): " + video_resolution);
				Console.WriteLine (" missile_view(2): " + missile_view);
				Console.WriteLine (" headlight_on_when_picked_up(2): " + headlight_on_when_picked_up);
				Console.WriteLine (" show_guided_missile_in_main_display(2): " + show_guided_missile_in_main_display);
				Console.WriteLine (" automap_always_hires(2): " + automap_always_hires);
			}
			Console.WriteLine (" num_mission_protocol_entries(1,2): " + num_mission_protocol_entries);

			int tmploop = 0;
			Console.WriteLine (" mission_protocol_entries(1,2):");
			while (tmploop < num_mission_protocol_entries) {
				Console.WriteLine ("  {0}:{1}", mission_arr[tmploop].mission_name, mission_arr[tmploop].maximum_level);
				tmploop = tmploop + 1;
			}
			if(descentversion == 1) {
				if (savegame_version != 7) {
					Console.WriteLine (" save_game_data_shareware: {0} bytes of data", saved_game_data_shareware.Length );
				}
			}

			Console.WriteLine (" multiplayer_macro_f9(1,2): " + multiplayer_macro_f9);
			Console.WriteLine (" multiplayer_macro_f10(1,2): " + multiplayer_macro_f10);
			Console.WriteLine (" multiplayer_macro_f11(1,2): " + multiplayer_macro_f11);
			Console.WriteLine (" multiplayer_macro_f12(1,2): " + multiplayer_macro_f12);

			Console.WriteLine (" kconfig(1,2): {0} bytes of data", configuration.Length);

			if (descentversion == 1) {
				Console.WriteLine (" controller_type(1): " + controller_type);
			}
			if (descentversion == 2) {
				if (savegame_version <= 20) {
					Console.WriteLine (" obsolete_winjoy_map(2): " + obsolete_winjoy_map);
				}
				Console.WriteLine (" control_type_dos(2): " + control_type_dos);
				Console.WriteLine (" control_type_win(2): " + control_type_win);
			}

			Console.WriteLine (" obsolete_joy_sens(2): " + obsolete_joy_sens);

			if (descentversion == 2) {
				tmploop = 0;
				Console.WriteLine (" weapons_order_arr(2):");
				while (tmploop < PLR_MAX_WEAPON_ORDER_ENTRIES) {
					Console.WriteLine ("  {0}:{1}", weapons_order_arr [tmploop].primaryOrder, weapons_order_arr [tmploop].secondaryOrder);
					++tmploop;
				}
				if (savegame_version >= 16) {
					Console.WriteLine (" cockpit_3dview1: " + cockpit_3dview1);
					Console.WriteLine (" cockpit_3dview2: " + cockpit_3dview2);
				}

				Console.WriteLine (" lifetimerankings_num_kills(2): " + lifetimerankings_num_kills);
				Console.WriteLine (" lifetimerankings_num_deaths(2): " + lifetimerankings_num_deaths);
				Console.WriteLine (" lifetimerankings_checksum(2): " + lifetimerankings_checksum);
				Console.WriteLine (" guidebot_callsign(2): " + guidebot_callsign);
				Console.WriteLine (" joystick_name(2): " + joystick_name);
			}

			return(0);
		}

		public int ImportFromFile( string filename )
		{
			int fileoffset = 0;
			import_valid = false;
			descentversion = -1;
			Encoding encBinary = Encoding.GetEncoding (28591);

			byte[] filedata = File.ReadAllBytes(filename);
			string filenameonly = Path.GetFileNameWithoutExtension(filename);
			SetName (filenameonly);

			SetSignature( BitConverter.ToInt32(filedata, fileoffset) );  //0
			fileoffset += 4;
			if (signature != FILE_SIGNATURE) {
				Console.WriteLine ("ERROR: Can't load PLR file.  PLR file signature invalid.  file signature {0} does not match {1}", signature, FILE_SIGNATURE);
				import_valid = false;
				return (-1);
			}
				
			SetSavegame_version( BitConverter.ToUInt16(filedata, fileoffset) );
			fileoffset += 2;  // now at 8

			if ((savegame_version >= 0) && (savegame_version <= 8)) {
				descentversion = 1;
			} else {
				descentversion = 2;
			}

			const int PLR_COMATIBLE_SAVEGAME_VERSION = 4;
			const int PLR_COMPATIBLE_STRUCT_VERSION = 16;

			if (descentversion == 1) {
				SetStruct_version (BitConverter.ToUInt16 (filedata, fileoffset));
				fileoffset += 2;
				if (savegame_version < PLR_COMATIBLE_SAVEGAME_VERSION || struct_version < PLR_COMPATIBLE_STRUCT_VERSION) {
					Console.WriteLine ("ERROR: savegame version {0} < {1} or PLR struct version {2} < {3}.  Can't load PLR file.", savegame_version, PLR_COMATIBLE_SAVEGAME_VERSION, struct_version, PLR_COMPATIBLE_STRUCT_VERSION);
					return (-1);
				}

				SetNum_mission_protocol_entries (BitConverter.ToInt32(filedata, fileoffset));
				fileoffset += 4;
				SetDefault_difficulty (BitConverter.ToInt32(filedata, fileoffset));
				fileoffset += 4;
				SetAuto_leveling (BitConverter.ToInt32(filedata, fileoffset));
				fileoffset += 4;

				if (savegame_version <= 5) {
					Console.WriteLine ("ERROR: savegame_version <=5.  Old style highest level info not supported.");
					return(-1);
				}
			}

			const int PLR_MISSION_MAXLEVEL_NAME_LENGTH = 9;
			const int PLR_MISSION_MAXLEVEL_ENTRY_LENGTH = PLR_MISSION_MAXLEVEL_NAME_LENGTH + 1;  // 9 bytes for mission name and 1 byte for max level

			int filesize_without_highlevels = filedata.Length - (PLR_MISSION_MAXLEVEL_ENTRY_LENGTH * num_mission_protocol_entries);

			if (savegame_version >= 0 && savegame_version <= 7) {
				Console.WriteLine ("ERROR: Descent 1 PLR file version {0} not supported at PLR save version 7 and older.", savegame_version);
				return (-1);
			} else if (savegame_version == 8) {
				if (filesize_without_highlevels == 2212) {
					Console.WriteLine ("ERROR: Shareware version not supported");
					return (-1);
				} else if (filesize_without_highlevels == 2252) {
					// valid full version PLR file but need to skip the next 2 Int32 data
				} else if (filesize_without_highlevels == (2212 + 2 * sizeof(Int32))) {
					Console.WriteLine ("ERROR: Shareware version not supported (v0.31 to v0.42 d1x).");
					return (-1);
				} else if (filesize_without_highlevels == (2252 + 2 * sizeof(Int32))) {
					// valid full version PLR file but need to skip the next 2 Int32 data
				} else {
					Console.WriteLine ("ERROR: file size {0} ({1} without high level data)not known to be a valid PLR file for Descent 1", filedata.Length, filesize_without_highlevels);
				}

				descentversion = 1;
				//Console.WriteLine ("ERROR: Descent 1 PLR files not implemented yet.");
				//return (-1);
			} else if (savegame_version == 24) {
				descentversion = 2;
			} else if (savegame_version > 255) {
				isbigendianfile = true;
				descentversion = 2;
				savegame_version = (UInt16)System.Net.IPAddress.NetworkToHostOrder ((int)savegame_version);
				Console.WriteLine ("ERROR: PLR file looks like it is bigendian.  That architecture is not supported.");
				return (-1);
			}
				
			//byte[] fileunknown1 = { 0, 0, 0 };
			//Array.Copy(filedata, 8, fileunknown1, 0, 3);
			//SetUnknown1( fileunknown1 );
			if (descentversion == 2) {
				SetGame_window_h (BitConverter.ToUInt16(filedata, fileoffset));
				fileoffset += 2;
				SetGame_window_w (BitConverter.ToUInt16(filedata, fileoffset));
				fileoffset += 2;
				SetDefault_difficulty (filedata[fileoffset++]);
				SetAuto_leveling (filedata [fileoffset++]);
				SetShow_reticle (filedata [fileoffset++]);
				SetCockpit_mode (filedata [fileoffset++]);
				SetVideo_resolution (filedata [fileoffset++]);
				SetMissile_view (filedata [fileoffset++]);
				SetHeadlight_on_when_picked_up (filedata [fileoffset++]);  // 16 here d2
				SetShow_guided_missile_in_main_display (filedata [fileoffset++]);
				SetAutomap_always_hires (filedata [fileoffset++]);  // 18 here d2
				SetNum_mission_protocol_entries (BitConverter.ToInt16(filedata, fileoffset));  // an extra byte so assuming this is a 16 bit number
				fileoffset += 2; //19 here d2... 21 next line
			}

			mission_arr = new PlrMission[num_mission_protocol_entries];

			//const int MISSION_PROTOCOL_ENTRIES_OFFSET_START = 21;
			mission_arr = new PlrMission[num_mission_protocol_entries];
			int loop = 0;
			while (loop < num_mission_protocol_entries) {
				PlrMission thisMiss = new PlrMission ();
				//int startoffset = loop * MISSION_PROTOCOL_ENTRY_LENGTH + fileoffset;
				thisMiss.mission_name = encBinary.GetString (filedata, fileoffset, PLR_MISSION_MAXLEVEL_NAME_LENGTH);
				fileoffset += PLR_MISSION_MAXLEVEL_NAME_LENGTH;
				thisMiss.maximum_level = filedata [fileoffset++];
				mission_arr [loop] = thisMiss;
				++loop;
			}

			if (descentversion == 1) {
				if (savegame_version != 7) {
					Array.Copy (filedata, fileoffset, saved_game_data_shareware, 0, PLR_SAVED_GAMED_SW_SIZE);
					fileoffset += PLR_SAVED_GAMED_SW_SIZE;
				}
			}

			//const int PLR_MISSION_PROTOCOL_ENTRIES_OFFSET = 21;
			const int MULTIPLAYER_MACRO_BUFFER_SIZE = 35;

			//int offset = PLR_MISSION_PROTOCOL_ENTRIES_OFFSET + (num_mission_protocol_entries * 10);
			multiplayer_macro_f9 = encBinary.GetString (filedata, fileoffset, MULTIPLAYER_MACRO_BUFFER_SIZE);
			fileoffset += MULTIPLAYER_MACRO_BUFFER_SIZE;
			multiplayer_macro_f10 = encBinary.GetString (filedata, fileoffset, MULTIPLAYER_MACRO_BUFFER_SIZE);
			fileoffset += MULTIPLAYER_MACRO_BUFFER_SIZE;
			multiplayer_macro_f11 = encBinary.GetString (filedata, fileoffset, MULTIPLAYER_MACRO_BUFFER_SIZE);
			fileoffset += MULTIPLAYER_MACRO_BUFFER_SIZE;
			multiplayer_macro_f12 = encBinary.GetString (filedata, fileoffset, MULTIPLAYER_MACRO_BUFFER_SIZE);
			fileoffset += MULTIPLAYER_MACRO_BUFFER_SIZE;

			const int PLR_CONTROL_MAX_TYPES_D1 = 7;
			const int PLR_CONTROL_MAX_TYPES_D2 = 8;
			const int PLR_MAX_CONTROLS_D1 = 50;
			const int PLR_MAX_CONTROLS_D2 = 60;
			if (descentversion == 1) {
				int configurationlength = PLR_MAX_CONTROLS_D1 * PLR_CONTROL_MAX_TYPES_D1;
				configuration = new byte[configurationlength];
				Array.Copy (filedata, fileoffset, configuration, 0, configurationlength);
				fileoffset += configurationlength;
			}
			if (descentversion == 2) {
				int configurationlength = (savegame_version < 20 ? PLR_MAX_CONTROLS_D1 : PLR_MAX_CONTROLS_D2) * PLR_CONTROL_MAX_TYPES_D2;
				configuration = new byte[configurationlength];
				Array.Copy (filedata, fileoffset, configuration, 0, configurationlength);
				fileoffset += configurationlength;
			}

			if (descentversion == 1) {
				SetController_type (filedata [fileoffset++]);
			}
			if (descentversion == 2) {
				if (savegame_version <= 20) {
					SetObsolete_winjoy_map (filedata [fileoffset++]);
				}
				SetControl_type_dos (filedata [fileoffset++]);
				SetControl_type_win (filedata [fileoffset++]);
			}
				
			SetObsolete_joy_sens (filedata [fileoffset++]);

			if (descentversion == 2) {
				weapons_order_arr = new PlrWeaponOrder[PLR_MAX_WEAPON_ORDER_ENTRIES];
				loop = 0;
				while (loop < PLR_MAX_WEAPON_ORDER_ENTRIES) {
					PlrWeaponOrder tmpWeaponOrder = new PlrWeaponOrder (filedata [fileoffset++], filedata [fileoffset++]);
					//tmpWeaponOrder.primaryOrder = filedata [fileoffset++];
					//tmpWeaponOrder.secondaryOrder = filedata [fileoffset++];
					weapons_order_arr [loop] = tmpWeaponOrder;
					loop++;
				}

				if (savegame_version >= 16) {
					cockpit_3dview1 = BitConverter.ToUInt32 (filedata, fileoffset);
					fileoffset += 4;
					cockpit_3dview2 = BitConverter.ToUInt32 (filedata, fileoffset);
					fileoffset += 4;
				}

				lifetimerankings_num_kills = BitConverter.ToUInt32 (filedata, fileoffset);
				fileoffset += 4;
				lifetimerankings_num_deaths = BitConverter.ToUInt32 (filedata, fileoffset);
				fileoffset += 4;
				lifetimerankings_checksum = BitConverter.ToUInt32 (filedata, fileoffset);
				fileoffset += 4;

				const int PLR_GUIDEBOT_MAX_LEN = 10;
				if (savegame_version >= 18) {
					guidebot_callsign = encBinary.GetString (filedata, fileoffset, PLR_GUIDEBOT_MAX_LEN);
					fileoffset += PLR_GUIDEBOT_MAX_LEN;
				}

				if (savegame_version >= 24) {
					joystick_name = encBinary.GetString (filedata, fileoffset, PLR_MAX_JOYSTICK_NAME_BUFFER);
					fileoffset += PLR_MAX_JOYSTICK_NAME_BUFFER;
				}
			}

			if (fileoffset != filedata.Length) {
				Console.WriteLine ("ERROR: problem reading file.  Parsed offset ended at {0} while file length is {1}", fileoffset, filedata.Length);
				return (-1);
			}

			imported_filesize = filedata.Length;
			import_valid = true;
			return (0);
		}

		public int ExportToFile (string filename)
		{
			if (!import_valid) {
				Console.WriteLine ("PLR import was not loaded successfully first.  Can't export an incomplete PLR file.");
				return (-1);
			}
				
			int filedata_index = 0;
			Encoding encBinary = Encoding.GetEncoding (28591);

			List<byte[]> filedata_l = new List<byte[]>();

			filedata_l.Add (BitConverter.GetBytes (signature));
			filedata_l.Add (BitConverter.GetBytes (savegame_version));
			if(descentversion == 1) {
				filedata_l.Add (BitConverter.GetBytes (struct_version));
				filedata_l.Add (BitConverter.GetBytes (num_mission_protocol_entries));
			}
			if (descentversion == 2) {
				filedata_l.Add (BitConverter.GetBytes (game_window_h));
				filedata_l.Add (BitConverter.GetBytes (game_window_w));
			}
			if (descentversion == 1) {
				filedata_l.Add (BitConverter.GetBytes (default_difficulty));
				filedata_l.Add (BitConverter.GetBytes (auto_leveling));
				if (savegame_version <= 5) {
					Console.WriteLine ("ERROR: savegame_version <=5.  Old style highest level info not supported.");
					return(-1);
				}
			}
			if (descentversion == 2) { // d2
				filedata_l.AddByte ((byte) default_difficulty);  //byte for d2
				filedata_l.AddByte ((byte) auto_leveling);  // byte for d2
				filedata_l.AddByte (show_reticle);
				filedata_l.AddByte (cockpit_mode);
				filedata_l.AddByte (video_resolution);
				filedata_l.AddByte (missile_view);
				filedata_l.AddByte (headlight_on_when_picked_up);
				filedata_l.AddByte (show_guided_missile_in_main_display);
				filedata_l.AddByte (automap_always_hires);
				filedata_l.Add (BitConverter.GetBytes ((Int16) num_mission_protocol_entries));
			}
			foreach (PlrMission tmpMission in mission_arr) {
				filedata_l.Add (encBinary.GetBytes(tmpMission.mission_name));
				//Console.WriteLine ("'{0}':{1} {2}", tmpMission.mission_name, tmpMission.mission_name.Length, tmpMission.maximum_level);
				filedata_l.AddByte (tmpMission.maximum_level);
			}
			if (descentversion == 1) {
				if (savegame_version != 7) {
					filedata_l.Add (saved_game_data_shareware);
				}
			}
			filedata_l.Add (encBinary.GetBytes (multiplayer_macro_f9));
			filedata_l.Add (encBinary.GetBytes (multiplayer_macro_f10));
			filedata_l.Add (encBinary.GetBytes (multiplayer_macro_f11));
			filedata_l.Add (encBinary.GetBytes (multiplayer_macro_f12));
			filedata_l.Add (configuration);
			if (descentversion == 1) {
				filedata_l.AddByte (controller_type);
			}
			if (descentversion == 2) {
				if (savegame_version <= 20) {
					filedata_l.AddByte (obsolete_winjoy_map);
				}
				filedata_l.AddByte (control_type_dos);
				filedata_l.AddByte (control_type_win);
			}
			filedata_l.AddByte (obsolete_joy_sens);
			if (descentversion == 2) {
				foreach (PlrWeaponOrder tmpWeaponOrder in weapons_order_arr) {
					filedata_l.AddByte (tmpWeaponOrder.primaryOrder);
					filedata_l.AddByte (tmpWeaponOrder.secondaryOrder);
					//Console.WriteLine ("{0}:{1}", tmpWeaponOrder.primaryOrder, tmpWeaponOrder.secondaryOrder);
				}
				if (savegame_version >= 16) {
					filedata_l.Add (BitConverter.GetBytes(cockpit_3dview1));
					filedata_l.Add (BitConverter.GetBytes (cockpit_3dview2));
				}
				filedata_l.Add (BitConverter.GetBytes (lifetimerankings_num_kills));
				filedata_l.Add (BitConverter.GetBytes (lifetimerankings_num_deaths));
				filedata_l.Add (BitConverter.GetBytes (lifetimerankings_checksum));
				filedata_l.Add (encBinary.GetBytes(guidebot_callsign));
				filedata_l.Add (encBinary.GetBytes(joystick_name));
			}

			// Now lets write the data to a new array, then another array the correct size...
			//  and then finally to a file.
			//  TODO: figure out how to get the total bytes of the List<[]> filedata_l so that
			//        we don't have to copy it twice!
			byte[] filedata_arr = new byte[10000];

			foreach (byte[] filedata_buffer in filedata_l) {
				filedata_buffer.CopyTo (filedata_arr, filedata_index);
				filedata_index += filedata_buffer.Length;
			}

			byte[] filedata_arr_truncated = new byte[filedata_index];

			Array.Copy (filedata_arr, filedata_arr_truncated, filedata_index);

			if (imported_filesize != filedata_arr_truncated.Length) {
				Console.WriteLine ("WARNING: imported filesize was {0}.  Exported filesize is {1}.", imported_filesize, filedata_arr_truncated.Length);
			}

			string filetowrite;
			if (overwriteExistingFile == true) {
				filetowrite = filename;
			} else {
				filetowrite = filename + ".new";
			}

			File.WriteAllBytes (filetowrite, filedata_arr_truncated);
			if (displayInfoMessages) {
				Console.WriteLine ("Wrote new PLR file to {0}", filetowrite);
			}

			return(0);
		}

		public int SetPrimary_auto_select (string [] weapons) {

			if (descentversion != 2) {
				Console.WriteLine ("Can't set primary auto select weapon search order in Descent 1 (yet)");
				return(-1);
			}
			Dictionary<string, byte> weaponDict = new Dictionary<string, byte> ();
			int weaponLocLoop = 0;
			foreach (string weapon in weapons) {
				int foundindex = Array.IndexOf( valid_d2_primary_weapons, weapon);
				//Console.WriteLine ("foundindex=" + foundindex);
				if( foundindex >= 0) {
					byte weaponfoundcount;
					weaponDict.TryGetValue(weapon, out weaponfoundcount);
					weaponDict [weapon] = ++weaponfoundcount;
					if (weaponDict [weapon] > 1) {
						Console.WriteLine ("SetPrimary_auto_select: '{0}' listed more than once", weapon);
						return(-1);
					}

					weapons_order_arr[weaponLocLoop].primaryOrder = (byte) foundindex;
						
				} else {
					Console.WriteLine("SetPrimary_auto_select: Invalid weapon '{0}'", weapon);
					string weaponprintlist = "";
					string[] thisdescentversionweaponlist;
					if (descentversion == 1) {
						thisdescentversionweaponlist = valid_d1_primary_weapons;
					} else {
						thisdescentversionweaponlist = valid_d2_primary_weapons;
					}
					foreach (string weap in thisdescentversionweaponlist) {
						weaponprintlist = weaponprintlist + weap + " ";
					}
					Console.WriteLine ("Valid Descent {0} primary weapons are... {1}", descentversion, weaponprintlist);
					return(-1);
				}

				weaponLocLoop++;
			}
				
			weapons_order_arr [weaponLocLoop++].primaryOrder = 255;  // set the separator to designate everything below this point should not be autoselected
			foreach(string weapon in valid_d2_primary_weapons) {
				if (!(Array.Exists (weapons, x => x == weapon))) {
					int foundindex = Array.IndexOf( valid_d2_primary_weapons, weapon);
					weapons_order_arr [weaponLocLoop++].primaryOrder = (byte) foundindex;
					}
			}

			return(1);
		}

		public int SetSecondary_auto_select (string [] weapons) {

			if (descentversion != 2) {
				Console.WriteLine ("Can't set secondary auto select weapon search order in Descent 1 (yet)");
				return(-1);
			}
			Dictionary<string, byte> weaponDict = new Dictionary<string, byte> ();
			int weaponLocLoop = 0;
			foreach (string weapon in weapons) {
				int foundindex = Array.IndexOf( valid_d2_secondary_weapons, weapon);
				//Console.WriteLine ("foundindex=" + foundindex);
				if( foundindex >= 0) {
					byte weaponfoundcount;
					weaponDict.TryGetValue(weapon, out weaponfoundcount);
					weaponDict [weapon] = ++weaponfoundcount;
					if (weaponDict [weapon] > 1) {
						Console.WriteLine ("SetSecondary_auto_select: '{0}' listed more than once", weapon);
						return(-1);
					}

					weapons_order_arr[weaponLocLoop].secondaryOrder = (byte) foundindex;
				} else {
					Console.WriteLine("SetSecondary_auto_select: Invalid weapon '{0}'", weapon);
					string weaponprintlist = "";
					string[] thisdescentversionweaponlist;
					if (descentversion == 1) {
						thisdescentversionweaponlist = valid_d1_secondary_weapons;
					} else {
						thisdescentversionweaponlist = valid_d2_secondary_weapons;
					}
					foreach (string weap in thisdescentversionweaponlist) {
						weaponprintlist = weaponprintlist + weap + " ";
					}
					Console.WriteLine ("Valid Descent {0} secondary weapons are... {1}", descentversion, weaponprintlist);
					return(-1);
				}

				weaponLocLoop++;
			}

			weapons_order_arr [weaponLocLoop++].secondaryOrder = 255;  // set the separator to designate everything below this point should not be autoselected
			foreach(string weapon in valid_d2_secondary_weapons) {
				if (!(Array.Exists (weapons, x => x == weapon))) {
					int foundindex = Array.IndexOf( valid_d2_secondary_weapons, weapon);
					weapons_order_arr [weaponLocLoop++].secondaryOrder = (byte) foundindex;
				}
			}

			return(1);
		}
	}
}
