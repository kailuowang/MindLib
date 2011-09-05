using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.URLRewriter.Config
{
    [Serializable]
    public class Expression
    {
        private string pattern;
        private string replace;

        public string Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        public string Replace
        {
            get { return replace; }
            set { replace = value; }
        }
    }
}
