using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace LiftSimulation
{

    public sealed class CodeActivity1 : CodeActivity
    {
        // Aktivitätseingabeargument vom Typ "string" definieren
        public InArgument<string> Text { get; set; }

        // Wenn durch die Aktivität ein Wert zurückgegeben wird, erfolgt eine Ableitung von CodeActivity<TResult>
        // und der Wert von der Ausführmethode zurückgegeben.
        protected override void Execute( CodeActivityContext context )
        {
            // Laufzeitwert des Texteingabearguments abrufen
            string text = context.GetValue( this.Text );
        }
    }
}
