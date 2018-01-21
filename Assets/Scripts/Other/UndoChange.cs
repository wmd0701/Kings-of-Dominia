interface UndoChange
{
    /// <summary>
    /// Speicher letzte Änderung
    /// </summary>
    void SaveLastChange(Change pi_Change);

    /// <summary>
    /// Entfernt letzte Änderung
    /// </summary>
    void RestoreLastSave();
}

/// <summary>
/// Änderungsklasse
/// </summary>
public abstract class Change { }