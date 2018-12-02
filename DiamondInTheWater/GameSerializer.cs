#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

#endregion

//-----------------------------------------------------------------
//	Author:				Spencer Chang
//	Date:				December 29, 2016
//	Notes:				
//-----------------------------------------------------------------

namespace DiamondInTheWater
{
    /// <summary>
    /// Uses binary serialization to serialize and deserialize objects
    /// from memory or stream.
    /// </summary>
    public static class GameSerializer
    {
        #region Fields
        #endregion

        #region Methods
        /// <summary>
        /// Serializes an object into a file.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="path">Path of file.</param>
        public static void Serialize(Object obj, string path)
        {
            // Serializes the object.
            BinaryFormatter format = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);

            // Saves to the path.
            format.Serialize(stream, obj);
            stream.Close();
        }

        /// <summary>
        /// Deserializes a file into memory.
        /// </summary>
        /// <param name="path">The path of the object.</param>
        /// <returns>The deserialized Object.</returns>
        public static object Deserialize(string path)
        {
            // Deserializes the object.
            BinaryFormatter format = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Saves the deserialized object.
            object obj = format.Deserialize(stream);
            stream.Close();

            // returns it.
            return obj;

        }
        #endregion
    }
}