using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WritingRemove : DragAcceptable {

    public Action onSelect;

    public override void Interact(Hand hand) {
        base.Interact(hand);
        Logger.Print("Chose: remove");
        onSelect();
    }

    public void Interact()
    { 
        Logger.Print("Chose: remove");
        onSelect();
    }
}

