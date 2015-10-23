using System.Collections.Generic;

namespace Doctrina
{
   public class BannedSymbolsClass
    {
       private readonly List<bool> _bannedSymbolMeets;
       private readonly List<string> _bannedSymbols;
       public  BannedSymbolsClass(string[] staticElemens )
       {
           _bannedSymbols = new List<string>(staticElemens);
            _bannedSymbolMeets=new List<bool>(staticElemens.Length);
           foreach (var VARIABLE in staticElemens)
           {
                _bannedSymbolMeets.Add(false);
           }
       }

       public void ClearBannedSymbolMeet()
       {
           for (int t = 0; t < _bannedSymbolMeets.Count; ++t)
           {
               _bannedSymbolMeets[t] = false;
           }
        }

       public bool IsBannedSymbolMeet(int position)
       {
           return _bannedSymbolMeets[position];
       }

       public void BannedSymbolMeet(int position)
       {
           _bannedSymbolMeets[position] = true;
       }

       public List<string> BannedSymbolsList
       {
           get { return _bannedSymbols; }
       } 
    }
}
