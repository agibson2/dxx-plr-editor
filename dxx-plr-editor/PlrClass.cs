﻿using System;
using System.IO;
using System.Text;

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
		public int descentversion = -1;
		bool isbigendianfile = false;
		// end non PLR file info

		public Int32 signature;

		// d1
		public Int16 savegame_version;  // saved_game_version in d1 and player_file_version in d2
		public Int16 struct_version;  // d1 only

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
		public byte[] configuration = new byte[PLR_CONFIGURATION_LENGTH];  //480

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

		public void SetSavegame_version( Int16 newSavegame_version )
		{
			savegame_version = newSavegame_version;
		}

		public void SetStruct_version( Int16 newStruct_version )
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

		public int SetResolution( Int16 newResolution )
		{
			if((newResolution >= 0) || (newResolution <= 4)) {
				resolution = newResolution;
				return 1;
			} else {
				return 0;
			}
		}

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

		public void SetMultiplayer_macro_f9( string newMultiplayer_macro_f9 )
		{
			multiplayer_macro_f9 = newMultiplayer_macro_f9;
		}

		public void SetMultiplayer_macro_f10( string newMultiplayer_macro_f10 )
		{
			multiplayer_macro_f10 = newMultiplayer_macro_f10;
		}

		public void SetMultiplayer_macro_f11( string newMultiplayer_macro_f11 )
		{
			multiplayer_macro_f11 = newMultiplayer_macro_f11;
		}

		public void SetMultiplayer_macro_f12( string newMultiplayer_macro_f12 )
		{
			multiplayer_macro_f12 = newMultiplayer_macro_f12;
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
			if (descentversion == 2) {
				Console.WriteLine (" resolution(2): " + resolution);
			}
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

			Console.WriteLine (" multiplayer_macro_f9(1,2): " + multiplayer_macro_f9);
			Console.WriteLine (" multiplayer_macro_f10(1,2): " + multiplayer_macro_f10);
			Console.WriteLine (" multiplayer_macro_f11(1,2): " + multiplayer_macro_f11);
			Console.WriteLine (" multiplayer_macro_f12(1,2): " + multiplayer_macro_f12);

			Console.WriteLine (" kconfig(1,2): {0} bytes of data", configuration.Length);

			if (descentversion == 1) {
				Console.WriteLine (" controller_type(1): " + controller_type);
			}
			if (descentversion == 2) {
				Console.WriteLine (" obsolete_winjoy_map(2): " + obsolete_winjoy_map);
				Console.WriteLine (" control_type_dos(2): " + control_type_dos);
				Console.WriteLine (" control_type_win(2): " + control_type_win);

				tmploop = 0;
				Console.WriteLine (" weapons_order_arr(2):");
				while (tmploop < PLR_MAX_WEAPON_ORDER_ENTRIES) {
					Console.WriteLine ("  {0}:{1}", weapons_order_arr [tmploop].primaryOrder, weapons_order_arr [tmploop].secondaryOrder);
					++tmploop;
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
			byte[] filedata = File.ReadAllBytes(filename);
			string filenameonly = Path.GetFileNameWithoutExtension(filename);
			import_valid = false;
			descentversion = -1;
			SetName (filenameonly);

			SetSignature( BitConverter.ToInt32(filedata, fileoffset) );  //0
			fileoffset += 4;
			if (signature != FILE_SIGNATURE) {
				Console.WriteLine ("ERROR: Can't load PLR file.  PLR file signature invalid.  file signature {0} does not match {1}", signature, FILE_SIGNATURE);
				import_valid = false;
				return (-1);
			}
				
			SetSavegame_version( BitConverter.ToInt16(filedata, fileoffset) );
			fileoffset += 2;  // now at 8

			if ((savegame_version >= 0) && (savegame_version <= 8)) {
				descentversion = 1;
			} else {
				descentversion = 2;
			}

			const int PLR_COMATIBLE_SAVEGAME_VERSION = 4;
			const int PLR_COMPATIBLE_STRUCT_VERSION = 16;

			if (descentversion == 1) {
				SetStruct_version (BitConverter.ToInt16 (filedata, fileoffset));
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
				savegame_version = System.Net.IPAddress.NetworkToHostOrder (savegame_version);
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
				thisMiss.mission_name = Encoding.ASCII.GetString (filedata, fileoffset, 8);
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
			multiplayer_macro_f9 = Encoding.ASCII.GetString (filedata, fileoffset, MULTIPLAYER_MACRO_BUFFER_SIZE);
			fileoffset += MULTIPLAYER_MACRO_BUFFER_SIZE;
			multiplayer_macro_f10 = Encoding.ASCII.GetString (filedata, fileoffset, MULTIPLAYER_MACRO_BUFFER_SIZE);
			fileoffset += MULTIPLAYER_MACRO_BUFFER_SIZE;
			multiplayer_macro_f11 = Encoding.ASCII.GetString (filedata, fileoffset, MULTIPLAYER_MACRO_BUFFER_SIZE);
			fileoffset += MULTIPLAYER_MACRO_BUFFER_SIZE;
			multiplayer_macro_f12 = Encoding.ASCII.GetString (filedata, fileoffset, MULTIPLAYER_MACRO_BUFFER_SIZE);
			fileoffset += MULTIPLAYER_MACRO_BUFFER_SIZE;

			const int PLR_CONTROL_MAX_TYPES_D1 = 7;
			const int PLR_CONTROL_MAX_TYPES_D2 = 8;
			const int PLR_MAX_CONTROLS_D1 = 50;
			const int PLR_MAX_CONTROLS_D2 = 60;
			if (descentversion == 1) {
				int configurationlength = PLR_MAX_CONTROLS_D1 * PLR_CONTROL_MAX_TYPES_D1;
				Array.Copy (filedata, fileoffset, configuration, 0, configurationlength);
				fileoffset += configurationlength;
			} else {
				int configurationlength = (savegame_version < 20 ? PLR_MAX_CONTROLS_D1 : PLR_MAX_CONTROLS_D2) * PLR_CONTROL_MAX_TYPES_D2;
				Array.Copy (filedata, fileoffset, configuration, 0, configurationlength);
				fileoffset += configurationlength;
			}

			if (descentversion == 1) {
				SetController_type (filedata [fileoffset++]);
			} else {
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
					guidebot_callsign = Encoding.ASCII.GetString (filedata, fileoffset, PLR_GUIDEBOT_MAX_LEN);
					fileoffset += PLR_GUIDEBOT_MAX_LEN;
				}

				if (savegame_version >= 24) {
					joystick_name = Encoding.ASCII.GetString (filedata, fileoffset, PLR_MAX_JOYSTICK_NAME_BUFFER);
					fileoffset += PLR_MAX_JOYSTICK_NAME_BUFFER;
				}
			}

			if (fileoffset != filedata.Length) {
				Console.WriteLine ("ERROR: problem reading file.  Parsed offset ended at {0} while file length is {1}", fileoffset, filedata.Length);
				return (-1);
			}

			import_valid = true;
			return (0);
		}

		public void ExportToFile()
		{
			//byte[] filedata;
			//int index = 0;

			//filedata[index++] = 
		}
	}
}
