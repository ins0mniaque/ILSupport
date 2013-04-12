using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

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

        private static readonly SymbolCategory [ ] symbolCategories =
        {
            new SymbolCategory ( PredefinedClassificationTypeNames.Operator, "!+-*/\\=<>`" ),
        };

        private static string IdentifySymbolClass ( char symbol )
        {
            foreach ( var category in symbolCategories )
                if ( category.Symbols.Contains ( symbol ) )
                    return category.Class;

            return null;
        }
    }
}