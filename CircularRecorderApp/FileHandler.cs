using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace CircularRecorderApp
    {
    public class FileHandler
        {
        public static bool DoesFileExists (string fileName)
            {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication ())
                {
                return storage.FileExists (fileName + ".wav");
                }
            }

        public static void SaveFile (string fileName)
            {
            fileName += ".wav";

            // first, we grab the current apps isolated storage handle
            var storage = IsolatedStorageFile.GetUserStoreForApplication ();

            // if that file exists... 
            if (storage.FileExists (fileName))
                {
                // then delete it
                storage.DeleteFile (fileName);
                }

            // now we set up an isolated storage stream to point to store our data
            var stream = new IsolatedStorageFileStream (fileName, FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication ());
            var streamArray = Utilities.MemoryStream.ToArray ();
            stream.Write (streamArray, 0, streamArray.Length);

            // ok, done with isolated storage... so close it
            stream.Close ();

            Utilities.MemoryStream = null;

            }

        public static ObservableCollection<Recording> GetAllRecordings ()
            {
            var recording = new List<Recording> ();

            using (var storage = IsolatedStorageFile.GetUserStoreForApplication ())
                {
                foreach (var file in storage.GetFileNames ())
                    {
                    if (file.EndsWith ("wav"))
                        {
                        recording.Add (new Recording ()
                        {
                            Name = Path.GetFileNameWithoutExtension (file),
                            CreationDateTimeObject = storage.GetCreationTime (file)
                        });
                        }

                    }

                return new ObservableCollection<Recording>(recording.OrderByDescending (item => item.CreationDateTimeObject).ToList ());
                }
            }

        public static void DeleteFile (string name)
            {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication ())
                {
                storage.DeleteFile (name + ".wav");
                }
            }
        }

    public class Recording
        {
        public DateTimeOffset CreationDateTimeObject
            {
            get;
            set;
            }

        public string Name
            {
            get;
            set;
            }

        public string CreationTime
            {
            get
                {
                return CreationDateTimeObject.ToString ("f");
                }
            }

        public string ImageIconUri
            {
            get;
            set;
            }
        }
    }
