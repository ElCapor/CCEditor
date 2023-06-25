using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCEditor.Classes.Master_File_Pieces
{
    public class FileComponent
    {
        protected bool CanLoad
        {
            get
            {
                
                return true;
            }
        }

        public virtual int SortOrder => 0;

        public virtual void WillDestroy()
        {
        }

        public virtual void UpdateFromInspector()
        {
        }

        public override string ToString()
        {
            return GetType().ToString();
        }
    }

}
