
//-----------------------------------
// für time reverse Operationen
// wenn der Spieler reverse button drückt, wird der Status von dem Spiel
// auf den letzten gespeicherten Zeitpunkt zurücksetzen
//-----------------------------------
interface TRInterface
{
    void SaveTRObject();
    void LoadTRObject(TRObject trobject);
}

//-----------------------------------
// alle Objekte, die ein TRInterface implementierendes Script trägt,
// sollen eine innere class implementieren, die die absract class TRObject erbt
//-----------------------------------
abstract public class TRObject { }