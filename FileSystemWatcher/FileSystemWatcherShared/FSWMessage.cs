using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProtoBuf;

namespace FSW
{
    [ProtoContract]
    public class FSWMessage
    {
        public FSWMessage() {}

        public FSWMessage(EventArgs e)
        {
            FileSystemEventArgs fea = e as FileSystemEventArgs;
            if (fea != null)
            {
                ChangeType = (int)fea.ChangeType;
                Name = fea.Name;
                Path = fea.FullPath;
            }
            else
            {
                RenamedEventArgs rea = e as RenamedEventArgs;
                if (rea != null)
                {
                    ChangeType = (int)fea.ChangeType;
                    Name = fea.Name;
                    Path = fea.FullPath;
                    OldName = rea.OldName;
                    OldPath = rea.OldFullPath;
                }
            }
        }

        [ProtoMember(10)]
        public int ChangeType { get; private set; }
        [ProtoMember(20)]
        public string Path { get; set; }
        [ProtoMember(30)]
        public string Name { get; set; }
        [ProtoMember(40)]
        public string OldPath { get; set; }
        [ProtoMember(50)]
        public string OldName { get; set; }
        [ProtoMember(60)]
        public bool IsFolder { get; set; }
    }
}