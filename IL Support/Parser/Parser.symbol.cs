using System.Linq;

using Microsoft.VisualStudio.Language.StandardClassification;

namespace ILSupport
{
    public static partial class ILParser
    {
        private class SymbolCategory
        {
            public SymbolCategory ( string @class, string symbols )
            {
                Class   = @class;
                Symbols = symbols;
            }

            public string Class   = null;
            public string Symbols = null;
        }

        public static string GetSymbols ( string @class )
        {
            return symbolCategories.Where  ( category => category.Class == @class )
                                   .Select ( category => category.Symbols )
                                   .FirstOrDefault ( );
        }

        public static string IdentifySymbolClass ( char symbol )
        {
            foreach ( var category in symbolCategories )
                if ( category.Symbols.Contains ( symbol ) )
                    return category.Class;

            return null;
        }

        private static readonly SymbolCategory [ ] symbolCategories =
        {
            new SymbolCategory ( PredefinedClassificationTypeNames.Operator, "!+-*/\\=<>`" ),
        };
    }
}