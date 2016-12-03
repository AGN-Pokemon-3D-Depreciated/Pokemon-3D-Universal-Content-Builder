using System.Collections.Generic;

namespace Universal_Content_Builder.Content.Interface
{
    public interface IContent
    {
        /// <summary>
        /// Get Source file.
        /// </summary>
        string GetSourceFile();

        /// <summary>
        /// Get Content importer.
        /// </summary>
        string GetImporter();

        /// <summary>
        /// Get Content processor.
        /// </summary>
        string GetProcessor();

        /// <summary>
        /// Get Content processor param.
        /// </summary>
        Dictionary<string, string> GetProcessorParam();

        /// <summary>
        /// Build file.
        /// </summary>
        bool BuildFile();
    }
}
