using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace UDT.Utilities
{
    public class Manifest : MonoBehaviour
    {
		//Manifest
        static string ManifestRead = "/Volumes/MonarchGameDrive/Monarch/Spacetriss/Assets/";
        static string ManifestSave = "/Volumes/MonarchGameDrive/Monarch/Spacetriss/Assets/";
        static string ManifestFileName = "Asset_Manifest.txt";

		//Backups
        static string ReadDir = "/Volumes/MonarchGameDrive/Monarch/Spacetriss/";
        static string SaveDir = "/Volumes/MonarchGameDrive/Dropbox/Monarch_Art/Spacetriss/";
        
        static string CopyRead = ReadDir + "Assets/AssetPacks/";
        static string CopySave = SaveDir + "AssetPacks/";
        static string CopyRead2 = ReadDir + "Assets/Art/";
        static string CopySave2 = SaveDir + "Art/";
		static string CopyRead3 = ReadDir + "Build/";
		static string CopySave3 = SaveDir + "Build/";


		[MenuItem("Monarch/Print Assets Manifest")]
        private static void PrintAllFiles(){
        /*

            Prints list of all files

        */

            print("Running Manifest Print...");
            List<string> testList = new List<string>();
            testList = GetAllDirFiles(ManifestRead).ToList();

            foreach(string item in testList){
                print(item);
            }
        }


        [MenuItem("Monarch/Save Assets Manifest")]
        private static void SaveManifest(){
        /*
         
            Saves assets list

        */

            List<string> testList = new List<string>();
            testList = GetAllDirFiles(ManifestSave);
            SaveFile(testList, ManifestSave, ManifestFileName);
        }


        public static List<string> GetAllDirFiles(string sDir, string exclusions = ""){
        /*
         
            Searches directory and outputs list

            sDir: Directory to read from
            return: List of files and subdirectories
                       
         */
            List<string> dirList = new List<string>();

            try
            {
                foreach (string d in Directory.GetDirectories(sDir)){

                    //grab all files, list exclusions to a var, then add to var
                    string[] allFiles = Directory.GetFiles(d);
                    string[] filesToExclude = Directory.GetFiles(d, exclusions);
                    IEnumerable<string> wantedFiles = allFiles.Except(filesToExclude);

                    foreach (string f in wantedFiles){
                        dirList.Add(f);
                    }
                    dirList.AddRange(GetAllDirFiles(d, exclusions));
                }
            }
            catch (System.Exception excpt){
                print(excpt.Message);
            }
            return dirList;
        }


        public static void SaveFile(List<string> input, string dir, string textFileName){
        /*
            Simple save list to text file and directory

            input: List of string.
            dir: Directory to save to.
            textFileName: Name of file to save. Extension not included, please add.
        */

            print("saving file..." + dir + textFileName);
            using (TextWriter tw = new StreamWriter(dir + textFileName)){

                foreach (string s in input)
                    tw.WriteLine(s);
            }

        }


        [MenuItem("Monarch/Copy Art Assets To Dropbox")]
        public static void CopyArtAssetsToDropbox(){
            /*

                Copy all files from list of directories
                TODO: Later make this into a string array           

            */

            CopyFiles(CopyRead, CopySave); //Asset packs
            CopyFiles(CopyRead2, CopySave2); //Art
            CopyFiles(CopyRead3, CopySave3); //Build
		}


        public static void CopyFiles(string copyRead, string copySave){
        /*

            Copy all files from one directory to another

        */
            string[] filePaths = GetAllDirFiles(copyRead, "*.meta").ToArray();
            foreach (string sourceFilename in filePaths)
            {
                //get new path replacing old to append files
                string destFilename = copySave + sourceFilename.Replace(copyRead, ""); 
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFilename));
                    File.Copy(sourceFilename, destFilename, true);
                    Debug.Log("copying file ..." + sourceFilename + "     to     " + destFilename);
                }
                catch (System.Exception excpt)
                {
                    print(excpt.Message);
                }
            }
            Debug.Log("Save complete!");
        }
    }
}

//TODO: Group public vars and simple declarations in Start