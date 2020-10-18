using System.Linq;

namespace NotepadIPlus
{
    public class CheckFileExtension
    {
        public bool IsThisFileWindowsCommandFile(string filepath)
        {
            string[] commandExtensions = { ".cmd", ".bat" };
            return commandExtensions.Any(x => filepath.ToLower().EndsWith(x));
        }

        public bool IsThisFileTextFile(string filepath) => filepath.ToLower().EndsWith(".txt");

        public bool IsThisFileWindowsShortcutFile(string filepath) => filepath.ToLower().EndsWith(".ink");

        public bool IsThisFileWindowsExecutableFile(string filepath) => filepath.ToLower().EndsWith(".exe");

        public bool IsThisFileVideoFile(string filepath)
        {
            string[] videoExtensions = {
                ".3g2", ".3gp", ".amv", ".asf", ".avi",
                ".drc", ".f4a", ".f4b", ".f4p", ".f4v",
                ".flv", ".gif", ".gifv", ".m2ts", ".m2v",
                ".m4p", ".m4v", ".mkv", ".mng", ".mov",
                ".mp2", ".mp4", ".mpe", ".mpeg", ".mpg",
                ".mpv", ".mts", ".mxf", ".nsv", ".ogg",
                ".ogv", ".qt", ".rm", ".rmvb", ".roq",
                ".svi", ".viv", ".vob", ".webm", ".wmv",
                ".yuv"
            };

            return videoExtensions.Any(x => filepath.EndsWith(x));
        }

        public bool IsThisFileMusicFile(string filepath)
        {
            string[] musicExtensions = {
                ".3gp", ".8svx", ".aa", ".aac", ".aax",
                ".act", ".aiff", ".alac", ".amr", ".ape",
                ".au", ".awb", ".cda", ".dct", ".dss",
                ".dvf", ".flac", ".gsm", ".iklax", ".ivs",
                ".m4a", ".m4b", ".m4p", ".mmf", ".mogg",
                ".mp3", ".mpc", ".msv", ".nmf", ".oga",
                ".ogg", ".opus", ".ra", ".raw", ".rf64",
                ".rm", ".tta", ".voc", ".vox", ".wav",
                ".webm", ".wma", ".wv"
            };

            return musicExtensions.Any(x => filepath.EndsWith(x));
        }
    }
}
