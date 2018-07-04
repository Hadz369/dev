using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfSearchControls
{
    /// <summary>
    /// Used to provide a very basic interface to a search object
    /// </summary>
    public interface iSearchItem
    {
        int Id { get; }
        string Value { get; }
        string SearchString { get; }
        string ToString();
    }
}
